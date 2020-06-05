using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Oculus.Platform.Samples.VrHoops;

namespace Com.Udomugo.OculusVRTutorial
{
    public class NetworkVRPlayer : MonoBehaviourPunCallbacks, IPunObservable
    {
        #region Public Fields

        public GameObject avatar_head;
        public GameObject avatar_body;
        public GameObject avatar_left_hand;
        public GameObject avatar_right_hand;

        //public GameObject enviro;

        private Transform playerGlobal;  // responsible for player avatar position
        private Transform playerLocal_head;   // responsible for head movements
        private Transform playerLocal_left_hand;   // responsible for left hand movements
        private Transform playerLocal_right_hand;   // responsible for right hand movements

        //private Transform enviro_trans;

        #endregion

        #region IPunObservable implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // streaming position data
                stream.SendNext(playerGlobal.position);
                stream.SendNext(playerGlobal.rotation);
                
                // streaming head data
                stream.SendNext(playerLocal_head.localPosition);
                stream.SendNext(playerLocal_head.localRotation);


                // streaming left hand data
                stream.SendNext(playerLocal_left_hand.localPosition);
                stream.SendNext(playerLocal_left_hand.localRotation);

                // streaming right hand data
                stream.SendNext(playerLocal_right_hand.localPosition);
                stream.SendNext(playerLocal_right_hand.localRotation);

                
                
            }
            else
            {
                this.transform.position = (Vector3)stream.ReceiveNext();
                this.transform.rotation = (Quaternion)stream.ReceiveNext();
                avatar_head.transform.localPosition = (Vector3)stream.ReceiveNext();
                avatar_head.transform.localRotation = (Quaternion)stream.ReceiveNext();
                avatar_left_hand.transform.localPosition = (Vector3)stream.ReceiveNext();
                avatar_left_hand.transform.localRotation = (Quaternion)stream.ReceiveNext();
                avatar_right_hand.transform.localPosition = (Vector3)stream.ReceiveNext();
                avatar_right_hand.transform.localRotation = (Quaternion)stream.ReceiveNext();

                // receiveing enviro data
                //enviro_trans.position = (Vector3)stream.ReceiveNext();
                //enviro_trans.rotation = (Quaternion)stream.ReceiveNext();
            }
        }

        #endregion

        
        private void Awake()
        {
            /*   
            if (photonView.IsMine)
            {
                PlayerManager.LocalPlayerInstance = this.gameObject;
            }
            // #Critical
            // we flag as don't destroy on load so that instace survives level sychronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(this.gameObject);
            */
        }
        

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("Player Instatiated");
            if (photonView.IsMine)
            {
                // only do this for the player that is me.
                playerGlobal = GameObject.Find("OVRPlayerController").transform;
                playerLocal_head = playerGlobal.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor").transform;
                playerLocal_left_hand = playerGlobal.Find("OVRCameraRig/TrackingSpace/LeftHandAnchor").transform;
                playerLocal_right_hand = playerGlobal.Find("OVRCameraRig/TrackingSpace/RightHandAnchor").transform;

                // Want to attach avatar to centerEyeAnchor.
                avatar_head.transform.SetParent(playerLocal_head);
                avatar_head.transform.localPosition = new Vector3(0, 0.5f, 0);
                //avatar_head.transform.localRotation = Quaternion.Euler(-90, 0, 0);

                avatar_left_hand.transform.SetParent(playerLocal_left_hand);
                avatar_left_hand.transform.localPosition = Vector3.zero;
                avatar_right_hand.transform.SetParent(playerLocal_right_hand);
                avatar_right_hand.transform.localPosition = Vector3.zero;
                avatar_body.transform.SetParent(playerGlobal);
                avatar_body.transform.localPosition = new Vector3(0, -0.5f, -1);

                avatar_head.SetActive(false);  // hides avatar head from player
            }

            //enviro_trans = enviro.transform;

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
