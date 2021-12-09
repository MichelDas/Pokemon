using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is Pokemon Master Data. Will not be changed from other Classes ( Only be changed from Inspector )
[CreateAssetMenu]
public class PokemonBase : ScriptableObject
{
    [SerializeField] new string name;
    [TextArea]
    [SerializeField] string description;
    [SerializeField] Sprite frontSprite;
    [SerializeField] Sprite backSprite;

    [SerializeField] PokemonType type1;
    [SerializeField] PokemonType type2;

    [SerializeField] int maxHP;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int spAttack;
    [SerializeField] int spDefense;
    [SerializeField] int speed;

    [SerializeField] List<LearnableMove> learnableMoves;


    public int MaxHP { get => maxHP; }
    public int Attack { get => attack;  }
    public int Defense { get => defense; }
    public int SpAttack { get => spAttack; }
    public int SpDefense { get => spDefense; }
    public int Speed { get => speed; }
    public string Name { get => name;  }
    public string Description { get => description;  }
    public Sprite FrontSprite { get => frontSprite;  }
    public Sprite BackSprite { get => backSprite;  }
    public PokemonType Type1 { get => type1;  }
    public PokemonType Type2 { get => type2;  }
    public List<LearnableMove> LearnableMoves { get => learnableMoves; }
}

[Serializable]
public class LearnableMove
{
    [SerializeField] MoveBase _base;
    [SerializeField] int level;  // at which level this move will be learned

    public MoveBase Base { get => _base; }
    public int Level { get => level; }
}

public enum PokemonType
{
    None,
    Normal,
    Fire,
    Water,
    Electric,
    Grass,
    Ice,
    Fighting,
    Poison,
    Ground,
    Flying,
    Psychic,
    Bug,
    Rock,
    Ghost,
    Dragon
}

public enum Stat
{
    Attack,
    Defense,
    SpAttack,
    SpDefense,
    Speed
}


public class TypeChart
{
    static float[][] chart =
    {
       //                   NOR     FIR     WAT     ELE     GRS     ICE     FIG     POI
        /*NOR*/ new float[] {1f,    1f,     1f,     1f,     1f,     1f,     1f,     1f},
        /*FIR*/ new float[] {1f,  0.5f,   0.5f,     1f,     2f,     2f,     1f,     1f},
        /*WAT*/ new float[] {1f,    2f,   0.5f,     1f,    0.5f,    1f,     1f,     1f},
        /*ELE*/ new float[] {1f,    1f,     2f,    0.5f,   0.5f,    1f,     1f,     1f},
        /*GRS*/ new float[] {1f,   0.5f,    2f,     1f,    0.5f,    1f,     1f,   0.5f},
        /*ICE*/ new float[] {1f,   0.5f,  0.5f,     1f,     2f,   0.5f,     1f,     1f},
        /*FIG*/ new float[] {2f,    1f,     1f,     1f,     1f,     2f,     1f,   0.5f},
        /*POI*/ new float[] {1f,    1f,     1f,     1f,     2f,     1f,     1f,   0.5f},
    };

    public static float GetEffectiveness(PokemonType attackType, PokemonType defenseType)
    {
        if(attackType == PokemonType.None || defenseType == PokemonType.None)
        {
            return 1f;
        }
        int row = (int)attackType - 1;
        int col = (int)defenseType - 1;
        return chart[row][col];
    }
}