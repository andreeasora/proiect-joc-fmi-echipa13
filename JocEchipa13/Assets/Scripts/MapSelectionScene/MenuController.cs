using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void OnClickMap1()
    {
        GameController.MapSceneName = "Map_1";
        SceneManager.LoadScene("GameCore", LoadSceneMode.Single);
    }
}
