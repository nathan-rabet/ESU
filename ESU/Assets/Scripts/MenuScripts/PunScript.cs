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
        public GameObject PrefabPlayer;
        public Camera MainCamera;
        public TMP_Text ClientState;

            void Start()
            {
                PhotonNetwork.SendRate = 20;
                PhotonNetwork.SerializationRate = 15;
                if (PhotonNetwork.NetworkClientState.ToString() == "ConnectedToMaster")
                {
                    PhotonNetwork.JoinLobby (TypedLobby.Default);
                }
                else
                {
                    PhotonNetwork.ConnectUsingSettings();
                }
                ClientState.text = PhotonNetwork.NetworkClientState.ToString();
            }

            public override void OnConnectedToMaster()
            {
                PhotonNetwork.JoinLobby (TypedLobby.Default);
                ClientState.text = PhotonNetwork.NetworkClientState.ToString();
            }


            public override void OnJoinedLobby()
            {
                    RoomOptions myRoomOption = new RoomOptions();
                    myRoomOption.MaxPlayers = 20;
                    
                    PhotonNetwork.JoinOrCreateRoom ("OfficialRoom", myRoomOption, TypedLobby.Default);
                    ClientState.text = PhotonNetwork.NetworkClientState.ToString();
            }

            public override void OnJoinedRoom () 
            {
                ClientState.text = PhotonNetwork.NetworkClientState.ToString();
                gameManager.GetComponent<GameManagerScript>().IsConnected();
            }

            public void SpawnPlayer()
            {
                if (PhotonNetwork.NetworkClientState.ToString() == "Joined")
                {
                    GameObject MyPlayer = PhotonNetwork.Instantiate(PrefabPlayer.name,SpawnPoint.position,Quaternion.identity,0) as GameObject;
                    MainCamera.GetComponent<TPSCamera>().lookAt = MyPlayer.transform;
                }
            }

            public void LeaveTheRoom()
            {
                PhotonNetwork.LeaveRoom();
            }
            public override void OnLeftRoom () 
            {
                SceneManager.LoadScene(0);
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
            }

            public override void OnPlayerEnteredRoom (Player newPlayer)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    gameManager.GetComponent<GameManagerScript>().SendToNewPlayer();
                    Debug.Log("Appel de la fonction: SendToNewPlayer");
                }
            }
        // Update is called once per frame
        void Update()
        {
        }
    }
