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

    // ������ �ʿ��� ����� ���
    // �ִ� �ο�, �츰�ο�, �츰�ο� �� ��ģ �ο�
    // ���� ������, ���־������ ���� ������ ���ּ�, ������
    // + Return to Mainmenu
    private void Start()
    {
        for(int i=0; i<dialogueTexts.Count; i++)
        {
            dialogueTexts[i].text = "";
        }
        dialogues.Clear();


        dialogues.Add("FINAL REPOPT");

        dialogues.Add("POLLUTION");
        // Pollution Report
        dialogues.Add("Space Trash : " + GameManagerEx.Instance.TrashCnt);
        dialogues.Add("Capsized SpaceShip : " + GameManagerEx.Instance.CapsizedShipCnt);
        dialogues.Add("Capsized Items : " + GameManagerEx.Instance.CapsizedItemCnt);

        dialogues.Add("POPULATION");
        // People Report
        dialogues.Add("Max Population : " + GameManagerEx.Instance.MaxPopulation.ToString("N0"));
        
        dialogues.Add("killed by Environment Pollution : " + (GameManagerEx.Instance.MaxPopulation - 
            (GameManagerEx.Instance.PEOPLEPERSHIP * GameManagerEx.Instance.CapsizedShipCnt + GameManagerEx.Instance.SpaceShipCnt * GameManagerEx.Instance.PEOPLEPERSHIP)).ToString("N0"));
        dialogues.Add("killed by the capsizing of a spaceship : " + (GameManagerEx.Instance.PEOPLEPERSHIP * GameManagerEx.Instance.CapsizedShipCnt).ToString("N0"));
        
        dialogues.Add("Survived Population : " + (GameManagerEx.Instance.SpaceShipCnt * GameManagerEx.Instance.PEOPLEPERSHIP).ToString("N0"));
        dialogues.Add("Excess Population : " + GameManagerEx.Instance.ExcessPopulation);

        // Final Score
        
        dialogues.Add("SCORE : " + GameManagerEx.Instance.FinalScore);

        //
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
                if (!isTyping)
                {
                    Managers.Data.DeleteAll();
                    Time.timeScale = 1;
                    Managers.Scene.LoadScene(SceneEnum.Mainmenu);
                }
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
