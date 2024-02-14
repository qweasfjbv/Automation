using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class FinalReport : MonoBehaviour
{
    public List<TextMeshProUGUI> dialogueTexts; // �����Ϳ��� �Ҵ��� Text ��ü ����Ʈ
    public List<string> dialogues;
    public float typingSpeed = 0.05f; // Ÿ���� �ӵ� ����

    private bool isTyping = false; // ���� Ÿ���� ������ ����
    private bool skipToTheEnd = false; // Space �ٸ� ���� Ÿ������ �ǳʶ��� ����

    private void Start()
    {
        for(int i=0; i<dialogueTexts.Count; i++)
        {
            dialogueTexts[i].text = "";
        }
        dialogues.Clear();
        dialogues.Add("Max Population : 100000000");
        dialogues.Add("Survived Population : " + GameManagerEx.Instance.CurPopulation);
        dialogues.Add("Space Trash : " + GameManagerEx.Instance.TrashCnt);
        dialogues.Add("Capsized Ship : " + GameManagerEx.Instance.CapsizedShipCnt);

        dialogues.Add("Return to MainMenu (space)");
    }

    private void Update()
    {
        // ����ڰ� Space �ٸ� ������ Ÿ���� �ǳʶٱ� �Ǵ� ���� Text�� �̵�
        if (Input.GetKeyDown(KeyCode.Space) )
        {
            if(dialogueTexts.Count > 0)
            {
                if (isTyping)
                {
                    skipToTheEnd = true;
                }
                else
                {
                    SoundManager.Instance.PlaySfxSound(Define.SoundType.CHAT);
                    StartCoroutine(TypeDialogue(dialogues[0], dialogueTexts[0]));
                    dialogueTexts.RemoveAt(0);
                    dialogues.RemoveAt(0);
                }
            }
            else
            {
                // ���θ޴��� �̵�, ������ �����
            }
        }
    }

    IEnumerator TypeDialogue(string dialogue, TextMeshProUGUI textComponent)
    {
        isTyping = true;
        textComponent.text = ""; 
        foreach (char letter in dialogue.ToCharArray())
        {
            if (skipToTheEnd)
            {
                textComponent.text = dialogue; // ��ü ��ȭ ������ �ٷ� ǥ��
                skipToTheEnd = false;
                break; // ���� ����
            }
            textComponent.text += letter; // �� ���ھ� �߰�
            yield return new WaitForSecondsRealtime(typingSpeed); // ������ �ӵ��� ���
        }
        isTyping = false;
    }

}
