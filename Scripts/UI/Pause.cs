using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [Header("Pause")]
    public GameObject[] pausePrevObjects;
    public Transform canvas;
    private int canvasChildCoint;

    public Image pauseBtnImage;
    public Sprite pauseBtnOnImage;
    public Sprite pauseBtnOffImage;


    private void Start()
    {
        // Canvas의 자식 오브젝트의 갯수를 구합니다.
       canvasChildCoint = canvas.childCount ;

    }
    public void PauseOn() // 퍼즈 온
    {
        CollectActiveObjects(); // 퍼즈할 때 활성화 돼 있는 UI 오브젝트들을 하나의 배열에 넣음
        ActiveDeactivateObjects(false); // UI 비활성화
        pauseBtnImage.sprite = pauseBtnOffImage; // Pause 버튼 아이콘 변경 
        Time.timeScale = 0; // 멈추기

    }
    public void PauseOff()
    {
        Time.timeScale = 1; // 재생
        ActiveDeactivateObjects(true); // 활성화
        pauseBtnImage.sprite = pauseBtnOnImage;
    }

  public void MainMenu() // 메인 메뉴로 돌아갈 때
    {
        Time.timeScale = 1; // 재생

        GameManager.instance.gameStop = true; // 게임 멈춤

        ActiveDeactivateObjects(false); // UI 비활성화
        pauseBtnImage.sprite = pauseBtnOnImage;

        // 0번과 1번, 2번은 비활성화 되면 안되기 때문에 다시 활성화
        pausePrevObjects[0].SetActive(true);
        pausePrevObjects[1].SetActive(true);
        pausePrevObjects[2].SetActive(true);

        // 활성화된 풀링 오브젝트들 비활성화
        GameManager.instance.GameLobby();

    }
    private void ActiveDeactivateObjects(bool isActive)
    {
        for(int i =0; i < canvasChildCoint; i++)
        {
            pausePrevObjects[i].SetActive(isActive);
            if(pausePrevObjects[i+1] == gameObject)
            {
                break;
            }
        }
    }
    private void CollectActiveObjects()
    {
        // Canvas의 자식 오브젝트의 갯수를 기반으로 배열을 초기화합니다.
        pausePrevObjects = new GameObject[canvasChildCoint];
        int parentCount = 0;

        // Canvas의 모든 자식 오브젝트를 순회합니다.
        foreach (Transform child in canvas)
        {
            // 만약 자식 오브젝트의 자식이 없다면 (부모 오브젝트라면)

            if (child.gameObject.activeSelf)
            {
                // 배열에 추가합니다.
                pausePrevObjects[parentCount] = child.gameObject;
                parentCount++;
            }
            
        }
    }
}
