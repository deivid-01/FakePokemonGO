using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIGame : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject GO_menuOptions;

    [Space]
    [Header("Scenes ")]
    public string pokedexScene;
    public string previousScene;

 

    public void ExitGame()
    {
        Application.Quit();
        StopAllCoroutines();
    }

    public void LogOut()
    {
        SceneManager.LoadScene(previousScene);

    }

    public void DisplayMenu()
    {
        if (!GO_menuOptions.activeInHierarchy)
        {
            GO_menuOptions.SetActive(true);
        }
    }

    public void HideMenu()
    {
        if (GO_menuOptions.activeInHierarchy)
        {
            GO_menuOptions.SetActive(false);
        }
    }
    public void DisplayPokedex()
    {
        
       SceneManager.LoadScene(pokedexScene);
    }
}
