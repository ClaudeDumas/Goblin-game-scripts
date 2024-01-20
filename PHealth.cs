using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PHealth : MonoBehaviour
{
    public int health = 100;
    public float immunityFrames = 0;
    public float maxImmunityFrames = 50;
    void Start(){
        print("Health: " + health);
    }
    void OnCollisionEnter(Collision col){
        if (col.gameObject.tag == "Enemy" && immunityFrames < 1){
            health -= 1;
            immunityFrames = maxImmunityFrames;
            print(health);
        }
        else{
            if (col.gameObject.tag == "Enemy2" && immunityFrames < 1){
                health -= 2;
                immunityFrames = maxImmunityFrames;
                print(health);
            }
        } 
    }
    void Update(){
        if (immunityFrames > 0){
            immunityFrames -= 1;
        }
        if (health == 0){
            Destroy(transform.gameObject);
        }
    }
}
