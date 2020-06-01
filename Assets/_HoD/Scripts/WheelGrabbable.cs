using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelGrabbable : OVRGrabbable
{
    public GameObject handler;

    public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        //handler.GetComponent<Rigidbody>().isKinematic = false;
        base.GrabBegin(hand, grabPoint);
    }


    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        //base.GrabEnd(linearVelocity, angularVelocity);
        linearVelocity.Scale(new Vector3(1, 1, 0));
        angularVelocity.Scale(new Vector3(0, 0, 0));
        //base.GrabEnd((linearVelocity), angularVelocity);
        base.GrabEnd(Vector3.zero, Vector3.zero);

        transform.position = handler.transform.position;
        transform.rotation = handler.transform.rotation;
        //handler.GetComponent<Rigidbody>().isKinematic = true;
    }
}
