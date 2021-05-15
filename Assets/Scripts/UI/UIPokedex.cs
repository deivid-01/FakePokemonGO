using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIPokedex : MonoBehaviour
{
    public RawImage rawImage_picture;
    
    public InputField input_pkmName;
    public List<RawImage> images_pkms;
    public List<Button> btns_list;
   
    public Text txt_name;
    public List<Text> txt_types;

    TouchScreenKeyboard keyboard;

    private void OnEnable()
    {
     //   GameEvent.instance.OnPokedexFoundName += UpdateName;
     //   GameEvent.instance.OnPokedexFoundTypes += UpdateTypes;
       // GameEvent.instance.OnPokedexFoundImage+= UpdateImage;
    }

    private void OnDisable()
    {
      //  GameEvent.instance.OnPokedexFoundName -= UpdateName;
      //  GameEvent.instance.OnPokedexFoundTypes -= UpdateTypes;
       // GameEvent.instance.OnPokedexFoundImage -= UpdateImage;
    }
    private void Start()
    {
        SetTouchKeyboard();
    }

    void UpdateName(string name)
    {
        txt_name.text = name;
    }

    void UpdateTypes(string[] types)
    {
        print(types[0]);
        for (int i = 0; i < types.Length; i++)
        {
            txt_types[i].text = types[i];
        }
    }

    void UpdateImage(Texture2D texture)
    {
        rawImage_picture.texture = texture;
        rawImage_picture.texture.filterMode = FilterMode.Point;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void SearchPokemon(InputField inputTxt)
    {
        
       // GameEvent.instance.PokemonCaptured(input_pkmName.text);
        print("Searching pokemon with name " + inputTxt.text);
    }
    public void SearchPokemon(Text txt_name)
    {
        string pkm_name = txt_name.text;
        //GameEvent.instance.SearchPokemon(pkm_name)
        print("Searching pokemon with name " + pkm_name);
    }

    public void NextPage() 
    {
        //GameEvent.instance.LoadNextList()
        print("Loading next page");
    }
    public void PreviousPage()
    {
        //  GameEvent.instance.LoadPreviousList();
        print("Loading previous Page");
    }

    void SetTouchKeyboard()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, autocorrection: true);
        keyboard.active = true;
    }
    public void OpenTouchKeyBoard()
    {
        print("Trying to open keyboard");

        if (keyboard != null)
        {
            if (!keyboard.active)
            {
                keyboard.active = true;
        print(keyboard.active);
            }
        }


    }
    public void CloseTouchKeyBoard()
    {
        print("Trying to close keyboard");

        if (keyboard != null)
        {
            if (keyboard.active)
            {
                keyboard.active = false;
            }
        }
    }

}
