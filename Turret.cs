using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] string targetTag;
    [SerializeField] float Damage;
    [SerializeField] float rotationSpeed;
    [SerializeField] float bulletHang;

    // Update is called once per frame
    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var hitCol in hitColliders)
        {
            if (hitCol.gameObject.tag == targetTag){
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(hitCol.transform.position - transform.position), rotationSpeed * Time.deltaTime);
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, radius)){
                    if (hit.transform.gameObject.tag == targetTag){
                        GetComponent<ParticleSystem>().Play();
                        hit.transform.gameObject.GetComponentInParent<Health>().BroadcastMessage("Damage", Damage);
                    }
                }
            }
        }
    }
}
