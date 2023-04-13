using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Scoreboard : MonoBehaviour {

    public Text playerScoreText;
    public Text[] botScoreTexts;

    private int playerScore = 0;
    private Dictionary<string, int> botScores = new Dictionary<string, int>();

    void Start () {
        // Initialize the bot scores to 0
        foreach (Text botScoreText in botScoreTexts) {
            botScores[botScoreText.name] = 0;
        }
    }

    public void AddPointToPlayer() {
        playerScoreText.text = "Player Score: " + playerScore;
    }

    public void AddPointToBot(string botName) {
        botScoreTexts[GetBotIndex(botName)].text = botName + " Score: " + botScores[botName];
    }

    private int GetBotIndex(string botName) {
        // Returns the index of the bot with the given name in botScoreTexts
        for (int i = 0; i < botScoreTexts.Length; i++) {
            if (botScoreTexts[i].name == botName) {
                return i;
            }
        }
        return -1; // Bot name not found
    }

}
