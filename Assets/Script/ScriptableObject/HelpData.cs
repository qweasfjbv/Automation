using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Automation/Help")]
public class HelpData : ScriptableObject
{

    [SerializeField] private int helpId;
    [SerializeField] private string helpContent;
    [SerializeField] private Sprite helpSprite;

    public int ID { get => helpId; set => helpId = value; }
    public string HelpContent { get => helpContent; set => helpContent = value; }
    public Sprite HelpSprite { get => helpSprite; set => helpSprite = value; }
}
