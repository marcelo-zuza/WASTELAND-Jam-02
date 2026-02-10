using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    public float health = 100f;
    private Animator anim;
    private Collider zombieCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        zombieCollider = GetComponent<Collider>();
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
        anim.SetBool("isDead", true);
        //zombieCollider.gameObject.SetActive(false);
        Debug.Log("Zombie morreu");
    }


}
