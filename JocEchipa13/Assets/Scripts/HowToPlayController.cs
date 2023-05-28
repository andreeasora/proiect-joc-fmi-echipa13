using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToPlayController : MonoBehaviour
{
    public GameObject firstPage;
    public GameObject secondPage;
    public GameObject thirdPage;
    public GameObject fourthPage;


    public void BackToMenu()
    {
        SceneManager.LoadScene("MapSelection", LoadSceneMode.Single);
    }
    public void GoToSecondPageNext()
    {
        secondPage.SetActive(true);
        firstPage.SetActive(false);
    }
    public void GoToFirstPagePrevious()
    {
        secondPage.SetActive(false);
        firstPage.SetActive(true);
    }
    public void GoToThirdPageNext()
    {
        secondPage.SetActive(false);
        thirdPage.SetActive(true);
    }
    public void GoToSecondPagePrevious()
    {
        secondPage.SetActive(true);
        thirdPage.SetActive(false);
    }
    public void GoToFourthPageNext()
    {
        thirdPage.SetActive(false);
        fourthPage.SetActive(true);
    }
    public void GoToThirdPagePrevious()
    {
        thirdPage.SetActive(true);
        fourthPage.SetActive(false);
    }

}
