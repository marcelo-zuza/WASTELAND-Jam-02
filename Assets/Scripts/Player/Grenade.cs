using NUnit.Framework;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float grenadeTimer;
    public bool grenadeTimerStarter;
    public float grenadeTimerLimit;
    public bool grenadeExplode;
    public GameObject explosion;
    public float radius = 5f;
    public float power = 500f;
    public float timer;
    public float explosionTime;
    private bool hasExploded;

    void Start()
    {
        timer = 0f;
        explosionTime = 2;
        hasExploded = false;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= explosionTime)
        {
            if (explosion != null && hasExploded == false)
            {
                Vector3 explosionPos = gameObject.transform.position;
                GameObject doExplosion = (GameObject)Instantiate(explosion, explosionPos, Quaternion.identity);
                Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject.GetComponent<Rigidbody>() != null && colliders[i].gameObject.tag != "Player")
                    {
                        GameObject objectTargeted = colliders[i].gameObject;
                        // if (objectTargeted.tag == "Target") objectTargeted.GetComponent<ManageNPC>().GotHitByGrenade();
                        {

                        }
                    }
                }
                hasExploded = true;
                Destroy(gameObject);
            }
        }
    }
}
