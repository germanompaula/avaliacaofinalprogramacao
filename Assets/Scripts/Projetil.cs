using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class Projetil : MonoBehaviour
{
    public float tempoVida = 0f;
   
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*StartCoroutine(Duracao());

        IEnumerator(Duracao)
        {
            yield return new WaitForSeconds(5);
            Destroy(gameObject);
        }*/
        Invoke("DestruirProjetil", tempoVida);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.CompareTag("Alien"))
        {
            coll.GetComponent<InimigoController>().DanoInimigo();
            DestruirProjetil();
        }
    }

    void DestruirProjetil()
    {
        Destroy(gameObject);
    }
}
