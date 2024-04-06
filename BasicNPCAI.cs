using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicNPCAI : MonoBehaviour
{
    [SerializeField] float BasicRange;
    [SerializeField] float SpottedEnemyRange;
    [SerializeField] float attackRange;
    [SerializeField] float acceleration;
    [SerializeField] string enemyTag;
    [SerializeField] float maxSpeed;
    private bool SpottedEnemy = false;
    List<GameObject> targets = new List<GameObject>();
    private bool chosenTarget;
    // Update is called once per frame
    void Update()
    {
        if (SpottedEnemy == false)
        {
            LookForPlayers(BasicRange);
        }
        else{
            LookForPlayers(SpottedEnemyRange);
            if ((transform.position - CloserPlayer(transform.position).transform.position).magnitude > attackRange)
            {
                moveManager mover = GetComponentInParent<moveManager>();
                if ((mover.Vel + (Pathing() * Time.deltaTime * acceleration)).magnitude < maxSpeed){
                    mover.Vel += (Pathing() * Time.deltaTime * acceleration);
                }
            }
        }
        targets.Clear();
    }
    void LookForPlayers(float range)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, range);
        foreach (var hitObj in hits)
        {
            if (hitObj.gameObject.tag == enemyTag)
            {
                targets.Add(hitObj.gameObject);
            }
        }
        if (targets.Count == 0)
        {
            SpottedEnemy = false;
        }
        else
        {
            SpottedEnemy = true;
        }
    }

    Vector3 Pathing()
    {
        // "Enemy" here refers to whoever the npc is against. 
        // This is usually the player considering most npcs are enemies.
        GameObject Enemy = CloserPlayer(transform.position);
        Vector3 EnemyPos = Enemy.transform.position;
        Vector3 SelfPos = transform.position;
        // this is the direction if the npc is not obstructed in any way.
        Vector3 BasicDir = EnemyPos - SelfPos;
        
        // Can the enemy see the player right now?
        RaycastHit hit;
        if (Physics.Raycast(SelfPos, BasicDir, out hit, SpottedEnemyRange))
        {
            if (hit.transform.gameObject.tag == enemyTag){
                RaycastHit hit2;
                // If there's just nothing in front of you, just go in the basic direction. This would mean the player is in the air.
                if (Physics.Raycast(SelfPos, new Vector3(BasicDir.x, 0, BasicDir.z), out hit2, (hit.transform.position - transform.position).magnitude) == false)
                {
                    return new Vector3(BasicDir.x, 0, BasicDir.z).normalized;
                }
                // If the hit object is the player, also go towards it.
                else
                {
                    if (hit2.transform.gameObject.tag == enemyTag)
                    {
                        return new Vector3(BasicDir.x, 0, BasicDir.z).normalized;
                    }
                    else
                    {
                        // if you go up a little bit (0.01 units), is the slope between 
                        // the original hit point and this new hit point climbable?
                        // (checking for slopes)
                        RaycastHit hit3;
                        if (Physics.Raycast(SelfPos + new Vector3(0, 0.01f, 0), new Vector3(BasicDir.x, 0, BasicDir.z).normalized + (transform.up * (GetComponentInParent<CharacterController>().slopeLimit/45)), out hit3, (hit.transform.position - transform.position).magnitude))
                        {
                            if (Vector3.Angle(hit2.point, hit3.point) < GetComponentInParent<CharacterController>().slopeLimit)
                            {
                                return new Vector3(BasicDir.x, 0, BasicDir.z).normalized;
                            }
                            else
                            {
                                // If you go up by the highest step you can take, is
                                // there space to take a step?
                                // (checking for steps)
                                RaycastHit hit4;
                                if (Physics.Raycast(SelfPos + new Vector3(0, GetComponentInParent<CharacterController>().stepOffset, 0), new Vector3(BasicDir.x, 0, BasicDir.z).normalized, out hit4, (hit.transform.position - transform.position).magnitude))
                                {
                                    if (new Vector3((hit4.point - transform.position).x, 0, (hit4.point - transform.position).z).magnitude < new Vector3((hit2.point - transform.position).x, 0, (hit2.point - transform.position).z).magnitude)
                                    {
                                        return new Vector3(BasicDir.x, 0, BasicDir.z).normalized;
                                    }
                                }

                            }
                        }
                        else
                        {
                            return new Vector3(BasicDir.x, 0, BasicDir.z).normalized;
                        }
                    }
                }
            }
        }
        // If the code gets to this point, something has gone wrong.
        // The pathfinding algorithm is incomplete or the player is in
        // an area unaccessible without special movement.
        return new Vector3(0,0,0);
    }

    GameObject CloserPlayer(Vector3 origin)
    {
        if (targets.Count > 1){
            GameObject final = null;
            for (int i = 1; i < targets.Count; i++)
            {
                if (MaxVector3(targets[i].transform.position - transform.position, targets[i-1].transform.position - transform.position) == targets[i].transform.position - transform.position)
                {
                    final = targets[i];
                }
            }
            return final;
        }
        return targets[0];
    }

    Vector3 MaxVector3(Vector3 a, Vector3 b)
    {
        if (a.magnitude > b.magnitude)
        {
            return a;
        }
        return b;
    }
}
