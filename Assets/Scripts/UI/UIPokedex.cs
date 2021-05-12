using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIPokedex : MonoBehaviour
{
    public RawImage rawImage_picture;
    public Text txt_name;
    public List<Text> txt_types;

    private void OnEnable()
    {
        GameEvent.instance.OnPokedexFoundName += UpdateName;
        GameEvent.instance.OnPokedexFoundTypes += UpdateTypes;
        GameEvent.instance.OnPokedexFoundImage+= UpdateImage;
    }

    private void OnDisable()
    {
        GameEvent.instance.OnPokedexFoundName -= UpdateName;
        GameEvent.instance.OnPokedexFoundTypes -= UpdateTypes;
        GameEvent.instance.OnPokedexFoundImage -= UpdateImage;
    }

    void UpdateName(string name)
    {
        txt_name.text = name;
    }

    void UpdateTypes(string[] types)
    {
        print(types[0]);
        for (int i = 0; i < types.Length; i++)
        {
            txt_types[i].text = types[i];
        }
    }

    void UpdateImage(Texture2D texture)
    {
        rawImage_picture.texture = texture;
        rawImage_picture.texture.filterMode = FilterMode.Point;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void SearchPokemon(string name)
    {
        GameEvent.instance.PokemonCaptured(name);
    }
}
