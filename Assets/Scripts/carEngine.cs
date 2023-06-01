using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carEngine : MonoBehaviour
{
    
    public Transform path;
    public int currentNode = 0;
    public List<Transform> nodes = new List<Transform>();

    private void Start()
    {
        
        Transform[] pathTransform = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < pathTransform.Length; i++)
        {
            if (pathTransform[i] != path.transform)
            {
                nodes.Add(pathTransform[i]);
            }
        }
        GetComponent<Rigidbody>().centerOfMass = centerOfMass;
    }
    public Vector3 centerOfMass; 
        // Update is called once per frame
        void Update()
    {
        
    }
    private void FixedUpdate()
    {
        Sensors();
        ApplySteer();
        Drive();
        CheckPointDistance();
        Breaking();
    }
    public void OnCollisionStay(Collision coll)
    {
        //CarlosF
        if (coll.gameObject.CompareTag("Obstacle")){
            Reposition();
        }
    }
    private void CheckPointDistance()
    {
        
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < 0.9f)
        {
            if(currentNode == nodes.Count - 1)
            {
                currentNode = 0;
            }
            else
            {
                currentNode++;
            }
        }
    }
    public float maxMotorTorque = 80f;
    public float currentSpeed;
    public float maxSpeed = 100f;
    private void Drive()
    {
        currentSpeed = 2 * Mathf.PI * wheelFrontLeft.radius * wheelFrontLeft.rpm * 60 / 100;
        if(currentSpeed <maxSpeed && !isBreaking)
        {
            wheelFrontLeft.motorTorque = maxMotorTorque;
            wheelFrontRight.motorTorque = maxMotorTorque;
        }
        else
        {
            wheelFrontLeft.motorTorque = 0; 
            wheelFrontRight.motorTorque = 0;
        }/*
          *  wheelFrontLeft.motorTorque = 500f;
          *  wheelFrontRight.motorTorque = 500f;
          */
    }

    public float maxSteerAngle = 45f;
    public WheelCollider wheelFrontLeft;
    public WheelCollider wheelFrontRight;

    private void ApplySteer()
    {
        Vector3 relativeVector =
       transform.InverseTransformPoint(nodes[currentNode].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        wheelFrontLeft.steerAngle = newSteer;
        wheelFrontRight.steerAngle = newSteer;
    }

    //T-4
    public bool isBreaking = false;
    public WheelCollider wheelBackLeft;
    public WheelCollider wheelBackRight;
    
    public float maxBreakTorque = 150f;
    private void Breaking()
    {
        if (isBreaking)
        {
            wheelBackLeft.brakeTorque = maxBreakTorque;
            wheelBackRight.brakeTorque = maxBreakTorque;
            isBreaking = false;
        }
        else
        {
            wheelBackLeft.brakeTorque = 0f;
            wheelBackRight.brakeTorque = 0f;
        }
    }
    [Header("Sensors")]
    public float sensorLength = 5f;
    public float frontSideSensorPosition = 0.2f;
    public float frontSensorAngle = 30f;
    public Vector3 frontSensorPosition = new Vector3(0f, 0.2f, 1.42f);


    private bool avoid = false;
    //Temporidazor de respawn
    public int temp = 50;
    private void Sensors()
    {
        RaycastHit hit;
        Vector3 sensorStarPose = transform.position;
        sensorStarPose += transform.forward * frontSensorPosition.z;
        sensorStarPose += transform.up * frontSensorPosition.y;
        float avoidMultiplier = 0;
        avoid = false;
        
        //Front Right
        sensorStarPose += transform.right * frontSideSensorPosition;
        if (Physics.Raycast(sensorStarPose, transform.forward, out hit, sensorLength))
        {
            if (hit.collider.CompareTag("Floor"))
            {
                
                Debug.DrawLine(sensorStarPose, hit.point);
                avoid = true;
                avoidMultiplier -= 1f;
                
            }
            else
            {
                //isBreaking = true;
            }
        }
        
        //Right Angle

        else if (Physics.Raycast(sensorStarPose, Quaternion.AngleAxis(frontSensorAngle, transform.up)*transform.forward, out hit, sensorLength))
        {
            if (hit.collider.CompareTag("Floor"))
            {
                Debug.DrawLine(sensorStarPose, hit.point);
                
                avoid = true;
                avoidMultiplier -= 0.5f;
               
            }
            else
            {
                //isBreaking = true;
            }
        }
        
        //Front Left
        sensorStarPose -= transform.right * frontSideSensorPosition * 2;
        if (Physics.Raycast(sensorStarPose,  transform.forward, out hit, sensorLength))
        {
            if (hit.collider.CompareTag("Floor"))
            {
                
                Debug.DrawLine(sensorStarPose, hit.point);
                avoid = true;
                avoidMultiplier += 1f;
                
            }
            else
            {
                //isBreaking = true;

            }
        }
        
        //Front left angle
        else if (Physics.Raycast(sensorStarPose, Quaternion.AngleAxis(-frontSensorAngle, transform.up) *
        transform.forward, out hit, sensorLength))
        {
            if (hit.collider.CompareTag("Floor"))
            {
                Debug.Log("Hola");
                Debug.DrawLine(sensorStarPose, hit.point);
                avoid = true;
                avoidMultiplier += 0.5f;
                
            }
            else
            {
                //isBreaking = true;

            }
        }
        
        //Front
        if (avoidMultiplier == 0)
        {
            if (Physics.Raycast(sensorStarPose, transform.forward, out hit, sensorLength))
            {
                if (hit.collider.CompareTag("Floor"))
                {
                    Debug.DrawLine(sensorStarPose, hit.point);
                    avoid = true;temp = 5;
                    if(hit.normal.x < 0)
                    {
                        avoidMultiplier = -1;
                    }
                    else
                    {
                        avoidMultiplier = 1;
                    }
                }else
                {
                   // isBreaking = true;

                }
                
            }
            
        }
        if (avoid)
        {
            wheelFrontLeft.steerAngle = maxSteerAngle * avoidMultiplier;
            wheelFrontRight.steerAngle = maxSteerAngle * avoidMultiplier;
        }
       
        if (transform.rotation.z > 0.2f || transform.rotation.z < -0.2f || transform.rotation.x > 0.2f || transform.rotation.x < -0.2f)
        {
            
            timerotation--;
            if (timerotation <= 0)
            {

                Reposition();
            }
        }
    }
    public int timerotation = 5;
    public void Reposition()
    {
        temp--;
        if (temp <= 0 || timerotation <= 0)
        {
            
            if (currentNode == 0)
            {
                
                transform.position = nodes[23].position;
                Vector3 eulerAngles = transform.eulerAngles;
                eulerAngles.z = 0f;
                eulerAngles.y = transform.eulerAngles.y;
                eulerAngles.x = 0f;
                transform.eulerAngles = eulerAngles;
                temp = 30;
                timerotation = 5;
            }
            else
            {
                transform.position = nodes[currentNode - 1].position;
                Vector3 eulerAngles = transform.eulerAngles;
                eulerAngles.z = 0f;
                eulerAngles.x = 0f;
                transform.eulerAngles = eulerAngles;
                temp = 30;
                timerotation = 5;
            }
            
        }
        currentSpeed = 0f;

    }
}
