using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anchor : MonoBehaviour
{
    [SerializeField]
    public Transform crank;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, ((crank.transform.rotation.y/270) * -400) - transform.position.y, 0);
    }
}
