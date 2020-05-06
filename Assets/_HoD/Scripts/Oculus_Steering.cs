using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oculus_Steering : MonoBehaviour
{
    public GameObject rightHand;
    public GameObject leftHand;
    public float maxTurnAngle = 45f;

    // Detect Collision with Player
    void OnTriggerStay(Collider other)
    {

        float leftPos = leftHand.transform.position.y;
        float rightPos = rightHand.transform.position.y;
        float leftSquez = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger);
        float rightSquez = OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger);


        if (other == leftHand && leftSquez > 0)
        {
            //Vector3 leftPos_v = new Vector3(leftHand.transform.position.x, leftHand.transform.position.y, 0);
            //transform.localEulerAngles = Vector3.Angle(Vector3.zero, leftPos_v);
            
            //float angle = Vector3.Angle(Vector3.zero, leftPos_v);
            //transform.rotation.SetAxisAngle(Vector3.forward, angle);
            //Quaternion.AngleAxis(angle, Vector3.forward);
        }

        if (other.attachedRigidbody) //issue == can steer wheel when not touching wheel if hands are squeezed 
        {

            Debug.Log("Debug: " + other.name + " " + other.tag);//debug print statement

            

            /*
            if ((leftPos > rightPos) && (leftSquez > 0 || rightSquez > 0)) //when leftHand elevation is higher, turn wheel to right
            {
                transform.localEulerAngles = Vector3.back * Mathf.Clamp(97, -maxTurnAngle, maxTurnAngle);
            }

            else if ((leftPos < rightPos) && (leftSquez > 0 || rightSquez > 0)) //when rightHand elevation is higher, turn wheel to left
            {
                transform.localEulerAngles = Vector3.back * Mathf.Clamp(-97, -maxTurnAngle, maxTurnAngle);
            }

            if (leftSquez == 0 && rightSquez == 0) //both hands not squeezing? set wheel to netural seat
            {
                transform.localEulerAngles = Vector3.zero;
            }
            */
        }
    }

    void Update()
    {
        //for joystick; takes the horizontal input to determine if steering wheel should rotate left or right
        /*if (Input.GetAxis("Horizontal") != 0)
        {

            transform.localEulerAngles = Vector3.back * Mathf.Clamp((Input.GetAxis("Horizontal") * 100), -maxTurnAngle, maxTurnAngle);
        }*/
    }

}
