using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Michsky.UI.ModernUIPack;
using System;
using System.Runtime.CompilerServices;

public class GamesStatGameOver : MonoBehaviour
{
    private Player[] players;
    private Tuple<int, int> score;

    public GameObject[] PlayersPrefab;
    private GameObject[] spawns;

    public Animator anim;
    private bool loadScene = true;

    public void goGameOverScene()
    {
        SceneManager.LoadScene(3);
        anim.SetTrigger("FadeIn");
    }
    
    //On GameOverRoom Load
    public void OnSceneLoaded()
    {
        loadScene = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PhotonNetwork.LeaveRoom();
        spawns = GameObject.FindGameObjectsWithTag("Respawn");
        PlacePrefab();
        Destroy(this.gameObject);
    }

    private void PlacePrefab()
    {
        List<Player> winners = new List<Player>();
        List<Player> loosers = new List<Player>();

        string win = "DEF";
        if (score.Item1 > score.Item2)
            win = "ATT";

        foreach (Player player in players)
        {
            if ((string)player.CustomProperties["Team"] == win)
                winners.Add(player);
            else
                loosers.Add(player);
        }

        for (int i = 0; i < winners.Count; i++)
        {
            for (int j = 0; j < i; j++)
            {
                if ((int)winners[j + 1].CustomProperties["Kill"] < (int)winners[j].CustomProperties["Kill"])
                {
                    Player swap = winners[j + 1];
                    winners[j + 1] = winners[j];
                    winners[j] = swap;
                }
            }
        }

        for (int i = 0; i < winners.Count; i++)
        {
            GameObject prefab = PlayersPrefab[0];
            switch ((string)winners[i].CustomProperties["Class"])
            {
                case "Policier":
                    prefab = PlayersPrefab[0];
                    break;
                case "Pompier":
                    prefab = PlayersPrefab[1];
                    break;
                case "Medecin":
                    prefab = PlayersPrefab[2];
                    break;
                case "Mercenaire":
                    prefab = PlayersPrefab[3];
                    break;
                case "Pyroman":
                    prefab = PlayersPrefab[4];
                    break;
                case "Drogueur":
                    prefab = PlayersPrefab[5];
                    break;
            }
            GameObject p = Instantiate(prefab, spawns[i].transform.position, spawns[i].transform.rotation);
            p.GetComponent<Animator>().SetBool("victory", true);
        }

        for (int i = 5; i < loosers.Count + 5; i++)
        {
            GameObject prefab = PlayersPrefab[0];
            switch ((string)loosers[i-5].CustomProperties["Class"])
            {
                case "Policier":
                    prefab = PlayersPrefab[0];
                    break;
                case "Pompier":
                    prefab = PlayersPrefab[1];
                    break;
                case "Medecin":
                    prefab = PlayersPrefab[2];
                    break;
                case "Mercenaire":
                    prefab = PlayersPrefab[3];
                    break;
                case "Pyroman":
                    prefab = PlayersPrefab[4];
                    break;
                case "Drogueur":
                    prefab = PlayersPrefab[5];
                    break;
            }
            Instantiate(prefab, spawns[i].transform.position, spawns[i].transform.rotation);
        }
    }

    void Start()
    {
            players = PhotonNetwork.PlayerList;
            GameStat game = GameObject.Find("/GAME/GameManager").GetComponent<GameStat>();
            score = new Tuple<int, int>(game.scoreAtt, game.scoreDEF);
            anim.SetTrigger("FadeOut");
    }
}
