using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private Transform[] tileMaps;

    private void Awake()
    {
        tileMaps = GetComponentsInChildren<Transform>();
    }

    public void MapReset()
    {
        for(int i = 1; i <tileMaps.Length; i++)
        {
            tileMaps[i].position = tileMaps[i].gameObject.GetComponent<Reposition>().initPos;
        }
    }
}
