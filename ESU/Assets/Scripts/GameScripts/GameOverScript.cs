using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("GameStat").GetComponent<GamesStatGameOver>().OnSceneLoaded();
    }

    public void LeaveGameOver()
    {
        SceneManager.LoadScene(0);
    }
}
