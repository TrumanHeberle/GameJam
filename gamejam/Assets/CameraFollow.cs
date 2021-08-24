using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
  public Transform target;
  public float responsiveness;

  void Update() {
    if (target == null) return;
    this.transform.position = Vector3.Lerp(this.transform.position, target.position+new Vector3(0.0f,0.0f,-10.0f), Time.deltaTime*responsiveness);
  }
}
