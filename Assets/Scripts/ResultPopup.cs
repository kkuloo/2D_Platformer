using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ResultPopup : MonoBehaviour
{
    public GameObject highScoreLabel;
    public TextMeshProUGUI titleLabel;
    public TextMeshProUGUI scoreLabel;

    private void OnEnable()
    {
        Time.timeScale = 0;

        if (GameManager.Instance.isCleared)
        {
            float highScore = PlayerPrefs.GetFloat("HighScore", 0);

            if (highScore<GameManager.Instance.timeLimit)
            {
                highScoreLabel.SetActive(true);
                PlayerPrefs.SetFloat("HighScore", GameManager.Instance.timeLimit);
                PlayerPrefs.Save();
            }
            else
            {
                highScoreLabel.SetActive(false);
            }

            titleLabel.text = "Cleared!";
            scoreLabel.text = GameManager.Instance.timeLimit.ToString("#.##");
        }
        else
        {
            titleLabel.text = "Game Over...";
            scoreLabel.text = "";
        }
    }

    public void PlayAgainPressed() 
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("GameScene");
    }
}
