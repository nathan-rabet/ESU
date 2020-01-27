﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class PunScript : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public Text TxtInfosPun;
        void Start()
        {
                PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby (TypedLobby.Default);
        }


        public override void OnJoinedLobby( )
        {
                RoomOptions myRoomOption = new RoomOptions();
                myRoomOption.MaxPlayers = 20;
                
                PhotonNetwork.JoinOrCreateRoom ("OfficialRoom", myRoomOption, TypedLobby.Default);
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
