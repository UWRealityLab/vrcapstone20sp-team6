using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrimController : MonoBehaviour
{
    private GameObject trim;

    // Start is called before the first frame update
    void Start()
    {
        trim = GameObject.Find("trim");
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 rot = trim.transform.rotation.eulerAngles;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            rot.y++;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            rot.y--;
        }

        trim.transform.rotation = Quaternion.Euler(rot);
    }
}
