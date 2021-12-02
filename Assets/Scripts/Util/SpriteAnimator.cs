using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator
{
    SpriteRenderer spriteRenderer;

    public List<Sprite> frames;

    float frameRate;

    int currentFrame;
    float timer;

    public List<Sprite> Frames { get => frames; private set => frames = value; }

    public SpriteAnimator(SpriteRenderer spriteRenderer, List<Sprite> frames, float frameRate = 0.16f)
    {
        this.spriteRenderer = spriteRenderer;
        this.Frames = frames;
        this.frameRate = frameRate;
    }

    public void Start()
    {
        currentFrame = 0;
        timer = 0;
        spriteRenderer.sprite = Frames[currentFrame];
    }


    public void HandleUpdate()
    {
        timer += Time.deltaTime;

        if(timer > frameRate)
        {
            
            currentFrame = (currentFrame + 1) % Frames.Count;
            spriteRenderer.sprite = Frames[currentFrame];
            timer -= frameRate;
        }
    }
}
