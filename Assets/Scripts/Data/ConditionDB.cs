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
                onAfterTurn = (Pokemon pokemon) =>
                {
                    pokemon.UpdateHP(pokemon.MaxHP/16);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is hurt by burn");
                }
            }
        },
    };

    //static void Poison(Pokemon pokemon)
    //{
    //    // takeDamage
    //}

}

public enum ConditionID
{
    None,
    Poison,
    Burn,
    Sleep,
    Paralysis,
    Freeze,
}
