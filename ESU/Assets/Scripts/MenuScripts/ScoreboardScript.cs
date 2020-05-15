using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon;

public class ScoreboardScript : MonoBehaviour
{
    public List<GameObject> HUDDefJoueur;
    public List<GameObject> HUDAttJoueur;

    private List<Photon.Realtime.Player> DefJoueur = new List<Photon.Realtime.Player>();

    private List<Photon.Realtime.Player> AttJoueur = new List<Photon.Realtime.Player>();
    
    void Start()
    {
        foreach (GameObject HUD in HUDDefJoueur)
        {
            HUD.SetActive(false);
        }
        foreach (GameObject HUD in HUDAttJoueur)
        {
            HUD.SetActive(false);
        }
    }


    void Update()
    {
        Photon.Realtime.Player[] playerList = PhotonNetwork.PlayerList;
        if (Input.GetKey("tab"))
        {
            DefJoueur = new List<Photon.Realtime.Player>();
            AttJoueur = new List<Photon.Realtime.Player>();
            foreach (GameObject HUD in HUDDefJoueur)
            {
                HUD.SetActive(false);
            }
            foreach (GameObject HUD in HUDAttJoueur)
            {
                HUD.SetActive(false);
            }
            foreach (Photon.Realtime.Player player in playerList)
            {
                if ((string)player.CustomProperties["Team"] == "ATT")
                {
                    AttJoueur.Add(player);
                }
                else
                {
                    DefJoueur.Add(player);
                }
            }
            for (int i = 0; i < AttJoueur.Count; i++)
            {
                GameObject HUD = HUDAttJoueur[i];
                Photon.Realtime.Player player = AttJoueur[i];

                HUD.transform.GetChild(0).GetComponent<TMP_Text>().text = player.NickName;
                HUD.transform.GetChild(1).GetComponent<TMP_Text>().text = player.CustomProperties["Kill"].ToString();
                HUD.transform.GetChild(2).GetComponent<TMP_Text>().text = player.CustomProperties["Death"].ToString();
                HUD.transform.GetChild(3).GetComponent<TMP_Text>().text = player.CustomProperties["Class"].ToString();
                HUDAttJoueur[i].SetActive(true);
            }
            for (int i = 0; i < DefJoueur.Count; i++)
            {
                GameObject HUD = HUDDefJoueur[i];
                Photon.Realtime.Player player = DefJoueur[i];

                HUD.transform.GetChild(0).GetComponent<TMP_Text>().text = player.NickName;
                HUD.transform.GetChild(1).GetComponent<TMP_Text>().text = player.CustomProperties["Kill"].ToString();
                HUD.transform.GetChild(2).GetComponent<TMP_Text>().text = player.CustomProperties["Death"].ToString();
                HUD.transform.GetChild(3).GetComponent<TMP_Text>().text = player.CustomProperties["Class"].ToString();
                HUDDefJoueur[i].SetActive(true);
            }
        }
    }
}
