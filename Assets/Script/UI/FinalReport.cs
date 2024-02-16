using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class FinalReport : MonoBehaviour
{
    public List<TextMeshProUGUI> dialogueTexts; // 에디터에서 할당할 Text 객체 리스트
    public List<string> dialogues;
    public float typingSpeed = 0.05f; // 타이핑 속도 조절

    private bool isTyping = false; // 현재 타이핑 중인지 여부
    private bool skipToTheEnd = false; // Space 바를 눌러 타이핑을 건너뛸지 여부

    // 점수에 필요한 모든요소 출력
    // 최대 인원, 살린인원, 살린인원 중 넘친 인원
    // 우주 쓰레기, 우주쓰레기로 인해 전복된 우주선, 아이템
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
        // 사용자가 Space 바를 누르면 타이핑 건너뛰기 또는 다음 Text로 이동
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
                textComponent.text = dialogue; // 전체 대화 내용을 바로 표시
                skipToTheEnd = false;
                break; // 루프 종료
            }
            textComponent.text += letter; // 한 글자씩 추가
            yield return new WaitForSecondsRealtime(typingSpeed); // 지정된 속도로 대기
        }
        isTyping = false;
    }

}
