using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carEngine : MonoBehaviour
{
    public Transform path;
    private int currentNode = 0;
    private List<Transform> nodes = new List<Transform>();
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
    }
        // Update is called once per frame
        void Update()
    {
        
    }
    private void FixedUpdate()
    {
        ApplySteer();
        Drive();
        CheckPointDistance();
    }

    private void CheckPointDistance()
    {
        if(Vector3.Distance(transform.position, nodes[currentNode].position) < 0.5f)
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
        if(currentSpeed <maxSpeed)
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
}
