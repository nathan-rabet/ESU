using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManagerScript : MonoBehaviour
{
    #region DefVariable

        string StadeGame = "CONNECTION";
        
        private int nbDefPlayer = 0;
        private int nbAttPlayer = 0;

        public TMP_Text DispDefPlayer;
        public TMP_Text DispAttPlayer;
        private string myClass = null;

        public PhotonView view;
        
    #endregion
    #region UI
        public GameObject teamMenuUI;
        public GameObject pauseMenuUI;
        public GameObject connectionMenuUI;
        public GameObject scoreboardMenu;
        public GameObject infosMenuUI;
        public GameObject InGameHUD;
        public GameObject DeathHUD;
        private bool showInfos = false;
        public TMP_Text FPS;
    #endregion



    #region Connection
    void Start()
    {
        DispDefPlayer.text = "Joueurs: 0";
        DispAttPlayer.text = "Joueurs: 0";
        
        connectionMenuUI.SetActive(true);
        teamMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        infosMenuUI.SetActive(false);
        InGameHUD.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Hashtable hash = new Hashtable();
        hash.Add("Team", "");
        hash.Add("Kill", 0);
        hash.Add("Death", 0);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public void IsConnected ()
    {
        connectionMenuUI.SetActive(false);
        teamMenuUI.SetActive(true);
        StadeGame = "EQUIPE";
    }
    
    #endregion
    #region updateUI
    void Update()
    {
        //Info FPS
        if (Input.GetKeyDown("f3"))
        {
            if (showInfos)
            {
                infosMenuUI.SetActive(false);
                showInfos = false;
            }
            else
            {
                infosMenuUI.SetActive(true);
                showInfos = true;
            }
        }
        if (showInfos)
        {
            FPS.text = "FPS: " + ((int)(1.0f / Time.smoothDeltaTime)).ToString() + "\nPing: " + (PhotonNetwork.GetPing()).ToString() + "\nClientState: " +PhotonNetwork.NetworkClientState.ToString()
            + "\nAttPlayers: " + nbAttPlayer + "\nDefPlayers: " + nbDefPlayer + "\nMyTeam: " + PhotonNetwork.LocalPlayer.CustomProperties["Team"];
        }

        switch(StadeGame)
        {
            case "INGAME":
            //Menu Echap
            if (Input.GetKeyDown("escape"))
            {
                if (pauseMenuUI.active)
                {
                    pauseMenuUI.SetActive(false);
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                else
                {
                    pauseMenuUI.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }
            //Tableau des score
            if (Input.GetKeyDown("tab"))
            {
                scoreboardMenu.SetActive(true);
            }
            if (Input.GetKeyUp("tab"))
            {
                scoreboardMenu.SetActive(false);
            }
            break;

            case "EN_ATT_REAPARITION":
            if (Input.anyKey)
            {
                DeathHUD.SetActive(false);
                foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player"))
                {
                    p.GetComponent<Player_Manager>().DestroyMe();
                }
                GameObject.Find("/GAME/PunManager").GetComponent<PunScript>().SpawnPlayer();
                InGameHUD.SetActive(true);
                StadeGame = "INGAME";
            }
            break;
        }
        
    }
    #endregion
    #region PlayerGestion
    public void AddDefPlayer()
    {
        if (nbDefPlayer<10 && PhotonNetwork.LocalPlayer.CustomProperties["Team"]=="") 
        {
            //Envoie RPC
            nbDefPlayer++;
            view.RPC ("NumberDef", RpcTarget.Others, nbDefPlayer);

            //Update HUD
            DispDefPlayer.text = "Joueurs: " + nbDefPlayer;
            teamMenuUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            InGameHUD.SetActive(true);
            
            //Mise a jour de la hashtable
            Hashtable hash = new Hashtable();
            hash.Add("Team", "DEF");
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

            //Mise a jour du statue de la partie
            StadeGame = "INGAME";
        }
    }

    public void AddAttPlayer()
    {
        if (nbAttPlayer<10 && PhotonNetwork.LocalPlayer.CustomProperties["Team"]=="") {
            //Envoie RPC
            nbAttPlayer++;
            view.RPC ("NumberAtt", RpcTarget.Others, nbAttPlayer);

            //Update HUD
            DispAttPlayer.text = "Joueurs: " + nbAttPlayer;
            teamMenuUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            InGameHUD.SetActive(true);

            //Mise a jour de la hashtable
            Hashtable hash = new Hashtable();
            hash.Add("Team", "ATT");
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

            //Mise a jour du statue de la partie
            StadeGame = "INGAME";
        }
    }
    #endregion
    #region PhotonPun
        public void SendToNewPlayer ()
        {
            if (PhotonNetwork.IsMasterClient)
                {
                    Debug.Log("Envoie du nombre de perso par Team" + nbAttPlayer + " " + nbDefPlayer );
                    view.RPC ("NumberAtt", RpcTarget.Others, nbAttPlayer);
                    view.RPC ("NumberDef", RpcTarget.Others, nbDefPlayer);
                }
        }
    #endregion
    #region PunRPC
        [PunRPC]
        void NumberAtt (int number)
        {
            nbAttPlayer = number;
            DispAttPlayer.text = "Joueurs: " + number;
        }

        [PunRPC]
        void NumberDef (int number)
        {
            nbDefPlayer = number;
            DispDefPlayer.text = "Joueurs: " + number;
        }
    #endregion

    //HUD de mort
    public void HUDMort(string Killer, int respTime)
    {
        StadeGame = "MORT";
        InGameHUD.SetActive(false);
        DeathHUD.SetActive(true);
        TMP_Text MDM = DeathHUD.transform.Find("Message de Mort").GetComponent<TMP_Text>();
        MDM.text = "Vous êtes mort par " + Killer;

        StartCoroutine(CompteRebours(respTime));
    }

    //Compte a rebours
    IEnumerator CompteRebours(int respTime)
        {
            TMP_Text Respawntime = DeathHUD.transform.Find("RespawnTime").GetComponent<TMP_Text>();
            Respawntime.text = "Réapparaître: " + respTime;

            while (respTime>0)
            {
                yield return new WaitForSeconds(1);
                respTime--;
                Respawntime.text = "Réapparaître: " + respTime;
            }

            Respawntime.text = "Appuyer sur une touche pour réapparaître";
            StadeGame = "EN_ATT_REAPARITION";
        }
}
