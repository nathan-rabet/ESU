using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

namespace ESU.Deltaplane {
    public class PunScript : MonoBehaviourPunCallbacks
    {
        public Text TxtInfosPun;
        public Transform SpawnPoint;
        public GameObject PrefabPlayer;

            void Start()
            {
                    PhotonNetwork.ConnectUsingSettings();
            }

            public override void OnConnectedToMaster()
            {
                PhotonNetwork.JoinLobby (TypedLobby.Default);
            }


            public override void OnJoinedLobby()
            {
                    RoomOptions myRoomOption = new RoomOptions();
                    myRoomOption.MaxPlayers = 20;
                    
                    PhotonNetwork.JoinOrCreateRoom ("OfficialRoom", myRoomOption, TypedLobby.Default);
            }

            public override void OnJoinedRoom () 
            {
                PhotonNetwork.Instantiate(PrefabPlayer.name,SpawnPoint.position,Quaternion.identity,0);
            }

            public void LeaveTheRoom()
            {
                PhotonNetwork.LeaveRoom();
            }
            public override void OnLeftRoom () 
            {
                SceneManager.LoadScene(0);
            }
        // Update is called once per frame
        void Update()
        {
            TxtInfosPun.text = PhotonNetwork.NetworkClientState.ToString();
        }
    }
}