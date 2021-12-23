using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PlayerController : MonoBehaviour
{
    Vector2 input;
    Character character;

    public UnityAction OnEncounted;
    public UnityAction<Collider2D> OnEnterTrainersView;

    [SerializeField] new string name;
    [SerializeField] Sprite sp;

    public string Name { get => name; }
    public Sprite Sp { get => sp;  }

    private void Awake()
    {
        character = GetComponent<Character>();
        
    }

    // Update is called once per frame
    public void HandleUpdate()
    {
        if(!character.IsMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if(input.x != 0)
            {
                input.y = 0;
            }


            if (input != Vector2.zero)
            {
                StartCoroutine(character.Move(input, OnMoveOver));
            }
        }

        character.HandleUpdate();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Interact();
        }
    }

    void Interact()
    {
        Vector3 faceDirection = new Vector3(character.Animator.MoveX, character.Animator.MoveY);
        Vector3 interactPos = transform.position + faceDirection;
        Collider2D collider2D = Physics2D.OverlapCircle(interactPos, 0.3f, GameLayers.Instance.InteractableLayer);

        if (collider2D)
        {
            collider2D.GetComponent<NPCController>()?.Interact(transform.position);
        }
    }

    void OnMoveOver()
    {
        CheckForEncounters();
        CheckIfTrainerView();
    }

    void CheckForEncounters()
    {
        if(Physics2D.OverlapCircle(transform.position, 0.2f, GameLayers.Instance.LongGrassLayer ))
        {
            // 10% possibility of encounter
            if(Random.Range(0,100) < 10)
            {
                Debug.Log("Monster Encounter");
                character.Animator.IsMoving = false;
                OnEncounted();
            }
        }
    }

    void CheckIfTrainerView()
    {
        Collider2D trainerCollider2D = Physics2D.OverlapCircle(transform.position, 0.2f, GameLayers.Instance.FovLayer);
        if (trainerCollider2D)
        {
            Debug.Log("Entered trainer Field of view");
            character.Animator.IsMoving = false;
            OnEnterTrainersView?.Invoke(trainerCollider2D);
        }
    }


}
