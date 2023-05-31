using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class coche : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform[] objective;
    public int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
           
       
        agent.destination = objective[i].position;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Respawn")
        {
            i++;
            if (i >= objective.Length)
            {
                i = 0;
            }
        }
    }
}
