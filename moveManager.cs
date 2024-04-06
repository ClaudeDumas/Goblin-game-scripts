using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveManager : MonoBehaviour
{

    [SerializeField] float frictionMultiplier = 1;
    public Vector3 Vel;
    [SerializeField] float speed = 3;
    public float gravVal = -9.81f;
    public float areaDensity = 1.22f;
    
    public float frictionCoefficient = 1;
    [SerializeField] float dragCoefficient = .59f;
    [SerializeField] float mass = 30;

    // Update is called once per frame
    void Update()
    {
        CharacterController charCol = GetComponent<CharacterController>();
        // check if slow enough to stop, which is when calculating friction would make it negative.
        if (Mathf.Min(Vel.magnitude, gravVal * frictionCoefficient * -1 * Time.deltaTime) == Vel.magnitude){
            Vel -= Vel;
        }
        else{
            // uses the actual friction formula, F = μN
            Vel -= Vel.normalized * gravVal * frictionCoefficient * frictionMultiplier * -1 * Time.deltaTime;
        }
        // air resistance
        Vel -= (((areaDensity * (Vel * Vel.magnitude) * (charCol.height * charCol.radius) * dragCoefficient)/2)/mass) * Time.deltaTime;
        // check for terminal velocity then apply gravity and air resistance
        if (Vel.y > -33)
        {
            // applying gravity
            Vel.y += (gravVal * Time.deltaTime * 2);
        }
        charCol.Move(Vel * Time.deltaTime * speed);
        // Make sure velocity doesn't go too far in the negatives while on the ground.
        if (charCol.isGrounded && Vel.y < 0){
            Vel.y = 0;
        }
    }
}
