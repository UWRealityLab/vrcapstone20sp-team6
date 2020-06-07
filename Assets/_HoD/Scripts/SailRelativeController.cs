using Com.Udomugo.HoD;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailRelativeController : MonoBehaviour
{
    public ShipActions ship_actions;
    private bool anchor_isDown;

    public float helm;
    public float wind_power;
    public float trim_power;
    public float rudder_power;
    public float rudder_power_cross;
    public float dot_rudder;
    public float dot_trim;
    public float dot_wind;
    public float wind_angle;
    public float rudder_angle;


    private GameObject world;
    private GameObject trim;
    private GameObject rudder;
    private GameObject ship;
    private GameObject wind;

    private Rigidbody rb;

    public Vector3 direction;
    public Vector3 rudder_face;

    private Vector3 rotate_count_vec;
    private Vector3 heading_rudder;

    // Start is called before the first frame update
    void Start()
    {
        
        world = GameObject.Find("world");
        trim = GameObject.Find("trim");
        rudder = GameObject.Find("rudder");
        ship = GameObject.Find("shipmkii");
        wind = GameObject.Find("wind");

        rb = world.GetComponent<Rigidbody>();

        //anchor.is_set = true;
    }

    // Update is called once per frame
    void Update()
    {
        anchor_isDown = ship_actions.anchorDown;
        rb.centerOfMass = ship.transform.position - world.transform.position;
        //world.GetComponent<Collider>().transform.position = ship.transform.position - world.transform.position;
        /*
        // This updates the trim value an sets the power modifier
        Vector3 forward = transform.TransformDirection(trim.transform.rotation * Vector3.forward);
        //Vector3 toOther = Vector3.forward - trim.transform.position;
        Vector3 toOther = (wind.transform.rotation * world.transform.forward) - trim.transform.position;
        power = Vector3.Dot(forward, toOther);
        if (power < 0)
        {
            power = 0;
        }
        */

        // This updates the rudder value and sets the angle modifier
        rudder_face = rudder.transform.forward;

        // getting the vector of down wind from the ship
        Vector3 down_wind = wind.transform.forward;

        // getting the vector that will push the boat in the direction of the wind
        Vector3 trim_facing = trim.transform.forward;

        // getting the vector that the rudder will send the ship towards
        heading_rudder = Vector3.Reflect(rudder_face, ship.transform.right);

        Vector3 cross_rudder_left = Vector3.Cross(heading_rudder, ship.transform.up);
        Vector3 cross_rudder_right = Vector3.Reflect(Vector3.Reflect(cross_rudder_left, ship.transform.right), ship.transform.forward);


        // getting the angle of the rudder in relation to the ship
        dot_rudder = Vector3.Dot(heading_rudder, ship.transform.forward);

        // getting the angle of the trim in relation to the ship
        dot_trim = Vector3.Dot(trim_facing, ship.transform.forward);

        // getting the angle of the wind in relatin to the trim
        dot_wind = Vector3.Dot(trim_facing, down_wind);

        wind_power = Vector3.Dot(trim_facing, down_wind);
        if (wind_power < 0)
        {
            wind_power = 0;
        }

        wind_angle = Vector3.Angle(trim_facing, down_wind);
        /*
        if (wind_angle < 0)
        {
            wind_angle = 90 + wind_angle;
        } else
        {
            wind_angle = 90 - wind_angle;
        }
        */
        rudder_angle = Vector3.Angle(rudder_face, ship.transform.forward);

        // trim power adds power in the z direction of the angle of the wind
        trim_power = Mathf.Cos((wind_angle * (Mathf.PI)) / 180);
        rudder_power = Mathf.Cos((rudder_angle * (Mathf.PI)) / 180);
        rudder_power_cross = Mathf.Sin((rudder_angle * (Mathf.PI)) / 180);

        //trim_power = Vector3.Dot(trim_facing, down_wind);
        if (trim_power < 0.1)
        {
            trim_power = 0;
        }

        Vector3 rot_axis = Vector3.Cross(ship.transform.position, world.transform.position);
        Vector3 rotate_vec = rudder.transform.right;
        rotate_count_vec = -rudder.transform.right;
        Vector3 rotate_offest_vec = rudder.transform.rotation * rb.velocity * -0.3f;

        
        Debug.DrawLine(ship.transform.position + (ship.transform.up * 10), rot_axis * -70, Color.black);
        //Debug.DrawLine(ship.transform.position + (ship.transform.up * 10), rb.velocity * -70, Color.black);
        Debug.DrawLine(ship.transform.position + (ship.transform.up * 10), rb.velocity * -100, Color.magenta);
        Debug.DrawLine(ship.transform.position + (ship.transform.up * 10), heading_rudder * 50, Color.yellow);
        Debug.DrawLine(ship.transform.position + (ship.transform.up * 10), trim_facing * 50, Color.green);
        //Debug.DrawLine(ship.transform.position + (ship.transform.up * 10), rotate_vec * 50, Color.red);
        Debug.DrawLine(ship.transform.position + (ship.transform.up * 10), rotate_count_vec * 50, Color.cyan);
        Debug.DrawLine(ship.transform.position + (ship.transform.up * 10), down_wind * 50, Color.blue);

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, 5);
    }

    private void FixedUpdate()
    {
        float sail_power = 0;
        if (!anchor_isDown)
        {
            if (ship_actions.sailsDown)
            {
                sail_power = 1;
            }

            float rudder_dir = -1;
            if (rudder_face.x > 0)
            {
                rudder_dir = 1;
                rotate_count_vec = rudder.transform.right;
            }

            // General wind force
            
            rb.AddForce(-ship.transform.forward * trim_power * wind_power * sail_power * (0.65f - rudder_power_cross), ForceMode.Acceleration);

            // Current version to demo
            //rb.AddForceAtPosition(-ship.transform.forward * trim_power * wind_power * sail_power * (0.65f - rudder_power_cross), Vector3.zero, ForceMode.Acceleration);

            // Rotating world during turn
            // this one works -> world.transform.Rotate(Vector3.up, Mathf.Max(rudder_angle - (Mathf.Pow(rudder_angle, -2)), 0) * rudder_dir * 0.01f * rb.velocity.magnitude * wind_power * Time.deltaTime, Space.World);

            //rb.MoveRotation(Quaternion.RotateTowards(world.transform.rotation, Quaternion.Euler(0, rudder_angle * rudder_dir, 0), Time.deltaTime * 240));

            //rb.AddRelativeTorque(Vector3.Cross(world.transform.position, ship.transform.forward) * (4.5f - rb.velocity.magnitude) * rudder_dir);

            rb.AddRelativeTorque(ship.transform.up * (4.5f - rb.velocity.magnitude) * 0.005f  * rudder_dir * (rudder_angle/20f) * rudder_power_cross);
            //rb.AddRelativeTorque(ship.transform.up * (4.5f - rb.velocity.magnitude) * 0.01f * rudder_dir * rudder_angle * (rudder_power_cross/2));

            //world.transform.Rotate(Vector3.up, (rudder_angle*  rudder_dir) + (rudder_power_cross * rb.velocity.magnitude) * wind_power * Time.deltaTime, Space.World);

            /*
            // Forces on world to give appearecne of turning with momentum
            rb.AddForce(-ship.transform.forward * rudder_power_cross * (Mathf.Cos(rudder_angle) * rb.velocity.magnitude) * trim_power * wind_power * sail_power, ForceMode.Acceleration);
            rb.AddForce(rotate_count_vec * rudder_power_cross * rb.velocity.magnitude * 0.1f, ForceMode.Acceleration);
            rb.AddForce(heading_rudder * rudder_power_cross * (Mathf.Sin(rudder_angle) * rb.velocity.magnitude) * 0.2f * trim_power * wind_power * sail_power, ForceMode.Acceleration);
            */


            //world.transform.SetPositionAndRotation(ship.transform.position - world.transform.forward, world.transform.rotation);
            //rb.MovePosition(ship.transform.forward);



        } else
        {
            rb.AddForce(-rb.velocity * 0.97f, ForceMode.VelocityChange);
        }
    }
}
