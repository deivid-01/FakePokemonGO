using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameEvent : MonoBehaviour
{

    #region Singlenton 
    public static GameEvent instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        { 
            gameObject.SetActive(false);
        }
    }

    #endregion

    #region LogIn Events
    public event Action<string, string> OnLogin;
    public event Action OnLoginSuccessed;
    public event Action<string> OnLoginFailed;
    #endregion

    #region SignUp Events
    public event Action<string,string, string,string> OnSignUp;
    public event Action<string> OnSignUpSuccessed;
    public event Action<string> OnSignUpFailed;
    #endregion


    #region GameEvents
    public event Action<Vector3> OnSpawnPokemon;
    #endregion
    #region Login Triggers
    public void Login(string email, string pass) => OnLogin?.Invoke(email, pass);
    public void LoginSuccessed() => OnLoginSuccessed?.Invoke();
    public void LoginFailed(string msg) => OnLoginFailed?.Invoke(msg);
    #endregion

    #region Signup Triggers
    public void SignUp(string username,string email, string pass, string passx2) => OnSignUp?.Invoke(username,email, pass, passx2);
    public void SignUpSuccessed(string msg) => OnSignUpSuccessed?.Invoke(msg);
    public void SignUpFailed(string msg) => OnSignUpFailed?.Invoke(msg);

    #endregion

    #region Game Triggers
    public void SpawnPokemon(Vector3 position) => OnSpawnPokemon?.Invoke(position);
    #endregion

}
