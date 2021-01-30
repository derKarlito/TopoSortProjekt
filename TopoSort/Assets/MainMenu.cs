using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   public void StartGame()
   {
       SceneManager.LoadScene("Scene1"); //Load the Game Scene
   }
    public void CreditScene()
    {
        SceneManager.LoadScene("Scene2"); //Load the Credit Scene
    }
   public void ExitGame()
   {
       Application.Quit(); //Quit the Game
   }
}
