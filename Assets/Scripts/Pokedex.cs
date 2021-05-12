using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;


public class Pokedex : MonoBehaviour
{
    private readonly string BASEPOKE_URl = "https://pokeapi.co/api/v2/";
    private void OnEnable()
    {
        GameEvent.instance.OnPokemonCaptured += SearchPokemon;
    }


    void SearchPokemon(string name)
    {
        StartCoroutine(FindPokemonByName(name));
    }

    IEnumerator FindPokemonByName(string name)
    {
        string pokemonURL = BASEPOKE_URl + "pokemon/" + name;

        UnityWebRequest pokeInfoRequest = UnityWebRequest.Get(pokemonURL);

        yield return pokeInfoRequest.SendWebRequest();

        if ( pokeInfoRequest.result==UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(pokeInfoRequest.error);
            yield break;
        }

        JSONNode pokeInfo = JSON.Parse(pokeInfoRequest.downloadHandler.text);
        JSONNode pokeTypes = pokeInfo["types"];

        string pokeName = pokeInfo["name"];

        GameEvent.instance.PokedexFoundName(pokeName);
        string pokeSpriteURL = pokeInfo["sprites"]["front_default"];

        string[] pokeTypesNames = new string[pokeTypes.Count];
        for (int i = 0, j = pokeTypes.Count -1; i < pokeTypes.Count; i++,j--)
        {
            pokeTypesNames[j] = pokeTypes[i]["type"]["name"];
        }
        GameEvent.instance.PokedexFoundTypes(pokeTypesNames);

        //Get pokemon Sprite

        UnityWebRequest pokeSpriteRequest = UnityWebRequestTexture.GetTexture(pokeSpriteURL);

        yield return pokeSpriteRequest.SendWebRequest();

        if (pokeSpriteRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(pokeSpriteRequest.error);
            yield break;
        }


        GameEvent.instance.PokedexFoundImage( DownloadHandlerTexture.GetContent(pokeSpriteRequest));



    }
}
