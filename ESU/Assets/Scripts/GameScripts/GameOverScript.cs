using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    private GameObject gameStat;
    void Start()
    {
        gameStat = GameObject.FindGameObjectWithTag("GameStat");
        gameStat.GetComponent<GamesStatGameOver>().OnSceneLoaded();
    }

    public void LeaveGameOver()
    {
        Destroy(gameStat);
        SceneManager.LoadScene(0);
    }
}
