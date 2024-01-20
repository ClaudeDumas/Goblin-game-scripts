using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineMaker : MonoBehaviour
{
    public GameObject mine;
    public float mineForce;
    public float mineRadius;
    public float distanceFactor;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)){
            GameObject Mine = Instantiate(mine);
            Mine.transform.position = transform.position + (transform.forward * 2);
            StartCoroutine (Boomit(Mine, mineRadius));
        }
    }
    private IEnumerator Boomit(GameObject m, float rad){
        yield return new WaitForSeconds(3);
        Collider[] hitColliders = Physics.OverlapSphere(m.transform.position, rad);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.GetComponentInParent<CharacterController>()){

                CharacterController charCon = hitCollider.gameObject.GetComponentInParent<CharacterController>();
                var direction = charCon.transform.position - m.transform.position;
                if (hitCollider.gameObject.GetComponentInParent<PlayerController>()){
                    PlayerController scr = hitCollider.gameObject.GetComponentInParent<PlayerController>();
                    scr.Vel += direction.normalized * mineForce * ((rad - direction.magnitude) * distanceFactor);
                }
                else
                {
                    if (hitCollider.gameObject.GetComponentInParent<moveManager>()){
                        moveManager scr = hitCollider.gameObject.GetComponentInParent<moveManager>();
                        scr.Vel += direction.normalized * mineForce * ((rad - direction.magnitude) * distanceFactor);
                    }else
                    {
                        charCon.Move(direction.normalized * mineForce * ((rad - direction.magnitude) * distanceFactor));
                    }
                }
            }
        }
        Destroy(m);
    }
}