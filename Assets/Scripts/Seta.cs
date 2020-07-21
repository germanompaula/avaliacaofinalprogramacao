using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seta : MonoBehaviour
{
    public Transform obj;
    //public float min;
    public float max;
    public Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        obj = GameObject.FindGameObjectWithTag("Nave").transform;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        anim.SetBool("dist", false);
        float distancia = (transform.position - obj.position).magnitude;

        if (distancia > max)
        {
            AlertaMovimento();
        }

    }

    public void AlertaMovimento()
    {
        anim.SetBool("dist", true);
        gameObject.GetComponent<SpriteRenderer>().enabled = true;

    }
}
