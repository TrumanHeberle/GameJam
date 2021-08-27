using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBehavior : MonoBehaviour
{
  public virtual Vector2 CheckMove() {
    // returns the direction to move
    return Vector2.zero;
  }

  public virtual bool CheckAttack() {
    // returns whether the character should attack
    return false;
  }
}
