using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Automation/Quest")]
public class QuestData : ScriptableObject
{
    [SerializeField] private int questId;
    [SerializeField] private string questName;
    [SerializeField] private List<Ingredient> ingredients = new List<Ingredient>();
    [SerializeField] private int populationLimit;
    [SerializeField] private string questDescription;

    public int ID { get => questId; }
    public List<Ingredient> Ingredients { get => ingredients; }
    public int PopulationLimit { get => populationLimit; }
    public string QuestName { get => questName; }
    public string QuestDescription { get => questDescription; }
    public void SetQuestData(QuestJsonData data)
    {
        questId = data.questId;
        ingredients = data.ingredients;
        populationLimit = data.populationLimit;
        questName = data.questName;
        questDescription = data.questDescription;

    }
}
