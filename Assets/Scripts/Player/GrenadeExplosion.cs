using UnityEngine;

public class GrenadeExplosion : MonoBehaviour
{

    [Header("Explosion Settings")]
    [SerializeField] private float explosionDelay = 3f;
    [SerializeField] private float explosionRadius = 15f;
    [SerializeField] private float explosionForce = 700f;
    [SerializeField] private float explosionDamage = 50f;

    [Header("Explosion Effects")]
    [SerializeField] private GameObject explosionEffects;
    [SerializeField] private AudioClip explosionSound;

    private float timer = 0f;
    private bool hasExploded = false;
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= explosionDelay && !hasExploded)
        {
            Explode();
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(!collision.gameObject.CompareTag("Player") && !hasExploded)
    //    {
    //        Explode();

    //    }
    //}

    void Explode()
    {
        hasExploded = true;
        //Explosion
        if(explosionEffects != null)
        {
            Instantiate(explosionEffects, transform.position, Quaternion.identity);
        }

        if(explosionEffects != null && explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        }

        // aplly damage around explosion
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach(Collider col in colliders)
        {
            Rigidbody targetRB = col.GetComponent<Rigidbody>();
            if(targetRB != null && !col.gameObject.CompareTag("Player"))
            {
                // apllying damage with IDamageable component
                IDamageable damageable = col.GetComponent<IDamageable>();
                if(damageable != null)
                {
                    damageable.TakeDamage(explosionDamage);
                }
                
            }
        }
        Destroy(gameObject, 0.5f);
    }
}
