using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpen : MonoBehaviour
{
    public RewardAlpha reward;
    public void Open()
    {
        reward.StartAlpha();
    }
}
