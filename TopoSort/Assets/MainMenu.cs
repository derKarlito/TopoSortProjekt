using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject CreditsCanvas; //Class Variable for showing/hiding Credits
   public void StartGame()
   {
       SceneManager.LoadScene("Scene1"); //Load the Game Scene
   }
   public void CreditScene()
    {
        CreditsCanvas.SetActive(true); //Show Credits
    }
    public void BackToMenu()
    {
        CreditsCanvas.SetActive(false); //Hide Credits
    }
   public void ExitGame()
   {
       Application.Quit(); //Quit the Game
   }
}
