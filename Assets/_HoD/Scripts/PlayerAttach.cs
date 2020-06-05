using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttach : MonoBehaviour
{
    //public GameObject player;
    //public GameObject playerVR;

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("Crate"))
        {
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.transform.parent = this.transform;
            other.transform.rotation = transform.rotation;

        }
    }
    /*
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
    */
}
