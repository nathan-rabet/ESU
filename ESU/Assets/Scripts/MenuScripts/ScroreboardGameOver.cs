using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon;

public class ScroreboardGameOver : MonoBehaviour
{
    public List<GameObject> HUDDefJoueur;
    public List<GameObject> HUDAttJoueur;

    private List<Photon.Realtime.Player> DefJoueur = new List<Photon.Realtime.Player>();

    private List<Photon.Realtime.Player> AttJoueur = new List<Photon.Realtime.Player>();



    public void UpdateMe(List<Photon.Realtime.Player> AttJoueur, List<Photon.Realtime.Player> DefJoueur)
    {
            for (int i = 0; i < AttJoueur.Count; i++)
            {
                GameObject HUD = HUDAttJoueur[i];
                Photon.Realtime.Player player = AttJoueur[i];

                HUD.transform.GetChild(0).GetComponent<TMP_Text>().text = player.NickName;
                HUD.transform.GetChild(1).GetComponent<TMP_Text>().text = player.CustomProperties["Kill"].ToString();
                HUD.transform.GetChild(2).GetComponent<TMP_Text>().text = player.CustomProperties["Death"].ToString();
                HUD.transform.GetChild(3).GetComponent<TMP_Text>().text = player.CustomProperties["Class"].ToString();
                HUD.SetActive(true);
            }
            for (int i = 0; i < DefJoueur.Count; i++)
            {
                GameObject HUD = HUDDefJoueur[i];
                Photon.Realtime.Player player = DefJoueur[i];

                HUD.transform.GetChild(0).GetComponent<TMP_Text>().text = player.NickName;
                HUD.transform.GetChild(1).GetComponent<TMP_Text>().text = player.CustomProperties["Kill"].ToString();
                HUD.transform.GetChild(2).GetComponent<TMP_Text>().text = player.CustomProperties["Death"].ToString();
                HUD.transform.GetChild(3).GetComponent<TMP_Text>().text = player.CustomProperties["Class"].ToString();
                HUD.SetActive(true);
            }
        
    }
    public void showSB()
    {
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }
}
