using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIPokemonInfo : MonoBehaviour
{
    public string previousScene;
    public GameObject GO_tableData;
    public GameObject GO_error;
    public Text txt_name;
    public Text txt_height;
    public Text txt_weight;
    public List<Text> txt_types;
    public List<Text> txt_abilities;
    public RawImage rwImg_picture;
    public Texture2D texture_error;

    private void Awake()
    {
        ResetUI();

    }
    private void OnEnable()
    {
        GameEvent.instance.OnPokemonFound += DisplayInfo;
        GameEvent.instance.OnPokemonFoundFail += DisplayError;

    }
    private void OnDisable()
    {
        GameEvent.instance.OnPokemonFound -= DisplayInfo;
        GameEvent.instance.OnPokemonFoundFail -= DisplayError;
    }
    private void Start()
    {
        // GameEvent.instance.FindPokemon(pokemonName);
      //  DisplayInfo(PokedexRequest.instance.pokemonFound);
    }

    void ResetUI()
    {
        txt_types.ForEach((txtType) => txtType.gameObject.SetActive(false));
        txt_abilities.ForEach((txtAbility) => txtAbility.gameObject.SetActive(false));
        txt_name.text = "";
        rwImg_picture.texture = Texture2D.blackTexture;
        GO_tableData.SetActive(false);
}

public void DisplayInfo(PokemonData pkmData)
    {
       
        if (!pkmData.isNull)
        {

            txt_name.text = StringTools.FirstCharToUpper(pkmData.name);
            txt_height.text = (StringTools.Decimeter2Meter(pkmData.height)).ToString() + "m";
            txt_weight.text = (StringTools.Hectogram2Kilogram(pkmData.weight)).ToString() + "kg";



            for (int i = 0; i < pkmData.types.Length; i++)
            {
                txt_types[i].text = StringTools.FirstCharToUpper(pkmData.types[i]);
                txt_types[i].gameObject.SetActive(true);
            }

            for (int i = 0; i < pkmData.abilities.Length; i++)
            {
                txt_abilities[i].text = StringTools.FirstCharToUpper(pkmData.abilities[i]);
                txt_abilities[i].gameObject.SetActive(true);

            }
            GO_tableData.SetActive(true);
            rwImg_picture.texture = pkmData.sprite;
            rwImg_picture.texture.filterMode = FilterMode.Point;
        }

        else
        {
            DisplayError();
        }
        
     


    }

   





    public void DisplayError()
    {
        print("Displaying Error...");
        GO_error.SetActive(true);
     
    }


    public void CloseScene()
    {
        SceneManager.LoadScene(previousScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
