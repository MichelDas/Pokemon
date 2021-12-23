using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionDB
{
    public static Dictionary<ConditionID, Condition> Conditions { get; set; } = new Dictionary<ConditionID, Condition>()
    {
        {
            ConditionID.Poison,
            new Condition()
            {
                Name = "Poison",
                StartMessage = " is poisoned",
                HUDName = "PSN",
                Id = ConditionID.Poison,
                onAfterTurn = (Pokemon pokemon) =>
                {
                    pokemon.UpdateHP(pokemon.MaxHP/8);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is hurt by poison");
                }
            } 
        },
        {
            ConditionID.Burn,
            new Condition()
            {
                Name = "Burn",
                StartMessage = " is burned",
                HUDName = "BRN",
                Id = ConditionID.Burn,
                onAfterTurn = (Pokemon pokemon) =>
                {
                    pokemon.UpdateHP(pokemon.MaxHP/16);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is hurt by burn");
                }
            }
        },
        {
            ConditionID.Paralysis,
            new Condition()
            {
                Name = "Paralysis",
                StartMessage = " is paralysed",
                HUDName = "PRLZ",
                Id = ConditionID.Paralysis,
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    // if true: can move, false: can't move
                    if(Random.Range(1,5) <= 1)
                    {
                        // can't move probability 25%
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} can't move because paralysis");
                        return false;

                    }
                    return true;
                }
            }
        },
        {
            ConditionID.Freeze,
            new Condition()
            {
                Name = "Freeze",
                StartMessage = " is frozen",
                HUDName = "FRZ",
                Id = ConditionID.Freeze,
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    // if true: can move, false: can't move
                    if(Random.Range(1,5) <= 1)
                    {
                        // can't move probability 25%
                        pokemon.CureStatus();
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is frozen");
                        return true;

                    }
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} can't move because freeze");
                    return false;
                }
            }
        },
        {
            ConditionID.Sleep,
            new Condition()
            {
                Name = "Sleep",
                StartMessage = " hase slept",
                HUDName="SLP",
                Id = ConditionID.Sleep,
                OnStart = (Pokemon pokemon) =>
                {
                    pokemon.StatusTime = Random.Range(1,4);
                },
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if(pokemon.StatusTime <= 0)
                    {
                        pokemon.CureStatus();
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} has woke up");
                        return true;
                    }
                    pokemon.StatusTime--;
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is fast asleep");
                    return false;

                }
            }
        },
        {
            ConditionID.Confusion,
            new Condition()
            {
                Name = "Confusion",
                StartMessage = " is confused",
                HUDName="CNF",
                Id = ConditionID.Confusion,
                OnStart = (Pokemon pokemon) =>
                {
                    pokemon.VolatileStatusTime = Random.Range(1,4);
                },
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if(pokemon.VolatileStatusTime <= 0)
                    {
                        pokemon.CureVolatileStatus();
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is not confused anymore");
                        return true;
                    }
                    pokemon.VolatileStatusTime--;
                    if(Random.Range(1,3) == 1)
                    {
                        return true;
                    }

                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is confused");
                    pokemon.UpdateHP(pokemon.MaxHP/8);
                    pokemon.StatusChanges.Enqueue($"It hurt itself in confusion");
                    return false;
                }
            }
        },
    };

}

public enum ConditionID
{
    None,
    Poison,
    Burn,
    Sleep,
    Paralysis,
    Freeze,
    Confusion,
}
