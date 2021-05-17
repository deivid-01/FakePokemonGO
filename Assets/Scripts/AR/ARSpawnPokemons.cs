using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
[RequireComponent(typeof(ARRaycastManager))]
public class ARSpawnPokemons : MonoBehaviour
{
    GameObject spawnedObject;

    public GameObject prefab;
    ARRaycastManager arRaycastManager;
    Vector2 touchPosition;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public List< Pokemon> pokemons;
    public GameObject GO_default;

    #region Singlenton
    public static ARSpawnPokemons instance;
    #endregion

    private void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();

        if (instance == null)
        {
            instance = this;
        }
        
       
    }

    private void OnEnable()
    {
        GameEvent.instance.OnSpawnPokemon += SpawnPokemon;
        GameEvent.instance.OnCatchedPokemon += RemovePokemon;

    }
    public void RemovePokemon(string name)
    {
        for (int i = 0; i < pokemons.Count; i++)
        {
            if (pokemons[i].name.Equals(name))
            {
                pokemons.Remove(pokemons[i]);
                break;
            }
        } 
    }

    private void OnDisable()
    {
        GameEvent.instance.OnCatchedPokemon -= RemovePokemon;
        GameEvent.instance.OnSpawnPokemon -= SpawnPokemon;

    }
    void SpawnPokemon(Vector3 position)
    {
        print("Numlist " + pokemons.Count);
        Instantiate(ChoosePokemon(), position, Quaternion.Euler(0,180,0));
    }

     GameObject ChoosePokemon()
    {
        if (pokemons.Count > 0)
        {
            int idx = Random.Range(0, pokemons.Count);

            GameObject GO_pokemon = pokemons[idx].gameObject;
           // pokemons.RemoveAt(idx);

            return GO_pokemon;
        }

        return GO_default;
        
    }


}
