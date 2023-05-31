using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    public Color linecolor;
    private List<Transform> nodes= new List<Transform>();
    // Start is called before the first frame update
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = linecolor;
        Transform[] pathTransform = GetComponentsInChildren<Transform>();
        //No me gusta que cree una lista nueva but okay
        nodes = new List<Transform>();

        foreach (Transform path in pathTransform)
        {
            nodes.Add(path);
        }

        for(int i = 0; i < nodes.Count; i++)
        {
            Vector3 currentNode = nodes[i].position;
            Vector3 previusNode = Vector3.zero;
            if (i > 0)
            {
                previusNode = nodes[i - 1].position;
            }else if(i == 0 && nodes.Count> 1) {
                previusNode= nodes[nodes.Count - 1].position;
            }
            Gizmos.DrawLine(previusNode, currentNode);
            Gizmos.DrawSphere(currentNode, 0.5f);
        }
    }
    
}
