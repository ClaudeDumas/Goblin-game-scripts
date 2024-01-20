using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMovement : MonoBehaviour
{
    public float OgTicksSptUntRev = 10f;
    public float TicksSptUntRev = 10f;
    public bool Reversed = false;
    public float xMovement = 2f;
    public float zMovement = 2f;
    public bool MovementOn = true;
    // Start is called before the first frame update
    void Start()
    {
       
    }
    // Update is called once per frame
    void Update()
    {
        if (MovementOn == true)
        {
        if (TicksSptUntRev >= 0)
        {
            if (Reversed == false)
            {
                Vector3 moveDirection = new Vector3(xMovement, 0f, zMovement);
                moveDirection *= Time.deltaTime;
                transform.position += moveDirection;
                TicksSptUntRev = TicksSptUntRev - 1;
            }
            else
            {
                Vector3 moveDirection = new Vector3((0 - xMovement), 0f, (0 - zMovement));
                moveDirection *= Time.deltaTime;
                transform.position += moveDirection;
                TicksSptUntRev = TicksSptUntRev - 1;
            }
        }
        else
        {
            TicksSptUntRev = OgTicksSptUntRev;
            if (Reversed == false)
            {
                Reversed = true;
            }
            else
            {
                Reversed = false;
            }
        }
        }
        
    }
}
