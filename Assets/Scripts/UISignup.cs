using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UISignup : MonoBehaviour
{
    [Header("InputFields")]
    public InputField inputUsername;
    public InputField inputEmail;
    public InputField inputPassword;
    public InputField inputPasswordx2;

    [Header("Message Box")]
    public GameObject goMsg;
    public Text txtMsg;

    TouchScreenKeyboard keyboard;

    private void Start()
    {
        GameEvent.instance.OnSignUpSuccessed += DisplaySuccess;
        GameEvent.instance.OnSignUpFailed+= DisplayError;
        SetTouchKeyboard();
    }

    private void OnDestroy()
    {
        GameEvent.instance.OnSignUpSuccessed -= DisplaySuccess;
        GameEvent.instance.OnSignUpFailed += DisplayError;

    }
    public void SignUp()
    {

        //GameEvent.instance.SignUp(inputUsername.text, inputEmail.text, inputPassword.text, inputPasswordx2.text);
    }

    public void Back()
    {
        SceneManager.LoadScene("Log in");
    }
    void ResetFields()
    {
        inputPassword.text = "";
        inputPasswordx2.text = "";
    }

    public void DisplaySuccess(string msg)
    {
        //Change box color 
        ResetFields();

        txtMsg.text = msg;

        if (!goMsg.activeInHierarchy)
        {
            StartCoroutine(ShowHideMsg(true));

        }
    }
    public void DisplayError(string msg)
    {
        //Change box color 
        ResetFields();
 
        txtMsg.text = msg;

        if (!goMsg.activeInHierarchy)
        {
            StartCoroutine(ShowHideMsg());
        }
    }

    IEnumerator ShowHideMsg (bool nextScene = false)
    {
        goMsg.SetActive(true); //Show
        yield return new WaitForSeconds(3);
        goMsg.SetActive(false); //Hide
        if ( nextScene)
            LoadNextScene();

    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("Login");
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
