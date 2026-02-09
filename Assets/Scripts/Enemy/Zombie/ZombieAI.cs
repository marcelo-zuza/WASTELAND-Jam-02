using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class ZombieAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    private Animator anim;
    [Header("Enemy AI Configuration")]
    public float visionRay = 10f;
    public float attackRay = 2f;
    public float zombieAttackDamage = 10f;
    public float attackInterval = 1.2f;

    private float attackTimer = 0f;
    private PlayerHealth playerHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        float playerDistance = Vector3.Distance(player.position, transform.position);

        if(playerDistance <= attackRay)
        {
            // Attack State
            Attack();
        }
        else if(playerDistance <= visionRay)
        {
            // Chasing State
            Chase();
        }
        if (attackTimer > 0) attackTimer -= Time.deltaTime;
    }

    void Chase()
    {
        
        agent.isStopped = false;
        agent.SetDestination(player.position);

        anim.SetBool("attacking", false);
        anim.SetBool("chasing", true);
    }

    void Attack()
    {
        agent.isStopped = true;
        transform.rotation.SetLookRotation(player.position);
        anim.SetBool("attacking", true);
        anim.SetBool("chasing", false);

        //Attacking intervals
        if(attackTimer <= 0)
        {
            playerHealth.TakeDamage(zombieAttackDamage);
            attackTimer = attackInterval;
        }
    }
}
