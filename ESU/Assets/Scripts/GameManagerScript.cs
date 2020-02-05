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
    }
    
    #endregion
    #region updateUI
    void Update()
    {
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
        if (Input.GetKeyDown("tab"))
        {
            scoreboardMenu.SetActive(true);
        }
        if (Input.GetKeyUp("tab"))
        {
            scoreboardMenu.SetActive(false);
        }

        if (showInfos)
        {
            FPS.text = "FPS: " + ((int)(1.0f / Time.smoothDeltaTime)).ToString() + "\nPing: " + (PhotonNetwork.GetPing()).ToString() + "\nClientState: " +PhotonNetwork.NetworkClientState.ToString()
            + "\nAttPlayers: " + nbAttPlayer + "\nDefPlayers: " + nbDefPlayer + "\nMyTeam: " + PhotonNetwork.LocalPlayer.CustomProperties["Team"];
        }
    }
    #endregion
    #region PlayerGestion
    public void AddDefPlayer()
    {
        if (nbDefPlayer<10 && PhotonNetwork.LocalPlayer.CustomProperties["Team"]=="") {
            nbDefPlayer++;
            view.RPC ("NumberDef", RpcTarget.Others, nbDefPlayer);
            DispDefPlayer.text = "Joueurs: " + nbDefPlayer;

            teamMenuUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Hashtable hash = new Hashtable();
            hash.Add("Team", "DEF");
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public void AddAttPlayer()
    {
        if (nbAttPlayer<10 && PhotonNetwork.LocalPlayer.CustomProperties["Team"]=="") {
            nbAttPlayer++;
            view.RPC ("NumberAtt", RpcTarget.Others, nbAttPlayer);
            DispAttPlayer.text = "Joueurs: " + nbAttPlayer;

            teamMenuUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Hashtable hash = new Hashtable();
            hash.Add("Team", "ATT");
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
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
}
