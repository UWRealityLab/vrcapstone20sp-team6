using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttach : MonoBehaviour
{
    public GameObject player;
    public GameObject playerVR;

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject == player)
        {
            player.transform.parent = this.transform;

        }
        
        if (other.gameObject == playerVR)
        {
            GameObject OVR = GameObject.Find("OVRPlayerController");
            OVR.transform.parent = this.transform;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            player.transform.parent = null;

        }

        if (other.gameObject == playerVR)
        {
            playerVR.transform.parent = null;

        }
    }
}
