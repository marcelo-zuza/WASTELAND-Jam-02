using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float playerHealth;
    public TextMeshProUGUI healthDisplay;
    void Start()
    {
        playerHealth = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        healthDisplay.text = playerHealth.ToString();
    }
}
