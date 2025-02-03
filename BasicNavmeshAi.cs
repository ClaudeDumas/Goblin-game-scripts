using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicNavmeshAi : MonoBehaviour
{
    private NavMeshAgent nav;
    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        target = CheckForCloserPlayer();
        if (target != null){
            nav.SetDestination(target.position);
            
        }
    }
    Transform CheckForCloserPlayer()
    {
       GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
       if (players.Length > 0){
        Transform subject = players[0].GetComponent<Transform>();
        for (int i = 1; i < players.Length; i++){
            Transform playerTransform = players[i].GetComponent<Transform>();
           if (playerTransform && (playerTransform.position-transform.position).magnitude < (subject.position-transform.position).magnitude){
            subject=playerTransform;
           }
        }
        return subject;
       }
       return null;
    }
}
