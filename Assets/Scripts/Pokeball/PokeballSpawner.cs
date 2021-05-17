using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeballSpawner : MonoBehaviour
{
    // Start is called before the first frame update

   public  GameObject pokeballPrefab;

 

    void Start()
    {
        GameEvent.instance.OnThrowingPokeball += SpawnPokeballAfter;
        StartCoroutine( SpawnPokeball(0));
    }

    private void OnDisable()
    {
        GameEvent.instance.OnThrowingPokeball -= SpawnPokeballAfter;

    }

    IEnumerator SpawnPokeball(int waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Instantiate(pokeballPrefab, pokeballPrefab.transform.position, Quaternion.identity);
    }
    void SpawnPokeballAfter()
    {
        StartCoroutine(SpawnPokeball(2));
       
    }
}
