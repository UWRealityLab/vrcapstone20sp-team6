using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Com.Udomugo.HoD
{
    public class FollowPhysics : MonoBehaviour
    {
        public Transform target;
        public float angle_to_target;
        public float target_mag;
        public GameObject axile;

        private Rigidbody rb;
        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 target_x_y;
            // Attempt to smooth the wheel motion
            Vector3 to_target = target.transform.position;

            angle_to_target = Vector3.Angle(this.transform.up, to_target);

            Debug.DrawLine(this.transform.position, target.position, Color.red);
            Debug.DrawLine(axile.transform.position, to_target, Color.blue);

            target_mag = Vector3.Distance(axile.transform.position, to_target);

            /*
            if (angle_to_target <= 90)
            {
                target_x_y = Vector3.Scale(target.position, new Vector3(1, 1, 0));
                target_x_y = Vector3.Scale(transform.position, new Vector3(0, 0, 1)) + target_x_y;
            } else
            {
                
                target_x_y = Vector3.Cross(Vector3.Scale(target.position, new Vector3(0, 0, 1)), transform.up);
                target_x_y = Vector3.Scale(transform.position, new Vector3(0, 0, 1)) + target_x_y;
            }*/
            
            target_x_y = Vector3.Scale(target.position, new Vector3(1, 1, 0));
            target_x_y = Vector3.Scale(transform.position, new Vector3(0, 0, 1)) + target_x_y;
            Vector3 new_target = Vector3.Project(target_x_y, this.transform.position);
            Debug.DrawLine(axile.transform.position, new_target, Color.green);
            //target_x_y = Vector3.ClampMagnitude(target_x_y, 0.8f);

            rb.MovePosition(Vector3.Slerp(transform.position, new_target, 1));
        }
    }
}