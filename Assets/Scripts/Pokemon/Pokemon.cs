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

    // if a move changes the status of the pokemon such as poisoned or boost
    // it will be recorded in the following queue
    public Queue<string> StatusChanges { get; private set; }

    public Condition Status { get; private set; }
    public bool HpChanged{ get; set; }

    Dictionary<Stat, string> statDic = new Dictionary<Stat, string>()
    {
        {Stat.Attack, "attack" },
        {Stat.Defense, "defence" },
        {Stat.SpAttack, "spcial attack" },
        {Stat.SpDefense, "special defence" },
        {Stat.Speed, "speed" }
    };

    public void Init()
    {
        StatusChanges = new Queue<string>();
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

        ResetStatBoost();

    }

    void ResetStatBoost()
    {
        StatBoosts = new Dictionary<Stat, int>()
        {
            {Stat.Attack, 0 },
            {Stat.Defense, 0 },
            {Stat.SpAttack, 0 },
            {Stat.SpDefense, 0 },
            {Stat.Speed, 0 }
        };
    }

    public void onBattleOver()
    {
        ResetStatBoost();
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
            StatBoosts[stat] = Mathf.Clamp(StatBoosts[stat] + boost, -6, 6);
            if(boost > 0)
            {
                StatusChanges.Enqueue($"{Base.Name}'s {statDic[stat]} is increased");
            }
            else
            {
                StatusChanges.Enqueue($"{Base.Name}'s {statDic[stat]} is decreased");
            }
        }
    }

    public int Attack { get {   return GetStat(Stat.Attack); } }
    public int Defense { get { return GetStat(Stat.Defense); } }
    public int SpAttack { get { return GetStat(Stat.SpAttack); } }
    public int SpDefense { get { return GetStat(Stat.Defense); } }
    public int Speed { get { return GetStat(Stat.Speed); } }
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

        UpdateHP(damage);
        if (HP <= 0)
        {
            HP = 0;
            damageDetails.Fainted = true;
        }

        return damageDetails;
    }

    public void UpdateHP(int damage)
    {
        HP = Mathf.Clamp(HP - damage, 0, maxHp);
        HpChanged = true;

    }

    public Move GetRandomMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }

    
    public void SetStatus(ConditionID conditionID)
    {
        // what kind of effect the move have
        Status = ConditionDB.Conditions[conditionID];
        // add to log
        StatusChanges.Enqueue($"{Base.Name}{Status.StartMessage}");

    }

    // Do it When turn ends
    public void OnAfterTurn()
    {
        Status?.onAfterTurn?.Invoke(this);
    }

}

public class DamageDetails
{
    public bool Fainted { get; set; }
    public float Critical { get; set; }
    public float TypeEffectiveness { get; set; }
}