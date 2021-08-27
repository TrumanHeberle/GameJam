using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
  [Tooltip("Scene Camera")]
  public CameraFollow sceneCamera;
  [Tooltip("Initial Player")]
  public Transform player;
  [Tooltip("List of Enemies")]
  public Transform[] enemies;
  [Tooltip("Max Number of Enemies")]
  public int maxEnemies = 20;
  [Tooltip("Max Spawn Attempts")]
  public int maxSpawns = 5;
  [Tooltip("Time Between Spawn Attempts")]
  public int spawnWaitTime = 10;
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

  public Transform characters;
  public Transform spawners;

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
      // start regulators
      StartCoroutine(SwapCharacters());
      StartCoroutine(SpawnEnemies());
    }

    private void Swap(Transform enemy) {
      // handles swapping player with enemy
      // swap controller
      BasicCharacter playerChar = player.GetComponent<BasicCharacter>();
      BasicCharacter enemyChar = enemy.GetComponent<BasicCharacter>();
      playerChar.Toggle();
      enemyChar.Toggle();
      // swap objects
      sceneCamera.target = enemy;
      enemy.transform.parent = player.transform.parent;
      player.transform.parent = characters.transform;
      player = enemy;
    }

    private void Spawn() {
      // handles spawning an enemy
      Transform enemy = Instantiate(enemies[Random.Range(0,enemies.Length)], characters);
      enemy.position = spawners.GetChild(Random.Range(0,spawners.childCount)).position + new Vector3(Random.Range(-1,1),Random.Range(0.5f,2.5f),0);
    }

    IEnumerator SwapCharacters() {
      while (true) {
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
        if (characters.childCount == 0) this.Spawn();
        this.Swap(characters.GetChild(0));
        music.Play();
      }
    }

    IEnumerator SpawnEnemies() {
      while (true) {
        // wait for prolonged time
        yield return new WaitForSeconds(spawnWaitTime);
        // spawn a few enemies
        int toSpawn = Mathf.Min(maxSpawns, maxEnemies-characters.childCount);
        while (toSpawn>0) {
          this.Spawn();
          toSpawn -= 1;
        }
      }
    }
}
