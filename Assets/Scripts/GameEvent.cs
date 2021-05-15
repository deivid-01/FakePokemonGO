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
    public event Action<string> OnPokemonCaptured;
    #endregion

    #region Pokedex Events
    public event Action<string> OnPokedexFoundName;
    public event Action<string[]> OnPokedexFoundTypes;
    public event Action<Texture2D> OnPokedexFoundImage;

    public event Action<string> OnFindingPokemon;
    public event Action<PokemonData>  OnPokemonFound;
    public event Action  OnPokemonFoundFail;

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
    public void PokemonCaptured(string name) => OnPokemonCaptured?.Invoke(name);
    #endregion
    #region Pokedex triggers
    public void PokedexFoundName(string name) => OnPokedexFoundName?.Invoke(name);
    public void PokedexFoundTypes(string[] types) => OnPokedexFoundTypes?.Invoke(types);
    public void PokedexFoundImage(Texture2D texture) => OnPokedexFoundImage?.Invoke(texture);

    public void FindPokemon(string name) => OnFindingPokemon?.Invoke(name);
    public void PokemonFound(PokemonData pkmData) => OnPokemonFound?.Invoke(pkmData);
    public void PokemonFoundFail() => OnPokemonFoundFail?.Invoke();
    #endregion
}
