using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon;
using Photon.Realtime;

public class GameStat : MonoBehaviour
{
    public int timeMin = 0;
    public int timeSec = 0;
    public int scoreAtt = 0;
    public int scoreDEF = 0;
    public TMP_Text timerHUD;
    public TMP_Text scoreATTHUD;
    public TMP_Text scoreDEFHUD;
    public PhotonView view;
    public GameObject GameStatPrafeb;

    [PunRPC]
    public void sendGamestat (int min, int sec, int scoreA, int scoreD)
    {
        StartCoroutine(UpdateSec(min, sec));
        
        scoreAtt = scoreA;
        scoreDEF = scoreD;
        scoreATTHUD.text = "" + scoreAtt;
        scoreDEFHUD.text = "" + scoreDEF;
    }
    [PunRPC]
    public void changeScoreRCP (int scoreA, int scoreD)
    {
        scoreAtt += scoreA;
        scoreDEF += scoreD;
        scoreATTHUD.text = "" + scoreAtt;
        scoreDEFHUD.text = "" + scoreDEF;
    }
    [PunRPC]
<<<<<<< HEAD
    public void GameOver()
    {
        GameObject game = Instantiate(GameStatPrafeb);
        game.GetComponent<GamesStatGameOver>().players = PhotonNetwork.PlayerList;
=======
    public void GameOver (Player[] playerlist)
    {
        GameObject game = Instantiate(GameStatPrafeb);
        game.GetComponent<GamesStatGameOver>().players = playerlist;
>>>>>>> 78ccc66ba71b00794b33edde8730ec9692518fd4
    }

    public void changeScore(int scoreA, int scoreD)
    {
        view.RPC("changeScoreRCP", RpcTarget.All, scoreA, scoreD);
    }

    public void SendToNewPlayer()
    {
        view.RPC("sendGamestat", RpcTarget.Others, timeMin, timeSec , scoreAtt, scoreDEF);
    }
    
    IEnumerator UpdateSec(int min, int sec)
    {
<<<<<<< HEAD
        timeMin = min;
=======
        timeMin = 1;
>>>>>>> 78ccc66ba71b00794b33edde8730ec9692518fd4
        timeSec = sec;
        while (timeMin>0 || timeSec>0)
        {
            timeSec--;
            if (timeSec <= -1)
            {
                timeSec = 59;
                if (timeMin>0)
                {
                    timeMin--;
                }
                else
                {
                    //Fin de partie
                }
            }

            //Affichage
            string timerSTR = "";
            if (timeMin<10)
            {
                timerSTR += "0"; 
            }
            timerSTR += timeMin + ":";
            if (timeSec<10)
            {
                timerSTR += "0";
            }
            timerSTR += timeSec;
            timerHUD.text = timerSTR;
            yield return new WaitForSeconds(1);
        }

        if (PhotonNetwork.IsMasterClient)
<<<<<<< HEAD
            view.RPC("GameOver", RpcTarget.All);
=======
            view.RPC("GameOver", RpcTarget.All, PhotonNetwork.PlayerList);
>>>>>>> 78ccc66ba71b00794b33edde8730ec9692518fd4
    }
}
