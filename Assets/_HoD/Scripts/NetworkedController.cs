//using Oculus.Platform;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

namespace Com.Udomugo.OculusVRTutorial
{
    public class NetworkedController : MonoBehaviourPunCallbacks
    {
        string _room = "Tutorial_Converge";

        bool isConnecting;

        // Start is called before the first frame update
        void Start()
        {
            //PhotonNetwork.ConnectUsingSettings();
            if (PhotonNetwork.IsConnected)
            {
                // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJointRandomFailed() and we'll create one.
                Debug.Log("Joining Random Room");
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // #Critical, we must first and formost connect to Photon Online Server.
                Debug.Log("Connecting to Online Server");
                isConnecting = PhotonNetwork.ConnectUsingSettings();    // keep track of the will to join a room, because when we come back from the game we will get a callback that we are connectedm, so we need to know what to do then
                PhotonNetwork.GameVersion = "1.0";
            }
        }

        /*
        void OnJoinedLobby()
        {
            Debug.Log("Joined Lobby");

            //Photon.Realtime.RoomOptions roomOptions = new Photon.Realtime.RoomOptions { };
            //PhotonNetwork.JoinOrCreateRoom(_room, roomOptions, TypedLobby.Default);
            //PhotonNetwork.JoinRandomRoom();
        }

        void OnJoinedRoom()
        {
            //PhotonNetwork.Instantiate("BGPlayerController", Vector3.zero, Quaternion.identity, 0);
        }
        */

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
            //progressLabel.SetActive(false);
            //controlPanel.SetActive(true);
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinRandomFailed() was called by PUN. No random room available, so we create one./nCalling: PhotonNetwork.CreateRoom");

            // #Critical: we dailed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room");

            PhotonNetwork.Instantiate("NetworkVRPlayer", Vector3.zero, Quaternion.identity, 0);

            /*
            // #Critical: We only load if we are the first player, else we rely on 'PhotonNetwork.AutomaticallySyncScene' to sync our instance scene.
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("We load the 'Room for 1' ");

                // #Critical
                // Load the Room Level
                PhotonNetwork.LoadLevel("Room for 1");
            }

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

