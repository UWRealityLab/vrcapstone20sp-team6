using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR.LegacyInputHelpers;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    public Transform PlayerTransform;

    private Vector3 _cameraOffset;

    [Range(0.01f, 1.0f)]
    public float SmoothFactor = 0.5f;

    public bool LookAtPlayer = false;

    public bool RotateAroundPlayer = true;

    public float RotationsSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        _cameraOffset = transform.position - PlayerTransform.position;    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // LateUpdate is called after update methods
    void LateUpdate()
    {
        if (RotateAroundPlayer)
        {
            Quaternion camTurnAngle =
                Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotationsSpeed, Vector3.up);
            Quaternion camTurnAngle_vet =
                Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * RotationsSpeed, Vector3.right);
            _cameraOffset = camTurnAngle * _cameraOffset;
            _cameraOffset = camTurnAngle_vet * _cameraOffset;
        }

        Vector3 newPos = PlayerTransform.position + _cameraOffset;

        transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);

        if (LookAtPlayer || RotateAroundPlayer)
        {
            transform.LookAt(PlayerTransform);
        }
    }
}
