using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsController : MonoBehaviour {
  private float damageMultiplier = 5.0f;
  private float playerMultiplier = 2.0f;
  private float strengthMultiplier = 2.0f;
  private float powerMultiplier = 1.25f;

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.GetComponent("BasicCharacter")) {
          BasicCharacter character = transform.parent.gameObject.GetComponent<BasicCharacter>();
          BasicCharacter otherChar = other.GetComponent<BasicCharacter>();
          //float ratio = character.stats.strength / otherChar.stats.strength;
          float ratio = Mathf.Pow(powerMultiplier, character.stats.strength-otherChar.stats.strength);
          bool killed = otherChar.Damage((int) ((character.isPlayer ? playerMultiplier*damageMultiplier : damageMultiplier) * ratio));
          if (killed) {
            //character.Strengthen(strengthMultiplier/ratio);
            character.Strengthen(strengthMultiplier/ratio/ratio);
            if (character.isPlayer) GameController.score++;
          }
        }
    }
}
