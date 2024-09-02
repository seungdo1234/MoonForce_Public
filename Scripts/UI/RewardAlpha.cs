using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardAlpha : MonoBehaviour
{
    public float lerpTime;

    private ItemSlot slot;
    private Image reward;

    private void Awake()
    {
        reward = GetComponent<Image>();
        slot = GetComponent<ItemSlot>();
    }
    public void StartAlpha()
    {
        // 스킬 이미지
        if (slot.item.type == ItemType.Book)
        {
            slot.skillBookImage.sprite = GameManager.instance.coolTime.skillSprites[slot.item.skillNum - 1];
            slot.skillBookImage.SetNativeSize();
        }

        StartCoroutine(Alpha(0, 1));
    }
    private IEnumerator Alpha(float start, float end)
    {
        float currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Lerp(start, end, currentTime / lerpTime);
            reward.color = new Color(reward.color.r, reward.color.g, reward.color.b, alpha);
            if (slot.item.type == ItemType.Book)
            {
                slot.skillBookImage.color = new Color(reward.color.r, reward.color.g, reward.color.b, alpha);
            }
            yield return null;
        }

    }
}
