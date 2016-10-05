using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Hud : MonoBehaviour {

    public static int score = 0;
    public static float intermission = 10;
    public static int waveNum = 0;
    public static int escaped = 10;
    public static int budget = 10000;

    public Text scoreText;
    public Text intermissionText;
    public Text waveText;
    public Text escapedText;
    public Text gameOverText;
    public Text budgetText;

	void Awake()
    {
        scoreText.text = "Score: " + score;
        waveText.text = "Wave: " + waveNum;
        escapedText.text = "Health: " + escaped;
        budgetText.text = "Money: $" + budget;
    }

    void Update()
    {
        if (escaped == 0)
        {
            scoreText.text = "";
            waveText.text = "";
            escapedText.text = "";
            intermissionText.text = "";
            budgetText.text = "";

            gameOverText.text = "Game Over!\nClick to Restart!";
        }
        else
        {
            scoreText.text = "Score: " + score;
            waveText.text = "Wave: " + waveNum;
            escapedText.text = "Health: " + escaped;
            budgetText.text = "Money: $" + budget;
            gameOverText.text = "";

            if (intermission > 0)
            {
                intermissionText.text = "Next wave in:\n" + Mathf.Round(intermission);
                intermission -= Time.deltaTime;
            }
            else
                intermissionText.text = "";
        }
    }
}
