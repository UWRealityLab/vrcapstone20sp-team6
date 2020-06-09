using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverItem : MonoBehaviour
{

    public GameObject delivery_spot;
    public ParticleSystem[] fireworks;
    private bool delivered = false;
    private bool setOffFireworks = false;
    //public GameObject firework;
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Crate"))
        {
            Debug.Log("Delivered");
            other.gameObject.SetActive(false);

            // removing crate from platform
            other.transform.parent = null;
            other.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            other.gameObject.transform.position = delivery_spot.transform.position;
            
            other.gameObject.SetActive(true);

            delivered = true;
            setOffFireworks = true;
        }
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        fireworks = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem firework in fireworks)
        {
            firework.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (setOffFireworks) {
            for (int i = 0; i < fireworks.Length * 2; i++)
            {
                int index = Random.Range(0, fireworks.Length - 1);
                StartCoroutine(fireworkAnim(fireworks[index]));
            }
            setOffFireworks = false;
        }
    }

    // Coroutine function to play firework particle after random time
    private IEnumerator fireworkAnim(ParticleSystem firework)
    {
        float randomWait = Random.Range(1f, 3.0f);
        yield return new WaitForSeconds(randomWait);
        firework.Play();
    }
}
