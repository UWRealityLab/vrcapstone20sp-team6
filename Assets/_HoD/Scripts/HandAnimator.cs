using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

namespace Com.Udomugo.OculusVRTutorial
{
    public class HandAnimator : MonoBehaviourPun
    {
        #region Private Fields

        [SerializeField]
        private Animator animator;
        

        #endregion

        #region MonoBehavior Callbacks

        // Use this for initialization
        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            if (!animator)
            {
                Debug.LogError("PlayerAnimatorManager is Missing Animator Component", this);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }
            if (!animator)
            {
                return;
            }
        }

        #endregion
    }
}
