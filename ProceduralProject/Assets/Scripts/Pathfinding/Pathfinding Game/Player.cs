using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    private List<MinionPathfinder.Node> pathToTarget = new List<MinionPathfinder.Node>();

    private void Awake()
    {
        instance = this;
    }
    public enum teamColor
    {
        blue,
        red,
    }
    public teamColor team;

    private LineRenderer line;
    public CharacterController controller;

    public HealthBar healthbar;

    public Transform target = null;

    public GameObject bulletPrefab;

    public float speed = 12f;
    public float jumpHeight = 20f;

    public float maxHealth = 100;
    public float health = 100;
    Vector3 velocity;

    private bool shouldCheckAgain = true;

    private float checkAgainIn = 0;

    private bool attacking = false;
    private int attackRange = 7;

    private float attackSpeed = 2;
    private float attackInterval = .5f;

    Renderer rend;

    public Transform body;


    private void Start()
    {
        health = maxHealth;

        rend = GetComponent<Renderer>();
        Renderer bodyRend = body.gameObject.GetComponent<Renderer>();

        healthbar.MaxHealth(maxHealth);

        if (team == teamColor.blue)
        {
            rend.material.color = Color.blue;
            bodyRend.material.color = Color.blue;
        }
        if (team == teamColor.red)
        {
            rend.material.color = Color.red;
            bodyRend.material.color = Color.red;

        }

        line = GetComponent<LineRenderer>();

    }
    // Update is called once per frame
    void Update()
    {
        health += Time.deltaTime;
        if (health > maxHealth) health = maxHealth;

        healthbar.SetHealth(health);
        checkAgainIn -= Time.deltaTime;
        if (checkAgainIn <= 0 || !attacking)
        {
            shouldCheckAgain = true;
            checkAgainIn = 1;
        }

        if (target != null)
        {
            if (shouldCheckAgain) FindPath(); //finds the quickest path
            if(!attacking)MoveAlongPath();

        
            attacking = ((target.position - transform.position).magnitude < attackRange && !target.transform.GetComponent<Ground>()) ? true : false; //attack the target if in range

        }

        if (attacking){
            attackInterval -= Time.deltaTime;
            if (attackInterval <= 0) {
                Attack();
                attackInterval = attackSpeed;
                    }
        }
        else
        {
            attackInterval = .5f;
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

        transform.position = Vector3.Lerp(transform.position, nextTarget, .015f);

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
            line.positionCount = positions.Length;
            line.SetPositions(positions);
        }
    }

    private void Attack()
    {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().MakeStats(target, (int)team, 1, 15, transform.position, 50);
        
    }
}
