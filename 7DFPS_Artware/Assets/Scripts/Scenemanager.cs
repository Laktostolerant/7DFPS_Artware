using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;




public class Scenechanger : MonoBehaviour
{
    // to load the next scene

    public void ButtonClick()
    {
        SceneManager.LoadScene(1);

    }
    public void Quitgame()
    {
        Application.Quit();
    }

}
