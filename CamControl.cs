using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    public float sensitivity = 10;
    public float AngLeft = 20;
    public float AngRight = 20;
    public float upAng = 90;
    public float downAng = 90;
    // Update is called once per frame
    void Update()
    {
        float msY = Input.GetAxis("Mouse X");
        float msX = Input.GetAxis("Mouse Y");
        msX = Mathf.Clamp(msX, -2, 2);
        transform.eulerAngles += new Vector3(msX * sensitivity, msY * sensitivity, 0);
        Vector3 parRo = transform.parent.transform.eulerAngles;
        Vector3 checker = transform.localEulerAngles;
        // 190 -> 200
        if (checker.y > 180 && checker.y < 180 + AngLeft){
            checker.y = 180 + AngLeft;
        }
        else if (checker.y < 180 && checker.y > 180 - AngRight){
            checker.y = 180 - AngRight;
        }
        if (checker.x > 180 && checker.x < 180 + downAng){
            checker.x = 180 + downAng;
        }
        else if (checker.x < 180 && checker.x > 180 - upAng){
            checker.x = 180 - upAng;
        }
        checker.y += parRo.y;
        checker.x += parRo.x;
        checker.z = 0;
        transform.eulerAngles = checker;
    }
}
