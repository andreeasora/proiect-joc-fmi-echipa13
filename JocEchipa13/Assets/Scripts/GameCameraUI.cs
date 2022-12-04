using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameCameraUI : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] TMP_Text playerScoreValueText;

    void Start()
    {
        playerScoreValueText.text = "0";
        player.onScoreUpdate += UpdateScoreValue;
    }
    
    void UpdateScoreValue() => playerScoreValueText.text = player.Score.ToString();
}
