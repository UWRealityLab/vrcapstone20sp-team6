using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Photon.Pun;
using UnityEditor.XR;
using UnityEngine;
using UnityStandardAssets._2D;
using UnityEngine.UI;

[RequireComponent(typeof(PhotonView))]
public class HelmController : MonoBehaviour
{
    public Text axile_info;
    public Text direction_info;

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
    private float turnDampening = 0.6f;

    public Transform directionalObject;

    //private int numberOfHandsOnHelm = 0;

    private bool reset_helm = false;

    private Transform reset_transform;

    public float pub_turn;

    public float rudd_turn;

    private float _syntheticAngle = float.NaN;
    private float _prevAngle = float.NaN;
    public float helm_wheel_ang;
    public float helm_wheel_dir;

    private Quaternion last_rot;

    public bool is_bounded_left;
    public bool is_bounded_right;
    private bool trend_pos;
    private bool start;

    public int num_rots_left;
    public int num_rots_right;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        worldRigidbody = world.GetComponent<Rigidbody>();
        reset_transform = transform;
        last_rot = Quaternion.identity;
        //directional_object = directionalObject.transform;
        rudd_turn = 0;
        is_bounded_left = false;
        is_bounded_right = false;
        num_rots_left = 0;
        num_rots_right = 0;
        start = true;
    }

    // Update is called once per frame
    void Update()
    {
        axile_info.text = helm_wheel_ang.ToString();
        direction_info.text = directionalObject.localEulerAngles.z.ToString();
        //UpdateBounds();
        last_rot = transform.rotation;
        //directional_object = directionalObject.transform;
        //UpdateAngle();
        // Method that restores hands original position in their parent object
        ReleaseHandsFromHelm();

        /*
        if (!(Mathf.Abs(currentHelmRotation).CompareTo(120) == 1))
        {
            ConvertHandRotationToHelmRotation();
        }*/

        ConvertHandRotationToHelmRotation();

        //TurnWorld();

        //UpdateRudderPos();

        //currentHelmRotation = -transform.rotation.eulerAngles.z;
    }

    void UpdateBounds ()
    {
        //helm_wheel_ang = transform.localEulerAngles.z;

        // initializing the turn trend
        if (start)
        {
            trend_pos = (helm_wheel_ang > 0) ? true : false;
            start = false;
        }

        if (helm_wheel_ang > 170)
        {
            helm_wheel_ang = 170;
            transform.rotation = Quaternion.Slerp(Quaternion.Euler(0, 0, transform.localEulerAngles.z), Quaternion.Euler(0, 0, 160), 0.7f);
        } else if (helm_wheel_ang < -170)
        {
            helm_wheel_ang = -170;
            transform.rotation = Quaternion.Slerp(Quaternion.Euler(0, 0, transform.localEulerAngles.z), Quaternion.Euler(0, 0, -160), 0.7f);
        }
    }

    void UpdateAngle(Quaternion q)
    {
        
        helm_wheel_ang = q.eulerAngles.z;
        helm_wheel_ang = (helm_wheel_ang > 180) ? helm_wheel_ang - 360 : helm_wheel_ang;
        if (float.IsNaN(_syntheticAngle))
        {
            _syntheticAngle = helm_wheel_ang;
            _prevAngle = helm_wheel_ang;
        }
        float dAngle = helm_wheel_ang - _prevAngle;
        if (dAngle < -180)
        {
            // eg it hopped from -170 to +170
            dAngle += 360;
        }
        else if (dAngle > 180)
        {
            // eg it hopped from +170 to -170
            dAngle -= 360;
        }
        _syntheticAngle += dAngle;
        

    }

    void UpdateAngleDir(Transform t)
    {
        helm_wheel_dir = t.transform.localEulerAngles.z;
        helm_wheel_dir = (helm_wheel_dir > 180) ? helm_wheel_dir - 360 : helm_wheel_dir;
        if (float.IsNaN(_syntheticAngle))
        {
            _syntheticAngle = helm_wheel_dir;
            _prevAngle = helm_wheel_dir;
        }
        float dAngle = helm_wheel_dir - _prevAngle;
        if (dAngle < -180)
        {
            // eg it hopped from -170 to +170
            dAngle += 360;
        }
        else if (dAngle > 180)
        {
            // eg it hopped from +170 to -170
            dAngle -= 360;
        }
        _syntheticAngle += dAngle;
    }

    private void UpdateRudderPos(int flux)
    {
        //rudd_turn = rudder.transform.localEulerAngles.y;
        //rudd_turn += helm_wheel_ang * 0.15f;
        //pub_turn = directionalObject.eulerAngles.z;
        rudd_turn += (helm_wheel_ang * 2f * Time.deltaTime)/flux;

        //var turn = pub_turn;
        /*
        if (!reset_helm)
        {
            if (turn >= 271)
            {
                turn = turn - 360;
            }

            if (turn > 270)
            {
                turn = 270;
                reset_helm = true;
            }
            if (turn < -270)
            {
                turn = -270;
                reset_helm = true;
            }
        }
        */
        //currentHelmRotation = turn;
        // this works for vector position, but we need rotation
        //rudder.transform.position = Vector3.Slerp(rudder.transform.forward, Quaternion.Euler(0, turn, 0) * rudder.transform.forward, Time.deltaTime * turnDampening);

        //// last working method
        //rudder.transform.rotation = Quaternion.Slerp(rudder.rotation, Quaternion.Euler(0, helm_wheel_ang * 0.015f, 0), Time.deltaTime * turnDampening);
        rudder.transform.rotation = Quaternion.Euler(0, rudd_turn * 7f, 0);
    }

    /*
    private void TurnWorld()
    {
        //Turns world compared to the helm
        var turn = transform.rotation.eulerAngles.z;
        if (turn < -350)
        {
            turn = turn + 360;
        }

        worldRigidbody.MoveRotation(Quaternion.RotateTowards(world.transform.rotation, Quaternion.Euler(0, turn, 0), Time.deltaTime * turnDampening));
    }*/

    private void ConvertHandRotationToHelmRotation()
    {
        bool maxTurn = (helm_wheel_dir > 0 && helm_wheel_ang > 350) || (helm_wheel_dir < 0 && helm_wheel_ang < -350);
        if (rightHandOnHelm == true && leftHandOnHelm == false)
        {
            UpdateAngleDir(rightHandOriginalParent);
            if (!maxTurn)
            {
                Quaternion newRot = Quaternion.Euler(0, 0, rightHandOriginalParent.transform.rotation.eulerAngles.z);
                directionalObject.rotation = newRot;
                UpdateAngle(newRot);
                if (helm_wheel_ang >= -170 && helm_wheel_ang <= 170)
                {
                    transform.parent = directionalObject;
                    
                } else
                {
                    helm_wheel_ang = (helm_wheel_ang > 0) ? 170 : -170;
                }
                
            }
        }
        else if (rightHandOnHelm == false && leftHandOnHelm == true)
        {
            UpdateAngleDir(rightHandOriginalParent);
            if (!maxTurn)
            {
                Quaternion newRot = Quaternion.Euler(0, 0, leftHandOriginalParent.transform.rotation.eulerAngles.z);
                directionalObject.rotation = newRot;
                UpdateAngle(newRot);
                if (helm_wheel_ang >= -170 && helm_wheel_ang <= 170)
                {
                    transform.parent = directionalObject;
                    
                }
                else
                {
                    helm_wheel_ang = (helm_wheel_ang > 0) ? 170 : -170;
                }
            }
        }
        else
        if (rightHandOnHelm == true && leftHandOnHelm == true)
        {
            UpdateAngleDir(rightHandOriginalParent);
            if (!maxTurn)
            {
                Quaternion newRotLeft = Quaternion.Euler(0, 0, leftHandOriginalParent.transform.rotation.eulerAngles.z);
                Quaternion newRotRight = Quaternion.Euler(0, 0, rightHandOriginalParent.transform.rotation.eulerAngles.z);
                Quaternion finalRot = Quaternion.Slerp(newRotLeft, newRotRight, 1.0f / 2.0f);
                directionalObject.rotation = finalRot;
                UpdateAngle(finalRot);
                if (helm_wheel_ang >= -170 && helm_wheel_ang <= 170)
                {
                    transform.parent = directionalObject;
                    UpdateRudderPos(50);
                }
                else
                {
                    helm_wheel_ang = (helm_wheel_ang > 0) ? 170 : -170;
                }
            }
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
            //helm_wheel_ang = 0;
            UpdateRudderPos(1);
        }

        if (leftHandOnHelm == true && OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch))
        {
            lefthand.transform.parent = leftHandOriginalParent;
            lefthand.transform.position = leftHandOriginalParent.position;
            lefthand.transform.rotation = leftHandOriginalParent.rotation;
            leftHandOnHelm = false;
            //numberOfHandsOnHelm--;
            //helm_wheel_ang = 0;
            UpdateRudderPos(1);

        }

        if (leftHandOnHelm == false && rightHandOnHelm == false)
        {
            // reset helm to not be parent of directional object
            transform.parent = null;
            transform.rotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 240);
            helm_wheel_ang = 0;
        }

        

        /*
        if (reset_helm)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            rudder.rotation = Quaternion.Euler(0, 0, 0);
            reset_helm = false;
        } */
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
