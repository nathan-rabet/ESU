using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
    #region DefVariable
        
        private int nbDefPlayer = 0;
        private int nbAttPlayer = 0;

        public TMP_Text DispDefPlayer;
        public TMP_Text DispAttPlayer;

        private string myTeam = null;
        private string myClass = null;

        public GameObject teamMenuUI;
        public GameObject PauseMenuUI;
        
    #endregion



    // Start is called before the first frame update
    void Start()
    {
        DispDefPlayer.text = "Joueurs: 0";
        DispAttPlayer.text = "Joueurs: 0";
        

        teamMenuUI.SetActive(true);
        PauseMenuUI.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        
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
