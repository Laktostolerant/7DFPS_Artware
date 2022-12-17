using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Scenemanager : MonoBehaviour
{
    // to load the next scene
    [SerializeField] int sceneToLoad;

    public void ButtonClick()
    {
        SceneManager.LoadScene(sceneToLoad);

    }
    public void Quitgame()
    {
        Application.Quit();
    }

}
