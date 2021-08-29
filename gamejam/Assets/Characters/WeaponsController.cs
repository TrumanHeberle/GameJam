using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsController : MonoBehaviour {
  [Tooltip("Damage to Deal Against Identical Strength")]
  public int baseDamage = 5;
  [Tooltip("Damage Multiplier for Player")]
  public float playerMultiplier = 2.5f;
  [Tooltip("Strength Multiplier per Hit")]
  public float strengthMultiplier = 2.0f;
  [Tooltip("Strength Multiplier per Kill")]
  public float killMultiplier = 2.0f;
  [Tooltip("Critical Hit Damage Multiplier")]
  public float critMultiplier = 3.0f;
  [Tooltip("Critical Hit Chance")]
  public float critChance = 0.2f;

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.GetComponent("BasicCharacter")) {
          BasicCharacter character = transform.parent.gameObject.GetComponent<BasicCharacter>();
          BasicCharacter otherChar = other.GetComponent<BasicCharacter>();
          // calculate damage and strength
          float strengthDiff = character.stats.strength-otherChar.stats.strength;
          float strengthRatio = character.stats.strength/otherChar.stats.strength;
          float damage = baseDamage;
          if (Random.value <= critChance) damage *= critMultiplier;
          if (character.isPlayer) damage *= playerMultiplier;
          damage *= strengthRatio;
          damage = Mathf.Min(otherChar.stats.health, damage);
          float strength = damage/otherChar.stats.maxHealth;
          strength *= strengthMultiplier/strengthRatio;
          // apply damage and strength
          bool killed = otherChar.Damage((int) damage);
          character.Strengthen(killed ? killMultiplier*strength : strength);
          if (killed && character.isPlayer) GameController.Instance.score++;
        }
    }
}
