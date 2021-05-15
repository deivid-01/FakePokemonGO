using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIPokedex : MonoBehaviour
{
    public string resultScene;
    public string gameScene;
    
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

        PokedexRequest.selectedPokemon = inputTxt.text;
        SceneManager.LoadScene(resultScene);
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

    public void CloseScene()
    {
        SceneManager.LoadScene(gameScene);
    }
    void SetTouchKeyboard()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, autocorrection: true);
        keyboard.active = true;
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

}
