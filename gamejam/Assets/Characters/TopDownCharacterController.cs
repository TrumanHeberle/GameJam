using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    public class TopDownCharacterController : MonoBehaviour
    {
        //Player movement speed (in meters per second)
        public float speed = 5;
        //Cooldown time between attacks (in seconds)
        public float cooldown = 0.5f;
        //Max time before combo ends (in seconds)
        public float maxTime = 0.8f;
        //Max number of attacks in combo
        public int maxCombo = 3;

        //Current combo
        private int combo = 0;
        //Time of last attack
        private float lastTime;

        private Animator animator;
        private SpriteRenderer spriteRender;
        private Rigidbody2D body;

        private void Start()
        {
            animator = GetComponent<Animator>();
            spriteRender = GetComponent<SpriteRenderer>();
            body = GetComponent<Rigidbody2D>();
            //Starts the looping coroutine
            StartCoroutine("Melee");
        }


        private void Update()
        {
            bool W = Input.GetKey(KeyCode.W);
            bool A = Input.GetKey(KeyCode.A);
            bool S = Input.GetKey(KeyCode.S);
            bool D = Input.GetKey(KeyCode.D);
            Vector2 dir = new Vector2((D?1:0)-(A?1:0), (W?1:0)-(S?1:0));
            if (A) {
              spriteRender.flipX = true;
            }
            else if (D) {
              spriteRender.flipX = false;
            }
            dir.Normalize();
            animator.SetBool("IdleOrMoving", dir.magnitude > 0);
            body.velocity = speed * dir;
        }

        IEnumerator Melee () {
           //Constantly loops so you only have to call it once
           while (true) {
               //Checks if attacking and then starts of the combo
               if (Input.GetMouseButtonDown(0)) {
                   combo++;
                   animator.SetInteger("attackCounter", combo);
                   animator.SetBool("isAttacking", true);
                   Debug.Log("Attack" + combo);
                   lastTime = Time.time;

                   //Combo loop that ends the combo if you reach the maxTime between attacks, or reach the end of the combo
                   while ((Time.time - lastTime) < maxTime && combo <= maxCombo) {
                     body.velocity = Vector2.zero;
                     animator.SetBool("IdleOrMoving", false);
                       //Attacks if your cooldown has reset
                       if (Input.GetMouseButtonDown(0) && (Time.time - lastTime) > cooldown) {
                           combo++;
                           animator.SetInteger("attackCounter", combo);
                           animator.SetBool("isAttacking", true);
                           lastTime = Time.time;
                       }
                       yield return null;
                   }
                   //Resets combo and waits the remaining amount of cooldown time before you can attack again to restart the combo
                   combo = 0;
                   animator.SetInteger("attackCounter", combo);
                   animator.SetBool("isAttacking", false);
                   yield return new WaitForSeconds(cooldown - (Time.time - lastTime));
               }
               yield return null;
           }
       }
    }
}
