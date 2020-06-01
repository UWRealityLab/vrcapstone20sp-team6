using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Com.Udomugo.OculusVRTutorial
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        /// <summary>
        /// The maximum number of players per room. When a room is full, it can't be joined byt new players, and so new room will be created.
        /// </summary>
        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new rom will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;
        #endregion

        #region Private Fields
        
        /// <summary>
        /// This client's version number. Users are separated from each other br gameVersion (which allows you to make breaking changes).
        /// </summary>


        string gameVersion = "1";

        /// <summary>
        /// Keep track of the current process. Since connection is asychronous and is based on several callbacks from Photon,
        /// we need to keep track of this to properly adjust the behavior when we recieve call back by Photon.
        /// Typically this is used for the OnConnectedToMaster() callback.
        /// </summary>
        bool isConnecting;

        #endregion

        #region Public Fields
        [Tooltip("The UI Panel To let the user enter name, connect and play")]
        [SerializeField]
        private GameObject controlPanel;
        [Tooltip("The UI Label to inform the user that the cannection is in progress")]
        [SerializeField]
        private GameObject progressLabel;

        #endregion

        #region MonoBehaviour CallBacks

        /// <summary>
        /// MonoBehaviouyr method called on GameOvject by Unity during early initilaization phase.
        /// </summary>
        void Awake()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        /// <summary>
        /// MonoBVehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>

        // Start is called before the first frame update
        void Start()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }

        #endregion

        #region Public Methods

        public void Connect()
        {
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);
            // we chack if we are connected or not, we join if we are, else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected)
            {
                // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJointRandomFailed() and we'll create one.
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // #Critical, we must first and formost connect to Photon Online Server.
                isConnecting = PhotonNetwork.ConnectUsingSettings();    // keep track of the will to join a room, because when we come back from the game we will get a callback that we are connectedm, so we need to know what to do then
                PhotonNetwork.GameVersion = gameVersion;
            }
        }

        #endregion

        #region MonoBehaviorPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("Pun Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            // we don't want to do anything if we are not attempting to join a room.
            // this case where isConnecting is false is tyipically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
            // we don't want to do anything.
            if (isConnecting)
            {
                // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called bash with OnJoinRandomFailed()
                PhotonNetwork.JoinRandomRoom();
                isConnecting = false;
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinRandomFailed() was called by PUN. No random room available, so we create one./nCalling: PhotonNetwork.CreateRoom");

            // #Critical: we dailed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room");

            PhotonNetwork.LoadLevel("HoD_PhotonTest");
            /*
            // #Critical: We only load if we are the first player, else we rely on 'PhotonNetwork.AutomaticallySyncScene' to sync our instance scene.
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("We load the 'Room for 1' ");

                // #Critical
                // Load the Room Level
                PhotonNetwork.LoadLevel("tropical_island_2_0");
            } */

            /*
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                Debug.Log("We load the 'Room for 2' ");

                // #Critical
                // Load the Room Level
                PhotonNetwork.LoadLevel("Room for 2");
            }
            */
        }

        #endregion
    }
}