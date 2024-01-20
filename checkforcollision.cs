using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkforcollision : MonoBehaviour
{
    public float m;
    // Start is called bfore the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0,0,m);
    }
    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
            Debug.Log("hit the thing");
        }
        Debug.Log("hit the thing");
    }
}
