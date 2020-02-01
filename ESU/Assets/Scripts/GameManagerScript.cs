using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class GameManagerScript : MonoBehaviour
{
    #region DefVariable
        
        private int nbDefPlayer = 0;
        private int nbAttPlayer = 0;

        public TMP_Text DispDefPlayer;
        public TMP_Text DispAttPlayer;

        private string myTeam = null;
        private string myClass = null;
        
    #endregion
    #region UI
        public GameObject teamMenuUI;
        public GameObject pauseMenuUI;
        public GameObject connectionMenuUI;
        public GameObject infosMenuUI;
        private bool showInfos = false;
        public TMP_Text FPS;
    #endregion



    // Start is called before the first frame update
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
    }

    public void IsConnected ()
    {
        connectionMenuUI.SetActive(false);
        teamMenuUI.SetActive(true);
    }
    // Update is called once per frame
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
        if (showInfos)
        {
            FPS.text = "FPS: " + ((int)(1.0f / Time.smoothDeltaTime)).ToString() + "\nPing: " + (PhotonNetwork.GetPing()).ToString() + "\nClientState: " +PhotonNetwork.NetworkClientState.ToString();
        }
    }

    public void AddDefPlayer()
    {
        if (nbDefPlayer<10 && myTeam==null) {
            nbDefPlayer++;
            myTeam = "DEF";
            DispDefPlayer.text = "Joueurs: " + nbDefPlayer;

            teamMenuUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void AddAttPlayer()
    {
        if (nbAttPlayer<10 && myTeam==null) {
            nbAttPlayer++;
            myTeam = "ATT";
            DispAttPlayer.text = "Joueurs: " + nbAttPlayer;

            teamMenuUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
