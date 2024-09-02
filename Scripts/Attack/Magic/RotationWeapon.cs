using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 삽이 있을때 회전시키는 오브젝트
public class RotationWeapon : MonoBehaviour
{
    public float rotationSpeed;


    // Update is called once per frame
    void Update()
    {
        if (true)
        {
            if (transform.childCount > 0 && transform.GetChild(0).gameObject.activeSelf)
            {
                float speed = GameManager.instance.statManager.baseRate * -rotationSpeed;
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
            }
        }
    }

}
