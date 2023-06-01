using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour
{
    public GameObject trash;
    public GameObject textOBJ;
    public GameObject Speed;
    public GameObject position;
    public GameObject Loops;

    public GameObject player;
    public NewBehaviourScript beh;
    public Rigidbody rb;
    private TextMeshProUGUI text;
    private TextMeshProUGUI speed;
    private TextMeshProUGUI Position;
    private TextMeshProUGUI Lap;
    private int countDown = 5;
    public GameObject[] Cars;
    public float[] carsSpeed;
    //Se incrementara en el player
    public int count = 0;
    
    int lap = 1;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i<Cars.Length; i++)
        { 
            carsSpeed[i] = Cars[i].GetComponent<carEngine>().maxSpeed;
            Cars[i].GetComponent<carEngine>().maxSpeed = 0f;
        }
        text = textOBJ.GetComponent<TextMeshProUGUI>();
        speed = Speed.GetComponent<TextMeshProUGUI>();
        Position = position.GetComponent<TextMeshProUGUI>();
        Lap = Loops.GetComponent<TextMeshProUGUI>();
        StartCoroutine(StartGame());
        //StartCoroutine(EliminarBasura(trash, 30f));
        Lap.text = lap.ToString() + "/3";

        rb = player.GetComponent<Rigidbody>();
        beh = player.GetComponent<NewBehaviourScript>();
    }

    // Update is called once per frame
    void Update()
    {
        Lap.text = beh.volta.ToString() + "/3"; 
        
        
        speed.text = ((int)(rb.velocity.magnitude * 10)).ToString();
        
    }
    
    IEnumerator StartGame()
    {
        text.text = countDown.ToString();
        yield return new WaitForSeconds(1f);
        countDown--;
        if(countDown == 0)
        {
            text.text = "GO";
            for (int i = 0; i < Cars.Length; i++)
            {
                Cars[i].GetComponent<carEngine>().maxSpeed = carsSpeed[i];
               
            }
            StartCoroutine(EliminarBasura(textOBJ, 1f));
        }
        else
        {

            text.text = countDown.ToString();
            StartCoroutine(StartGame());
        }
        
    }
    public void ChangeStage()
    {
        SceneManager.LoadScene(1);
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void Tutorial()
    {

    }
    IEnumerator EliminarBasura(GameObject obj, float f)
    {
        
        
        yield return new WaitForSeconds(f);
        Destroy(obj);

    }
}
