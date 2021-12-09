using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PokemonParty : MonoBehaviour
{
    [SerializeField]
    private List<Pokemon> pokemons;

    public List<Pokemon> Pokemons { get => pokemons; }

    private void Start()
    {
        foreach(Pokemon pokemon in Pokemons)
        {
            // initializing all pokemons in the player party
            pokemon.Init();
        }

    }

    public Pokemon GetHealthyPokemon()
    {
        return Pokemons.Where(monster => monster.HP > 0).FirstOrDefault();
        
    }
}
