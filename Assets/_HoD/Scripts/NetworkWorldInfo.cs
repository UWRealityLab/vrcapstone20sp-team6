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
            trans = GetComponent<Transform>();
            m_PhotonView = GetComponent<PhotonView>();
        }
        void Update()
        {

        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            //Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName);   // not seeen if you're the player connecting

            if (m_PhotonView.IsMine)
            {
                m_PhotonView.RPC("WorldUpdate", RpcTarget.OthersBuffered, trans.position, trans.rotation);
            }
        }

        [PunRPC]
        void WorldUpdate(Vector3 world_pos, Quaternion world_rotation)
        {
            Debug.Log("World update");
            trans.position = world_pos;
            trans.rotation = world_rotation;
        }
    }
}
