using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using System;
using Firebase.Extensions;
public class Database : MonoBehaviour
{

    FirebaseAuth auth;
    FirebaseUser user;

    public static string  error;
    public void Awake()
    {
        CheckDatabase();
    }

    void CheckDatabase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            DependencyStatus dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
                Debug.Log("Firebase is ready for use.");
            }
            else
            {
                Debug.LogError(String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));

            }
        });
    }
    public  void Start()
    {
        GameEvent.instance.OnLogin += TryLogin; 

    }

   

    private void OnDestroy()
    {
       GameEvent.instance.OnLogin -= TryLogin;
 
    }

    void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    void TryLogin(string email,string pass)
    {
        StartCoroutine(Login(email,pass));
    }

    IEnumerator Login(string email, string password)
    {
        //Call the Firebase auth signin function passing the email and password
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(predicate: () => loginTask.IsCompleted);

        //If there are no errors
        if (loginTask.Exception == null)
        {
            user = loginTask.Result;
            //Debug.LogFormat("User signed in successfully: {0} ({1})", user.DisplayName, user.Email);
            GameEvent.instance.LoginSuccessed();
        
        }
        else
        {
            //If there are errors handle them
            //Debug.LogWarning(message: $"Failed to register task with {loginTask.Exception}");
            FirebaseException firebaseEx = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
            
            HandlerError(errorCode);
            
        }
    }

    void HandlerError(AuthError errorCode)
    {
        string message = "Login Failed";
        switch (errorCode)
        {
            //Empty values
            case AuthError.MissingEmail:
                message = "Missing Email";
                break;
            //Empty values
            case AuthError.MissingPassword:
                message = "Missing Password";
                break;

            case AuthError.WrongPassword:
                message = "Wrong Password";
                break;

            case AuthError.InvalidEmail:
                message = "Invalid Email";
                break;
            case AuthError.UserNotFound:
                message = "Account does not exist";
                break;
        }

        GameEvent.instance.LoginFailed(message);
    }

    
}
