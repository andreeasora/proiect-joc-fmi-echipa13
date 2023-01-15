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
    private string user="Guest";	  
    private Transform playerObj;

    public void readName(string us)
    {
	user = us;
	Debug.Log(user);
    }	
    public void saveScore()
    {	
 	  int currentScore = PlayerPrefs.GetInt("score", 0);
        ScoreboardEntryData EntryData = new ScoreboardEntryData();
        EntryData.entryName = user;
	  EntryData.entryScore = currentScore;
	  if(currentScore != 0){
		Scoreboard scoreboard = new Scoreboard(); 
	  	scoreboard.AddEntry(EntryData);
	  }
	  		
	  SceneManager.LoadScene("LoseScreen", LoadSceneMode.Single);
    }
}
