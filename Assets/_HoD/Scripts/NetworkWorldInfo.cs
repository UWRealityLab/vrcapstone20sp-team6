using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Com.Udomugo.HoD
{
    [RequireComponent(typeof(PhotonView))]
    public class NetworkWorldInfo : MonoBehaviourPunCallbacks
    {
        private Transform trans;
        private PhotonView m_PhotonView;

        private void Start()
        {
            m_PhotonView = GetComponent<PhotonView>();
        }


        public override void OnPlayerEnteredRoom(Player other)
        {
            //Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName);   // not seeen if you're the player connecting

            if (PhotonNetwork.IsMasterClient)
            {
                m_PhotonView.RPC("WorldUpdate", RpcTarget.Others, transform.position, transform.rotation);
            }
        }

        [PunRPC]
        void WorldUpdate(Vector3 world_pos, Quaternion world_rotation)
        {
            transform.position = world_pos;
            transform.rotation = world_rotation;
        }
    }
}
