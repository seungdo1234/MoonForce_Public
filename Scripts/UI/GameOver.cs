using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public float lerpTime;


    public GameObject deadImage;
    public Button lobbyBtn;
    public Text lobbyBtnText;


    private void OnEnable()
    {
        lobbyBtn.interactable = false;
        lobbyBtn.gameObject.SetActive(false);

        StartCoroutine(AlphaStart());
    }

    private IEnumerator AlphaStart()
    {
        yield return new WaitForSeconds(1.5f);

        lobbyBtn.gameObject.SetActive(true);
        float currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, currentTime / lerpTime);
            lobbyBtn.image.color = new Color(lobbyBtn.image.color.r, lobbyBtn.image.color.g, lobbyBtn.image.color.b, alpha);
            lobbyBtnText.color = new Color(lobbyBtnText.color.r, lobbyBtnText.color.g, lobbyBtnText.color.b, alpha);
            yield return null;
        }

        lobbyBtn.interactable = true;
    }
}
