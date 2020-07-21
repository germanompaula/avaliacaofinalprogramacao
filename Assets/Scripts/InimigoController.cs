using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InimigoController : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidade = 0f;
    public float min, max;
    public float tempoVida = 0f;

    [Header("Componentes de Vida")]
    public Animator animator;
    public float hp, hpMax = 100;
    public float damage = 20;
    public string damageTag;
    public Image vida;// seve para mostrar a quantidade de vida do inimigo

    public GameObject pontuacao;




    // Start is called before the first frame update
    void Start()
    {
        float x = Random.Range(min, max);
        transform.localPosition = new Vector3(x, transform.localPosition.y, 0);

        hp = hpMax;
        animator = GetComponent<Animator>();

        pontuacao = GameObject.FindGameObjectWithTag("Pontuador");

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, -velocidade * Time.deltaTime, 0);
        Invoke("Destruir", tempoVida);
        UpdateHealthBar();
        

        if(hp <= 0)
        {
            gameObject.GetComponent<PolygonCollider2D>().enabled = false;
           
        }
        else
        {
            gameObject.GetComponent<PolygonCollider2D>().enabled = true;
        }

        
    }


    public void DanoInimigo()
    {
        hp -= damage;

        if (hp <= 0f)
        {
            animator.SetBool("morto", true);
            Invoke("Destruir", .7f);

            pontuacao.GetComponent<Pontuacao>().PontuacaoNave(1);
        }
    }

    private void UpdateHealthBar()
    {
       vida.fillAmount = hp / hpMax;
    }

    void Destruir()
    {
        Destroy(gameObject);
    }

}
