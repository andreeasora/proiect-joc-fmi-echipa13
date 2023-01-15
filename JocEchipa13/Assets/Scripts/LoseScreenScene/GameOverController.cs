using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameOverController : MonoBehaviour
{  		
    public void LoadGame()
    {
	SceneManager.LoadScene("GameCore", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
	SceneManager.LoadScene("MapSelection", LoadSceneMode.Single);
    }
    public void saveScore()
    {		
	SceneManager.LoadScene("SaveScore", LoadSceneMode.Single);
    }
	
				 		 
}
