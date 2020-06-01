using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RudderController : MonoBehaviour
{
    private GameObject rudder;

    // Start is called before the first frame update
    void Start()
    {
        rudder = GameObject.Find("rudder");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rot = rudder.transform.rotation.eulerAngles;

        if (Input.GetKeyDown(KeyCode.A))
        {
            rot.y++;
        } else if (Input.GetKeyDown(KeyCode.D))
        {
            rot.y--;
        }

        rudder.transform.rotation = Quaternion.Euler(rot);
    }
}
