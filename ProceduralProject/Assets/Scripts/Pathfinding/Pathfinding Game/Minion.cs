using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{

    public enum minionTypes
    {
        caster,
        melee,
        cannon,
    }

    minionTypes minionType;
    public enum teamColor
    {
        blue,
        red,
    }
    teamColor team;

    public HealthBar healthbar;
    public BillBoard billboard;

    public Transform cube;
    public Transform capsule;

    public GameObject bulletPrefab;


    private float maxHealth = 999;
    public float health = 999;
    private float damage = 999;
    private float attackSpeed = 999;
    private float attackRange = 999;

    private float aggroRange = 5;

    private float attackInterval = 0;
    private bool attacking = false;

    public Transform target = null;

    private List<MinionPathfinder.Node> pathToTarget = new List<MinionPathfinder.Node>();

    private bool shouldCheckAgain = true;

    private float checkAgainIn = 0;

    private LineRenderer line;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        { //destroys this minion and removes it from the List
            if (team == teamColor.blue)
            {
                Map.singleton.blueMinions.Remove(gameObject);
                Destroy(gameObject);
                Destroy(this);
                return;
            }
            else if (team == teamColor.red)
            {
                Map.singleton.redMinions.Remove(gameObject);
                Destroy(gameObject);
                Destroy(this);
                return;
            }
        }
        if (gameObject != null){

            checkAgainIn -= Time.deltaTime;
            if (checkAgainIn <= 0 || !attacking)
            {
                shouldCheckAgain = true;
                checkAgainIn = 1;
            }
            if (shouldCheckAgain) FindPath(); //finds the quickest path

            if (target == null || (target.position - transform.position).magnitude >= aggroRange)findTarget(); 

            attacking = ((target.position - transform.position).magnitude < attackRange) ? true : false; //attack the target if in range



            if (!attacking) MoveAlongPath();
            else
            {
                attackInterval -= Time.deltaTime;
                if (attackInterval <= 0)
                {
                    Attack();
                    attackInterval = attackSpeed;
                }
            }
        }
    }

    public void makeStats(int type, int color) { //sets the type of minion and its color and stats

        minionType = (minionTypes)type;
        team = (teamColor)color;


        if (cube) cube.gameObject.SetActive(minionType == minionTypes.melee);

        if (capsule) capsule.gameObject.SetActive(minionType == minionTypes.caster);

        if (minionType == minionTypes.melee)
        {
            maxHealth = 45;
            health = maxHealth;
            damage = 6;
            attackSpeed = 1;
            attackRange = 2;


        }
        else if (minionType == minionTypes.caster)
        {
            maxHealth = 25;
            health = maxHealth;
            damage = 10;
            attackSpeed = 1.5f;
            attackRange = 5;

        }

        healthbar.MaxHealth(maxHealth);

        line = GetComponent<LineRenderer>();

        Renderer cubeRend = cube.gameObject.GetComponent<Renderer>();
        Renderer capsuleRend = capsule.gameObject.GetComponent<Renderer>();
        if (team == teamColor.red)//Sets up color
        {
            cubeRend.material.color = Color.red;
            capsuleRend.material.color = Color.red;
        }
        else if (team == teamColor.blue)
        {
            cubeRend.material.color = Color.blue;
            capsuleRend.material.color = Color.blue;
        }

        if (team == teamColor.blue)
        {
            foreach (GameObject redTower in Map.singleton.redTowers)
            {
                if (!redTower.gameObject.GetComponent<Tower>().isDestroyed)
                {
                    target = redTower.transform;
                    break;
                }
            }
        }
        else if (team == teamColor.red)
        {
            foreach (GameObject blueTower in Map.singleton.blueTowers)
            {
                if (!blueTower.gameObject.GetComponent<Tower>().isDestroyed)
                {
                    target = blueTower.transform;
                    break;
                }
            }
        }

    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        healthbar.SetHealth(health);
    }
    private void MoveAlongPath()
    {
        if (pathToTarget == null) return;
        if (pathToTarget.Count < 2) return;

        // grab first item in path and move to that node 

        Vector3 nextTarget = pathToTarget[1].position;
        nextTarget.y += 1;

        transform.position = Vector3.Lerp(transform.position, nextTarget, .01f);

        float d = (nextTarget - transform.position).magnitude;

        if (d < .25f)
        {
            shouldCheckAgain = true;
        }
    }

    private void FindPath()
    {
        shouldCheckAgain = false;

        if (target && Map.singleton)
        {
            MinionPathfinder.Node start = Map.singleton.Lookup(transform.position);
            MinionPathfinder.Node end = Map.singleton.Lookup(target.position);

            if (start == null || end == null || start == end)
            {
                pathToTarget.Clear();
                return;
            }

            pathToTarget = MinionPathfinder.Solve(start, end);

            //?
            //Rendering the path on a LineRenderer
            Vector3[] positions = new Vector3[pathToTarget.Count];

            for (int i = 0; i < pathToTarget.Count; i++)
            {
                positions[i] = pathToTarget[i].position + new Vector3(0, .5f, 0);
            }
        }
    }

    private void findTarget() {



        float smallestDist = 10000;
        GameObject closestObj = null;

            if (team == teamColor.blue)
            {
                foreach (GameObject redTower in Map.singleton.redTowers)
                {
                   if (!redTower.gameObject.GetComponent<Tower>().isDestroyed)
                   {

                        float dist = (transform.position - redTower.transform.position).magnitude;
                         if (dist <= smallestDist)
                         {
                              smallestDist = dist;
                              target = redTower.transform;
                         }

                   }

                }
             }
            else if (team == teamColor.red)
            {
                foreach (GameObject blueTower in Map.singleton.blueTowers)
                {
                    if (!blueTower.gameObject.GetComponent<Tower>().isDestroyed)
                    {
                        float dist = (transform.position - blueTower.transform.position).magnitude;

                        if (dist <= smallestDist)
                        {
                            smallestDist = dist;
                            target = blueTower.transform;
                        }

                    }
                }
            }
        


    

        if (team == teamColor.red) //finds the closest enemy of the opposing color
        {

            foreach (GameObject blueMinion in Map.singleton.blueMinions)
            {
                float dist = (transform.position - blueMinion.transform.position).magnitude;
                if (dist <= smallestDist)
                {
                    smallestDist = dist;
                    closestObj = blueMinion;
                }
            }

            foreach (GameObject blueTower in Map.singleton.blueTowers)
            {
                float dist = (transform.position - blueTower.transform.position).magnitude;
                if (dist <= smallestDist)
                {
                    smallestDist = dist;
                    closestObj = blueTower;
                }
            }

            if (Player.instance.team == Player.teamColor.blue)
            {
                float dist = (transform.position - Player.instance.transform.position).magnitude;
                if (dist <= smallestDist)
                {
                    smallestDist = dist;
                    closestObj = Player.instance.gameObject;
                }
            }

        }
        else if (team == teamColor.blue)//same but for blue
        {

            foreach (GameObject redMinion in Map.singleton.redMinions)
            {
                float dist = (transform.position - redMinion.transform.position).magnitude;
                if (dist <= smallestDist)
                {
                    smallestDist = dist;
                    closestObj = redMinion;
                }
            }

            foreach (GameObject redTower in Map.singleton.redTowers)
            {
                float dist = (transform.position - redTower.transform.position).magnitude;
                if (dist <= smallestDist)
                {
                    smallestDist = dist;
                    closestObj = redTower;
                }
            }

            if (Player.instance.team == Player.teamColor.red)
            {
                float dist = (transform.position - Player.instance.transform.position).magnitude;
                if (dist <= smallestDist)
                {
                    smallestDist = dist;
                    closestObj = Player.instance.gameObject;
                }
            }

        }

        if (smallestDist < aggroRange && closestObj != null) { // if something is in aggro range and the minion is not already aggrod, aggro on the closest thing
            target = closestObj.transform;
        }
    }

    private void Attack(){

        if (minionType == minionTypes.melee)
        {
            if (target.GetComponent<Minion>()){
                target.GetComponent<Minion>().TakeDamage(damage);
            }
            else if (target.GetComponent<Tower>())
            {
                target.GetComponent<Tower>().TakeDamage(damage);
            }
            else if (target.GetComponent<Player>())
            {
                target.GetComponent<Player>().TakeDamage(damage);
            }
        }
        if (minionType == minionTypes.caster)
        {
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                bullet.GetComponent<Bullet>().MakeStats(target, (int)team, .5f, 5, transform.position, damage);
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if((Player.instance.team == Player.teamColor.red && team == teamColor.blue) || (Player.instance.team == Player.teamColor.blue && team == teamColor.red)) Player.instance.target = transform;
        }
    }

}


