using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBehavior : MonoBehaviour
{
  public virtual Vector2 checkMove() {
    // returns the direction to move
    return Vector2.zero;
  }

  public virtual bool checkAttack() {
    // returns whether the character should attack
    return false;
  }
}
