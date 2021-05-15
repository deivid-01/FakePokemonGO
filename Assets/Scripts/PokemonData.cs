using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PokemonData 
{
    public string name;
    public string height;
    public string weight;
    public string[] types;
    public string[] abilities;
    public Texture2D sprite;

    public PokemonData(string name, string height, string weight, string[] types, string[] abilities, Texture2D sprite)
    {
        this.name = name;
        this.height = height;
        this.weight = weight;
        this.types = types;
        this.abilities = abilities;
        this.sprite = sprite;
    }
}
