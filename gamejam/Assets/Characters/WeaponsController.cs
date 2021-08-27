using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsController : MonoBehaviour {
  private float damageMultiplier = 3.0f;
  private float playerMultiplier = 4.0f;
  private float strengthMultiplier = 1.0f;

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.GetComponent("BasicCharacter")) {
          BasicCharacter character = transform.parent.gameObject.GetComponent<BasicCharacter>();
          BasicCharacter otherChar = other.GetComponent<BasicCharacter>();
          float ratio = character.stats.strength / otherChar.stats.strength;
          bool killed = otherChar.Damage((int) ((character.isPlayer ? playerMultiplier*damageMultiplier : damageMultiplier)*ratio));
          if (killed) {
            character.Strengthen(otherChar.stats.strength*strengthMultiplier);
            if (character.isPlayer) GameController.score++;
          }
        }
    }
}
