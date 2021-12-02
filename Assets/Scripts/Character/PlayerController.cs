using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PlayerController : MonoBehaviour
{
    Vector2 input;
    Character character;

    [SerializeField] LayerMask solidObjectLayer;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] LayerMask longGrassLayer;
    public UnityAction StartBattle;

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
                StartCoroutine(character.Move(input, CheckForEncounters));
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
            collider2D.GetComponent<NPCController>()?.Interact();
        }
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
                StartBattle();
            }
        }
    }


}
