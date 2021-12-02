using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, IInteractable
{
    [SerializeField] Dialog dialog;
    [SerializeField] List<Vector2> movePattern;
    [SerializeField] float timeBetweenPattern;

    
    Character character;

    NPCState state;
    int currentPattern;

    float idleTimer = 0;

    private void Awake()
    {
        currentPattern = 0;
        state = NPCState.Idle;
        character = GetComponent<Character>();
    }

    public void Interact()
    {
        if(state == NPCState.Idle)
        {
            state = NPCState.Dialog;
            StartCoroutine(DialogManager.Instance.ShowDialog(dialog, OnDialogFinished));
        }
        
    }

    void OnDialogFinished()
    {
        state = NPCState.Idle;
    }

    private void Update()
    {

        if(state == NPCState.Idle)
        {
            idleTimer += Time.deltaTime;
            if(idleTimer > timeBetweenPattern)
            {
                idleTimer = 0;
                StartCoroutine(Walk());
            }
        }
        character.HandleUpdate();
    }

    IEnumerator Walk()
    {
        state = NPCState.Walk;
        yield return character.Move(movePattern[currentPattern]);
        currentPattern = (currentPattern + 1) % movePattern.Count;
        state = NPCState.Idle;
    }
}

public enum NPCState
{
    Idle,
    Walk,
    Dialog
}
