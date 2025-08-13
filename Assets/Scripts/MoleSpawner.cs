using System.Collections;
using UnityEngine;
using TMPro;

public class MoleSpawner : MonoBehaviour
{
    public GameObject molePrefab;
    public Transform[] spawnPoints;
    public float gameTime = 60f;
    public TMP_Text timer;
    public TMP_Text freezeMessage;
    public float moleVisibleTime = 0.8f; // mole stays visible

    private bool gameActive = true;
    public GameObject currentMole;
    private Coroutine moleLifeCoroutine;

    public bool isTimerFrozen = false;

    void Start()
    {
        StartCoroutine(SpawnRoutine());

        if (freezeMessage != null)
            freezeMessage.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!gameActive)
            return;


        if (!isTimerFrozen)
            gameTime -= Time.deltaTime;

        if (gameTime <= 0)
        {
            gameTime = 0;
            gameActive = false;
            Debug.Log("Game Over! Time's up.");
        }

        UpdateTimerText();
    }

    IEnumerator SpawnRoutine()
    {
        while (gameActive)
        {
            Spawn();
            yield return new WaitForSeconds(moleVisibleTime);
        }
    }

    public void Spawn()
    {
        if (!gameActive)
            return;

        if (currentMole != null)
        {
            Destroy(currentMole);
            Debug.Log("Missed mole. Spawning next.");
        }

        int index = Random.Range(0, spawnPoints.Length);
        currentMole = Instantiate(molePrefab, spawnPoints[index].position, Quaternion.identity);
        Debug.Log("Spawned mole at position: " + currentMole.transform.position);
    }

    public void MoleHit()
    {
        if (currentMole != null)
        {
            Destroy(currentMole);
            currentMole = null;
            Debug.Log("Mole hit!");
        }
    }

    void UpdateTimerText()
    {
        if (gameActive)
        {
            int seconds = Mathf.CeilToInt(gameTime);
            timer.text = seconds.ToString();
        }
        else
        {
            timer.text = "0";
        }
    }
}
