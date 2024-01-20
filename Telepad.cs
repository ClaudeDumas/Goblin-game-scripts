using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telepad : MonoBehaviour
{
    public Vector3 newPosition;
    // Update is called once per frame
    void OnControllerColliderHit(ControllerColliderHit other)
    {
        print("hit");
        Transform thing = other.gameObject.transform;
        thing.position = newPosition;
    }
}
