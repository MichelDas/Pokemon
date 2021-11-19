using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MoveBase : ScriptableObject
{
    [SerializeField] new string name;
    [TextArea]
    [SerializeField] string description;
    [SerializeField] PokemonType type;

    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] int pp;

    public string Name { get => name; }
    public string Description { get => description; }
    public PokemonType Type { get => type; }
    public int Power { get => power; }
    public int Accuracy { get => accuracy; }
    public int PP { get => pp; }
}


