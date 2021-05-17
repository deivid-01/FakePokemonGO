using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Pokeball : MonoBehaviour
{
    Rigidbody rg;

    public string pokedexInfoScene;

    private void Start()
    {
        rg = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals("pokemon"))
        {
            string name = collision.collider.GetComponent<Pokemon>().name;
           

            UIPokemonInfo.previousScene = SceneManager.GetActiveScene().name;
            GameEvent.instance.FindPokemon((name).ToLower());
            GameEvent.instance.CatchedPokemon(name);
            

            StartCoroutine(DestroyPokemon(collision.collider.gameObject));
        }
    }

    IEnumerator DestroyPokemon(GameObject go)
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(go);
        SceneManager.LoadScene(pokedexInfoScene);
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
