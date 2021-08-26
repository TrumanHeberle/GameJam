using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
  private Transform characters;
  private Transform spawners;

    private void Start() {
      characters = GameObject.Find("Characters").transform;
      spawners = GameObject.Find("Spawners").transform;
    }

    private void Update() {
      if (Input.GetKey(KeyCode.Space)) {
        this.Swap(characters.GetChild(0), characters.GetChild(1));
      }
    }

    private void Swap(Transform c1, Transform c2) {
      // TODO: work on swapping controllers
      Vector3 p1 = c1.position;
      c1.position = c2.position;
      c2.position = p1;
    }
}
