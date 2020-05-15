using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;

//
// Ce script se concentre sur l'affichage des Canvas/HUD durant les différent
// états de la partie.
//
// PS: Vincent c'est ici ou tu peut gérer HUD en cours de partie (vie, inventaire, etc...)

public class GameManagerScript : MonoBehaviour
{
    #region DefVariable

        public string StadeGame = "CONNECTION"; //Variable sur l'état de la partie. 
                                                //Elle permet de savoir si le joueur choisit son équipe ou classe.
                                                //Si il en jeu en vie ou mort ect...
                                                //Elle permet d'éviter que le joueur est la possibilité d'activé
                                                //des interfaces et donc de les superposé.
                                                //(Par exemple le tableau des score pendant le choix de classe)
        
        private int nbDefPlayer = 0; //Variable sur le nombre de Def (A changé c'est de la merde)
        private int nbAttPlayer = 0; //Variable sur le nombre de Att (A changé c'est de la merde)

        public TMP_Text DispDefPlayer; //Recup du text sur le nombre de déf a afficher
        public TMP_Text DispAttPlayer; //Recup du text sur le nombre de att a afficher

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
        public GameObject ClassDefMenu;
        public GameObject ClassAttMenu;
        public GameObject GameHUD;
        public GameObject mainplayer;
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
        ClassDefMenu.SetActive(false);
        GameHUD.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Hashtable hash = new Hashtable();
        hash.Add("Team", "");
        hash.Add("Class", "");
        hash.Add("Kill", 0);
        hash.Add("Death", 0);
        hash.Add("Weapon", 0);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public void IsConnected ()
    {
        connectionMenuUI.SetActive(false);
        teamMenuUI.SetActive(true);
        GameHUD.SetActive(true);
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
            + "\nAttPlayers: " + nbAttPlayer + "\nDefPlayers: " + nbDefPlayer + "\nMyTeam: " + PhotonNetwork.LocalPlayer.CustomProperties["Team"] + "\nMyClass: " + PhotonNetwork.LocalPlayer.CustomProperties["Class"]
            + "\nWeapon: " + PhotonNetwork.LocalPlayer.CustomProperties["Weapon"];
        }

        switch(StadeGame)
        {
            case "INGAME":
            //Menu Echap
            if (Input.GetKeyDown("escape"))
            {
                pauseMenuUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                StadeGame = "PAUSE";
            }
            //Tableau des score
            if (Input.GetKeyDown("tab"))
            {
                scoreboardMenu.SetActive(true);
                StadeGame = "TAB";
            }

            break;

            //En pause echap
            case "PAUSE": 
                if (Input.GetKeyDown("escape"))
                {
                        pauseMenuUI.SetActive(false);
                        Cursor.lockState = CursorLockMode.Locked;
                        Cursor.visible = false;
                        StadeGame = "INGAME";
                }
            break;

            //Affichage du tableau des scores
            case "TAB":
                if (Input.GetKeyUp("tab"))
                {
                    scoreboardMenu.SetActive(false);
                    StadeGame = "INGAME";
                }
            break;

            //Choix de la class
            case "CHOOSINGCLASS":
            if (Input.GetKeyDown("escape"))
            {
                pauseMenuUI.SetActive(true);
                ClassDefMenu.SetActive(false);
                ClassAttMenu.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                StadeGame = "PAUSE";
            }
            break;

            case "EN_ATT_REAPARITION":
            if (Input.anyKey) //Si une touche appuyer
            {
                DeathHUD.SetActive(false); //Désactivation de l'HUD de mort
                GameObject.Find("/GAME/PunManager").GetComponent<PunScript>().SpawnPlayer(); //Respawn de moi
                InGameHUD.SetActive(true); //Activation de l'HUD dans la partie en vie
                StadeGame = "INGAME"; //Statue de la partie en partie
            }
            break;
        }
        
    }

    public void ChangeClassButton()
    {
        pauseMenuUI.SetActive(false);
        if ((string)PhotonNetwork.LocalPlayer.CustomProperties["Team"]=="DEF")
                {
                    ClassDefMenu.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    StadeGame = "CHOOSINGCLASS";
                }
                if ((string)PhotonNetwork.LocalPlayer.CustomProperties["Team"]=="ATT")
                {
                    ClassAttMenu.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    StadeGame = "CHOOSINGCLASS";
                }
    }

    #endregion
    #region PlayerGestion
    public void ChangePlayerClass(string newClass)
    {
        //Mise a jour de la hashtable
        Hashtable hash = new Hashtable();
        hash.Add("Class", newClass);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        if (ClassDefMenu.active || ClassAttMenu.active)
        {
            ClassDefMenu.SetActive(false);
            ClassAttMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            StadeGame = "INGAME";
        }
        mainplayer.GetComponent<Player_Manager>().Death(PhotonNetwork.LocalPlayer, 10);
    }

    public void AddDefPlayer()
    {
        if (nbDefPlayer<10 && (string)PhotonNetwork.LocalPlayer.CustomProperties["Team"]=="") 
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
            hash.Add("Class", "Policier");
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

            //Mise a jour du statue de la partie
            StadeGame = "INGAME";
        }
    }

    public void AddAttPlayer()
    {
        if (nbAttPlayer<10 && (string)PhotonNetwork.LocalPlayer.CustomProperties["Team"]=="") {
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
            hash.Add("Class", "Mercenaire");
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

            //Mise a jour du statue de la partie
            StadeGame = "INGAME";
        }
    }
    #endregion
    #region PhotonPun
        public void SendToNewPlayer () //Function d'envoie de donné sur la partie
        {
            if (PhotonNetwork.IsMasterClient)
                {
                    Debug.Log("Envoie du nombre de perso par Team" + nbAttPlayer + " " + nbDefPlayer );
                    view.RPC ("NumberAtt", RpcTarget.Others, nbAttPlayer); //Envoi att
                    view.RPC ("NumberDef", RpcTarget.Others, nbDefPlayer); //Envoi def
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
