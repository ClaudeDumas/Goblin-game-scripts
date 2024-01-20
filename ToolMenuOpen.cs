using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class ToolMenuOpen : MonoBehaviour
{
    public float AlphaSpeed = 0.1f;
    public KeyCode onButton = KeyCode.F;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool on = Input.GetKey(onButton);
        Color color = GetComponent<Image>().color;
        if (color.a <= 1f)
        {
         if (on == true)
         {
         color.a = color.a + AlphaSpeed;
         GetComponent<Image>().color = color;
         }
        }
        if (color.a >= 0)
        {
            if (on == false)
            {
                color.a = color.a - AlphaSpeed;
                GetComponent<Image>().color = color;
            }
        }
            
    }

    private string GetDebuggerDisplay()
    {
        return ToString();
    }
}
