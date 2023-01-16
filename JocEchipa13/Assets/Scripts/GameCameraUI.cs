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
    [SerializeField] private Image usedWeapon;
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private Sprite weaponGreen, weaponRed;

    int lastFilledHeartIdx;

    void Start()
    {
        lastFilledHeartIdx = Player.maxLives - 1;
        playerScoreValueText.text = "0";
        
        UpdateUsedWeapon();

        player.onScoreUpdate += UpdateScoreValue;
        player.onLifeLost += RemoveHeart;
        player.onAmmoUpdate += UpdateAmmo;
        player.onWeaponChange += UpdateUsedWeapon;
    }

    void RemoveHeart() => hearts[lastFilledHeartIdx--].sprite = emptyHeartSprite;

    void UpdateScoreValue() => playerScoreValueText.text = player.Score.ToString();

    void UpdateUsedWeapon()
    {
        if (player.WeaponType == 1)
        {
            usedWeapon.sprite = weaponGreen;
            ammoText.text = "INF";
        }
        else
        {
            usedWeapon.sprite = weaponRed;
            ammoText.text = player.Weapon2Ammo.ToString() + " / " + Player.maxWeapon2Ammo.ToString();
        }
    }

    void UpdateAmmo()
    {
        if (player.WeaponType == 2)
            ammoText.text = player.Weapon2Ammo.ToString() + " / " + Player.maxWeapon2Ammo.ToString();
    }
}
