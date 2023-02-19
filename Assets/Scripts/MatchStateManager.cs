using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MatchStateManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    GameObject buttons;
    [SerializeField]
    TMP_Text scoreText, timerText, gameOverText, finalScore;

    int score = 0;
    float timeLeft = 100;
    bool gameOver = false;    

    void Start()
    {
        timeLeft = ConfigManager.Instance.GetMatchDuration();
    }

    void Update()
    {
        if (gameOver)
        {
            return;
        }
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                player.GetComponent<PlayerController>().DisableShip();
            }
            EndMatch();
        }
        else
        {
            timerText.text = ((int)(timeLeft / 60)).ToString() + ":" + ((int)(timeLeft % 60)).ToString("00");
        }
    }

    public void OnPlayAgain()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnGoToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void IncreaseScore(int points) { 
        score += points;
        scoreText.text = "Score: " + score.ToString();
    }

    public void EndMatch()
    {
        if (gameOver)
        {
            return;
        }
        gameOver = true;
        scoreText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        buttons.SetActive(true);
        gameOverText.gameObject.SetActive(true);
        finalScore.text = "Score: " + score.ToString();
        finalScore.gameObject.SetActive(true);
        buttons.transform.parent.gameObject.GetComponent<Image>().enabled = true;
       
    }
}
