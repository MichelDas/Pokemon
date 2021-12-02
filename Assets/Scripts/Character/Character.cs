using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    CharacterAnimator animator;
    [SerializeField] float moveSpeed;

    public bool IsMoving { get; set; }

    public CharacterAnimator Animator { get => animator; }

    private void Awake()
    {
        animator = GetComponent<CharacterAnimator>();
    }

    public IEnumerator Move(Vector2 moveVec, UnityAction OnMoveOver = null)
    {
        Animator.MoveX = moveVec.x;
        Animator.MoveY = moveVec.y;
        Vector3 targetPos = transform.position;
        targetPos += (Vector3) moveVec;
        if (!IsWalkable(targetPos))
        {
            yield break;
        }

        IsMoving = true;

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
        IsMoving = false;
        OnMoveOver?.Invoke();
    }

    public void HandleUpdate()
    {
        animator.IsMoving = IsMoving;
    }

    bool IsWalkable(Vector2 targetPos)
    {
        return !Physics2D.OverlapCircle(targetPos, 0.2f, GameLayers.Instance.SolidObjectLayer | GameLayers.Instance.InteractableLayer);
    }
}
