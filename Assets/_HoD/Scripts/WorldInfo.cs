using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldInfo : MonoBehaviour
{
    public Text world_vel;
    public Text x_vel;
    public Text y_vel;
    public Text z_vel;
    private Rigidbody world_rb;
    // Start is called before the first frame update
    void Start()
    {
        world_rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        world_vel.text = world_rb.velocity.magnitude.ToString();
        x_vel.text = world_rb.velocity.x.ToString();
        y_vel.text = world_rb.velocity.y.ToString();
        z_vel.text = world_rb.velocity.z.ToString();
    }
}
