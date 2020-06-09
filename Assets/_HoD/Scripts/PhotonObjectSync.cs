using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Collections.Specialized;

namespace Com.Udomugo.HoD
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(OVRGrabbable))]
    [RequireComponent(typeof(PhotonView))]
    public class PhotonObjectSync : MonoBehaviour
    {
        private Rigidbody m_Body;
        private PhotonView m_PhotonView;
        private OVRGrabbable m_Grab;

        public void Start()
        {
            this.m_Body = GetComponent<Rigidbody>();
            this.m_PhotonView = GetComponent<PhotonView>();
            this.m_Grab = GetComponent<OVRGrabbable>();
        }

        public void Update()
        {
            if (this.m_Grab.isGrabbed)
            {
                if (!this.m_PhotonView.IsMine) // Cannot update object information if we don't own the photonview
                {
                    this.m_PhotonView.RequestOwnership();
                }
                if (this.m_Body.isKinematic) // Check to make sure ovrgrabbable has already changed kinematic info
                {
                    m_PhotonView.RPC("ChangeKinematic", RpcTarget.Others, this.m_Body.isKinematic);
                }
            }
            else
            {
                if (this.m_PhotonView.IsMine && !this.m_Body.isKinematic) // Check to make sure ovrgrabbable has already changed kinematic info
                {
                    m_PhotonView.RPC("ChangeKinematic", RpcTarget.Others, this.m_Body.isKinematic);
                }
            }
        }

        // Send object rigidbody kinematic info
        [PunRPC]
        void ChangeKinematic(bool kinematic)
        {
            this.m_Body.isKinematic = kinematic;
        }

    }
}
