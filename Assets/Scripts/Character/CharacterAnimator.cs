using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] List<Sprite> walkDownSprites;
    [SerializeField] List<Sprite> walkUpSprites;
    [SerializeField] List<Sprite> walkLeftSprites;
    [SerializeField] List<Sprite> walkRightSprites;

    [SerializeField] FaceDirection defaultDirection;

    public float MoveX { get; set; }
    public float MoveY { get; set; }
    public bool IsMoving { get; set; }
    public FaceDirection DefaultDirection { get => defaultDirection;  }

    SpriteAnimator walkDownAnim;
    SpriteAnimator walkUpAnim;
    SpriteAnimator walkLeftAnim;
    SpriteAnimator walkRightAnim;

    SpriteAnimator currentAnim;

    SpriteRenderer spriteRenderer;

    bool wasPreviouslyMoving;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        walkDownAnim = new SpriteAnimator(spriteRenderer, walkDownSprites);
        walkUpAnim = new SpriteAnimator(spriteRenderer, walkUpSprites);
        walkLeftAnim = new SpriteAnimator(spriteRenderer, walkLeftSprites);
        walkRightAnim = new SpriteAnimator(spriteRenderer, walkRightSprites);
        SetFaceDirection(DefaultDirection);
        currentAnim = walkDownAnim;
    }

    private void Update()
    {
        SpriteAnimator prevAnim = currentAnim;

        if(MoveX >= 1)
        {
            currentAnim = walkRightAnim;
        }
        else if (MoveX <= -1)
        {
            currentAnim = walkLeftAnim;
        }
        else if (MoveY >= 1)
        {
            currentAnim = walkUpAnim;
        }
        else if (MoveY <= -1)
        {
            currentAnim = walkDownAnim;
        }

        // If a New animation is called then the animation will be called
        // from the first frame
        if(prevAnim != currentAnim || wasPreviouslyMoving != IsMoving)
        {
            currentAnim.Start();
        }


        if(IsMoving)
        {
            currentAnim.HandleUpdate();
        }
        else
        {
            spriteRenderer.sprite = currentAnim.Frames[0];
        }

        wasPreviouslyMoving = IsMoving;
    }


    public void SetFaceDirection(FaceDirection dir)
    {
        switch (dir)
        {
            case FaceDirection.Up:
                MoveY = 1;
                break;
            case FaceDirection.Down:
                MoveY = -1;
                break;
            case FaceDirection.Right:
                MoveX = 1;
                break;
            case FaceDirection.Left:
                MoveX = -1;
                break;
        }
    }
}

public enum FaceDirection
{
    Up,
    Down,
    Left,
    Right
}
