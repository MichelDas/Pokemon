using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    bool isMoving;
    Vector2 input;

    CharacterAnimator animator;

    [SerializeField] LayerMask solidObjectLayer;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] LayerMask longGrassLayer;

    private void Awake()
    {
        animator = GetComponent<CharacterAnimator>();
    }

    // Update is called once per frame
    public void HandleUpdate()
    {
        if(!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if(input.x != 0)
            {
                input.y = 0;
            }


            if (input != Vector2.zero)
            {
                animator.MoveX = input.x;
                animator.MoveY = input.y;
                Vector2 targetPos = transform.position;
                targetPos += input;
                if(IsWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }
            }
        }
        animator.IsMoving = isMoving;
        if( Input.GetKeyDown(KeyCode.Z))
        {
            Interact();

        }
        
    }

    void Interact()
    {
        Vector3 faceDirection = new Vector3(animator.MoveX, animator.MoveY);
        Vector3 interactPos = transform.position + faceDirection;
        Collider2D collider2D = Physics2D.OverlapCircle(interactPos, 0.3f, interactableLayer);

        if (collider2D)
        {
            collider2D.GetComponent<NPCController>()?.Interact();
        }
    }

    IEnumerator Move(Vector3  targetPos)
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                moveSpeed * Time.deltaTime
                );
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
        CheckForEncounters();
    }

    bool IsWalkable(Vector2 targetPos)
    {
        return !Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectLayer | interactableLayer);
    }

    void CheckForEncounters()
    {
        if(Physics2D.OverlapCircle(transform.position, 0.2f, longGrassLayer ))
        {
            // 10% possibility of encounter
            if(Random.Range(0,100) < 10)
            {
                Debug.Log("Monster Encounter");
                animator.IsMoving = false;
            }
        }
    }


}
