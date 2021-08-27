using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
  // Health Parameters
  [Tooltip("Healthbar UI")]
  [SerializeField]
  private Slider healthbar;
  [Tooltip("Max Health Value")]
  [SerializeField]
  private int _maxHealth = 100;
  public int maxHealth {
    get { return _maxHealth; }
    set {
      healthbar.maxValue = value;
      _maxHealth = value;
    }
  }
  // current health
  public int health {
    get { return (int) healthbar.value; }
    set { healthbar.value = value; }
  }

  // Strength Parameters
  [Tooltip("Character Strengthbar UI")]
  [SerializeField]
  private Slider strengthbar;
  [Tooltip("Character Strength Level UI")]
  [SerializeField]
  private Text strengthtext;
  [Tooltip("Character Strength")]
  [SerializeField]
  private float _strength = 1;
  // current power level (int)
  public int level {
    get { return Mathf.FloorToInt(_strength); }
  }
  // current power level (float)
  public float strength {
    get { return _strength; }
    set {
      _strength = value;
      strengthtext.text = level.ToString();
      strengthbar.value = value-level;
    }
  }

  private void Start() {
    health = maxHealth;
    strengthbar.maxValue = 1;
    strength = strength;
  }
}
