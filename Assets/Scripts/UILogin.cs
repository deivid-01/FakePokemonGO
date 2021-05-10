using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UILogin : MonoBehaviour
{
    [Header("InputFields")]
    public InputField inputEmail;
    public InputField inputPassword;
    
    [Header("Error Box")]
    public GameObject goError;
    public Text txtError;

    TouchScreenKeyboard keyboard;
    private void Start()
    {
        GameEvent.instance.OnLoginSuccessed += LoadNextScene;
        GameEvent.instance.OnLoginFailed+= DisplayError;
        SetTouchKeyboard();
    }
    private void OnDestroy()
    {
        GameEvent.instance.OnLoginSuccessed -= LoadNextScene;
        GameEvent.instance.OnLoginFailed -= DisplayError;
   
    }

    public void Login()
    {

        GameEvent.instance.Login(inputEmail.text, inputPassword.text);
    }
    void ResetFields()
    {
        inputPassword.text = "";
    }

    public void DisplayError(string msg)
    {
        ResetFields();
        txtError.text = msg;

        if (!goError.activeInHierarchy)
        { 
            StartCoroutine(ShowHideError());
        }
    }

    IEnumerator ShowHideError()
    {
        goError.SetActive(true); //Show
        yield return new WaitForSeconds(3);
        goError.SetActive(false); //Hide
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("Game");
        StopAllCoroutines();
    }
    public void NewAccount()
    {
        SceneManager.LoadScene("Sign up");
        StopAllCoroutines();

    }

    public void ExitGame()
    {
        StopAllCoroutines();
        Application.Quit();
    }

    #region TouchKeyboard
    void SetTouchKeyboard()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, autocorrection: true);
        keyboard.active = false;
    }
    public void OpenTouchKeyBoard()
    {
        if (keyboard != null)
        {
            if (!keyboard.active)
            {
                keyboard.active = true;
            }
        }
        
    
    }
    public void CloseTouchKeyBoard()
    {
        if (keyboard != null)
        {
            if (keyboard.active)
            {
                keyboard.active = false;
            }
        }
    }

    #endregion


}
