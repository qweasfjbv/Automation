using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI dialogueText;

    private List<string[]> dialogues = new List<string[]>();

    private int curDialogueIndex = 0;
    private int curLineIndex = 0;
    private bool isTyping = false; // 현재 타이핑 중인지 확인

    private void Start()
    {
        GenerateTutorialLine();
        StartCoroutine(TypeDialogue(dialogues[curDialogueIndex][curLineIndex]));
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!CanNextTalk(curDialogueIndex))
            {
                if (isTyping)
                {
                    // 타이핑 중이면 현재 대사를 즉시 완료
                    StopAllCoroutines();
                    dialogueText.text = "Cheer up!";
                    isTyping = false;
                }
                else
                {
                    StartCoroutine(TypeDialogue("Cheer up!"));
                }

            }
            else
            {
                if (isTyping)
                {
                    // 타이핑 중이면 현재 대사를 즉시 완료
                    StopAllCoroutines();
                    dialogueText.text = dialogues[curDialogueIndex][curLineIndex];
                    isTyping = false;
                }
                else
                {
                    ShowNextLine();
                }
            }
        }
    }
    IEnumerator TypeDialogue(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        isTyping = false;
    }

    private bool CanNextTalk(int dialogIndex)
    {
        Debug.Log(dialogIndex);
        switch (dialogIndex) {
            case 2:
                return false;
            default:
                return true;
        }

    }

    void ShowNextLine()
    {

        curLineIndex++;

        if (curDialogueIndex < dialogues.Count &&
            curLineIndex < dialogues[curDialogueIndex].Length)
        {
            StartCoroutine(TypeDialogue(dialogues[curDialogueIndex][curLineIndex]));
        }
        else
        {
            curDialogueIndex++;
            curLineIndex = 0;

            if (curDialogueIndex < dialogues.Count)
            {
                StartCoroutine(TypeDialogue(dialogues[curDialogueIndex][curLineIndex]));
            }
            else
            {
                dialogueText.text = "";
            }
        }

    }

    private void GenerateTutorialLine()
    {
        dialogues.Add(new string[]
        {
            "HI, I am T-Robo.",
            "I'll Help you."
        });

        dialogues.Add(new string[]{
            "This is second talkdata",
            "second_second talkdata."
        });

        dialogues.Add(new string[] { "DOit!"});

        dialogues.Add(new string[]
        {
            "GoodJOB!"
        });

    }

}
