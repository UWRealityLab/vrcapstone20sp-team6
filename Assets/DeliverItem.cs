using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverItem : MonoBehaviour
{

    public GameObject delivery_spot;
    public ParticleSystem particles;
    private bool delivered = false;
    //public GameObject firework;
    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("Crate"))
        {
            Debug.Log("Delivered");
            other.gameObject.SetActive(false);

            // removing crate from platform
            other.gameObject.transform.parent = null;

            other.gameObject.transform.position = delivery_spot.transform.position;
            other.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            other.gameObject.SetActive(true);

            delivered = true;

            //particles.Play();

            /*
            //Here we check if we indeed find the Particle system and can use it
            //or else we would get an error if we work with not existing component
            if (particles != null)
            {
                //if (other.CompareTag("Ground"))
                //{
                particles.Play(); //Here we use the Play function to start the particle system
                                  //}
                                  //else
                                  //{
                //particles.Stop(); //Here we use the Stop function to stop the particle system from playing
            }
            */
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        particles.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (delivered) {
            particles.GetComponent<ParticleSystem>().Play();
            
        }
    }
}
