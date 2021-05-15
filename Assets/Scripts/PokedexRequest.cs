using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class PokedexRequest : MonoBehaviour
{
    private readonly string BASEPOKE_URl = "https://pokeapi.co/api/v2/";

    public static string selectedPokemon="";

    private void OnEnable()
    {
        GameEvent.instance.OnPokemonCaptured += SearchPokemon;
        GameEvent.instance.OnFindingPokemon += SearchPokemon;
    }
    private void OnDisable()
    {
        GameEvent.instance.OnPokemonCaptured -= SearchPokemon;
        GameEvent.instance.OnFindingPokemon -= SearchPokemon;
        StopAllCoroutines();
    }
    private void Start()
    {
        SearchPokemon(selectedPokemon);
    }


    void SearchPokemon(string name)
    {
      
        StartCoroutine(FindPokemonByName(name));
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
        print("hola");
        CoroutineWithData coroutineLoadInfo = new CoroutineWithData(this, LoadPokeInfo(pokemonURL:BASEPOKE_URl + "pokemon/" + name));
        yield return coroutineLoadInfo.coroutine;

        if (coroutineLoadInfo.result == null) yield break;

        JSONNode pokeInfo = (JSONNode)coroutineLoadInfo.result;

        //Get Pokemon Sprite
  
        CoroutineWithData coroutineLoadSprite = new CoroutineWithData(this, LoadPokeSrpite(spriteURL: pokeInfo["sprites"]["front_default"]));
        yield return coroutineLoadSprite.coroutine;
        
        //Set Data
   
        PokemonData pokemonData = new PokemonData(name:  pokeInfo["name"],
                                                  height: pokeInfo["height"],
                                                  weight: pokeInfo["weight"],
                                                  types: GetTypes(pokeInfo["types"]),
                                                  abilities:GetAbilities(pokeInfo["abilities"]),
                                                  sprite: (Texture2D)coroutineLoadSprite.result);

        //Trigger Pokemon found
        GameEvent.instance.PokemonFound(pokemonData);
    
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