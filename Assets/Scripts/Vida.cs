using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vida : MonoBehaviour
{

    public float Duracao = 0f;
    private int health;
    //public GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        Invoke("DestruirVida", Duracao);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Nave"))
        {
            health = 1;
            other.GetComponent<NaveController>().AumentarVida(health);
            DestruirVida();
        }
    }

    public void DestruirVida()
    {
        Destroy(gameObject);
    }
}
