using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pokemon
{
    [SerializeField] PokemonBase _base;
    [SerializeField] int level;
    [SerializeField] int hp;
    [SerializeField] int maxHp;
   

    // Move that the pokemon can use
    public List<Move> Moves;

    public Dictionary<Stat, int> Stats { get; set; }
    public Dictionary<Stat, int> StatBoosts { get; set; }


    public void Init()
    {
        Moves = new List<Move>();

        // for now reset the max hp value
        
        maxHp = Base.MaxHP;
        // need to change upper portion


        foreach (LearnableMove learnableMove in Base.LearnableMoves)
        {
            if (Level >= learnableMove.Level)
            {
                Moves.Add(new Move(learnableMove.Base));
            }
            // at most 4 moves can be learned
            if (Moves.Count >= 4)
            { break; }
        }

        CalculateStats();
        HP = MaxHP;

        StatBoosts = new Dictionary<Stat, int>()
        {
            {Stat.Attack, 0 },
            {Stat.Defense, 0 },
            {Stat.SpAttack, 0 },
            {Stat.SpDefense, 0 },
            {Stat.Speed, 0 }
        };

    }

    void CalculateStats()
    {
        Stats = new Dictionary<Stat, int>();
        //Stats doesn't change
        Stats.Add(Stat.Attack, Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5);
        Stats.Add(Stat.Defense, Mathf.FloorToInt((Base.Defense * Level) / 100f) + 5);
        Stats.Add(Stat.SpAttack, Mathf.FloorToInt((Base.SpAttack * Level) / 100f) + 5);
        Stats.Add(Stat.SpDefense, Mathf.FloorToInt((Base.SpDefense * Level) / 100f) + 5);
        Stats.Add(Stat.Speed, Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5);
        MaxHP = Mathf.FloorToInt((Base.MaxHP * Level) / 100f) + 10;
    }

    int GetStat(Stat stat)
    {
        int statValue = Stats[stat];
        int boost = StatBoosts[stat];
        Debug.Log(stat + " " + StatBoosts[stat]);
        float[] boostValues = new float[] { 1, 1.5f, 2f,2.5f,3,3.5f, 4 };
        if(boost >= 0)
            statValue =  Mathf.FloorToInt(statValue * boostValues[boost]);
        else
            statValue = Mathf.FloorToInt(statValue / boostValues[-boost]);

        return statValue;
    }

    public void ApplyBoosts(List<StatBoost> statBoosts)
    {
        foreach(StatBoost statBoost in statBoosts)
        {
            Stat stat = statBoost.stat;
            int boost = statBoost.boost;
            int boostVal = StatBoosts[stat] + boost;
            //if (boostVal > 5)
            //    boostVal = 5;
            //else if (boostVal < -5)
            //    boostVal = -5;
            //StatBoosts[stat] = boostVal;
            StatBoosts[stat] = Mathf.Clamp(StatBoosts[stat] + boost, -6, 6);
        }
    }

    public int Attack { get {   return GetStat(Stat.Attack); } }
    public int Defense { get { return Mathf.FloorToInt((Base.Defense * Level) / 100f) + 5; } }
    public int SpAttack { get { return Mathf.FloorToInt((Base.SpAttack * Level) / 100f) + 5; } }
    public int SpDefense { get { return Mathf.FloorToInt((Base.SpDefense * Level) / 100f) + 5; } }
    public int Speed { get { return Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5; } }
    public int MaxHP { get => maxHp; private set => maxHp = value; }
    public int HP { get => hp; set => hp = value; }
    public PokemonBase Base { get => _base; }
    public int Level { get => level; }

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

        float attack = attacker.Attack;
        float defense = Defense;

        if(move.Base.Category == MoveCategory.Special)
        {
            attack = attacker.SpAttack;
            defense = SpDefense;
        }

        float modifiers = Random.Range(0.85f, 1f)*type*critical;
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attack / Defense) + 2;
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