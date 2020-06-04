using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Com.Udomugo.HoD
{
    public class IslandRadiusUpdate : MonoBehaviour
    {
        public float radius;
        public bool inRadius;

        private Vector3 position;
        private PhotonView photonView;
        private Vector3 shipPosition;

        [SerializeField]
        private GameObject ship; // This assumes that the ship position is in the same context as islands placed in the world.
        private GameObject world;

        // Start is called before the first frame update
        void Start()
        {
            position = this.transform.TransformPoint(this.transform.position);
            shipPosition = ship.GetComponent<Transform>().position;
            photonView = GetComponent<PhotonView>();
        }

        // Update is called once per frame
        void Update()
        {
            inRadius = Vector3.Distance(shipPosition, position) <= radius;
            if (inRadius && PhotonNetwork.IsMasterClient)
            { // Ship within island bounds and masterclient view
                photonView.RPC("WorldPosition", RpcTarget.Others, world.transform.position); // masterclient updates other players' world position.
            }
        }

        [PunRPC]
        void WorldPosition(Vector3 world_pos)
        {
            Debug.Log("WorldPosition");
            world.transform.position = world_pos;
        }
    }
}
