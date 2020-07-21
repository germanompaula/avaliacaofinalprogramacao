using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pontuacao : MonoBehaviour
{
    public TMP_Text pontoText;
    //private int ponto;
    private int acum =0;

    // Start is called before the first frame update
    void Start()
    {
        acum = 0;

        pontoText.text = "Pontos: " + acum.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PontuacaoNave(int ponto)
    {


        acum = acum + ponto;

        pontoText.text = "Pontos: " + acum.ToString();

    }
}
