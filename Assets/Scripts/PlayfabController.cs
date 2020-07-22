using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using TMPro;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class PlayfabController : MonoBehaviour
{
    [Header("Dados do Jogador Playfab")]
    public string NickName; //armazena o nome de usuário do jogador
    public static string PlayFabID; //armazena o ID do Playfab do jogador
    private string userEmail; //armazena o email do jogador
    private string userPassword; //armazena a senha do jogador
    private string username; //armazena o nome de usuario de jogador

    [Header("Menus")]
    public GameObject MenuLogin; //Painel de login que será inativado quando efetuado o login.
    public GameObject MenuCriarConta; //Painel de criação de conta de usuário que será inativado quando efetuado o login.
    public GameObject MenuPrincipal; //Painel do menu principal do jogo que será ativado quando efetuado o login.
    public GameObject MenuMensagem; //Painel que exibe feedbacks para o jogador

    //Campos utilizados para efetuar o login do jogador
    [Header("Login")]
    public TMP_InputField inputUserEmailLogin; //Campo para o jogador informar o email de acesso
    public TMP_InputField inputUserSenhaLogin; //Campo para o jogador informar a senha de acesso
    public Toggle lembrarme; //Determina se os dados do jogador serão salvos ou não

    //Campos utilizados para criar uma nova conta para o jogador
    [Header("Criar Conta")]
    public TMP_InputField inputUsername;  //Campo para o jogador informar o username para cadastro
    public TMP_InputField inputEmail; //Campo para o jogador informar o email para cadastro
    public TMP_InputField inputConfirmarEmail; //Campo para o jogador confirmar o email para cadastro
    public TMP_InputField inputSenha; //Campo para o jogador informar a senha para cadastro
    public TMP_InputField inputConfirmarSenha; //Campo para o jogador confirmar a senha para cadastro

    [Header("Mensagem ao Jogador")]
    //Texto utilizado para passar informação ao jogador
    public TextMeshProUGUI Mensagem;

    //Singleton
    public static PlayfabController instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        //Preenche o campo de usuário automaticamente com a última informação salva
        if(PlayerPrefs.GetString("Lembrar") == "true")
        {
            inputUserEmailLogin.text = PlayerPrefs.GetString("User");
        }
    }

    #region Login



    //Método para criar uma conta de usuário
    public void Btn_CriarConta()
    {
        if (string.IsNullOrEmpty(inputUsername.text) || string.IsNullOrEmpty(inputEmail.text) || string.IsNullOrEmpty(inputSenha.text))
        {
            //Caso ao menos um dos campos esteja em branco, é solicitado ao jogador que os preencha.
            MostrarMensagem("Preencha todos os campos!");
        }
        else if(DadosValidosParaCriarConta())
        {
            //Caso os campos estejam preenchidos, é efetuada a chamada de criação da conta com os dados informados
            username = inputUsername.text;
            userEmail = inputEmail.text;
            userPassword = inputSenha.text;
            var registerRequest = new RegisterPlayFabUserRequest { Email = userEmail, Password = userPassword, Username = username };
            PlayFabClientAPI.RegisterPlayFabUser(registerRequest, ContaCriadaComSucesso, FalhaCriacaoConta);
        }
    }

    private bool DadosValidosParaCriarConta()
    {
        string erro = ""; //Variável para informar que erro apresentou

        //verifica se todos os campos estão preenchidos
        if (inputEmail.text == "" || inputConfirmarEmail.text == "" ||
            inputSenha.text == "" || inputConfirmarSenha.text == "" ||
            inputUsername.text == "" ) erro += "- Todos os campos devem estar preenchidos corretamente!\n";
        
        //verifica se o email informado confere
        if (inputEmail.text != inputConfirmarEmail.text) erro += "- O email não confere!\n";
        
        //verifica se a senha informada confere
        if (inputSenha.text != inputConfirmarSenha.text) erro += "- A senha não confere!\n";

        if (erro != "")
        {
            //Mostra os erros encontrados
            MostrarMensagem("Não foi possível efetuar o login. \n MOTIVO(S):\n" + erro);
            return false;
        }
        else
        {
            return true;
        }
    }

    //Método para tratamento em caso de sucesso com a chamada de Criação da conta de usuário
    private void ContaCriadaComSucesso(RegisterPlayFabUserResult result)
    {
        //Conta criada com sucesso

        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName = username },
        sucesso =>
        {
            var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword };
            PlayFabClientAPI.LoginWithEmailAddress(request, LoginEfetuado, LoginEmailFalha);
        }
        , falha =>
        {
            Debug.Log(falha.ErrorMessage);
        });

    }


    //Método para tratamento em caso de falha com a chamada de Criação de conta
    private void FalhaCriacaoConta(PlayFabError error)
    {
        //Tratamento para alguns tipos de falha. Retornando com a mensagem ao jogador
        switch (error.Error)
        {
            case PlayFabErrorCode.InvalidParams:
                //dados informados inválidos
                MostrarMensagem("Não foi possível criar a sua conta!\nVerifique os dados informados!");
                break;

            case PlayFabErrorCode.UsernameNotAvailable:
                // Nome de usuário já em uso
                MostrarMensagem("Não foi possível criar a sua conta!\nNome de usuário já em uso!");
                break;

            case PlayFabErrorCode.EmailAddressNotAvailable:
                // Email ou senha inválidos
                MostrarMensagem("Já possui uma conta para o e-mail informado!");
                break;

            default:
                //Erro inesperado!
                Debug.LogError(error.ErrorMessage);
                MostrarMensagem(error.ErrorMessage);
                break;
        }
    }


    //Método para efetuar o Login do jogador
    public void Btn_Entrar()
    {
        if (string.IsNullOrEmpty(inputUserEmailLogin.text) || string.IsNullOrEmpty(inputUserSenhaLogin.text))
        {
            //Caso ao menos um dos campos esteja em branco, é solicitado ao jogador que os preencha.
            MostrarMensagem("Preencha todos os campos!");
        }
        else if(DadosValidosParaLogin())
        {
            //Caso os campos estejam preenchidos, é efetuada a chamada de login com os dados informados
            userEmail = inputUserEmailLogin.text;
            userPassword = inputUserSenhaLogin.text;
            Login();
        }
    }


    private bool DadosValidosParaLogin()
    {
        string erro = "";

        //verifica se os campos email e senha estão vazios
        if (inputUserEmailLogin.text == "") erro += "- Preencha o campo Email ou usuário!\n";
        if (inputUserSenhaLogin.text == "") erro += "- Preencha o campo Senha!\n";

        if (erro != "")
        {
            //Mostra as mensagens de erro encontradas
            MostrarMensagem("Não foi possível efetuar o login. \n MOTIVO(S):\n" + erro);
            return false;
        }
        else
        {
            return true;
        }
    }
  
    
    public void Login()
    {
        //Chama a API para efetuar o Login com a plataforma PLAYFAB
        var request = new LoginWithPlayFabRequest { Username = inputUserEmailLogin.text, Password = inputUserSenhaLogin.text };
        PlayFabClientAPI.LoginWithPlayFab(request, LoginEfetuado, FalhaLogin);
    }


    private void LoginEfetuado(LoginResult resultado)
    {
        //Login efetuado com sucesso

        //Salva informação de entrada (login) na máquina do jogador caso assim ele deseje
        if (lembrarme.isOn)
        {
            PlayerPrefs.SetString("Lembrar", "true");
            PlayerPrefs.SetString("User", inputUserEmailLogin.text);
        }
        else
        {
            PlayerPrefs.SetString("Lembrar", "false");
            PlayerPrefs.SetString("User", "");
        }
        //Salva o PlayFabID do jogador
        PlayFabID = resultado.PlayFabId;

        //Salva informação do NickName
        PegaDisplayName(PlayFabID);

        //Retira o menu login da tela
        MenuLogin.SetActive(false);

        //Retira o Menu criar conta da tela
        MenuCriarConta.SetActive(false);

        //Exibe o menu principal do jogo
        MenuPrincipal.SetActive(true);
    }


    private void FalhaLogin(PlayFabError erro)
    {
        var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword };
        PlayFabClientAPI.LoginWithEmailAddress(request, LoginEfetuado, LoginEmailFalha);
    }


    //Método para tratamento em caso de falha com a chamada de Login
    private void LoginEmailFalha(PlayFabError error)
    {
        //Tratamento para alguns tipos de falha. Retornando com a mensagem ao jogador
        switch (error.Error)
        {
            case PlayFabErrorCode.AccountNotFound:
                // Conta não encontrada
                MostrarMensagem("Não foi possível efetuar o login!\nConta não encontrada!");
                break;

            case PlayFabErrorCode.InvalidEmailOrPassword:
                // Email ou senha inválidos
                MostrarMensagem("Não foi possível efetuar o login!\nE-mail ou senha inválidos");
                break;

            default:
                // erro inesperado!
                MostrarMensagem("Não foi possível efetuar o login!\nVerifique os dados informados!");
                break;
        }
    }
    #endregion

    #region Outros Métodos
    //Método utilizado para exibir uma mensagem ao jogador sobre a solicitação que ele fez.
    public void MostrarMensagem(string texto)
    {
        Mensagem.text = texto;
        MenuMensagem.SetActive(true);
    }

    public void PegaDadosJogador(string id)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = PlayFabID,
            Keys = null
        }, result => {

            if (result.Data == null || !result.Data.ContainsKey(id))
            {
                Debug.Log("Conteúdo vazio!");
            }

            else if (result.Data.ContainsKey(id))
            {
                PlayerPrefs.SetString(id, result.Data[id].Value);
            }

        }, (error) => {
            Debug.Log(error.GenerateErrorReport());
        });
    }

    public void SalvaDadosJogador(string id, string valor)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() {
                {id, valor}
            }
        },
        result => Debug.Log("Dados do jogador atualizados com sucesso!"),
        error => {
            Debug.Log(error.GenerateErrorReport());
        });
    }

    public void PegaDisplayName(string playFabId)
    {
        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest()
        {
            PlayFabId = playFabId,
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowDisplayName = true
            }
        },
        result => NickName = result.PlayerProfile.DisplayName,
        error => Debug.Log(error.ErrorMessage));
    }
    #endregion    

}
