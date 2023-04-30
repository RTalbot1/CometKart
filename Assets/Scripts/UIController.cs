
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;
public class UIController : MonoBehaviour
{
    public Button startButton;
    public TMP_Text lable;
    public GameObject mainMenu;
    public GameObject quit;
    public TMP_Text countdownText;
    public float countdownDuration = 3f;
    private GameManager gameManager;
    void Start()
    {
        // Get a reference to the GameManager script
        gameManager = FindObjectOfType<GameManager>();
        countdownText.gameObject.SetActive(false);
        lable.gameObject.SetActive(false);
        mainMenu.SetActive(false);
        quit.SetActive(false);
    }
    void Update()
    {
       if(gameManager.raceFinished)
       {
            lable.gameObject.SetActive(true);
            int p = gameManager.playerPos;
            Debug.Log(p);
            if(p == 5)
            {
                lable.text = "You placed first!";
            }
            else if(p == 6)
            {
                lable.text = "You placed second!";
            }
            else if(p == 7)
            {
                lable.text = "You placed third!";
            }
           else if(p == 8)
            {
                lable.text = "You placed forth!";
            }
            mainMenu.SetActive(true);
            quit.SetActive(true);
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
        gameManager.StartRace();
    }
    public void QuitGame() {  
       Application.Quit();
    }  
    public void ReturnToMainMenu()
    {
        // Return to the main menu scene
        SceneManager.LoadScene("MainMenu");
    }
}
