using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsController : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.GetComponent("BasicCharacter")) {
          BasicCharacter player = transform.parent.gameObject.GetComponent<BasicCharacter>();
          BasicCharacter otherChar = other.GetComponent<BasicCharacter>();
          otherChar.Damage((int) (player.stats.strength / otherChar.stats.strength) * 5);
        }
    }
}
