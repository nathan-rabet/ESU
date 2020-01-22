using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

namespace Com.DeltaPlane.ESU
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region setget

        private string RoomName;
        public void setget()
        {
            if (roomName_field != null)
            {
                RoomName = roomName_field.text;
            }
        }

        #endregion
        #region Private Serializable Fields

        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 6;

        [Tooltip("The name of the new room.")]
        [SerializeField]
        private TMP_InputField roomName_field;
        
        
        #endregion

        #region Private Fields

        private string gameVersion = "1";

        #endregion

        #region MonoBehaviour CallBacks

        void Awake()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        void Start()
        {
            
        }
        #endregion

        #region Public Methods

        public void Connect()
        {
            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected)
            {
                // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // #Critical, we must first and foremost connect to Photon Online Server.
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }

        public void CreateRoom()
        {
            Debug.Log("Create a new room");
            setget();
            RoomOptions newRoomOptions = new RoomOptions();
            newRoomOptions.MaxPlayers = maxPlayersPerRoom;
            PhotonNetwork.CreateRoom(RoomName, newRoomOptions);
        }

        #endregion
        
        #region MonoBehaviourPunCallbacks Callbacks


        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Launcher: OnConnectedToMaster() was called by PUN");
        }


        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("PUN Basics Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }


        #endregion
    }

}
