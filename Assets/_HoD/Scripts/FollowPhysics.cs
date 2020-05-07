using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Udomugo.HoD
{
    public class FollowPhysics : MonoBehaviour
    {
        public Transform target;

        private Rigidbody rb;
        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            rb.MovePosition(target.transform.position);
        }
    }
}