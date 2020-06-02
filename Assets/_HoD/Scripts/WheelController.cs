using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    private GameObject axile;

    // Start is called before the first frame update
    void Start()
    {
        axile = GameObject.Find("axile");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rot = axile.transform.rotation.eulerAngles;

        if (Input.GetKeyDown(KeyCode.A))
        {
            rot.z++;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            rot.z--;
        }

        axile.transform.rotation = Quaternion.Euler(rot);
    }
}
