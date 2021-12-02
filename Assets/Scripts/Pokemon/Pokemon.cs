using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokemon
{
    public PokemonBase Base { get; set; }
    public int Level { get; set; }

    public int HP { get; set; }

    // Move that the pokemon can use
    public List<Move> Moves;

    public Pokemon(PokemonBase pBase, int pLevel)
    {
        Base = pBase;
        Level = pLevel;
        HP = MaxHP;

        if(pBase == null)
        {
            Debug.Log("blabla");
        }


        Moves = new List<Move>();

        foreach (LearnableMove learnableMove in pBase.LearnableMoves)
        {
            if (Level >= learnableMove.Level)
            {
                Moves.Add(new Move(learnableMove.Base));
            }
            // at most 4 moves can be learned
            if (Moves.Count >= 4)
            { break; }
        }
    }

    public int Attack {  get { return Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5; } }
    public int Defense { get { return Mathf.FloorToInt((Base.Defense * Level) / 100f) + 5; } }
    public int SpAttack { get { return Mathf.FloorToInt((Base.SpAttack * Level) / 100f) + 5; } }
    public int SpDefense { get { return Mathf.FloorToInt((Base.SpDefense * Level) / 100f) + 5; } }
    public int Speed { get { return Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5; } }
    public int MaxHP { get { return Mathf.FloorToInt((Base.MaxHP * Level) / 100f) + 10; } }


    public DamageDetails TakeDamage(Move move, Pokemon attacker)
    {
        //Critical move
        float critical = 1f;
        //if 6.25% then cricital
        if(Random.value * 100 <= 6.25f)
        {
            critical = 2f;
        }
        float type = TypeChart.GetEffectiveness(move.Base.Type, Base.Type1) * TypeChart.GetEffectiveness(move.Base.Type, Base.Type2);

        DamageDetails damageDetails = new DamageDetails
        {
            Fainted = false,
            Critical = critical,
            TypeEffectiveness = type

        };

        float modifiers = Random.Range(0.85f, 1f)*type*critical;
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attacker.Attack / Defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        HP -= damage;
        if(HP <= 0)
        {
            HP = 0;
            damageDetails.Fainted = true;
        }
        return damageDetails;
    }

    public Move GetRandomMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }

}

public class DamageDetails
{
    public bool Fainted { get; set; }
    public float Critical { get; set; }
    public float TypeEffectiveness { get; set; }
}