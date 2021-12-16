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
        Animator.MoveX = Mathf.Clamp(moveVec.x, -1f, 1f);
        Animator.MoveY = Mathf.Clamp(moveVec.y, -1, 1f);
        Vector3 targetPos = transform.position;
        targetPos += (Vector3) moveVec;
        if (!IsPathClear(targetPos))
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

    bool IsPathClear(Vector3 targetPos)
    {
        Vector3 diff = targetPos - transform.position;
        Vector3 dir = diff.normalized;
        return Physics2D.BoxCast(transform.position + dir, new Vector2(0.2f, 0.2f), 0, dir, diff.magnitude - 1, GameLayers.Instance.SolidObjectLayer | GameLayers.Instance.InteractableLayer | GameLayers.Instance.PlayerLayer) == false;
    }

    bool IsWalkable(Vector2 targetPos)
    {
        return !Physics2D.OverlapCircle(targetPos, 0.2f, GameLayers.Instance.SolidObjectLayer | GameLayers.Instance.InteractableLayer);
    }

    // Look toward targetPos
    public void LookToward(Vector3 targetPos)
    {
        float xDiff = Mathf.Floor(targetPos.x) - Mathf.Floor(transform.position.x);
        float yDiff = Mathf.Floor(targetPos.y) - Mathf.Floor(transform.position.y);

        if(xDiff == 0 || yDiff == 0)
        {
            Animator.MoveX = Mathf.Clamp(xDiff, -1f, 1f);
            Animator.MoveY = Mathf.Clamp(yDiff, -1, 1f);
        }
        
    }
}
