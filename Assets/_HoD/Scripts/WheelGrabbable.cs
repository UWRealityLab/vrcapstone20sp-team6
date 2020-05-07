using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelGrabbable : OVRGrabbable
{
    public Transform handler;
    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        //base.GrabEnd(linearVelocity, angularVelocity);
        linearVelocity.Scale(new Vector3(0.10f, 0.10f, 0.10f));
        angularVelocity.Scale(new Vector3(0.10f, 0.10f, 0.10f));
        //base.GrabEnd((linearVelocity), angularVelocity);
        base.GrabEnd(Vector3.zero, Vector3.zero);

        transform.position = handler.transform.position;
        transform.rotation = handler.transform.rotation;
    }
}
