using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveManager : MonoBehaviour
{

    [SerializeField] float frictionMultiplier;
    public Vector3 Vel;
    public float playerSpeed;
    public float gravVal;
    public float areaDensity = 1.22f;
    
    public float frictionCoefficient = 1f;
    [SerializeField] float dragCoefficient = .59f;
    [SerializeField] float mass = 30;

    // Update is called once per frame
    void Update()
    {
        CharacterController charCol = GetComponent<CharacterController>();
        // check for terminal velocity then apply gravity and air resistance
        if (Vel.y > -33)
        {
            Vel.y += (gravVal * Time.deltaTime);
            // air resistance
            Vel.y -= (((areaDensity * Vel.y * Vel.y * (charCol.height * charCol.radius) * dragCoefficient)/2)/mass) * Time.deltaTime;
        }
        if (Mathf.Min(Vel.magnitude, gravVal * frictionCoefficient * -1 * Time.deltaTime) == Vel.magnitude){
            Vel -= Vel;
        }else{
            Vel -= Vel.normalized * gravVal * frictionCoefficient * frictionMultiplier * -1 * Time.deltaTime;
        }
        charCol.Move(Vel * Time.deltaTime * playerSpeed);
        // reseting gravity in case jetpack was used and no longer is.
        gravVal = -9.81f;
        // Make sure velocity doesn't go too far in the negatives while on the ground.
        if (charCol.isGrounded && Vel.y < 0){
            Vel.y = 0;
        }
    }
}
