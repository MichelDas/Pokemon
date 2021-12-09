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

    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;

    public GameState state = GameState.FreeRoam;

    void Start()
    {
        playerController.StartBattle += StartBattle;
        battleSystem.BattleOver += EndBattle;
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


    void Update()
    {
        if(state == GameState.FreeRoam)
        {
        }
        else if (state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
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
            
        }
        else if (state == GameState.Dialog)
        {
            
        }
    }
    public void StartBattle()
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        // For this I need to get pokemon party data from Player and
        // wild pokemon data MapArea
        PokemonParty playerParty = playerController.GetComponent<PokemonParty>();
        Pokemon wildPokemon = FindObjectOfType<MapArea>().GetRandomWildPokemon();
        battleSystem.StartBattle(playerParty, wildPokemon);
    }

    public void EndBattle()
    {
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
    }
}
