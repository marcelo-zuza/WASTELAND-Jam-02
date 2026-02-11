using Unity.VisualScripting;
using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    public float health = 100f;
    private Animator anim;
    private Collider[] zombieCollider;
    private ZombieAI zombieAI;
    private Rigidbody zombieRigidBody;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        zombieCollider = GetComponentsInChildren<Collider>();
        zombieAI = GetComponent<ZombieAI>();
        zombieRigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damageNum)
    {
        health -= damageNum;
        if(health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        anim.SetBool("IsDead", true);
        foreach (var col in zombieCollider) col.enabled = false;
        //zombieCollider.gameObject.SetActive(false);
        zombieAI.isDead = true;
        Debug.Log("Zombie morreu");
    }


}
