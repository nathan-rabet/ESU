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

public class GamesStatGameOver : MonoBehaviour
{
    private Player[] players;
    private Tuple<int, int> score;

    public Animator anim;

    public void goGameOverScene()
    {
        SceneManager.LoadScene(0);
        anim.SetTrigger("FadeIn");
    }

    public void DestroyMe()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PhotonNetwork.LeaveRoom();
        Destroy(this.gameObject);
    }

    void Start()
    {
        players = PhotonNetwork.PlayerList;
        GameStat game = GameObject.Find("/GAME/GameManager").GetComponent<GameStat>();
        score = new Tuple<int, int>(game.scoreAtt, game.scoreDEF);
        anim.SetTrigger("FadeOut");
    }
}
