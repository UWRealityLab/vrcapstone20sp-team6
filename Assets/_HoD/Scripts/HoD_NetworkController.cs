using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UIElements;

namespace Com.Udomugo.OculusVRTutorial
{
    public class HoD_NetworkController : MonoBehaviourPunCallbacks, IPunObservable
    {
        #region Public Feilds

        [SerializeField]
        public GameObject ship;

        [SerializeField]
        public GameObject enviro;

        private Transform ship_pos;
        private Transform enviro_pos;

        // Start is called before the first frame update
        void Start()
        {
            ship_pos = ship.transform;
            enviro_pos = enviro.transform;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(ship_pos.position);
                stream.SendNext(ship_pos.rotation);

                stream.SendNext(enviro_pos.position);
                stream.SendNext(enviro_pos.rotation);
            }
        }

        #endregion

        
    }
}