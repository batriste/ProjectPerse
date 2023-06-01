using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallPainting : MonoBehaviour
{
    public Material[] materiales;
    public GameObject walls;
    //Ayudado by Carlos.F ya que no tenia el inteligent pero se agradece muy fuerte
    // Start is called before the first frame update
    void Start()
    {
        int x = 0;

        for(int i = 0; i< walls.transform.childCount; i++)
        {
           
            walls.transform.GetChild(i).GetComponent<MeshRenderer>().material = materiales[x];
            x++;
            if(x >= materiales.Length)
            {
                x = 0;
            }
        }
        InvokeRepeating("ChangeWalls", 20f, 15f);
    }
    public void ChangeWalls()
    {
        for (int i = 0; i < walls.transform.childCount; i++)
        {
            walls.transform.GetChild(i).GetComponent<MeshRenderer>().material = materiales[Random.Range(0, materiales.Length)];
        }
    }
    
}
