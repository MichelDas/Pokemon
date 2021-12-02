using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    FreeRoam,
    Battle,
    Dialog
}

public class GameController : MonoBehaviour
{
    public GameState state = GameState.FreeRoam;

    [SerializeField] PlayerController playerController;
    //[SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;


    // Start is called before the first frame update
    void Start()
    {
        DialogManager.Instance.OnshowDialog += ShowDialog;
        DialogManager.Instance.OnCloseDialog += CloseDialog;
    }

    void ShowDialog()
    {
        state = GameState.Dialog;
    }

    void CloseDialog()
    {
        if(state == GameState.Dialog)
        {
            state = GameState.FreeRoam;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(state == GameState.FreeRoam)
        {
        }
        else if (state == GameState.Battle)
        {
            // This code will be uncommented later
            //battleSystem.HandleUpdate();
        }
        else if(state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
    }

    private void FixedUpdate()
    {
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
        }
        else if (state == GameState.Battle)
        {
            // This code will be uncommented later
            //battleSystem.HandleUpdate();
        }
        else if (state == GameState.Dialog)
        {
            
        }
    }


    public void EndBattle()
    {
        state = GameState.FreeRoam;
        // More codes need to be written here
    }
}
