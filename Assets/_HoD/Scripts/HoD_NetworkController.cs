using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.Udomugo.OculusVRTutorial
{
    public class HoD_NetworkController : MonoBehaviourPunCallbacks, IPunObservable
    {
        #region Public Feilds

        [SerializeField]
        public GameObject ship;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {

            }
        }

        #endregion

        
    }
}