using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UIElements;

public class ShipController : MonoBehaviourPunCallbacks
{
    public GameObject path_object;
    public float theta;
    private Quaternion rudder_rot;

    private GameObject trim;
    private GameObject wheel;

    private GameObject rudder;
    private GameObject world;


    //private Vector3 wind_direction;

    /// <summary> isolate the z-Component of a rotation and returns it as the y axis</summary>
    private Quaternion yRotation(Quaternion q)
    {
        theta = Mathf.Atan2(q.z, q.w);
        //theta *= 0.1f;

        // quaternion representing rotation about the y axis
        return new Quaternion(0, Mathf.Sin(theta), 0, Mathf.Cos(theta));
    }

    // Start is called before the first frame update
    void Start()
    {
        // get wind variable
        // get sail configuration
        // get rudder heading
        // check water reading --> what way are the wave pointing as a result of the wind

        trim = GameObject.Find("trim");
        rudder = GameObject.Find("rudder");
        wheel = GameObject.Find("axile");
        world = GameObject.Find("world");

    }

    // Update is called once per frame
    void Update()
    {
        // use for input
        rudder_rot = yRotation(wheel.transform.localRotation);
        rudder.transform.rotation = rudder_rot;
        //Instantiate(path_object, transform.position, transform.rotation).transform.parent = world.transform;


    }
}
