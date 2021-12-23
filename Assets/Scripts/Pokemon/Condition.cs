using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
public class Condition
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string HUDName { get; set; }
    public string StartMessage { get; set; }
    public ConditionID Id { get; set; }
    public Action<Pokemon> OnStart;
    public Func<Pokemon, bool> OnBeforeMove;
    public Action<Pokemon> onAfterTurn;
    
}
