using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Automation/Quest")]
public class QuestData : ScriptableObject
{
    [SerializeField] private int questId;
    [SerializeField] private string questName;
    // Lv 0 : Inven, Lv 1 : Mass Driver
    [SerializeField] private int questLv;
    [SerializeField] private List<Ingredient> ingredients = new List<Ingredient>();
    [SerializeField] private float timeLimit;


    public int ID { get => questId; }
    public int Lv { get => questLv; }
    public List<Ingredient> Ingredients { get => ingredients; }
    public float TimeLimit { get => timeLimit; }
    public string QuestName { get => questName; }

    public void SetQuestData(QuestJsonData data)
    {
        questId = data.questId;
        questLv= data.questLv;
        ingredients = data.ingredients;
        timeLimit = data.timeLimit;
        questName = data.questName;
    }
}
