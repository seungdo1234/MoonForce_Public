using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    public static ChestManager instance;

    [Header("# Reward")]
    public Percent[] chestPer;
    public Percent[] qualityPer;

    public int[] bronzeChest;
    public int[] silverChest;
    public int[] goldChest;
    public int[] specialChest;

    [Header("# HealthUp")]
    public int healthUpDropPer;
    public int[] healthUpQualityPer;
    private void Awake()
    {
        instance = this;
    }

    public int Percent(int [] percent)
    {
        int random = Random.Range(1, 101);
        int percentSum = 0;
        int result = 0;

        for (int i = 0; i < percent.Length; i++)
        {
            percentSum += percent[i];
            if (random <= percentSum)
            {
                result = i;
                break;
            }
        }

        return result;
    }
}

[System.Serializable]
public class Percent
{
    public int[] percent;
}
