using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float health, damage, experience;
    public int bottlecaps;
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
    public AnimationStateChanger animationStateChanger;
    public AudioSource audioSource;
    void Start()
    {
        player = GameObject.Find("User").transform;
        agent = GetComponent<NavMeshAgent>();
        setParameters();
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
    public void setParameters()
    {
        bottlecaps = (int)Random.Range(damage, health);
        experience = (int)Random.Range(damage, health);
    }
    public void Patroling()
    {
        if (!walkPointSet)
        {
            animationStateChanger.ChangeState("Breathing Idle", 1f);
            SearchWalkPoint();
        }
        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            animationStateChanger.ChangeState("Walking", 1f);
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
        animationStateChanger.ChangeState("Running", 1f);
    }
    public void Attacking()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            Ray ray = new Ray(transform.position + Vector3.up * 1.5f, (player.position - transform.position).normalized);
            if (Physics.Raycast(ray, out RaycastHit hit, attackRange, playerMask))
            {
                animationStateChanger.ChangeState("Stable Sword Outward Slash", 1f);
                if (hit.collider.CompareTag("Player"))
                {
                    GameEventsManager.Instance.TriggerPlayerDamaged(damage);
                    Debug.DrawRay(ray.origin, ray.direction * attackRange, Color.red, 1f);
                    Debug.Log("Enemy hit player with ray attack");
                }
            }
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    public void ResetAttack()
    {
        animationStateChanger.ChangeState("Breathing Idle", 1f);
        alreadyAttacked = false;
    }
    public void TakeDamage(float damage)
    {
        Debug.Log($"[Enemy] Taking damage: {damage}");
        health -= damage;
        
        if (health <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        Debug.Log($"[Enemy] Died");
        audioSource.Play();
        GameEventsManager.Instance.TriggerEnemyKilled(experience, bottlecaps);
        Destroy(gameObject, audioSource.clip.length - 0.2f);

    }


}
