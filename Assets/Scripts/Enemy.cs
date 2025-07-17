using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float health, damage, experience;
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask groundMask, playerMask;
    public Vector3 walkPoint;
    public float walkPointRange;
    public bool walkPointSet;
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public float sightRange, attackRange;
    public bool playerInSight, playerInAttack;
    void Start()
    {
        player = GameObject.Find("User").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInSight = Physics.CheckSphere(transform.position, sightRange, playerMask);
        playerInAttack = Physics.CheckSphere(transform.position, attackRange, playerMask);
        if (!playerInAttack && !playerInSight)
        {
            Patroling();
        }
        if (!playerInAttack && playerInSight)
        {
            Chasing();
        }
        if (playerInAttack && playerInSight)
        {
            Attacking();
        }
    }
    public void Patroling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }
        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }
        Vector3 distanceToPoint = transform.position - walkPoint;
        if (distanceToPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }
    public void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, Vector3.down, 2f, groundMask))
        {
            walkPointSet = true;
        }
    }
     public void Chasing()
    {
        agent.SetDestination(player.position);
    }
    public void Attacking()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    public void ResetAttack()
    {
        alreadyAttacked = false;
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        GetComponent<AudioSource>().Play();
        if (health <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        //logic for dying 
        player.GetComponent<PlayerStats>().AddExperience(experience);
        Destroy(gameObject);

    }

    //simple collide attack for testing
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStats>().TakeDamage(damage);
        }
    }

}
