using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCharacter : MonoBehaviour
{
    [Tooltip("Player Speed")]
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

    // current combo
    private int combo = 0;
    // time of last attack
    private float lastTime;

    private Animator animator;
    private SpriteRenderer spriteRender;
    private Rigidbody2D body;
    public CharacterStats stats;
    private bool isFlipped;

    private void Start() {
        // get parameters
        animator = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        stats = GetComponent<CharacterStats>();
        isFlipped = spriteRender.flipX;
        //Starts the looping coroutine
        StartCoroutine("Melee");
    }

    private void Update() {
        bool W = Input.GetKey(KeyCode.W);
        bool A = Input.GetKey(KeyCode.A);
        bool S = Input.GetKey(KeyCode.S);
        bool D = Input.GetKey(KeyCode.D);
        bool E = Input.GetKey(KeyCode.E);
        bool Q = Input.GetKey(KeyCode.Q);
        Vector2 dir = new Vector2((D?1:0)-(A?1:0), (W?1:0)-(S?1:0));
        dir.Normalize();
        this.Move(speed*dir);
        if (E) this.Damage(1);
        if (Q) this.Strengthen(0.33f/stats.strength);
    }

    private void PlaySound(AudioSource src) {
      // handles playing an audio source
      AudioSource audio = Instantiate(src, this.transform.position, Quaternion.identity);
      audio.transform.parent = this.transform;
      audio.Play();
      Destroy(audio.gameObject, audio.clip.length);
    }

    public void Kill() {
      // handles killing the player
      // TODO: implement
    }

    public void Strengthen(float strength) {
      // handles strengthening the player
      stats.strength += strength;
    }

    public void Damage(int damage) {
      // handles damaging the player
      stats.health -= damage;
      if (stats.health <= 0) {
        this.Kill();
      } else {
        this.PlaySound(hurtNoise);
      }
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
      this.PlaySound(attackNoise);
    }

    IEnumerator Melee () {
       //Constantly loops so you only have to call it once
       while (true) {
           //Checks if attacking and then starts of the combo
           if (Input.GetMouseButtonDown(0)) {
               combo++;
               this.Attack(combo);
               lastTime = Time.time;

               //Combo loop that ends the combo if you reach the maxTime between attacks, or reach the end of the combo
               while ((Time.time-lastTime) < maxTime || animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f) {
                 body.velocity = Vector2.zero;
                 animator.SetBool("IdleOrMoving", false);
                   //Attacks if your cooldown has reset
                   if (Input.GetMouseButtonDown(0) && (Time.time-lastTime) > cooldown && combo < maxCombo) {
                       combo++;
                       this.Attack(combo);
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
