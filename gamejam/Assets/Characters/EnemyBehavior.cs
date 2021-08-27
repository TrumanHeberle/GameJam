using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : BasicBehavior
{
  public override Vector2 checkMove() {
    // returns the direction to move
    GameController game = GameObject.Find("Game Controller").GetComponent<GameController>();
    if (game.player == null) return new Vector2(Random.Range(-1,1), Random.Range(-1,1));
    return new Vector2(game.player.position.x-transform.position.x, game.player.position.y-transform.position.y);
  }

  public override bool checkAttack() {
    // returns whether the character should attack
    GameController game = GameObject.Find("Game Controller").GetComponent<GameController>();
    if (game.player == null) return false;
    Vector2 dir = new Vector2(game.player.position.x-transform.position.x, game.player.position.y-transform.position.y);
    return dir.magnitude <= 1;
  }
}
