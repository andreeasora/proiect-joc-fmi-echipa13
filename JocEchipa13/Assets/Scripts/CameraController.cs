using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Player player;

    private Camera cam;

    private float cameraMinX;
    private float cameraMaxX;
    private float cameraMinY;
    private float cameraMaxY;

    void Start()
    {
        cam = GetComponent<Camera>();

        cameraMinX = -30.0f + cam.orthographicSize * cam.aspect;
        cameraMaxX = 30.0f - cam.orthographicSize * cam.aspect;
        cameraMinY = -30.0f + cam.orthographicSize;
        cameraMaxY = 30.0f - cam.orthographicSize;
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
