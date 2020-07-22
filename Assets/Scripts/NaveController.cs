    using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NaveController: MonoBehaviour
{
    [Header("Tiro")]
    public GameObject prefabProjetil; //Objeto do projétil
    public float velocidadeProjetil; //Velocidade do projétil
    public GameObject shotPoint; //Local por onde sai o projétil
    public float tempoDoTiro;
    public AudioSource tiro;





    [Header("Movimento")]
    public int direccaoX;
    public int direccaoY;
    public float speed;

    [Header("Componentes de Vida")]
    public int unidadeVida = 3;
    public Animator animator;
    //public Text vidaText;
    public AudioSource explosao;



    void Start()
    {
        animator = GetComponent<Animator>();
     
    }

    void Update()
    {
        direcao();
        transform.Translate(direccaoX*Time.deltaTime*speed,direccaoY*Time.deltaTime*speed,0);
        Atirar();
        //vidaText.text = unidadeVida.ToString();
    }

    public void direcao()
    {
    if (Input.GetMouseButtonDown(0))
        {
            direccaoX = direccaoX* -1 ;       
        }
    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Alien"))
        {
            DanoNave();
        }
    }
    void Perdeu()
    {
        
        SceneManager.LoadScene("gameover");
    }

    void Atirar()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {

            GameObject projectile = Instantiate(prefabProjetil, shotPoint.transform.position, transform.rotation);

            projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, velocidadeProjetil);
            tiro.Play();

        }
       
    }

     
    public void DanoNave()
    {
        unidadeVida = unidadeVida - 1;
                //StartCoroutine(animacaoDano()); Depois mudar para Start Coroutine
        animator.SetBool("dano", true);
        Invoke("FimDano", 1);

        if (unidadeVida <= 0)
        {
            FimDano();
            animator.SetBool("morto", true);
            Invoke("Perdeu", 1.5f);
            gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            explosao.Play();


        }
    }

    public void AumentarVida(int health)
    {
        unidadeVida += health;
    }
     
    public void FimDano()
    {
        animator.SetBool("dano", false);
       // GetComponent<PolygonCollider2D>().enabled = true;
    }
}
