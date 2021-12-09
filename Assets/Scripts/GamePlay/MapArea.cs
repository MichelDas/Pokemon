using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    public List<Pokemon> pokemons;

    public Pokemon GetRandomWildPokemon()
    {
        int r = Random.Range(0, pokemons.Count);
        Pokemon pokemon = pokemons[r];
        pokemon.Init();  // init the Random pokemon that we get from here
        return pokemon;
    }
}
