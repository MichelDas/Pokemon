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


    void Update()
    {
        if(state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
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

    public void StartBattle()
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);
        battleSystem.StartBattle();
    }

    public void EndBattle()
    {
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
    }
}
