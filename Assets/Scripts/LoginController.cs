using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginController : MonoBehaviour
{
    public InputField input_username;
    public InputField input_password;
    TouchScreenKeyboard keyboard;

    private void Start()
    {
        SetTouchKeyboard();
    }

    public void Login()
    {
        print(input_username.text);
        print(input_password.text);

        // Search in firebase
    }
    public void NewAccount()
    {
        print("...Loading next scene");
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
