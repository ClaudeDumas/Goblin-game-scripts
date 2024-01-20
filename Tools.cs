using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : MonoBehaviour
{
    public GameObject screwdriver;
    public GameObject mine;
    private bool screwdriverOn = false;
    public float Moving = 30;
    public float Strength = 30;
    public float RotateTimesOrigin = 20;
    public float RotateTimes = 20;
    private void OnMouseOver()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Moving = 30;
                screwdriverOn = true;
            } 
        }

    private void Update()
    {
        if (GameObject.Find("Screwdriver2") == false)
        {
            if (screwdriverOn == true)
            {
                GameObject screw = Instantiate(screwdriver);
                screwdriverOn = false;
                screw.transform.position = transform.position + new Vector3(0, 10, 0);
                screw.name = "Screwdriver2";
            }
        }
        else
        {
            if (RotateTimes > 0)
            {
                GameObject screw = GameObject.Find("Screwdriver2");
                screw.transform.Rotate(0, 100 * Time.deltaTime, 0);
                RotateTimes -= 1;
            }
            else
            {
                RotateTimes = RotateTimesOrigin;
                Destroy(GameObject.Find("Screwdriver2"));
            }
        }
        if (Moving > 0) {
            Vector3 moveDirection = new Vector3(0, Strength, 0);
            moveDirection *= Time.deltaTime;
            transform.position += moveDirection;
            Moving = Moving - 1;
        }
    }
}
