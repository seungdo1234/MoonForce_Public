using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public Transform[] gameObjects;

    public float lerpTime;

    private Animator rockAnim;
    private void Awake()
    {
        gameObjects = GetComponentsInChildren<Transform>();
        rockAnim = gameObjects[1].GetComponent<Animator>();

        gameObjects[2].localPosition = new Vector3(2.9f, -5.3f, 0);
    }

    private void OnEnable()
    {
        gameObjects[1].position = gameObjects[3].position;
        gameObjects[2].gameObject.SetActive(false);
        StartCoroutine(MeteorStart());
    }

    private IEnumerator MeteorStart()
    {
        float currentTime = 0f;

        yield return new WaitForSeconds(0.5f);

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime;
            float x_Pos = Mathf.Lerp(gameObjects[3].position.x, gameObjects[4].position.x, currentTime / lerpTime);
            float y_Pos = Mathf.Lerp(gameObjects[3].position.y, gameObjects[4].position.y, currentTime / lerpTime);
            gameObjects[1].position = new Vector3(x_Pos, y_Pos, 0);

            yield return null;
        }

        rockAnim.SetTrigger("End");
        gameObjects[2].gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);
    }
}
