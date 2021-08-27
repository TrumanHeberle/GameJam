using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCharacter : MonoBehaviour
{
    [Tooltip("Character Speed")]
    public float speed = 5;
    [Tooltip("Attack Cooldown Time")]
    public float cooldown = 0.5f;
    [Tooltip("Combo Time Window")]
    public float maxTime = 0.8f;
    [Tooltip("Combo Limit")]
    public int maxCombo = 3;

    [Tooltip("Hurt Noise Source")]
    public AudioSource hurtNoise;
    [Tooltip("Attack Noise Source")]
    public AudioSource attackNoise;
    [Tooltip("Strengthen Noise Source")]
    public AudioSource strengthenNoise;

    // current combo
    private int combo = 0;
    // time of last attack
    private float lastTime;

    private Animator animator;
    private SpriteRenderer spriteRender;
    private Rigidbody2D body;
    private BasicBehavior playerBehavior;
    private BasicBehavior enemyBehavior;
    public bool isPlayer = false;
    private BasicBehavior behavior { get { return isPlayer ? playerBehavior : enemyBehavior; } }
    public CharacterStats stats { get { return GetComponent<CharacterStats>(); } }

    // Core Functions
    private void Start() {
        // get parameters
        animator = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        playerBehavior = (BasicBehavior) GetComponent<PlayerBehavior>();
        enemyBehavior = (BasicBehavior) GetComponent<EnemyBehavior>();
        //Starts the looping coroutine
        StartCoroutine("Melee");
    }

    private void Update() {
        Move(speed*behavior.CheckMove());
        bool E = Input.GetKey(KeyCode.E);
        bool Q = Input.GetKey(KeyCode.Q);
        if (E) Damage(1);
        if (Q) Strengthen(0.33f/stats.strength);
    }

    private void PlaySound(AudioSource src) {
      // handles playing an audio source
      if (src == null) return;
      AudioSource audio = Instantiate(src, transform.position, Quaternion.identity);
      audio.transform.parent = transform;
      audio.Play();
      Destroy(audio.gameObject, audio.clip.length);
    }

    public void Toggle() {
      // handles swapping control from player to enemy or vice versa
      isPlayer = !isPlayer;
    }

    public void Kill() {
      // handles killing the player
      animator.SetBool("isDead", true);
      // TODO: handle death better, this is temporary
      Destroy(gameObject);
    }

    public void Strengthen(float strength) {
      // handles strengthening the player
      stats.strength += strength;
      PlaySound(strengthenNoise);
    }

    public bool Damage(int damage) {
      // handles damaging the player, returns whether the character has died
      if (stats.health == 0) return false;
      stats.health -= damage;
      if (stats.health <= 0) {
        Kill();
        return true;
      }
      PlaySound(hurtNoise);
      return false;
    }

    public void Move(Vector2 direction) {
      // handles moving the player
      if (direction.x < 0) {
        transform.localScale = new Vector2(-1f, 1f);
        foreach (Transform child in transform) {
          if(child.name != "HitBox") {
            child.localScale = new Vector2(-1f, 1f);
          }
        }
      } else if (direction.x > 0) {
        transform.localScale = new Vector2(1f, 1f);
        foreach (Transform child in transform) {
          if(child.name != "HitBox") {
            child.localScale = new Vector2(1f, 1f);
          }
        }
      }
      animator.SetBool("IdleOrMoving", direction.magnitude>0);
      body.velocity = direction;
    }

    public void Attack(int attackId) {
      // handles the player's attack
      animator.SetInteger("attackCounter", attackId);
      animator.SetBool("isAttacking", true);
      PlaySound(attackNoise);
    }

    IEnumerator Melee () {
       //Constantly loops so you only have to call it once
       while (true) {
           //Checks if attacking and then starts of the combo
           if (behavior.CheckAttack()) {
               combo++;
               Attack(combo);
               lastTime = Time.time;

               //Combo loop that ends the combo if you reach the maxTime between attacks, or reach the end of the combo
               while ((Time.time-lastTime) < maxTime || animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f) {
                 body.velocity = Vector2.zero;
                 animator.SetBool("IdleOrMoving", false);
                   //Attacks if your cooldown has reset
                   if ((Time.time-lastTime) > cooldown && combo < maxCombo && behavior.CheckAttack()) {
                       combo++;
                       Attack(combo);
                       lastTime = Time.time;
                   }
                   yield return null;
               }
               //Resets combo and waits the remaining amount of cooldown time before you can attack again to restart the combo
               combo = 0;
               animator.SetInteger("attackCounter", combo);
               animator.SetBool("isAttacking", false);
               yield return new WaitForSeconds(cooldown-(Time.time-lastTime));
           }
           yield return null;
       }
   }
}
