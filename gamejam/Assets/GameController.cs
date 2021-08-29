using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
  // Make Singleton
  public static GameController Instance { get; private set; }
  private void Awake() { Instance = this; }

  // Regular Class
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
  [Tooltip("Swap Noise")]
  public AudioSource swapNoise;

  [HideInInspector]
  public CameraFollow sceneCamera;
  [HideInInspector]
  public Transform characters;
  [HideInInspector]
  public Transform spawners;
  private List<Coroutine> coroutines = new List<Coroutine>();
  private AudioSource warn;
  private int _score = 0;
  public int score {
    get { return _score; }
    set {
      _score = value;
      Text scoretext = sceneCamera.GetComponentInChildren<Text>();
      scoretext.text = "Souls Freed: " + _score;
    }
  }

    private void Start() {
      // get parameters
      sceneCamera = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
      characters = GameObject.Find("Characters").transform;
      spawners = GameObject.Find("Spawners").transform;
      // start background music
      if (music != null) {
        music = Instantiate(music, transform.position, Quaternion.identity);
        music.transform.parent = transform;
        music.Play();
      }
      // start regulators
      coroutines.Add(StartCoroutine(SwapCharacters()));
      coroutines.Add(StartCoroutine(SpawnEnemies()));
    }

    public void PlaySound(AudioSource src) {
      PlaySound(src, transform);
    }

    public void PlaySound(AudioSource src, Transform origin) {
      // handles playing an audio source
      if (src == null) return;
      AudioSource audio = Instantiate(src, origin.position, Quaternion.identity);
      audio.transform.parent = origin;
      audio.Play();
      Destroy(audio.gameObject, audio.clip.length);
    }

    private void Swap(Transform enemy) {
      // handles swapping player with enemy
      if (player == null) return;
      BasicCharacter playerChar = player.GetComponent<BasicCharacter>();
      BasicCharacter enemyChar = enemy.GetComponent<BasicCharacter>();
      enemyChar.stats.health = enemyChar.stats.maxHealth;
      playerChar.Toggle();
      enemyChar.Toggle();
      sceneCamera.target = enemy;
      player = enemy;
      // play swap noise
      PlaySound(swapNoise);
    }

    private Transform Spawn() {
      // handles spawning an enemy
      Transform enemy = Instantiate(enemies[Random.Range(0,enemies.Length)], characters);
      enemy.position = spawners.GetChild(Random.Range(0,spawners.childCount)).position + new Vector3(Random.Range(-2.0f,2.0f),Random.Range(-0.5f,3.5f),0);
      BasicCharacter enemyChar = enemy.GetComponent<BasicCharacter>();
      enemyChar.stats.strength = Random.Range(1,characters.GetChild(Random.Range(0,characters.childCount)).GetComponent<BasicCharacter>().stats.strength);
      return enemy;
    }

    public void Restart() {
      // handles restarting the game
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void End() {
      // handles ending the game
      foreach (Coroutine coroutine in coroutines) StopCoroutine(coroutine);
      foreach (Transform character in characters) Destroy(character.gameObject);
      sceneCamera.target = GameObject.Find("EndTarget").transform;
      if (warn != null) Destroy(warn.gameObject);
      music.Play();
    }

    private IEnumerator SwapCharacters() {
      while (true) {
        // wait for prolonged time
        yield return new WaitForSeconds(Random.Range(minSwapTime, maxSwapTime));
        // warn player
        if (music != null) music.Pause();
        if (warnNoise != null) {
          warn = Instantiate(warnNoise, transform.position, Quaternion.identity);
          warn.transform.parent = transform;
          warn.Play();
        }
        yield return new WaitForSeconds(warnTime);
        // swap player
        if (warn != null) Destroy(warn.gameObject);
        if (characters.childCount == 0) Swap(Spawn());
        else Swap(characters.GetChild(Random.Range(0,characters.childCount)));
        music.Play();
      }
    }

    private IEnumerator SpawnEnemies() {
      while (true) {
        // wait for prolonged time
        yield return new WaitForSeconds(spawnWaitTime);
        // spawn a few enemies
        int toSpawn = Mathf.Min(maxSpawns, maxEnemies-characters.childCount);
        while (toSpawn>0) {
          Spawn();
          toSpawn -= 1;
        }
      }
    }
}
