using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : BasicBehavior
{
  private float retargetChance = 0.1f;
  private float attackChance = 0.1f;
  private float aggressiveChance = 0.67f;
  private Transform target;
  private bool aggressive = false;

  private void Start() {
    StartCoroutine(ChangeTarget());
    StartCoroutine(ChooseAction());
  }

  public override Vector2 CheckMove() {
    // returns the direction to move
    if (target == null) return Vector2.zero;
    return (aggressive ? 0.67f : -0.67f) * new Vector2(target.position.x-transform.position.x, target.position.y-transform.position.y).normalized;
  }

  public override bool CheckAttack() {
    // returns whether the character should attack
    if (target == null || !aggressive) return false;
    Vector2 dir = new Vector2(target.position.x-transform.position.x, target.position.y-transform.position.y);
    return dir.magnitude <= 1 && Random.value <= attackChance;
  }

  private Transform Retarget() {
    // get possible targets
    List<Transform> options = new List<Transform>();
    Vector2 pos = new Vector2(transform.position.x, transform.position.y);
    if (GameController.characters != null) {
      Vector2 other;
      foreach (Transform character in GameController.characters) {
        other = new Vector2(character.position.x, character.position.y);
        if (transform != character) {
          options.Add(character);
        }
      }
    }
    // set target
    options.Sort(delegate(Transform a, Transform b) {
      Vector2 va = new Vector2(a.position.x, a.position.y);
      Vector2 vb = new Vector2(b.position.x, b.position.y);
      return Vector2.Distance(pos,va).CompareTo(Vector2.Distance(pos,vb));
    });
    foreach (Transform option in options) {
      if (Random.value >= retargetChance) return option;
    }
    return null;
  }

  IEnumerator ChangeTarget() {
    while (true) {
      target = Retarget();
      yield return new WaitForSeconds(1);
    }
  }

  IEnumerator ChooseAction() {
    while (true) {
      aggressive = Random.value <= aggressiveChance;
      yield return new WaitForSeconds(3);
    }
  }
}
