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

        //Moves = new List<Move>();

        //foreach(LearnableMove learnableMove in pBase.LearnableMoves)
        //{
        //    if(Level >= learnableMove.Level)
        //    {
        //        Moves.Add(new Move(learnableMove.Base));
        //    }
        //    // at most 4 moves can be learned
        //    if(Moves.Count >= 4)
        //    { break; }
        //}
    }

    public int Attack {  get { return Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5; } }
    public int Defense { get { return Mathf.FloorToInt((Base.Defense * Level) / 100f) + 5; } }
    public int SpAttack { get { return Mathf.FloorToInt((Base.SpAttack * Level) / 100f) + 5; } }
    public int SpDefense { get { return Mathf.FloorToInt((Base.SpDefense * Level) / 100f) + 5; } }
    public int Speed { get { return Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5; } }
    public int MaxHP { get { return Mathf.FloorToInt((Base.MaxHP * Level) / 100f) + 10; } }



}
