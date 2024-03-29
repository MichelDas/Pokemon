﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerController : MonoBehaviour
{
    [SerializeField] new string name;
    [SerializeField] Sprite sp;
    [SerializeField] GameObject exclamation;
    [SerializeField] Dialog dialog;
    [SerializeField] GameObject fov;

    Character character;

    public string Name { get => name;  }
    public Sprite Sp { get => sp; }

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    private void Start()
    {
        SetFovRotation(character.Animator.DefaultDirection);
    }

    //トレーナーバトル開始する
    public IEnumerator TriggerTrainerBattle(PlayerController player)
    {
        exclamation.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        exclamation.SetActive(false);

        var diff = player.transform.position - transform.position;
        //normalized part biyog korsi jate ek ghor kom jai
        var MoveVoc = diff - diff.normalized;
        MoveVoc = new Vector2(Mathf.Round(MoveVoc.x), Mathf.Round(MoveVoc.y));
        yield return character.Move(MoveVoc);

        StartCoroutine(DialogManager.Instance.ShowDialog(dialog, StartBattle));

    }

    void StartBattle()
    {
        GameController.Instance.StartTrainerBattle(this);
    }

    void SetFovRotation(FaceDirection dir)
    {
        float angle = 0;
        switch (dir)
        {
            case FaceDirection.Up:
                angle = 180;
                break;
            case FaceDirection.Down:
                angle = 0;
                break;
            case FaceDirection.Right:
                angle = 90;
                break;
            case FaceDirection.Left:
                angle = -90;
                break;
        }
        fov.transform.eulerAngles = new Vector3(0, 0, angle);
    }
}
