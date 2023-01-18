using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    void Update()
    {
        // Weapon facing the mouse pointer
        var mousePos = Mouse.current.position.ReadValue();
        var playerPos = Camera.main.WorldToScreenPoint(transform.position);
        var playerToMouse = new Vector3(mousePos.x - playerPos.x, mousePos.y - playerPos.y, transform.up.z);
        float anglesToRotate = Mathf.Atan2(playerToMouse.y, playerToMouse.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, anglesToRotate);
    }

    
}
