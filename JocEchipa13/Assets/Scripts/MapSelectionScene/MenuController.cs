using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void OnClickHowToPlay()
    {
        SceneManager.LoadScene("HowToPlay", LoadSceneMode.Single);
    }

    public void OnClickMap1()
    {
        GameController.MapSceneName = "Map_1";
        SceneManager.LoadScene("GameCore", LoadSceneMode.Single);
    }

    public void OnClickMap2()
    {
        GameController.MapSceneName = "Map_2";
        SceneManager.LoadScene("GameCore", LoadSceneMode.Single);
    }
    public void OnClickScoreboard()
    {
        SceneManager.LoadScene("Scoreboard", LoadSceneMode.Single);
    }
	

    public void OnClickOptions()
    {
        SceneManager.LoadScene("Options", LoadSceneMode.Single);
    }
}
