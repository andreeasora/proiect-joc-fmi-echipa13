using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Player player;

    private float cameraMinX, cameraMaxX, cameraMinY, cameraMaxY;

    void Start()
    {
        Camera cam = GetComponent<Camera>();
        cameraMinX = Player.minX + cam.orthographicSize * cam.aspect;
        cameraMaxX = Player.maxX - cam.orthographicSize * cam.aspect;
        cameraMinY = Player.minY + cam.orthographicSize;
        cameraMaxY = Player.maxY - cam.orthographicSize;
    }

    void LateUpdate()
    {
        var playerPos = player.transform.position;
        var newCameraPos = new Vector3(playerPos.x, playerPos.y, transform.position.z);
        if (newCameraPos.x < cameraMinX)
            newCameraPos.x = cameraMinX;
        else if (newCameraPos.x > cameraMaxX)
            newCameraPos.x = cameraMaxX;
        if (newCameraPos.y < cameraMinY)
            newCameraPos.y = cameraMinY;
        else if (newCameraPos.y > cameraMaxY)
            newCameraPos.y = cameraMaxY;
        transform.position = newCameraPos;
    }
}
