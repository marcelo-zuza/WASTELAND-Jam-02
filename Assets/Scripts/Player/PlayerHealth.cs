using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    
    public float playerHealth = 100f;
    public TextMeshProUGUI healthDisplay;
    
    [Header("Damage display configuration")]
    public float flashDuration = 0.2f;
    public GameObject deadDisplay;
    public GameObject damageFlash;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        healthDisplay.text = playerHealth.ToString();
    }

    public void TakeDamage(float amountOfDamage)
    {
        if (playerHealth <= 0) return;
        playerHealth -= amountOfDamage;
        StartCoroutine(DamageFlash());
        if(playerHealth <= 0)
        {
            PlayerDies();
        }

    }

    void PlayerDies()
    {
        Debug.Log("You Died");
        deadDisplay.gameObject.SetActive(true);
    }

    IEnumerator DamageFlash()
    {
        if (damageFlash == null) yield break;
        damageFlash.gameObject.SetActive(true);
        yield return new WaitForSeconds(flashDuration);
        damageFlash.gameObject.SetActive(false);
    }
}
