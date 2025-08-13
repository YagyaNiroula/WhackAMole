using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class HammerController : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text timeUpMessage;
    public string gameOverSceneName = "GameOverScene";
    public float delayBeforeGameOver = 2f;

    public int score = 0;
    private MoleSpawner ms;
    private bool gameEnded = false;

    // Bonus tracking
    public int bonusHitsRequired = 4;    // Number of hits needed for bonus
    public float bonusWindow = 2f;       // Time window in seconds
    private bool bonusActivated = false;

    private List<float> hitTimes = new List<float>();  // Track timestamps of hits

    void Start()
    {
        score = 0;
        ms = FindObjectOfType<MoleSpawner>();
        scoreText.text = score.ToString();

        if (timeUpMessage != null)
            timeUpMessage.gameObject.SetActive(false);
    }

    void Update()
    {
        if (ms == null) return;

        // Check for game over
        if (ms.gameTime <= 0 && !gameEnded)
        {
            gameEnded = true;
            ShowTimeUpAndLoadGameOver();
            return;
        }

        // Check for player clicks
        if (Input.GetMouseButtonDown(0) && ms.gameTime > 0)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null && hit.transform.gameObject == ms.currentMole)
            {
                ms.MoleHit();
                score++;
                scoreText.text = score.ToString();

                CheckForBonus();
            }
        }
    }

    void CheckForBonus()
    {
        if (bonusActivated) return;

        // Add current hit timestamp
        hitTimes.Add(Time.time);

        // Remove hits older than bonusWindow from the first hit in the list
        while (hitTimes.Count > 0 && Time.time - hitTimes[0] > bonusWindow)
            hitTimes.RemoveAt(0);

        // Check if enough hits occurred in the current window
        if (hitTimes.Count >= bonusHitsRequired)
        {
            StartCoroutine(BonusFreeze());
            bonusActivated = true;  // Trigger bonus only once
            hitTimes.Clear();       // Reset hits after bonus
        }
    }

    IEnumerator BonusFreeze()
    {
        Debug.Log("BONUS! Timer frozen for 3 seconds!");

        // Show bonus message
        if (ms.freezeMessage != null)
            ms.freezeMessage.gameObject.SetActive(true);

        ms.isTimerFrozen = true; // Pause game timer

        yield return new WaitForSeconds(3f); // Bonus duration

        ms.isTimerFrozen = false; // Resume timer

        // Hide bonus message
        if (ms.freezeMessage != null)
            ms.freezeMessage.gameObject.SetActive(false);

        Debug.Log("Timer resumed!");
    }

    void ShowTimeUpAndLoadGameOver()
    {
        if (ms.currentMole != null)
        {
            Destroy(ms.currentMole);
            ms.currentMole = null;
        }

        if (timeUpMessage != null)
            timeUpMessage.gameObject.SetActive(true);

        StartCoroutine(LoadGameOverAfterDelay(delayBeforeGameOver));
    }

    IEnumerator LoadGameOverAfterDelay(float delay)
    {
        GameData.finalScore = score;
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(3);
    }
}
