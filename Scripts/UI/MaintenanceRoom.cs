using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaintenanceRoom : MonoBehaviour
{
    public Text gemStoneText;
    public Text coinText;


    private void OnEnable()
    {
        gemStoneText.text = string.Format("{0}", PlayerPrefs.GetInt("GemStone"));
        coinText.text = string.Format("{0}", GameManager.instance.gold);
    }


}
