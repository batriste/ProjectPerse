using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float acceleration = 500f;
    private float currentAcceleration = 0f;
    public float breakingForce = 300f;
    private float currentBreakingForce = 0f;

    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider backRight;
    [SerializeField] WheelCollider backLeft;

    private float currentTurnAngle = 0f;
    private float maxTurnAngle = 15f;

    [SerializeField] Transform FrontRightTransform;
    [SerializeField] Transform FrontLeftTransform;
    [SerializeField] Transform BackRightTransform;
    [SerializeField] Transform BackLeftTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void FixedUpdate()
    {
        currentAcceleration = acceleration * Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.Space))
        {
            currentBreakingForce = breakingForce;
        }
        else
        {
            currentBreakingForce = 0f;
        }
        frontRight.motorTorque = currentAcceleration;
        frontLeft.motorTorque = currentBreakingForce;

        frontRight.brakeTorque = currentBreakingForce;
        frontLeft.brakeTorque = currentBreakingForce;
        backLeft.brakeTorque = currentBreakingForce;
        backRight.brakeTorque = currentBreakingForce;

        currentTurnAngle = maxTurnAngle * Input.GetAxis("Horizontal");
        frontLeft.steerAngle= currentTurnAngle;
        frontRight.steerAngle= currentTurnAngle;
        UpdateWheel(frontLeft, FrontLeftTransform);
        UpdateWheel(frontRight, FrontRightTransform);
        UpdateWheel(backRight, BackRightTransform);
        UpdateWheel(backLeft, BackLeftTransform);
    }

    void UpdateWheel(WheelCollider col, Transform trans)
    {
        Vector3 position;
        Quaternion rotation;
        col.GetWorldPose(out position, out rotation);
        
        trans.position = position;
        trans.rotation = rotation;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
