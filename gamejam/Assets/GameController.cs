using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
  [Tooltip("Scene Camera")]
  public CameraFollow sceneCamera;
  [Tooltip("Initial Player")]
  public Transform player;
  [Tooltip("Time for Swap Warning")]
  public int warnTime = 5;
  [Tooltip("Minimum Time for Next Swap")]
  public int minSwapTime = 5;
  [Tooltip("Maximum Time for Next Swap")]
  public int maxSwapTime = 10;
  [Tooltip("Background Music")]
  public AudioSource music;
  [Tooltip("Swap Warning Noise")]
  public AudioSource warnNoise;

  private Transform characters;
  private Transform spawners;

    private void Start() {
      // get parameters
      characters = GameObject.Find("Characters").transform;
      spawners = GameObject.Find("Spawners").transform;
      // start background music
      if (music != null) {
        music = Instantiate(music, this.transform.position, Quaternion.identity);
        music.transform.parent = this.transform;
        music.Play();
      }
      // swapping randomly
      StartCoroutine(SwapCharacters());
    }

    private void Swap(Transform enemy) {
      // handles swapping player with enemy
      sceneCamera.target = enemy;
      // TODO: swap player/enemy reference
      // TODO: swap AIs
    }

    IEnumerator SwapCharacters() {
      while(true) {
        // wait for prolonged time
        yield return new WaitForSeconds(Random.Range(minSwapTime, maxSwapTime));
        // warn player
        if (music != null) music.Pause();
        AudioSource warn = null;
        if (warnNoise != null) {
          warn = Instantiate(warnNoise, this.transform.position, Quaternion.identity);
          warn.transform.parent = this.transform;
          warn.Play();
        }
        yield return new WaitForSeconds(warnTime);
        // swap player
        if (warn != null) Destroy(warn.gameObject);
        this.Swap(characters.GetChild(0));
        music.Play();
      }
    }
}
