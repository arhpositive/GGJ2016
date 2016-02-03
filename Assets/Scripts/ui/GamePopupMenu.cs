using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.ui
{
    public class GamePopupMenu : MonoBehaviour
    {
        public GameObject _creditsPanel;

        public void FinalizeGame(bool isTied, bool leftPlayerWins, bool isTimedVictory, Color textColor)
        {
            Time.timeScale = 0.0f;
            gameObject.SetActive(true);
            Text[] allText = gameObject.GetComponentsInChildren<Text>();

            foreach (Text scoreText in allText)
            {
                if (scoreText.gameObject.tag == "PopupText")
                {
                    scoreText.color = textColor;
                    if (isTied)
                    {
                        scoreText.text = "Game is tied!";
                    }
                    else
                    {
                        if (leftPlayerWins)
                        {
                            scoreText.text = "Red wins!\n";
                        }
                        else
                        {
                            scoreText.text = "Blue wins!\n";
                        }

                        if (isTimedVictory)
                        {
                            scoreText.text += "Timed Victory!";
                        }
                        else
                        {
                            scoreText.text += "Domination!";
                        }
                    }
                }
            }
        }

        public void ShowCredits()
        {
            _creditsPanel.SetActive(true);
            gameObject.SetActive(false);
        }

        public void RestartGame()
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene(0);
        }

    }

}
