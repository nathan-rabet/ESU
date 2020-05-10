using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System;


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
        UpdateHUD();
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
        if (win == "DEF")
            GameObject.Find("/GAME/Menu/ScoreboardMenu").GetComponent<ScroreboardGameOver>().UpdateMe(loosers, winners);
        else
            GameObject.Find("/GAME/Menu/ScoreboardMenu").GetComponent<ScroreboardGameOver>().UpdateMe(winners, loosers);

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
            p.GetComponentInChildren<TextMesh>().text = winners[i].NickName;
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
            GameObject p = Instantiate(prefab, spawns[i].transform.position, spawns[i].transform.rotation);
            p.GetComponentInChildren<TextMesh>().text = loosers[i - 5].NickName;
        }
    }

    public void UpdateHUD()
    {
        int totalscore = score.Item1 + score.Item2;
        GameObject.Find("/HUD/Score/ATT").GetComponent<TMP_Text>().text = score.Item1.ToString();
        GameObject.Find("/HUD/Score/DEF").GetComponent<TMP_Text>().text = score.Item2.ToString();
        if (totalscore != 0)
            GameObject.Find("/HUD/Score/Background/Loading Bar").GetComponent<Image>().fillAmount = score.Item1 / totalscore;
    }

    void Start()
    {
            players = PhotonNetwork.PlayerList;
            GameStat game = GameObject.Find("/GAME/GameManager").GetComponent<GameStat>();
            score = new Tuple<int, int>(game.scoreAtt, game.scoreDEF);
            anim.SetTrigger("FadeOut");
    }
}
