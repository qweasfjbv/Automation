using UnityEngine;
using UnityEngine.EventSystems;

public class ChildClickHandler : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {

        eventData.Use();
    }
}