using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private int MapIndex = 1;
    public void QuitGame ()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    public void ChangeMap()
    {
        if (MapIndex == 1)
            MapIndex = 2;
        else
            MapIndex = 1;
    }

    public void JoinMultiplayerGame ()
    {
        SceneManager.LoadScene(MapIndex);
    }
}
