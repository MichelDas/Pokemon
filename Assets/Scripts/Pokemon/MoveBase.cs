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

    [SerializeField] MoveCategory category; // indicates what kind of move is this
    [SerializeField] MoveTarget target;     // who will be effected by this move
    [SerializeField] MoveEffects effects;   // what kind of effect it will have

    public string Name { get => name; }
    public string Description { get => description; }
    public PokemonType Type { get => type; }
    public int Power { get => power; }
    public int Accuracy { get => accuracy; }
    public int PP { get => pp; }
    public MoveCategory Category { get => category; set => category = value; }
    public MoveTarget Target { get => target; }
    public MoveEffects Effects { get => effects; }

}



public enum MoveCategory
{
    Physical,
    Special,
    Stat,
}

public enum MoveTarget
{
    Foe,
    Self,
}

[System.Serializable]
public class MoveEffects
{
    [SerializeField] List<StatBoost> boosts;
    [SerializeField] ConditionID status;
    [SerializeField] ConditionID volatileStatus;
    public List<StatBoost> Boosts { get => boosts; }
    public ConditionID Status { get => status; }
    public ConditionID VolatileStatus { get => volatileStatus; }
}

[System.Serializable]
public class StatBoost
{
    public Stat stat;
    public int boost;
}


