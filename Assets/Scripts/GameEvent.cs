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
            GameObject.DontDestroyOnLoad(instance);
        }
    }

    #endregion

    public event Action<string, string> OnLogin;
    public event Action OnLoginSuccessed;
    public event Action<string> OnLoginFailed;


    public void Login(string email, string pass) => OnLogin?.Invoke(email, pass);
    public void LoginSuccessed() => OnLoginSuccessed?.Invoke();
    public void LoginFailed(string msg) => OnLoginFailed?.Invoke(msg);



}
