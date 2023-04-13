using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour
{
    public TMP_Text lapText;
    public TMP_Text raceResultText;
    public Button startButton;
    public TMP_Text countdownText;

    public float countdownDuration = 3f;

    private GameManager gameManager;
    void Start()
    {
        // Get a reference to the GameManager script
        gameManager = FindObjectOfType<GameManager>();
        //startButton.SetActive(true);

        // Set up the UI components
        lapText.text = "Lap: 0/" + gameManager.numLaps;
        raceResultText.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(false);

        // Add an OnClick event to the Start button
    }

    void Update()
    {
        // Update the lap text based on the player's progress
        lapText.text = "Lap " + gameManager.playerLaps + "/" + gameManager.numLaps;

        // Check if the race has finished
        if (gameManager.raceFinished)
        {
            raceResultText.gameObject.SetActive(true);
            countdownText.gameObject.SetActive(false);
        }
    }

    public void StartCountdown()
    {
        // Hide the start button and show the countdown text
        startButton.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(true);

        // Start the countdown
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        // Count down from the duration of the countdown
        for (float i = countdownDuration; i >= 0; i -= Time.deltaTime)
        {
            countdownText.text = Mathf.Ceil(i).ToString();
            yield return null;
        }

        // Start the race after the countdown finishes
        countdownText.text = "GO!";
        //yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);
        lapText.gameObject.SetActive(true);
        gameManager.StartRace();
    }

    public void ReturnToMainMenu()
    {
        // Return to the main menu scene
        SceneManager.LoadScene("MainMenu");
    }
}
