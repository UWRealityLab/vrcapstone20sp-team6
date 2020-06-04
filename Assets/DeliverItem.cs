using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverItem : MonoBehaviour
{

    public GameObject delivery_spot;
    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("Crate"))
        {
            Debug.Log("Delivered");
            other.gameObject.SetActive(false);

            // removing crate from platform
            other.gameObject.transform.parent = null;

            other.gameObject.transform.position = delivery_spot.transform.position;
            other.gameObject.SetActive(true);

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
