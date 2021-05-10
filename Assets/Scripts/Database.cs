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
        GameEvent.instance.OnSignUp += TrySignUp;

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
    void TrySignUp(string username,string email, string pass, string passx2)
    {
        StartCoroutine(SignUp(username,email, pass,passx2));
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


    IEnumerator SignUp(string username, string email, string password, string passwordx2)
    {
        if (ValidateData(username, password, passwordx2))
        {
            //Call the Firebase auth signin function passing the email and password
            var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
            //Wait until the task password
            yield return new WaitUntil(predicate: () => registerTask.IsCompleted);

            if (registerTask.Exception != null)
            {
                FirebaseException firebaseEx = registerTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                HandlerErrorSignUp(errorCode);
            }
            else
            {
                StartCoroutine(SetUser(username, registerTask.Result));

            }
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

    void HandlerErrorSignUp(AuthError errorCode)
        {
            string message = "Register Failed";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing password";
                    break;
                case AuthError.WeakPassword:
                    message = "Weak password";
                    break;
                case AuthError.EmailAlreadyInUse:
                    message = "Email already in use";
                    break;
            }
            GameEvent.instance.SignUpFailed(message);

        }

    bool ValidateData(string username,string pass,string passx2)
    {
            if (username.Length == 0)
            {
                //If the username field is blank show a warning
                GameEvent.instance.SignUpFailed("Missing Username");
                return false;
            }
            else if (pass.CompareTo(passx2) != 0)
            {
                //If the password does not match show a warning
                GameEvent.instance.SignUpFailed("ssword Does Not Match!");
                return false;
            }

            return true;
    }
      
   IEnumerator SetUser(string username,FirebaseUser result)
        {
            user = result;

            if (user != null)
            {
                //Create a user profile and set the username
                UserProfile profile = new UserProfile { DisplayName = username };

                //Call the Firebase auth update user profile function passing the profile with the username
                var profileTask = user.UpdateUserProfileAsync(profile);
                //Wait until the task completes
                yield return new WaitUntil(predicate: () => profileTask.IsCompleted);

                if (profileTask.Exception != null)
                {
                    //If there are errors handle them
                    Debug.LogWarning(message: $"Failed to register task with {profileTask.Exception}");
                    FirebaseException firebaseEx = profileTask.Exception.GetBaseException() as FirebaseException;
                    AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                    GameEvent.instance.SignUpFailed("Username Set Failed!");
                }
                else
                {
                    GameEvent.instance.SignUpSuccessed("User created");
                }
            }
        }


    }
