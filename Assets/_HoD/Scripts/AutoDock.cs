using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDock : MonoBehaviour
{
    public LoadingDock[] features;

    public Rigidbody world;

    public bool auto_docking;

    public float radius;
    // Start is called before the first frame update
    void Start()
    {
        auto_docking = false;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var f in features) {
            if (radius > Vector3.Distance(Vector3.zero, f.transform.position))
            {
                auto_docking = true;
                // think of a way to specifiy the boarder of a featuer with a dock on it. Perhaps a perimeter of game objects that asjust the curve.
            }
        }
    }
}
