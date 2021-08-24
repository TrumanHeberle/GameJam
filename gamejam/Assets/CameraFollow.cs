using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
  // transform object to track
  public Transform target;
  // responsiveness to shift camera
  public float responsiveness = 5;

  void Update() {
    if (target == null) return;
    this.transform.position = Vector3.Lerp(this.transform.position, target.position+new Vector3(0.0f,0.0f,-10.0f), Time.deltaTime*responsiveness);
  }
}
