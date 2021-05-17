using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.SceneManagement;
using System;
public class PokedexRequest : MonoBehaviour
{
    private readonly string BASEPOKE_URl = "https://pokeapi.co/api/v2/";


    public static int LIMIT = 9;


    public PokemonData pokemonFound;
    public List<PokemonData> pokemonsFound;

    #region Singlenton 

    public static PokedexRequest instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }



    #endregion


    private void OnEnable()
    {
        if (GameEvent.instance == null)
        {
            print("It's null");
        }
        GameEvent.instance.OnFindingPokemon += SearchPokemon;
        GameEvent.instance.OnFindingPokemons += SearchPokemons;
    }
    private void OnDisable()
    {
      
        GameEvent.instance.OnFindingPokemon -= SearchPokemon;
        GameEvent.instance.OnFindingPokemons -= SearchPokemons;
        StopAllCoroutines();
    }
    private void Start()
    {
    
 
    }

 


    void SearchPokemon(string name)
    {
        print("Searching pokemon with name " + name);
        StartCoroutine(FindPokemonByName(name));
    }

    void SearchPokemons(int page)
    {

        StartCoroutine(FindPokemons(page));


    }


    IEnumerator LoadPokeInfo(string pokemonURL)
    {

        UnityWebRequest pokeInfoRequest = UnityWebRequest.Get(pokemonURL);

        yield return pokeInfoRequest.SendWebRequest();

        if (pokeInfoRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            GameEvent.instance.PokemonFoundFail();
            Debug.LogError(pokeInfoRequest.error);
            yield return null;
        }

        yield return JSON.Parse(pokeInfoRequest.downloadHandler.text);
    }

    IEnumerator LoadPokeSrpite(string spriteURL)
    {

        UnityWebRequest pokeSpriteRequest = UnityWebRequestTexture.GetTexture(spriteURL);

        yield return pokeSpriteRequest.SendWebRequest();

        if (pokeSpriteRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(pokeSpriteRequest.error);
            yield break;
        }

        yield return DownloadHandlerTexture.GetContent(pokeSpriteRequest);
    }


    IEnumerator FindPokemonByName(string name)
    {

        //Get Main Data
        
            CoroutineWithData coroutineLoadInfo = new CoroutineWithData(this, GetPokemonData(name));
            yield return coroutineLoadInfo.coroutine;
        try
        {
            pokemonFound = (PokemonData)coroutineLoadInfo.result;
            print("data is ready");
            StartCoroutine(CheckSceneActive());
        }
        catch (Exception e)
        {
            GameEvent.instance.PokemonFoundFail();
        }

    }

    IEnumerator CheckSceneActive()
    {
        if (SceneManager.GetActiveScene().name == "PokemonInfo")
        {
            GameEvent.instance.PokemonFound(pokemonFound);
        }
        yield return new WaitForSeconds(1f);
        CheckSceneActive();
    }
    IEnumerator GetPokemonData(string name)
    {
        if (name.Length <= 0) yield break;
        CoroutineWithData coroutineLoadInfo = new CoroutineWithData(this, LoadPokeInfo(pokemonURL: BASEPOKE_URl + "pokemon/" + name));
        yield return coroutineLoadInfo.coroutine;

        if (coroutineLoadInfo.result == null) yield break;

        JSONNode pokeInfo = (JSONNode)coroutineLoadInfo.result;

        //Get Pokemon Sprite

        CoroutineWithData coroutineLoadSprite = new CoroutineWithData(this, LoadPokeSrpite(spriteURL: pokeInfo["sprites"]["front_default"]));
        yield return coroutineLoadSprite.coroutine;

        //Set Data

        pokemonFound = new PokemonData(name: pokeInfo["name"],
                                                  height: pokeInfo["height"],
                                                  weight: pokeInfo["weight"],
                                                  types: GetTypes(pokeInfo["types"]),
                                                  abilities: GetAbilities(pokeInfo["abilities"]),
                                                  sprite: (Texture2D)coroutineLoadSprite.result,
                                                  isNull: false);
     
        yield return pokemonFound;
    }

    IEnumerator FindPokemons(int page)
    {
        //Get Main Data
       
        string pokemonsURL = BASEPOKE_URl + $"pokemon?limit={LIMIT}&offset={LIMIT*(page)}";
      
        CoroutineWithData coroutineLoadInfo = new CoroutineWithData(this, LoadPokeInfo(pokemonsURL));
        yield return coroutineLoadInfo.coroutine;

        if (coroutineLoadInfo.result == null) yield break;

        JSONNode pokemonsInfo = (JSONNode)coroutineLoadInfo.result;

        string[]names = GetPokemonNames(pokemonsInfo["results"]);

        GameEvent.instance.PokemonFoundNames(names);

        Texture2D[] textures = new Texture2D[names.Length];
        int i = 0;
        int total = names.Length;
        while (i < total)
        {
            string pokeURL = BASEPOKE_URl + "pokemon/" + names[i];
            CoroutineWithData coroutineLoadInfo2 = new CoroutineWithData(this, LoadPokeInfo(pokeURL));
            yield return coroutineLoadInfo2.coroutine;

            JSONNode pokeInfo = (JSONNode)coroutineLoadInfo2.result;

            CoroutineWithData coroutineLoadSprite = new CoroutineWithData(this, LoadPokeSrpite(spriteURL: pokeInfo["sprites"]["front_default"]));
            yield return coroutineLoadSprite.coroutine;

            textures[i] = (Texture2D)coroutineLoadSprite.result;
            GameEvent.instance.PokemonFoundTexture(i, textures[i]);

            i++;
        }


    }

  

    string[] GetPokemonNames(JSONNode pokemons)
    {
        string[] pokeNames = new string[pokemons.Count];
        for (int i = 0, j = pokemons.Count - 1; i < pokemons.Count; i++, j--)
        {
            pokeNames[j] = pokemons[i]["name"];
          
        }

        return pokeNames;
    }


    string [] GetTypes(JSONNode pokeTypes)
    {

        string[] pokeTypesNames = new string[pokeTypes.Count];
        for (int i = 0, j = pokeTypes.Count - 1; i < pokeTypes.Count; i++, j--)
        {
            pokeTypesNames[j] = pokeTypes[i]["type"]["name"];
        }

        return pokeTypesNames;


    }
    string[] GetAbilities(JSONNode pokeTypes)
    {
        int pokeTypesLength = (pokeTypes.Count > 2) ? 2 : pokeTypes.Count; 

        string[] pokeAbilitiesNames = new string[pokeTypesLength];
        for (int i = 0, j = pokeTypesLength - 1; i < pokeTypesLength; i++, j--)
        {
            pokeAbilitiesNames[j] = pokeTypes[i]["ability"]["name"];
        }

        return pokeAbilitiesNames;


    }
}

public class CoroutineWithData
{
    public Coroutine coroutine { get; private set; }
    public object result;
    private IEnumerator target;
    public CoroutineWithData(MonoBehaviour owner, IEnumerator target)
    {
        this.target = target;
        this.coroutine = owner.StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        while (target.MoveNext())
        {
            result = target.Current;
            yield return result;
        }
    }
}