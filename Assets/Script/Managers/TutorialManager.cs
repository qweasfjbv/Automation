using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI dialogueText;

    private List<string[]> dialogues = new List<string[]>();
    private string prevDialogue = "";

    private int curDialogueIndex = 0;
    private int curLineIndex = 0;
    private bool isTyping = false; // 현재 타이핑 중인지 확인

    private void Start()
    {
        GenerateTutorialLine();
        StartCoroutine(TypeDialogue(dialogues[curDialogueIndex][curLineIndex]));
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!CanNextTalk(curDialogueIndex))
            {
                if (isTyping)
                {

                    StopAllCoroutines();
                    dialogueText.text = prevDialogue;
                    isTyping = false;
                }
                else
                {
                    if (dialogueText.text == prevDialogue)
                    {
                        dialogueText.text = "";
                    }
                    else
                    {
                        StartCoroutine(TypeDialogue(prevDialogue));
                    }
                }

            }
            else
            {
                if (isTyping)
                {

                    StopAllCoroutines();
                    dialogueText.text = dialogues[curDialogueIndex][curLineIndex];
                    isTyping = false;
                }
                else
                {
                    prevDialogue = dialogues[curDialogueIndex][curLineIndex];
                    ShowNextLine();
                }
            }
        }
    }
    private IEnumerator TypeDialogue(string line)
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


    private void ShowNextLine()
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

    private bool CanNextTalk(int dialogIndex)
    {
        switch (dialogIndex)
        {
            case 1:
                return CheckContructMode();
            case 2:
                return CheckBuildMM();
            case 3:
                return CheckBeltLink();
            case 4:
                return CheckSmelterLink();
            case 5:
                return CheckSmelterOutputId();
            default:
                return true;
        }

    }
    private void GenerateTutorialLine()
    {
        dialogues.Add(new string[]
        {
            "Welcome to \n\"K-pler project\"!!  \n (space)",
            "I'm Tutorial-Robot.",
            "and you are 'AI'",
            "Our goal is to transform alien planets into an environment similar to Earth.",
            "Let's go through some basics to get you started.",
            "You can move the screen by dragging, and zoom in/out by scrolling the mouse wheel.",
            "You can view the dictionary by pressing the button in the top left corner.",
            "or 'd' key and 'b' key.",
            "You can build structures using the UI located at the bottom center of the screen.",
            "or F1 ~ f6, 1 ~ 6 key",

            "Press F1 ~ F6 to enable 'contruct mode'"
        });

        dialogues.Add(new string[] {
            "",
            "In construct mode, you can build/unbuild buildings.",
            "build : left click \n unbuild : right click \n R : rotate direction",
            "When you drag, building direction will change automatically.",
            "If you don't want this, you can hold the Shift key to lock the direction of the building.",

            "Try building the mining machine on coalvein facing to the right. (refer to dictionary)",
        });


        dialogues.Add(new string[]{
            "",
            "Great!",
            "now, try connecting one belt to the mining machine."
        });

        dialogues.Add(new string[] {
            "",
            "you can see an item on belt. right?",
            "it is coal ore, one of the most basic material.",
            "now, build smelter to the right of the belt."
        });

        dialogues.Add(new string[]
        {
            "",
            "if you click smelter, you can see building information",
            "and you can change the items manufactured in the factory",
            "Change making item to coal ingot"
        });

        dialogues.Add(new string[]
        {
            "",
            "Good job! \nThis is all you need to do for your task.",
            "Tutorial is done.",
            "don't forget your goal.",
            "create robots, send them to other planets.",
            "Then, the robots will work to make the environment similar to that of Earth.",
            "you are our last hope.",
            "But, don't worry",
            "I will always be here to help you with our goals"
        });

    }

    private bool CheckContructMode()
    {
        return Managers.Input.Mode != InputManager.InputMode.None;
    }
    private bool CheckBuildMM()
    {
        var tmp = Managers.Map.GetTileOnPoint(new Vector2(2, 2));

        if (tmp == null || tmp.id == -1 || tmp.building.GetComponent<MiningMachine>() == null) return false;
        else
        {
            if (tmp.rot == 1) return true;
        }
        return false;
    }
    private bool CheckBeltLink()
    {
        var tmp = Managers.Map.GetTileOnPoint(new Vector2(2, 2));
        if (CheckBuildMM())
        {
            return tmp.building.GetComponent<MiningMachine>().NextBelt != null;
        }
        return false;
    }
    private bool CheckSmelterLink()
    {

        var tmp = Managers.Map.GetTileOnPoint(new Vector2(4, 2));

        if (tmp == null || tmp.id == -1 || tmp.building.GetComponent<Smelter>() == null) return false;
        

        return true;
    }
    private bool CheckSmelterOutputId()
    {
        if (!CheckSmelterLink()) return false;

        return Managers.Map.GetTileOnPoint(new Vector2(4, 2)).building.GetComponent<Smelter>().OutputItemId == 19;
    }

}