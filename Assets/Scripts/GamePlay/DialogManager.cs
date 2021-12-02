using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;
    [SerializeField] float lettersPerSecond;

    public UnityAction OnshowDialog;
    public UnityAction OnCloseDialog;
    public UnityAction OnDialogFinishd;

    Dialog dialog;
    int currentLine = 0;
    bool isTyping;

    public bool IsShowing { get; set; }

    public static DialogManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    // As we are moving from free roaming,
    // So it may sometime occur that PlayerHandler and this work on the same time
    //if we execute this at the same frame
    public IEnumerator ShowDialog(Dialog dialog, UnityAction OnFinished)
    {
        yield return new WaitForEndOfFrame();
        IsShowing = true;
        OnshowDialog?.Invoke();
        OnDialogFinishd = OnFinished;
        this.dialog = dialog;
        dialogBox.SetActive(true);
        StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
    }

    IEnumerator TypeDialog(string line)
    { 
        isTyping = true;
        dialogText.text = "";
        foreach(char letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        isTyping = false;

    }

    public void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Z) && isTyping == false)
        {
            currentLine++;
            if (currentLine < dialog.Lines.Count)
                StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
            else
            {

                currentLine = 0;
                dialogBox.SetActive(false);
                OnDialogFinishd?.Invoke();
                OnCloseDialog?.Invoke();
                IsShowing = false;
            }
                
        }
    }
}
