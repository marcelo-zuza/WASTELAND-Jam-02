using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class ZombieAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    private Animator anim;

    public float visionRay = 10f;
    public float attackRay = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float playerDistance = Vector3.Distance(player.position, transform.position);

        if(playerDistance <= attackRay)
        {
            // Attack State
            agent.isStopped = true;
            transform.rotation.SetLookRotation(player.position);
            anim.SetBool("attacking", true);
            anim.SetBool("chasing", false);
        }
        else if(playerDistance <= visionRay)
        {
            // Chasing State
            agent.isStopped = false;
            agent.SetDestination(player.position);

            anim.SetBool("attacking", false);
            anim.SetBool("chasing", true);
        }
    }
}
