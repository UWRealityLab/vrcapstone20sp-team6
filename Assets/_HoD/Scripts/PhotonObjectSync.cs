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
            /*
            if (this.m_Grab.isGrabbed)
            {
                if (!this.m_PhotonView.IsMine)
                {
                    this.m_PhotonView.RequestOwnership();
                }
                this.m_Body.useGravity = false;
                // Already accounted for in OVR Grabbable
                //this.m_Body.isKinematic = true;
                m_PhotonView.RPC("ChangeGravity", RpcTarget.All, false);
            }
            else
            {
                if (!this.m_PhotonView.IsMine)
                {
                    this.m_PhotonView.RequestOwnership();
                }
                this.m_Body.useGravity = true;
                //this.m_Body.isKinematic = false;
                m_PhotonView.RPC("ChangeGravity", RpcTarget.All, true);
            }
            */
        }

        public void Update()
        {
            if (this.m_Grab.isGrabbed)
            {
                if (!this.m_PhotonView.IsMine)
                {
                    this.m_PhotonView.RequestOwnership();
                }
                if (this.m_Body.useGravity == true)
                {
                    this.m_Body.useGravity = false;
                    //this.m_Body.isKinematic = true;
                    m_PhotonView.RPC("ChangeGravity", RpcTarget.All, this.m_Body.useGravity);
                }
            }
            else
            {
                if (!this.m_PhotonView.IsMine)
                {
                    this.m_PhotonView.RequestOwnership();
                }
                if (this.m_Body.useGravity == false)
                {
                    this.m_Body.useGravity = true;
                    //this.m_Body.isKinematic = false;
                    m_PhotonView.RPC("ChangeGravity", RpcTarget.All, this.m_Body.useGravity);
                }
            }
        }

        [PunRPC]
        void ChangeGravity(bool grabbed)
        {
            this.m_Body.useGravity = grabbed;
            this.m_Body.isKinematic = !grabbed;
        }

    }
}
