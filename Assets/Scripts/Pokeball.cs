using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokeball : MonoBehaviour
{
    Rigidbody rg;
    private void Start()
    {
        rg = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals("pokemon"))
        {
            StartCoroutine( CatchPokemon(collision.collider.gameObject));
        }
    }

    IEnumerator CatchPokemon(GameObject GOPokemon)
    {
        GOPokemon.SetActive(false);
        rg.constraints = RigidbodyConstraints.FreezePosition;
        //Add vibration to pokeball
        yield return new WaitForSeconds(2f);

         bool escape= ((int)Random.Range(0, 2)==1)?true:false;

        if (escape)
        {
            print("Pokemon escaped");
            GOPokemon.SetActive(true);
        }
        else
        {
            print("Pokemon captured");

            GameEvent.instance.PokemonCaptured(GOPokemon.GetComponent<Pokemon>().name);
        }
    }

   
}
