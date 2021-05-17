using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIPokedex : MonoBehaviour
{
    public string resultScene;
    public string gameScene;
    
    public List<Text>pokemonNames;
    public List<RawImage>pokemonImages;
    public GameObject previousPage;
    public GameObject nextPage;



    int actualPage = 0;

    TouchScreenKeyboard keyboard;

    private void Awake()
    {
        ResetUI();
    }

    void ResetUI()
    {

        previousPage.SetActive(false);
        pokemonImages.ForEach((RawImage img) =>  img.texture = Texture2D.blackTexture);
        pokemonNames.ForEach((Text txt) =>  txt.text = "");
    }

    private void OnEnable()
    {
        GameEvent.instance.OnPokemonFoundNames += UpdateNames;
        GameEvent.instance.OnPokemonFoundTexture += UpdatePicture;
       
    }
    private void OnDisable()
    {

      //  GameEvent.instance.OnPokemonsFound -= UpdateData;
        GameEvent.instance.OnPokemonFoundNames -= UpdateNames;
        GameEvent.instance.OnPokemonFoundTexture -= UpdatePicture;

    }
    void UpdateNames(string[] names)
    {

        for (int i = 0; i < names.Length; i++)
        {
            pokemonNames[i].text = StringTools.FirstCharToUpper(names[i]);
        }
    }

    void UpdatePicture(int idx,Texture2D texture)
    {

        pokemonImages[idx].texture = texture;
        pokemonImages[idx].texture.filterMode = FilterMode.Point;

    }


    public Sprite CreateNewSprite(Texture2D dst) => Sprite.Create(dst, //Texture 
                                        new UnityEngine.Rect(0, 0, dst.width, dst.height),//Rect Propierties
                                        new Vector2(0.5f, 0.5f),                          //Offset
                                        100);
    private void Start()
    {
        SetTouchKeyboard();
        GetUIData();
    }

    void GetUIData()
    {
        GameEvent.instance.FindPokemons(actualPage);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void SearchPokemon(InputField inputTxt)
    {

        // GameEvent.instance.PokemonCaptured(input_pkmName.text);
        GameEvent.instance.FindPokemon((inputTxt.text).ToLower());
        //PokedexRequest.selectedPokemon = inputTxt.text;
        UIPokemonInfo.previousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(resultScene);
    }
    public void SearchPokemon(int i )
    {
        string pkm_name = (pokemonNames[i].text).ToLower();
        print(pkm_name);
        GameEvent.instance.FindPokemon(pkm_name);
        UIPokemonInfo.previousScene = SceneManager.GetActiveScene().name;

        SceneManager.LoadScene(resultScene);
   
    }

    public void NextPage() 
    {     
        actualPage += 1;

        if (actualPage > 0)
            previousPage.SetActive(true);

        GetUIData();
    }
    public void PreviousPage()
    {
    
        if (actualPage == 0)
        {
            previousPage.SetActive(false);
            return;
        }
        actualPage -= 1;
        GetUIData();

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
