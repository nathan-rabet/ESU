using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

    public class PunScript : MonoBehaviourPunCallbacks
    {
        public GameObject gameManager;
        public Transform SpawnPoint;
        public GameObject[] PlayerPrefab;
        public Camera MainCamera;
        public TMP_Text ClientState;

            void Start()
            {
                PhotonNetwork.SendRate = 20; //Set tickrate 20/sec
                PhotonNetwork.SerializationRate = 15; //Set Update 15/sec
                if (PhotonNetwork.NetworkClientState.ToString() == "ConnectedToMaster") //Si connecté
                {
                    PhotonNetwork.JoinLobby (TypedLobby.Default); //Rejoindre le lobby
                }
                else
                {
                    PhotonNetwork.ConnectUsingSettings(); //Connection aux serveurs Photon
                }
                ClientState.text = PhotonNetwork.NetworkClientState.ToString(); //Affichage
            }

            public override void OnConnectedToMaster() //Si connecté au serveur maître de Photon
            {
                PhotonNetwork.JoinLobby (TypedLobby.Default); //Rejoindre le lobby
                ClientState.text = PhotonNetwork.NetworkClientState.ToString(); //Affichage
            }


            public override void OnJoinedLobby() //Si on rentre dans un lobby
            {
                    RoomOptions myRoomOption = new RoomOptions(); //Création de la var de room
                    myRoomOption.MaxPlayers = 20; //20 joueurs max
                    
                    PhotonNetwork.JoinOrCreateRoom ("OfficialRoom", myRoomOption, TypedLobby.Default); //Essai de rejoindre la partie en crée une sinon
                    ClientState.text = PhotonNetwork.NetworkClientState.ToString(); //Affichage
            }

            public override void OnJoinedRoom () //Si partie rejoind ou crée
            {
                ClientState.text = PhotonNetwork.NetworkClientState.ToString(); //Affichage
                gameManager.GetComponent<GameManagerScript>().IsConnected(); //Appel de la fonction IsConnected de GameManager
            }

            public void SpawnPlayer() //Function de création de joueur
            {
                if (PhotonNetwork.NetworkClientState.ToString() == "Joined") //Si on est en partie
                {
                    //
                    //  Ajout d'un switch en fonction de la classe choisit (Rajout en paramètre de cette Fonction)
                    //
                    GameObject classprefab = PlayerPrefab[0];
                    switch (gameManager.GetComponent<GameManagerScript>().MyClass)
                    {
                        case "Policier":
                            classprefab = PlayerPrefab[0];
                            break;
                        case "Pompier":
                            classprefab = PlayerPrefab[1];
                            break;
                        case "Medecin":
                            classprefab = PlayerPrefab[2];
                            break;
                        case "Mercenaire":
                            classprefab = PlayerPrefab[3];
                            break;
                        case "Pyroman":
                            classprefab = PlayerPrefab[4];
                            break;
                        case "Drogueur":
                            classprefab = PlayerPrefab[5];
                            break;
                    }

                    //Instanciation
                    GameObject MyPlayer = PhotonNetwork.Instantiate(classprefab.name,SpawnPoint.position,Quaternion.identity,0) as GameObject; //Instantier le prefab
                    MainCamera.GetComponent<TPSCamera>().lookAt = MyPlayer.transform.Find("posCam").transform; //Set de la var lookAt de la cam
                }
            }

            public void LeaveTheRoom() //Function pour quitter la partie
            {
                PhotonNetwork.LeaveRoom(); //Quitter la partie
            }
            public override void OnLeftRoom () //Si quitter la partie
            {
                SceneManager.LoadScene(0); //Chargement du menu
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name); //Déchargement de la partie
            }

            public override void OnPlayerEnteredRoom (Player newPlayer) //Quand un joueur rentre dans la partie
            {
                if (PhotonNetwork.IsMasterClient) //Si je suis le maître
                {
                    gameManager.GetComponent<GameManagerScript>().SendToNewPlayer(); //Appel  de la fonction SendToNewPlayer de GameManagerScript
                }
            }
        // Update is called once per frame
        void Update()
        {
        }
    }
