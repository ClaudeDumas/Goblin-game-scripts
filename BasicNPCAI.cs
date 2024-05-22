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
    [SerializeField] float checkForTargetsPeriod;
    [SerializeField] float changePathPeriod;
    [SerializeField] float jumpPower;
    [SerializeField] float degreeCheckPeriod;
    [SerializeField] float checkDownPeriod;
    public bool SpottedEnemy = false;
    List<GameObject> targets = new List<GameObject>();
    private GameObject chosenTarget;
    List<Vector3> targetSpots = new List<Vector3>();
    void Start()
    {
        StartCoroutine(WaitAndCheck());
    }
    // Update is called once per frame
    void Update()
    {
        if (SpottedEnemy == false)
        {
            LookForPlayers(BasicRange);
        }
        else{
            LookForPlayers(SpottedEnemyRange);
            CheckForCloserPlayer(transform.position);
            if ((transform.position - chosenTarget.transform.position).magnitude > attackRange)
            {
                moveManager mover = GetComponentInParent<moveManager>();
                if ((mover.Vel + (Pathing() * Time.deltaTime * acceleration)).magnitude < maxSpeed){
                    mover.Vel += (Pathing() * Time.deltaTime * acceleration);
                }
            }
            targets.Clear();
        }
    }
    IEnumerator WaitAndCheck()
    {
        while (SpottedEnemy == false)
        {
            LookForPlayers(BasicRange);
            yield return new WaitForSeconds(checkForTargetsPeriod);
        }
        StartCoroutine(WaitAndPath());
        yield break;
    }
    IEnumerator WaitAndPath()
    {
        while (SpottedEnemy == true)
        {
            moveManager mover = GetComponentInParent<moveManager>();
            yield return new WaitForSeconds(changePathPeriod);
        }
        StartCoroutine(WaitAndCheck());
        yield break;
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

    bool CanGo(Vector3 direction)
    {
        Vector3 directionXZ = new Vector3(direction.x, 0, direction.z);
        CharacterController charCon = GetComponentInParent<CharacterController>();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, directionXZ, out hit, SpottedEnemyRange, 3) == false)
        {
            return true;
        }
        RaycastHit hit2;
        // Can you go up via stepping?
        if (Physics.Raycast(transform.position + new Vector3(0, charCon.stepOffset, 0), directionXZ, out hit2, charCon.radius * 2, 3) == false)
        {
            return true;
        }
        else
        {
            if ((hit2.point - transform.position).magnitude > (hit.point - transform.position).magnitude + charCon.radius);
            {
                return true;
            }
        }
        // RaycastHit hit3;
        // Can you go up via slope?
        // if (Physics.Raycast(transform.position + new Vector3(0, 0.01f, 0), direction, out hit3, (hit.transform.position - transform.position).magnitude), 3)
        // {
        // }
        // else
        // {
        //     return true;
        // }
        return false;   
    }
    Vector3 Pathing()
    {
        // "Enemy" here refers to whoever the npc is against. 
        // This is usually the player considering most npcs are enemies.
        GameObject Enemy = chosenTarget;
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

    void CheckForCloserPlayer(Vector3 origin)
    {
        chosenTarget = targets[0];
        if (targets.Count > 1){
            for (int i = 1; i < targets.Count; i++)
            {
                if (MaxVector(targets[i].transform.position - transform.position, targets[i-1].transform.position - transform.position) == targets[i-1].transform.position - transform.position)
                {
                    chosenTarget = targets[i];
                }
            }
        }
    }
    Vector3 MaxVector(Vector3 a, Vector3 b)
    {
        if (a.magnitude > b.magnitude)
        {
            return a;
        }
        return b;
    }







    void Pathfind()
    {
        // I lowkey fw circles

        // check for existing wrong options from previous loops of the algorithm by checking an array 
        // and denying that possible position if it comes up, eventually removing all possible incorrect
        // points

        // to find loops through and environment check your existing predicted points and see if the new point
        // is one of those, then go back to the previous point and say that way was wrong. If all possible 
        // points from one point were wrong, that point was wrong. rinse and repeat.

        // check if any possible points were accessible from each point, this will allow a 
        // shortening of the final path.

        /* anyway here's an idea of how this algorithm works:
        1. Go up by how far the character can step up
        2. check in a circle for where you can go, starting in the direction of where the target is.
        It can tell if there's a wall because it will know when it checks if it can't step up.
        Stops when you can go somewhere and changes the predicted position based on that.
        3. go forward a set interval
        4. repeat step 2 and 3, checking what will happen if it goes up by the stepOffset when the ray hits 
        something. If it can be stepped over, it continues. If not, it stops and 
        */
        CharacterController charCon = GetComponent<CharacterController>();
        // Where does the program think the thing will end up?
        Vector3 predictedPosition = transform.position;
        // Where is it going?
        Vector3 direction = new Vector3(0,0,0);
        // base direction, in the direction of the target. This is kinda like A* because when it checks where to go it takes what the closest direction to the target is first. On a 2d plane.
        Vector3 baseDir = chosenTarget.transform.position;
        baseDir = new Vector3(baseDir.x, 0, baseDir.z);
        Vector3 rightBaseDir = new Vector3(baseDir.z, 0, baseDir.x);
        // Mathf.PI decided it's gone now so here we are.
        float pi = 3.141592653589f;
        float deg = 0;
        while (predictedPosition != chosenTarget.transform.position) {
            for (float i = 0; i < 360; i += degreeCheckPeriod)
            {
                deg = i;
                // this is so that it goes in an arc of rays
                if (deg % 2 == 1)
                {
                    // they don't have the "//" like python :sob:
                    // eg. 3 -> 358, 7 -> 356, etc.
                    deg = 360 - (i - (i/2 - 0.5))
                }
                else
                {
                    // eg. 4 -> 2, 6 -> 3, 360 -> 180, etc.
                    deg = i - i/2
                }
                direction = transform.position + new Vector3((baseDir.magnitude * Mathf.Sin(i * (pi/180))), 0, (baseDir.magnitude * Mathf.Cos(i * (pi/180))));
                // Raycasting time!!! it gets even more confusing here.
                RaycastHit hit;
                if (Physics.Raycast(predictedPosition + new Vector3(0, charCon.stepOffset, 0), direction, out hit, baseDir.magnitude))
                {
                    Vector3 newSpot = (hit.point + (hit.point - (predictedPosition + new Vector3(0, charCon.stepOffset, 0)).normalized * charCon.radius));
                    if (targetSpots.Contains(newSpot))
                    {
                        continue
                    }
                    else
                    {
                        targetSpots.Add(newSpot);
                    }
                    break;
                }
                else
                {
                
                }
            }
        }
    }
}


