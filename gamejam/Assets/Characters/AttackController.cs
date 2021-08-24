using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{

    private Animator animator;
    private SpriteRenderer spriteRender;

    //Cooldown time between attacks (in seconds)
   public float cooldown = 0.5f;
   //Max time before combo ends (in seconds)
   public float maxTime = 0.8f;
   //Max number of attacks in combo
   public int maxCombo = 3;
   //Current combo
   int combo = 0;
   //Time of last attack
   float lastTime;
    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
        //Starts the looping coroutine
        StartCoroutine("Melee");
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
                   //Attacks if your cooldown has reset
                   if (Input.GetMouseButtonDown(0) && (Time.time - lastTime) > cooldown) {
                       combo++;
                       animator.SetInteger("attackCounter", combo);
                       animator.SetBool("isAttacking", true);
                       Debug.Log("Attack " + combo);
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
