using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField] GameObject hook;
    [SerializeField] GameObject head;
    [SerializeField] float hookFrames;
    [SerializeField] float hitLength;
    [SerializeField] float strength;
    private Vector3 hitPos;
    private GameObject currentHook;
    private Transform hitObject;
    private Vector3 relativePos;

    // Update is called once per frame
    void Update()
    {
        PlayerController charCon = GetComponentInParent<PlayerController>();
        CharacterController charCol = GetComponent<CharacterController>();
        Vector3 Vel = charCon.Vel;
        if (currentHook == null){
            if (Input.GetKeyDown(KeyCode.G)){
                RaycastHit hit;
                if (Physics.Raycast(head.transform.position, head.transform.forward, out hit, hitLength)){
                    if (hit.transform.tag != "Player"){
                        hitPos = hit.point;
                        hitObject = hit.transform;
                        relativePos = hitObject.position - hitPos;
                        GameObject newHook = Instantiate(hook);
                        newHook.transform.eulerAngles = head.transform.eulerAngles;
                        newHook.transform.position = head.transform.position + head.transform.forward;
                        currentHook = newHook;
                    }else{
                        // just sets value to an impossibly high one for when it hits nothing.
                        hitPos = new Vector3(99999, 99999, 99999);
                    }
                }else{
                    hitPos = new Vector3(99999, 99999, 99999);
                }
            }
        }
        else{
            if (Input.GetKey(KeyCode.G) && hitPos != new Vector3(99999, 99999, 99999)){
                LineRenderer line = currentHook.GetComponent<LineRenderer>();
                line.SetPosition(0, transform.position);
                line.SetPosition(1, currentHook.transform.position);
                hitPos = hitObject.position - relativePos;
                if (currentHook.transform.position != hitPos){
                    currentHook.transform.position += ((hitPos - currentHook.transform.position)/hookFrames) * Time.deltaTime;
                }
                if ((hitPos - transform.position).magnitude > 0.3){
                    charCon.enabled = false;
                    Vector3 addedVal = (currentHook.transform.position - transform.position) * strength * Time.deltaTime;
                    print(addedVal);
                    // check if slow enough to stop, which is when calculating friction would make it negative. (imported from normal moving script)
                    if (Mathf.Min(charCon.Vel.magnitude, charCon.gravVal * charCon.frictionCoefficient * -1 * Time.deltaTime) == charCon.Vel.magnitude){
                        charCon.Vel -= charCon.Vel;
                    }
                    else{
                        // uses the actual friction formula, F = μN
                        charCon.Vel -= charCon.Vel.normalized * charCon.gravVal * charCon.frictionCoefficient * charCon.frictionMultiplier * -1 * Time.deltaTime;
                    }
                    // air resistance
                    charCon.Vel -= (((charCon.areaDensity * (charCon.Vel * charCon.Vel.magnitude) * (charCol.height * charCol.radius) * charCon.dragCoefficient)/2)/charCon.mass) * Time.deltaTime;
                    charCol.Move((charCon.Vel + ((currentHook.transform.position - transform.position) * strength)) * Time.deltaTime * charCon.playerSpeed);
                }
            }
            else{
                Destroy(currentHook);
                charCon.enabled = true;
                charCon.Vel += (currentHook.transform.position - transform.position) * strength;
            }
        }
    }
}
