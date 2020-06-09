using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateAttach : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("Crate"))
        {
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.transform.parent = this.transform;
            other.transform.rotation = this.transform.rotation;

        }
    }
}
