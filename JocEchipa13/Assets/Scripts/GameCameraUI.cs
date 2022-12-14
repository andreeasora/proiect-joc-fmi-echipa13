using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameCameraUI : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] TMP_Text playerScoreValueText;
    [SerializeField] Image[] hearts;
    [SerializeField] Sprite emptyHeartSprite;

    int lastFilledHeartIdx;

    void Start()
    {
        lastFilledHeartIdx = Player.maxLives - 1;
        playerScoreValueText.text = "0";
        player.onScoreUpdate += UpdateScoreValue;
        player.onLifeLost += RemoveHeart;
    }

    void RemoveHeart() => hearts[lastFilledHeartIdx--].sprite = emptyHeartSprite;

    void UpdateScoreValue() => playerScoreValueText.text = player.Score.ToString();
}
