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


    //Respawn
    public Transform path;
    public int currentNode = 0;
    public List<Transform> nodes = new List<Transform>();

    public GameObject win;
    public GameObject lose;
    // Start is called before the first frame update
    void Start()
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
    private void CheckPointDistance()
    {

        if (Vector3.Distance(transform.position, nodes[currentNode].position) < 1.1f)
        {
            if (currentNode == nodes.Count - 1)
            {
                currentNode = 0;
            }
            else
            {
                currentNode++;
            }
        }
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
    bool t = false;
    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            if (t)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1f;
            }
            t = !t;
        }
        if (Input.GetKey(KeyCode.O))
        {
            //Lose
            lose.SetActive(!lose.activeInHierarchy);
        }
        if (Input.GetKey(KeyCode.I))
        {
            //Win
            win.SetActive(!win.activeInHierarchy);
        }
        if (Input.GetKey(KeyCode.R))
        {
          
                if (currentNode == 0)
                {

                    transform.position = nodes[23].position;
                    Vector3 eulerAngles = transform.eulerAngles;
                    eulerAngles.z = 0f;
                    eulerAngles.y = transform.eulerAngles.y;
                    eulerAngles.x = 0f;
                    transform.eulerAngles = eulerAngles;
                    
                
                }
                else
                {
                    transform.position = nodes[currentNode - 1].position;
                    Vector3 eulerAngles = transform.eulerAngles;
                    eulerAngles.z = 0f;
                    eulerAngles.x = 0f;
                    transform.eulerAngles = eulerAngles;
                    
                }
            currentAcceleration = 0f;
            

        }
    }
    public int Vuelta = 0;
    public int volta = 1;
    int i = 1;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Point") && i == 1 )
        {
            Vuelta++;
            i++;
        }
        if (other.CompareTag("Point1") && i == 2)
        {
            Vuelta++;
            i++;
        }
        if (other.CompareTag("Point2") && i == 3)
        {
            Vuelta++;
            i++;
        }
        if (other.CompareTag("Point3") && i == 4)
        {
            Vuelta++;
            i++;
        }
        if (other.CompareTag("Point4") && i == 5)
        {
            Vuelta++;
            i++;
        }
        if (other.CompareTag("Point5") && i == 6)
        {
            Vuelta++;
            i++;
        }
        if (other.CompareTag("Point6") && i == 7)
        {
            Vuelta++;
            i= 1;
        }
        
        if (Vuelta == 8 && volta == 1)
        {
            volta = 2;
            Vuelta = 1;
        }
        if (Vuelta == 8 && volta == 2)
        {
            volta = 3;
            Vuelta = 1;
        }
        if (Vuelta == 8 && volta == 3)
        {
            volta = 3;
            Vuelta = 1;
            //Win
            win.SetActive(!win.activeInHierarchy);
        }
    }
}
