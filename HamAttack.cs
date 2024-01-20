using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamAttack : MonoBehaviour
{
    public GameObject hammer;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && GameObject.Find("HAMMER") == null){
            GameObject H = Instantiate(hammer);
            H.name = "HAMMER";
            H.transform.position = transform.position + transform.forward + transform.up;
            H.transform.eulerAngles = transform.eulerAngles;
            H.transform.eulerAngles += new Vector3(0, 80, 0);
            StartCoroutine (Swing(H));
        }
    }
    private IEnumerator Swing(GameObject m){
        for (int i = 0; i < 10; i++){
            yield return new WaitForSeconds(0.01f);
            m.transform.eulerAngles += new Vector3(0,10,0);
        }
        Destroy(m);
    }
}
