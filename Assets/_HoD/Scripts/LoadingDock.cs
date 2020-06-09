using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingDock : MonoBehaviour
{
    public GameObject non_grab_crate;
    private void OnTriggerStay(Collider other)
    {
        
        if (other.gameObject.CompareTag("Crate"))
        {
            other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            other.gameObject.GetComponent<Rigidbody>().Sleep();
            other.gameObject.transform.rotation = this.transform.rotation;
            other.gameObject.transform.parent = this.transform;
            GameObject dopple = Instantiate(non_grab_crate, other.gameObject.transform);
            dopple.SetActive(false);
            dopple.transform.localPosition = Vector3.zero;
            dopple.transform.localScale = Vector3.one;
            dopple.transform.parent = this.transform;
            dopple.SetActive(true);
            Destroy(other.gameObject);
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
