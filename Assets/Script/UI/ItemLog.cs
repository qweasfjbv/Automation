using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemLog : MonoBehaviour
{
    [SerializeField]
    private Image itemLogImage;

    [SerializeField]
    private TMP_Text itemLogText;

    private float showTime;

    private void Awake()
    {
        showTime = 3.0f;

        StartCoroutine(NaturalDisappear());
    }

    public void SetItemLog(int id, int cnt)
    {
        itemLogText.text = "Get " + Managers.Resource.GetItemData(id).Name + "!\nx" + cnt;
        itemLogImage.sprite = Managers.Resource.GetItemSprite(id);
    }

    private IEnumerator NaturalDisappear()
    {
        yield return new WaitForSeconds(showTime);

        while (itemLogImage.color.a > 0)
        {
            var imageColor = itemLogImage.color;
            var textColor = itemLogText.color;
            imageColor.a -= 2.0f * Time.unscaledDeltaTime;
            textColor.a -= 2.0f * Time.unscaledDeltaTime;
            itemLogImage.transform.parent.GetComponent<Image>().color = imageColor;
            itemLogImage.color = imageColor;
            itemLogText.color = textColor;

            yield return null;
        }

        Destroy(gameObject);
    }
}