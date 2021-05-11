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

    }

    private void OnDisable()
    {
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
            pokemons.RemoveAt(idx);

            return GO_pokemon;
        }

        return GO_default;
        
    }
    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
        touchPosition = default;
        return false;
    }

    private void Update()
    {
        /*
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;
        if (arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;

            //if (spawnedObject == null)
            {
                spawnedObject = Instantiate(prefab, hitPose.position, hitPose.rotation);
            }
            //else
            {
              //  spawnedObject.transform.position = hitPose.position;
            }
        }
        */
    }
}
