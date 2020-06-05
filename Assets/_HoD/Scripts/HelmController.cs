using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class HelmController : MonoBehaviour
{
    private PhotonView PV;

    // Righthand
    public GameObject righthand;
    private Transform rightHandOriginalParent;
    private bool rightHandOnHelm = false;

    // Lefthand
    public GameObject lefthand;
    private Transform leftHandOriginalParent;
    private bool leftHandOnHelm = false;

    public Transform[] snappPositions;

    public Transform rudder;

    //Helm/objects to control with helm
    public GameObject world;
    private Rigidbody worldRigidbody;

    public float currentHelmRotation = 0;

    // Turn dampening, lower number makes the world take longer time to reach target rotation
    // For world to just copy steering helm movement use high number like 9999
    private float turnDampening = 20;

    public Transform directionalObject;

    //private int numberOfHandsOnHelm = 0;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        worldRigidbody = world.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Method that restores hands original position in their parent object
        ReleaseHandsFromHelm();

        /*
        if (!(Mathf.Abs(currentHelmRotation).CompareTo(120) == 1))
        {
            ConvertHandRotationToHelmRotation();
        }*/

        ConvertHandRotationToHelmRotation();
        TurnWorld();

        //UpdateRudderPos();

        currentHelmRotation = -transform.rotation.eulerAngles.z;
    }


    private void UpdateRudderPos()
    {
        var turn = transform.rotation.eulerAngles.z;
        if (turn < -350)
        {
            turn = turn + 360;
        }
        Vector3.Slerp(rudder.transform.forward, Quaternion.Euler(0, turn, 0) * rudder.transform.forward, Time.deltaTime * turnDampening);
    }


    private void TurnWorld()
    {
        //Turns world compared to the helm
        var turn = transform.rotation.eulerAngles.z;
        if (turn < -350)
        {
            turn = turn + 360;
        }

        worldRigidbody.MoveRotation(Quaternion.RotateTowards(world.transform.rotation, Quaternion.Euler(0, turn, 0), Time.deltaTime * turnDampening));
    }

    private void ConvertHandRotationToHelmRotation()
    {
        if (rightHandOnHelm == true && leftHandOnHelm == false)
        {
            Quaternion newRot = Quaternion.Euler(0, 0, rightHandOriginalParent.transform.rotation.eulerAngles.z);
            directionalObject.rotation = newRot;
            transform.parent = directionalObject;
        }
        else if (rightHandOnHelm == false && leftHandOnHelm == true)
        {
            Quaternion newRot = Quaternion.Euler(0, 0, leftHandOriginalParent.transform.rotation.eulerAngles.z);
            directionalObject.rotation = newRot;
            transform.parent = directionalObject;
        }
        else
        if (rightHandOnHelm == true && leftHandOnHelm == true)
        {
            Quaternion newRotLeft = Quaternion.Euler(0, 0, leftHandOriginalParent.transform.rotation.eulerAngles.z);
            Quaternion newRotRight = Quaternion.Euler(0, 0, rightHandOriginalParent.transform.rotation.eulerAngles.z);
            Quaternion finalRot = Quaternion.Slerp(newRotLeft, newRotRight, 1.0f / 2.0f);
            directionalObject.rotation = finalRot;
            transform.parent = directionalObject;
        }
    }

    private void ReleaseHandsFromHelm()
    {
        if (rightHandOnHelm == true && OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
        {
            righthand.transform.parent = rightHandOriginalParent;
            righthand.transform.position = rightHandOriginalParent.position;
            righthand.transform.rotation = rightHandOriginalParent.rotation;
            rightHandOnHelm = false;
            //numberOfHandsOnHelm--;
        }

        if (leftHandOnHelm == true && OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch))
        {
            lefthand.transform.parent = leftHandOriginalParent;
            lefthand.transform.position = leftHandOriginalParent.position;
            lefthand.transform.rotation = leftHandOriginalParent.rotation;
            leftHandOnHelm = false;
            //numberOfHandsOnHelm--;
        }

        if (leftHandOnHelm == false && rightHandOnHelm == false)
        {
            // reset helm to not be parent of directional object
            transform.parent = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            // Place RightHand
            // I use Oculus integration Change this "OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch)"
            if (rightHandOnHelm == false && OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
            {
                PlaceHandOnHelm(ref righthand, ref rightHandOriginalParent, ref rightHandOnHelm);
            }

            // Place LeftHand
            // Oculus integration Change this "OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch)"
            if (leftHandOnHelm == false && OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch))
            {
                PlaceHandOnHelm(ref lefthand, ref leftHandOriginalParent, ref leftHandOnHelm);
            }
        }

    }

    private void PlaceHandOnHelm(ref GameObject hand, ref Transform originalParent, ref bool handOnHelm)
    {
        if (!PV.IsMine)
        {
            PV.RequestOwnership();
        }
        // Set variables to the first snapp position in array
        var shortestDistance = Vector3.Distance(snappPositions[0].position, hand.transform.position);
        var bestSnapp = snappPositions[0];
        // lop through all snapp positions
        foreach (var snappPosition in snappPositions)
        {
            // if no hand is child of this snapp position
            if (snappPosition.childCount == 0)
            {
                // Distance between hand and snapp position
                var distance = Vector3.Distance(snappPosition.position, hand.transform.position);
                // if distance is shorter than current shortest and this snapp to the bestsnapp
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    bestSnapp = snappPosition;
                }

            }
        }

        // we need XHandOriginalParent to be able to reset hand after release
        originalParent = hand.transform.parent;

        // set best snapp as parent and hand position to snapp position
        hand.transform.parent = bestSnapp.transform;
        hand.transform.position = bestSnapp.transform.position;

        handOnHelm = true;
        //numberOfHandsOnHelm++;
    }
}
