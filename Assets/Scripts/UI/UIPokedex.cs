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
    }

    private void OnEnable()
    {

        GameEvent.instance.OnPokemonsFound += UpdateData;
    }
    private void OnDisable()
    {

        GameEvent.instance.OnPokemonsFound -= UpdateData;
    }

 
    void UpdateData(List<PokemonMainData> pkmsData)
    {

        for (int i = 0; i < pkmsData.Count; i++)
        {
            pokemonNames[i].text = StringTools.FirstCharToUpper( pkmsData[i].name ) ;
        
            pokemonImages[i].texture= pkmsData[i].sprite;
            pokemonImages[i].texture.filterMode = FilterMode.Point;


        }
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
        GameEvent.instance.FindPokemon(inputTxt.text);
        //PokedexRequest.selectedPokemon = inputTxt.text;
        SceneManager.LoadScene(resultScene);
    }
    public void SearchPokemon(int i )
    {
        string pkm_name = (pokemonNames[i].text).ToLower();
        print(pkm_name);
        GameEvent.instance.FindPokemon(pkm_name);

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
