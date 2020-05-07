using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.Udomugo.HoD
{
    public class EnviroController : MonoBehaviourPunCallbacks, IPunObservable
    {
        private Vector3 enviro_pos;
        private Quaternion enviro_rot;
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
            }
            else
            {
                enviro_pos = (Vector3)stream.ReceiveNext();
                enviro_rot = (Quaternion)stream.ReceiveNext();
            }
        }
        void Awake()
        {
            enviro_pos = transform.position;
            enviro_rot = transform.rotation;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            /*
            if (photonView.IsMine)
            {
                transform.position = Vector3.Lerp(transform.position, enviro_pos, Time.deltaTime * 5);
                transform.rotation = Quaternion.Lerp(transform.rotation, enviro_rot, Time.deltaTime * 5);
            }*/
            
        }
    }
}