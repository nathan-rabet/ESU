using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PunScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Text TxtInfosPun;
        void Start()
        {
                PhotonNetwork.ConnectUsingSettings();
        }

        public void ConnectToRoom()
        {
                RoomOptions myRoomOption = new RoomOptions();
                myRoomOption.MaxPlayers = 20;
                
                PhotonNetwork.JoinOrCreateRoom ("OfficialRoom", myRoomOption, TypedLobby.Default);
        }

    // Update is called once per frame
    void Update()
    {
        TxtInfosPun.text = PhotonNetwork.NetworkClientState.ToString();
    }
}
