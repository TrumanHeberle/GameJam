using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : BasicBehavior
{
  public override Vector2 checkMove() {
    // returns the direction to move
    bool W = Input.GetKey(KeyCode.W);
    bool A = Input.GetKey(KeyCode.A);
    bool S = Input.GetKey(KeyCode.S);
    bool D = Input.GetKey(KeyCode.D);
    return new Vector2((D?1:0)-(A?1:0), (W?1:0)-(S?1:0));
  }

  public override bool checkAttack() {
    // returns whether the character should attack
    return Input.GetMouseButtonDown(0);
  }
}
