using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RudderInfo : MonoBehaviour
{
    public Text rudder_info;

    private float _syntheticAngle;
    private float _prevAngle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var rudder_angle = transform.localEulerAngles.y;


        rudder_angle = (rudder_angle > 180) ? rudder_angle - 360 : rudder_angle;
        if (float.IsNaN(_syntheticAngle))
        {
            _syntheticAngle = rudder_angle;
            _prevAngle = rudder_angle;
        }
        float dAngle = rudder_angle - _prevAngle;
        if (dAngle < -180)
        {
            // eg it hopped from -170 to +170
            dAngle += 360;
        }
        else if (dAngle > 180)
        {
            // eg it hopped from +170 to -170
            dAngle -= 360;
        }
        _syntheticAngle += dAngle;

        rudder_info.text = rudder_angle.ToString();
    }
}
