using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float frictionMultiplier;
    [SerializeField] float boostPower;
    public Vector3 Vel;
    public float playerSpeed;
    [SerializeField] float ogGravVal;
    public float gravVal;
    private bool slamming = false;
    [SerializeField] float jumpPower;
    [SerializeField] float jetpackGravity;
    public float areaDensity = 1.22f;
    public float slammingVel;
    public float frictionCoefficient = 1f;
    [SerializeField] float wasdSpeed;
    [SerializeField] float maxWasdSpeed;
    [SerializeField] float maxSlideSpeed;
    private bool sliding = false;
    private bool outOfSlide = false;
    private bool canBoost = false;
    public float dragCoefficient = .59f;
    public float mass = 30;
    [SerializeField] float rotation;
    [SerializeField] float outOfSlideWindow;
    [SerializeField] float fastWasdSpeed;
    [SerializeField] GameObject plat;
    // Update is called once per frame
    void Update()
    {
        CharacterController charCol = GetComponent<CharacterController>();

        if (charCol.isGrounded){
            // Jump
            if (Input.GetKeyDown(KeyCode.Space)) {
                if (outOfSlide != true){
                    Vel.y += Mathf.Sqrt(jumpPower * -3 * gravVal);
                }
                else{
                    Vel.y += Mathf.Sqrt(jumpPower * -3 * gravVal) + maxSlideSpeed;
                }
            }
            // Slide
            if (Input.GetKey(KeyCode.LeftShift)){
                sliding = true;
                if ((Vel + (transform.forward * (playerSpeed * 0.5f))).magnitude < maxSlideSpeed){
                    Vel += transform.forward * (playerSpeed * 0.5f);
                }
            }
            else{
                if (sliding == true){
                    sliding = false;
                    StartCoroutine(outOfSlideFunc());
                }
            }
            // Backflip
            if (Input.GetKey(KeyCode.LeftControl) && (Input.GetKeyDown(KeyCode.S))){
                if (outOfSlide != true){
                    Vel += (transform.forward * -1);
                    Vel.y += Mathf.Sqrt(jumpPower * -3 * gravVal);
                }
                else{
                    Vel += (transform.forward * -1) * maxSlideSpeed;
                    Vel.y += Mathf.Sqrt(jumpPower * -3 * gravVal) + maxSlideSpeed;
                }
            }
            slamming = false;
            canBoost = true;
        }
        else{
            if (Input.GetKeyDown(KeyCode.LeftControl)){
                slamming = true;
            }
            // Jetpack
            if (Input.GetKey(KeyCode.Space)){
                gravVal = -2;
                if (Vel.y < -1){
                    Vel.y = -1;
                }
            }
            // Mid-air boost
            if (Input.GetKeyDown(KeyCode.LeftShift) && canBoost){
                Vel += (transform.forward + transform.up) * boostPower;
                canBoost = false;
            }
        }
        // Turning
        if (Input.GetKey(KeyCode.L)) {
            transform.eulerAngles += new Vector3(0, rotation, 0);
        }
        if (Input.GetKey(KeyCode.K)) {
            transform.eulerAngles += new Vector3(0, 0-rotation, 0);
        }
        // Basic movement keys
        if ((Vel.x + Input.GetAxis("Horizontal")) < maxWasdSpeed && (Vel.x + Input.GetAxis("Horizontal")) > -(maxWasdSpeed) && (Vel.z + Input.GetAxis("Vertical")) < maxWasdSpeed && (Vel.z + Input.GetAxis("Vertical"))> -(maxWasdSpeed))
        {
            Vel += Input.GetAxis("Horizontal") * transform.right * wasdSpeed;
            Vel += Input.GetAxis("Vertical") * transform.forward * wasdSpeed;
            if (new Vector3(Vel.x, 0, Vel.z).magnitude > new Vector3(Vel.normalized.x * 3, 0, Vel.normalized.z * (maxWasdSpeed - 1)).magnitude && sliding == false){
                Vector3 added = new Vector3(0,0,0);
                added += Vel.normalized;
                Vel.x = added.x * 3;
                Vel.z = added.z * 3;
            }
        }else{
            Vel += Input.GetAxis("Horizontal") * transform.right * fastWasdSpeed;
            Vel += Input.GetAxis("Vertical") * transform.forward * fastWasdSpeed;
        }
        // Make a platform
        if (Input.GetKeyDown(KeyCode.Z)){
            GameObject Plat = Instantiate(plat);
            Plat.transform.position = transform.position + (transform.forward * 4);
        }
    
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
        if (slamming){
            Vel.y = slammingVel;
        }
        charCol.Move(Vel * Time.deltaTime * playerSpeed);
        // reseting gravity in case jetpack was used and no longer is.
        gravVal = ogGravVal;
        // Make sure velocity doesn't go too far in the negatives while on the ground.
        if (charCol.isGrounded && Vel.y < 0){
            Vel.y = 0;
        }
    }
    // wait to enable the jetpac
    private IEnumerator outOfSlideFunc(){
        outOfSlide = true;
        yield return new WaitForSeconds(outOfSlideWindow);
        outOfSlide = false;
    }
}
