using Com.Udomugo.HoD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrimController : MonoBehaviour
{
    public ShipActions ship_actions;
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
            ship_actions.braceDir = 1;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            ship_actions.braceDir = 2;
        } else if (Input.GetKeyDown(KeyCode.W))
        {
            ship_actions.braceDir = 0;
        } else if (Input.GetKeyDown(KeyCode.X))
        {
            ship_actions.anchorDown = !ship_actions.anchorDown;
        } else if (Input.GetKeyDown(KeyCode.F))
        {
            ship_actions.hoistDown = !ship_actions.hoistDown;
        }


        //trim.transform.rotation = main_sail.transform.rotation;
    }
}
