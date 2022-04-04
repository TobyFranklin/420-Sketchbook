using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public enum teamColor
    {
        blue,
        red,
    }
    public teamColor team;

    public HealthBar healthbar;

    public GameObject bulletPrefab;

    public Transform bottom;
    public Transform cylinder;

    public float health = 50;
    public float attackRange = 9;
    public bool isDestroyed = false;

    public Transform target;

    private float attackSpeed = 3;
    private float attackInterval = 0;

    // Start is called before the first frame update
    void Start()
    {

        Renderer bottomRend = bottom.gameObject.GetComponent<Renderer>();
        Renderer cylinderRend = cylinder.gameObject.GetComponent<Renderer>();

        healthbar.MaxHealth(health);

        if (team == teamColor.red)
        {
            bottomRend.material.color = Color.red;
            cylinderRend.material.color = Color.red;


        }
        else if (team == teamColor.blue)
        {
            bottomRend.material.color = Color.blue;
            cylinderRend.material.color = Color.blue;
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDestroyed)
        {

            attackInterval -= Time.deltaTime;

            if (target == null || (target.position - transform.position).magnitude >= attackRange) findTarget();
            else
            {
                if (attackInterval <= 0)
                {
                    Attack();
                    attackInterval = attackSpeed;
                }
            }

            if (health <= 0){ //destroys this minion and removes it from the List

                isDestroyed = true;

                if (team == teamColor.blue)
                {
                    Map.singleton.blueTowers.Remove(gameObject);
                    Destroy(gameObject);
                    Destroy(this);
                    return;
                }
                else if (team == teamColor.red)
                {
                    Map.singleton.redTowers.Remove(gameObject);
                    Destroy(gameObject);
                    Destroy(this);
                    return;
                }
            }
        }
        else
        {
            if (cylinder) cylinder.gameObject.SetActive(false);
        }
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        healthbar.SetHealth(health);
    }

    private void findTarget()
    {

        float smallestDist = 10000;
        GameObject closestObj = null;

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

        if (smallestDist < attackRange && closestObj != null)
        { // if something is in aggro range and the minion is not already aggrod, aggro on the closest thing
            target = closestObj.transform;
        }
    }

    private void Attack()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().MakeStats(target, (int)team, 2, 15, transform.position + new Vector3(0, 5, 0), 20);

    }
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if ((Player.instance.team == Player.teamColor.red && team == teamColor.blue) || (Player.instance.team == Player.teamColor.blue && team == teamColor.red)) Player.instance.target = transform;
        }
    }
}
