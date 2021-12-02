using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState
{
    Start,
    PlayerAction,
    PlayerMove,
    EnemyMove,
    Busy,
}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogBox dialogBox;

    [SerializeField] GameController gameController;

    BattleState state;
    int currentAction;  // 0 for fight, 1 for run
    int currentMove;  // 0: upper left, 1: upper right,
                      // 3: lower left, 4: lower right

    // Start is called before the first frame update
    public void StartBattle()
    {
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        state = BattleState.Start;
        playerUnit.Setup();
        enemyUnit.Setup();
        playerHud.SetData(playerUnit.Pokemon);
        enemyHud.SetData(enemyUnit.Pokemon);
        dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);
        yield return dialogBox.TypeDialog($"A wild {enemyUnit.Pokemon.Base.name} apeared.");
        yield return new WaitForSeconds(.5f);
        PlayerAction();
    }

    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        dialogBox.EnableActionSelector(true);
        StartCoroutine(dialogBox.TypeDialog("Choose an action"));
    }

    void PlayerMove()
    {
        state = BattleState.PlayerMove;
        dialogBox.EnableDialogText(false);
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableMoveSelector(true);
    }

    //PlayerMove 実行する
    IEnumerator PerformPlayerMove()
    {
        state = BattleState.Busy;

        //技を洗濯 select move
        Move move = playerUnit.Pokemon.Moves[currentMove];
        yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name} has used {move.Base.Name}");
        playerUnit.PlayerAttackAnim();
        yield return new WaitForSeconds(0.7f);
        enemyUnit.PlayerHitAnimation();
        DamageDetails damageDetails = enemyUnit.Pokemon.TakeDamage(move, playerUnit.Pokemon);
        yield return enemyHud.UpdateHP();
         yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name} has fainted");
            enemyUnit.PlayerFaintAnimation();
            yield return new WaitForSeconds(0.7f);
            gameController.EndBattle();
        }
        else
        {
            // go to enemy move
            StartCoroutine(EnemyMove());
        }

    }

    IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;

        //技を洗濯 select move
        Move move = enemyUnit.Pokemon.GetRandomMove();
        yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name} has used {move.Base.Name}");
        enemyUnit.PlayerAttackAnim();
        yield return new WaitForSeconds(0.7f);
        playerUnit.PlayerHitAnimation();
        DamageDetails damageDetails = playerUnit.Pokemon.TakeDamage(move, enemyUnit.Pokemon);
        yield return playerHud.UpdateHP();

        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name} has fainted");
            playerUnit.PlayerFaintAnimation();
            yield return new WaitForSeconds(0.7f);
            gameController.EndBattle();
        }
        else
        {
            // go to enemy move
            PlayerMove();
        }
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if(damageDetails.Critical > 1f)
        {
            yield return dialogBox.TypeDialog("It was a critical hit");
        }
        if (damageDetails.TypeEffectiveness > 1f)
        {
            yield return dialogBox.TypeDialog("It was effective");
        }
        else if(damageDetails.TypeEffectiveness < 1f)
        {
            yield return dialogBox.TypeDialog("It was not so effective");
        }
    }

    public void HandleUpdate()
    {
        if (state == BattleState.PlayerAction)
            HandleActionSelection();
        else if (state == BattleState.PlayerMove)
            HandleMoveSelection();
    }

    void HandleActionSelection()
    {
        // if down is pressed then run if up is pressed fight
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentAction < 1)
                currentAction++;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentAction > 0)


                currentAction--;
        }

        dialogBox.UpdateActionSelection(currentAction);


        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentAction == 0)
            {
                PlayerMove();
            }

        }
    }

    // 0: upper left, 1: upper right,
    // 3: lower left, 4: lower right
    void HandleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentMove < playerUnit.Pokemon.Moves.Count - 1)
                currentMove++;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentMove > 0)
                currentMove--;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentMove < playerUnit.Pokemon.Moves.Count - 2)
                currentMove += 2;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentMove > 1)
                currentMove -= 2;
        }

        dialogBox.UpdateMoveSelection(currentMove, playerUnit.Pokemon.Moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PerformPlayerMove());
        }
    }

}
