using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCharactor : MonoBehaviour
{
    private bool isFlip;
    private SpriteRenderer sprite;
    private SpriteRenderer shadowSprite;
    private MainMenuAnimation mainMenu;
    private void Awake()
    {
        mainMenu = GetComponentInParent<MainMenuAnimation>();
        SpriteRenderer[] sr = GetComponentsInChildren<SpriteRenderer>();

        sprite = sr[0];
        shadowSprite = sr[1];
    }

  
    private void Update()
    {
        if (!isFlip && mainMenu.isRotation)
        {
            isFlip = true;
            sprite.flipY = true;
            shadowSprite.transform.localPosition = new Vector3(shadowSprite.transform.localPosition.x, 0.35f, shadowSprite.transform.localPosition.z);


        }
        else if(isFlip && !mainMenu.isRotation)
        {
            isFlip = false;
            sprite.flipY = false;
            shadowSprite.transform.localPosition = new Vector3(shadowSprite.transform.localPosition.x, -0.35f, shadowSprite.transform.localPosition.z);

        }
    }
}
