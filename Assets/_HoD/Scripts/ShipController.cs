using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShipController : MonoBehaviourPunCallbacks, IPunObservable
{
    public WindGeneration wind;
    public GameObject player;
    public float shipSpeed;
    public float shipHelm;
    public GameObject enviro;
    public Camera mainCamera;

    private Vector3 wind_direction;
    private Transform player_pos;
    private Rigidbody ship_rb;
    private Rigidbody enviro_rb;

    [SerializeField]
    private GameObject ship;


    public GameObject wheel;
    private Vector3 last_place;
    //private OVRPlayerController vrPlayer;

    // Start is called before the first frame update
    void Start()
    {
        // get wind variable
        // get sail configuration
        // get rudder heading
        // check water reading --> what way are the wave pointing as a result of the wind
        ship = GameObject.Find("Colonial Ship");
        ship_rb = ship.GetComponent<Rigidbody>();
        enviro_rb = enviro.GetComponent<Rigidbody>();
        last_place = this.transform.position;
        //wheel = GameObject.Find("Wheel");
        mainCamera.transform.parent = this.transform;

        //shipHelm = wheel.transform.rotation.z;

        //player.transform.SetParent(ship.transform);
        //player.transform.parent = ship.transform;
        //enviro_rb.AddForce(wind_direction * wind.speed);
    }

    // Update is called once per frame
    void Update()
    {
        // use for input
        wind_direction = wind.heading;
        shipHelm = wheel.transform.rotation.z;
        //enviro_rb.AddForce(wind_direction * wind.speed);
    }

    private void FixedUpdate()
    {
        // use for physics
        //ship_rb.AddForce(wind_direction);
        //ship_rb.MovePosition(transform.position + (wind_direction * wind.speed * shipSpeed* Time.deltaTime));
        //ship_rb.velocity = wind_direction * wind.speed * shipSpeed;
        //this.transform.localRotation = new Vector3(0f, 0f, (shipHelm * shipSpeed));
        this.transform.localEulerAngles = new Vector3(0f, (shipHelm * 100), 0f);
        
        //enviro_rb.AddForce((wind_direction + Vector3.Cross(this.transform.position, last_place)) - ();

        float drag = Vector3.Dot(this.transform.position, wind_direction);
        Vector3 cut = Vector3.Cross(this.transform.position, wind_direction);
        //enviro_rb.velocity = cut * wind.speed * shipSpeed;
        Rigidbody ship = this.GetComponent<Rigidbody>();

        // two best sailing methods
        enviro_rb.velocity = ((-1) * wind_direction) * wind.speed * shipSpeed;
        //ship.AddForce(wind_direction * wind.speed * shipSpeed);

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(ship.transform.position);
            stream.SendNext(ship.transform.rotation);
        }

        ship.transform.position = (Vector3)stream.ReceiveNext();
        ship.transform.rotation = (Quaternion)stream.ReceiveNext();
    }
}
