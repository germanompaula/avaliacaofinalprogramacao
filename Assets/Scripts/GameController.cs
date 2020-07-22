using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public AudioSource musicaMenu;
    public AudioSource musicaJogo;

    public static GameController instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }


    // Update is called once per frame
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Login" && !musicaMenu.isPlaying)
        {
            if (musicaJogo.isPlaying) musicaJogo.Stop();
            musicaMenu.Play();
        }
        else if (SceneManager.GetActiveScene().name == "Play")
        {
            if (musicaMenu.isPlaying) musicaMenu.Stop();
            musicaJogo.Play();
        }
    }
    
    public void Btn_Entrar()
    {
        SceneManager.LoadScene("Play");
    }

    public void Btn_Restart()
    {
        SceneManager.LoadScene("Play");
    }

    public void Btn_Sair()
    {
        Application.Quit();
    }
}
