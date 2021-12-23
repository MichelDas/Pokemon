using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] PokemonBase _base;
    [SerializeField] int level;
    [SerializeField] bool isPlayerUnit;

    [SerializeField] private BattleHud hud;

    public Pokemon Pokemon { get; set; }
    public bool IsPlayerUnit { get => isPlayerUnit; }
    public BattleHud Hud { get => hud; }

    Vector3 originalPos;
    Color originalColor;
    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        originalPos = transform.localPosition;
        originalColor = image.color;
    }

    public void Setup(Pokemon pokemon)
    {
        Pokemon = pokemon;

        if (IsPlayerUnit)
        {
            image.sprite = Pokemon.Base.BackSprite;
        }
        else
        {
            image.sprite = Pokemon.Base.FrontSprite;

            
        }
        
        
        image.color = originalColor;
        PokemonEnterAnimation();
        hud.SetData(pokemon);
        hud.gameObject.SetActive(true);
    }

    public void PokemonEnterAnimation()
    {
        if (IsPlayerUnit)
        {
            // move to right
            transform.localPosition = new Vector3(-850, originalPos.y);

        }
        else
        {
            // move to left
            transform.localPosition = new Vector3(850, originalPos.y);

        }
        transform.DOLocalMoveX(originalPos.x, 1f);
    }

    public void PlayerAttackAnim()
    { 
        Sequence sequence = DOTween.Sequence();
        if (IsPlayerUnit)
        {
            sequence.Append(transform.DOLocalMoveX(originalPos.x + 50f, 0.25f));
            sequence.Append(transform.DOLocalMoveX(originalPos.x, 0.2f));
        }
        else
        {
            sequence.Append(transform.DOLocalMoveX(originalPos.x - 50f, 0.25f));
            sequence.Append(transform.DOLocalMoveX(originalPos.x, 0.2f));
        }
    }

    public void Clear()
    {
        hud.gameObject.SetActive(false);
    }

    public void PlayerHitAnimation()
    {
        if (Pokemon.HpChanged)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(image.DOColor(Color.gray, 0.1f));
            sequence.Append(image.DOColor(originalColor, 0.1f));
        }

    }

    public void PlayerFaintAnimation()
    {
        
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOLocalMoveY(originalPos.y - 150, 0.5f));
            sequence.Join(image.DOFade(0, 0.5f));
        
        
    }

   
}
