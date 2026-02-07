using UnityEngine;
using UnityEngine.AI;

public class ZombieMovement : MonoBehaviour
{
    [Header("Referências")]
    private Transform playerTransform;
    private NavMeshAgent navMeshAgent;
    private Animator animator; // Se tiver animações

    [Header("Detecção")]
    public float detectionRange = 20f; // Distância para detectar o player
    public float stopChasingRange = 30f; // Distância para parar de perseguir
    public float fieldOfViewAngle = 90f; // Ângulo de visão (em graus)

    [Header("Ataque")]
    public float attackRange = 2f; // Distância para atacar
    public float attackCooldown = 1.5f;
    private float attackTimer = 0f;
    public int attackDamage = 10;

    [Header("Movimento")]
    public float moveSpeed = 5f;
    public float stoppingDistance = 0.5f;

    private bool isChasing = false;
    private bool canSeePlayer = false;

    void Start()
    {
        // Encontrar o player
        playerTransform = Object.FindAnyObjectByType<ManageWeapons>()?.transform;
        
        // Configurar NavMeshAgent
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent != null)
        {
            navMeshAgent.speed = moveSpeed;
            navMeshAgent.stoppingDistance = stoppingDistance;
        }

        // Animator (opcional)
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (playerTransform == null) return;

        // Verificar se consegue ver o player
        canSeePlayer = CanSeePlayer();

        if (canSeePlayer)
        {
            // Começar a perseguir
            isChasing = true;
            ChasePlayer();

            // Verificar se chegou perto para atacar
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer <= attackRange)
            {
                AttackPlayer();
            }
        }
        else
        {
            // Parar de perseguir
            isChasing = false;
            StopChasing();
        }

        // Reduzir cooldown do ataque
        if (attackTimer > 0)
            attackTimer -= Time.deltaTime;
    }

    /// <summary>
    /// Verifica se o zumbi consegue ver o player
    /// </summary>
    bool CanSeePlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // Fora do alcance de detecção
        if (distanceToPlayer > detectionRange)
            return false;

        // Verificar ângulo de visão
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if (angle > fieldOfViewAngle / 2f)
            return false;

        // Raycast para verificar obstáculos
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, directionToPlayer, distanceToPlayer))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, directionToPlayer, out hit, distanceToPlayer))
            {
                if (hit.transform != playerTransform)
                    return false; // Há um obstáculo no caminho
            }
        }

        return true;
    }

    /// <summary>
    /// Persegue o player
    /// </summary>
    void ChasePlayer()
    {
        if (navMeshAgent != null)
        {
            navMeshAgent.SetDestination(playerTransform.position);
        }

        // Animação de corrida (opcional)
        if (animator != null)
        {
            animator.SetBool("IsRunning", true);
        }

        Debug.Log("Zumbi perseguindo player!");
    }

    /// <summary>
    /// Para de perseguir
    /// </summary>
    void StopChasing()
    {
        if (navMeshAgent != null)
        {
            navMeshAgent.ResetPath();
        }

        // Animação de parado (opcional)
        if (animator != null)
        {
            animator.SetBool("IsRunning", false);
        }

        Debug.Log("Zumbi parou de perseguir!");
    }

    /// <summary>
    /// Ataca o player
    /// </summary>
    void AttackPlayer()
    {
        if (attackTimer <= 0)
        {
            print("Zumbi atacou o player!");
            
            // Chamar função de dano do player aqui
            // playerTransform.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
            
            attackTimer = attackCooldown;

            // Animação de ataque (opcional)
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }
        }
    }

    /// <summary>
    /// Visualizar range de detecção no editor
    /// </summary>
    void OnDrawGizmosSelected()
    {
        // Círculo de detecção
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Cone de visão
        Gizmos.color = Color.cyan;
        Vector3 leftDirection = Quaternion.Euler(0, -fieldOfViewAngle / 2f, 0) * transform.forward * detectionRange;
        Vector3 rightDirection = Quaternion.Euler(0, fieldOfViewAngle / 2f, 0) * transform.forward * detectionRange;
        Gizmos.DrawLine(transform.position, transform.position + leftDirection);
        Gizmos.DrawLine(transform.position, transform.position + rightDirection);

        // Range de ataque
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
