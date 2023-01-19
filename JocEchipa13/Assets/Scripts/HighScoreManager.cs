using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HighScoreManager : MonoBehaviour
{
    [SerializeField] TMP_Text highScoreText;
    public int currentHighScore;

    void Start()
    {
        currentHighScore = PlayerPrefs.GetInt("highscore", 0);
        highScoreText.text = "High Score: " + currentHighScore;
    }

    public void SaveHighScore(int newScore)
    {
        if (newScore > currentHighScore)
        {
            currentHighScore = newScore;
            PlayerPrefs.SetInt("highscore", currentHighScore);
            PlayerPrefs.Save();
            highScoreText.text = "High Score: " + currentHighScore;
        }
    }
}
