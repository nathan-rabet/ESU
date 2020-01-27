using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
        
        public void OnjoinedLobby()
        {
                RoomOptions myRoomOption = new RoomOptions();
                myRoomOption.MaxPlayers = 20;
                
                PhotonNetwork.JoinOrCreateRoom ("OfficialRoom", myRoomOption, TypedLobby.Default);
        }

        private void OnConnectedToMaster()
        {
            Debug.Log("Connected To the master");
        }

        public void LeaveTheRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
        void OnLeftRoom () 
        {
            SceneManager.LoadScene(0);
        }

    // Update is called once per frame
    void Update()
    {
        TxtInfosPun.text = PhotonNetwork.NetworkClientState.ToString();
    }
}
