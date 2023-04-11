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
    [SerializeField] private Sprite weaponGreen, weaponRed, weaponPurple;

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
        switch (player.WeaponType)
        {
            case 0:
                usedWeapon.sprite = weaponGreen;
                ammoText.text = "INF";
                break;
            case 1:
                usedWeapon.sprite = weaponRed;
                ammoText.text = player.Weapon2Ammo.ToString() + " / " + Player.maxWeapon2Ammo.ToString();
                break;
            case 2:
                usedWeapon.sprite = weaponPurple;
                ammoText.text = (player.Weapon3Ready ? "1 / 1" : "0 / 1");
                break;
        }
    }

    void UpdateAmmo()
    {
        if (player.WeaponType == 1)
            ammoText.text = player.Weapon2Ammo.ToString() + " / " + Player.maxWeapon2Ammo.ToString();
        else if (player.WeaponType == 2)
            ammoText.text = (player.Weapon3Ready ? "1 / 1" : "0 / 1");
    }
}
