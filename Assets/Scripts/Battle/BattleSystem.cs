using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum BattleState
{
    Start,
    ActionSelection,
    MoveSelection,
    PerformMove,
    Busy,
    PartyScreen,
    BattleOver,
}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] PartyScreen partyScreen;

    //[SerializeField] GameController gameController;
    public UnityAction BattleOver;

    BattleState state;
    int currentAction;  // 0 for fight, 1 for run
    int currentMove;  // 0: upper left, 1: upper right, // 3: lower left, 4: lower right
    int currentMemeber;

    PokemonParty playerParty; // This is a list got from Gamecontroller which got it from player
    Pokemon wildPokemon;  // this is a wild pokemon from gamecontroller which got it from MapArea

    // This will be called from outside this class to start battle
    public void StartBattle(PokemonParty playerParty, Pokemon wildPokemon)
    {
        this.playerParty = playerParty;
        this.wildPokemon = wildPokemon;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        state = BattleState.Start;
        playerUnit.Setup(playerParty.GetHealthyPokemon());
        enemyUnit.Setup(wildPokemon);
        partyScreen.Init();

        // HUD works
        dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);
        yield return dialogBox.TypeDialog($"A wild {enemyUnit.Pokemon.Base.name} apeared.");
        yield return new WaitForSeconds(.5f);
        ChooseFirstTurn();
    }

    void ChooseFirstTurn()
    {
        if(playerUnit.Pokemon.Speed >= enemyUnit.Pokemon.Speed)
        {
            ActionSelection();
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }

    // This will make Player select an action
    void ActionSelection()
    {
        state = BattleState.ActionSelection;
        dialogBox.EnableActionSelector(true);
        StartCoroutine(dialogBox.TypeDialog("Choose an action"));
    }

    // This will make player select Move
    void MoveSelection()
    {
        state = BattleState.MoveSelection;
        dialogBox.EnableDialogText(false);
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableMoveSelector(true);
    }

    void OpenPartyAction()
    {
        state = BattleState.PartyScreen;
        partyScreen.gameObject.SetActive(true);
        partyScreen.SetPartyData(playerParty.Pokemons);
    }



    // This will be called When player select a move
    // After this is done Enemy Move will be called
    IEnumerator PlayerMove()
    {
        state = BattleState.PerformMove;
        //技を洗濯 select move
        Move move = playerUnit.Pokemon.Moves[currentMove];

        yield return RunMove(playerUnit, enemyUnit, move);
        
        if(state == BattleState.PerformMove)
        {
            StartCoroutine(EnemyMove());
        }
    }

    // This will be called when player finishes his move
    // after this is done Action selection will be called
    IEnumerator EnemyMove()
    {
        state = BattleState.PerformMove;
        //技を洗濯 select move
        yield return RunMove(enemyUnit, playerUnit, enemyUnit.Pokemon.GetRandomMove());

        if (state == BattleState.PerformMove)
            ActionSelection();
    }

    IEnumerator RunMove(BattleUnit sourceUnit, BattleUnit targetUnit, Move move)
    {
        move.PP--;
        yield return dialogBox.TypeDialog($"{sourceUnit.Pokemon.Base.Name} has used {move.Base.Name}");
        sourceUnit.PlayerAttackAnim();
        yield return new WaitForSeconds(0.7f);
        targetUnit.PlayerHitAnimation();

        if (move.Base.Category == MoveCategory.Stat)
        {
            MoveEffects effects = move.Base.Effects;
            if (effects.Boosts != null) 
            {
                if (move.Base.Target == MoveTarget.Self)
                {
                    sourceUnit.Pokemon.ApplyBoosts(effects.Boosts);
                }
                else
                {
                    targetUnit.Pokemon.ApplyBoosts(effects.Boosts);
                }
            }
            //yield return Show
        }   
        else
        {
            DamageDetails damageDetails = targetUnit.Pokemon.TakeDamage(move, sourceUnit.Pokemon);
            yield return targetUnit.Hud.UpdateHP();
            yield return ShowDamageDetails(damageDetails);
        }

        if (targetUnit.Pokemon.HP <= 0)
        {
            yield return dialogBox.TypeDialog($"{targetUnit.Pokemon.Base.Name} has fainted");
            targetUnit.PlayerFaintAnimation();
            yield return new WaitForSeconds(0.7f);
            CheckForBattleOver(targetUnit);
        }
        else
        {
            // go to enemy move
            //StartCoroutine(EnemyMove());
            //ActionSelection();
        }
        yield return null;
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

    void CheckForBattleOver(BattleUnit faintedUnit)
    {
        // if the fainted pokemon is player's pokemon
        if (faintedUnit.IsPlayerUnit)
        {
            Pokemon nextPokemon = playerParty.GetHealthyPokemon();
            if (nextPokemon == null)
            {
                state = BattleState.BattleOver;
                BattleOver();
            }
            else
            {
                OpenPartyAction();  // if he has more party pokemon, go to select pokemon screen
            }
        }
        else
        {
            // if the fainted pokemon is not player pokemon i.e. Wild pokemon
            // battle over as it is wild pokemon
            state = BattleState.BattleOver;
            BattleOver();
        }
    }

    public void HandleUpdate()
    {
        // This 2 are being modified
        if (state == BattleState.ActionSelection)
            HandleActionSelection();
        else if (state == BattleState.MoveSelection)
            HandleMoveSelection();
        else if(state == BattleState.PartyScreen)
        {
            HandlePartySelection();
        }
    }

    void HandleActionSelection()
    {
        // 0: Fight     1: Bag
        // 2: Pokemon   3: Run
        if (Input.GetKeyDown(KeyCode.RightArrow))
                currentAction++;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
                currentAction--;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
                currentAction+=2;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
                currentAction-=2;
        currentAction = Mathf.Clamp(currentAction, 0, 3);

        dialogBox.UpdateActionSelection(currentAction);


        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentAction == 0)
            {
                MoveSelection();
            }
            else if(currentAction == 2)
            {
                OpenPartyAction();
                
            }
        }
    }

    // 0: upper left, 1: upper right,
    // 3: lower left, 4: lower right
    void HandleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
                currentMove++;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
                currentMove--;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
                currentMove += 2;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
                currentMove -= 2;

        currentMove = Mathf.Clamp(currentMove, 0, playerUnit.Pokemon.Moves.Count - 1);

        dialogBox.UpdateMoveSelection(currentMove, playerUnit.Pokemon.Moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            // Here Run Move will be called
            StartCoroutine(PlayerMove());
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            dialogBox.EnableDialogText(true);
            dialogBox.EnableActionSelector(true);
            dialogBox.EnableMoveSelector(false);
            ActionSelection();
        }
    }

    void HandlePartySelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            currentMemeber++;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            currentMemeber--;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            currentMemeber += 2;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            currentMemeber -= 2;

        currentMemeber = Mathf.Clamp(currentMemeber, 0, playerParty.Pokemons.Count - 1);

        partyScreen.UpdateMemberSelection(currentMemeber);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            // Select monster
            Pokemon selectedMember = playerParty.Pokemons[currentMemeber];

            if (selectedMember.HP <= 0)
            {
                partyScreen.SetMessage("Pokemon has fainted");
                return;
            }

            if (selectedMember == playerUnit.Pokemon)
            {
                partyScreen.SetMessage("Pokemon is out");
                return;
            }
            

            partyScreen.gameObject.SetActive(false);
            state = BattleState.Busy;
            StartCoroutine(SwitchPokemon(selectedMember));

        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            partyScreen.gameObject.SetActive(false);
            ActionSelection();
        }

    }

    IEnumerator SwitchPokemon(Pokemon newPokemon)
    {
        bool fainted = playerUnit.Pokemon.HP <= 0;

        if (!fainted)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.name} return!");
            playerUnit.PlayerFaintAnimation();
            yield return new WaitForSeconds(1.5f);
        }

      
        playerUnit.Setup(newPokemon);
        playerUnit.Hud.SetData(playerUnit.Pokemon);
        dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);
        yield return dialogBox.TypeDialog($" Go {playerUnit.Pokemon.Base.Name}");
        if (fainted)
        {
            //ActionSelection();
            ChooseFirstTurn();
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
        
        //ActionSelection();
    }
}
