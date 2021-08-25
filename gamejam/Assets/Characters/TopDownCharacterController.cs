using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cainos.PixelArtTopDown_Basic
{
    public class TopDownCharacterController : MonoBehaviour
    {
        [Tooltip("Player Speed")]
        public float speed = 5;
        [Tooltip("Attack Cooldown Time")]
        public float cooldown = 0.5f;
        [Tooltip("Combo Time Window")]
        public float maxTime = 0.8f;
        [Tooltip("Combo Limit")]
        public int maxCombo = 3;
        [Tooltip("Maximum Health")]
        public int maxHealth = 100;
        [Tooltip("Health Bar")]
        public Slider healthbar;

        [Tooltip("Hurt Noise Clip")]
        public AudioClip hurtNoise;
        [Tooltip("Attack Noise Clip")]
        public AudioClip attackNoise;

        // current combo
        private int combo = 0;
        // time of last attack
        private float lastTime;

        private Animator animator;
        private SpriteRenderer spriteRender;
        private Rigidbody2D body;
        private bool isFlipped;

        private void Start() {
            // get parameters
            animator = GetComponent<Animator>();
            spriteRender = GetComponent<SpriteRenderer>();
            body = GetComponent<Rigidbody2D>();
            // set up player attributes
            isFlipped = spriteRender.flipX;
            healthbar.maxValue = maxHealth;
            healthbar.value = maxHealth;
            //Starts the looping coroutine
            StartCoroutine("Melee");
        }

        private void Update() {
            bool W = Input.GetKey(KeyCode.W);
            bool A = Input.GetKey(KeyCode.A);
            bool S = Input.GetKey(KeyCode.S);
            bool D = Input.GetKey(KeyCode.D);
            bool E = Input.GetKey(KeyCode.E);
            Vector2 dir = new Vector2((D?1:0)-(A?1:0), (W?1:0)-(S?1:0));
            dir.Normalize();
            this.Move(speed*dir);
            if (E) this.Damage(1);
        }

        public void Kill() {
          // handles killing the player
          // TODO: implement
        }

        public void Damage(int damage) {
          // handles damaging the player
          healthbar.value -= damage;
          if (healthbar.value <= healthbar.minValue) {
            this.Kill();
          } else {
            AudioSource.PlayClipAtPoint(hurtNoise, this.transform.position);
          }
        }

        public void Move(Vector2 direction) {
          // handles moving the player
          if (direction.x < 0) {
            spriteRender.flipX = !isFlipped;
          } else if (direction.x > 0) {
            spriteRender.flipX = isFlipped;
          }
          animator.SetBool("IdleOrMoving", direction.magnitude>0);
          body.velocity = direction;
        }

        public void Attack(int attackId) {
          // handles the player's attack
          Debug.Log(attackId);
          animator.SetInteger("attackCounter", attackId);
          animator.SetBool("isAttacking", true);
          AudioSource.PlayClipAtPoint(attackNoise, this.transform.position);
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
}
