using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour
{
    public GameObject obj;
    public float spawnRate;
    public float tempo = 0f;

 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    void Update()
    {
       if(Time.time > tempo)
       {
            tempo = Time.time + spawnRate;
           Instantiate(obj, transform.position, obj.transform.rotation);
       }

    }

}
