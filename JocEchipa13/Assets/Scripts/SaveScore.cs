using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ScoreboardLogic.Scoreboards;

public class SaveScore : MonoBehaviour
{
    public InputField usernameInput;
    public bool alreadyName;
    private string user;	  
    private ScoreboardEntryData EntryData = new ScoreboardEntryData();
   


    public void saveScore()
    {
	EntryData.entryName = "user";
	EntryData.entryScore = 4;
	//Scoreboard scoreboard = FindObjectOfType<Scoreboard>();
	//scoreboard.AddEntry(EntryData); 		
	SceneManager.LoadScene("LoseScreen", LoadSceneMode.Single);
    }
}
