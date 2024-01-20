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
        if (currentHook == null){
            if (Input.GetKeyDown(KeyCode.G)){
                GameObject newHook = Instantiate(hook);
                newHook.transform.eulerAngles = head.transform.eulerAngles;
                newHook.transform.position = head.transform.position + head.transform.forward;
                currentHook = newHook;
                RaycastHit hit;
                if (Physics.Raycast(head.transform.position, head.transform.forward, out hit, hitLength)){
                    hitPos = hit.point;
                    hitObject = hit.transform;
                    relativePos = hitObject.position - hitPos;
                }
            }
        }
        else{
            if (Input.GetKey(KeyCode.G)){
                LineRenderer line = currentHook.GetComponent<LineRenderer>();
                line.SetPosition(0, transform.position);
                line.SetPosition(1, currentHook.transform.position);
                if (hitObject != null){
                    hitPos = hitObject.position - relativePos;
                }
                if (currentHook.transform.position != hitPos){
                    currentHook.transform.position += ((hitPos - currentHook.transform.position)/hookFrames) * Time.deltaTime;
                }
            }
            else{
                Destroy(currentHook);
                hitPos = transform.position;
            }
        }
    }
}
