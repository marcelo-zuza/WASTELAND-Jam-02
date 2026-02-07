using UnityEngine;

public class RecoilWeapon : MonoBehaviour
{
    [Header("Configuração do coice da arma")]
    public float recoilX = -5f;
    public float recoilY = 2f;
    public float returnSpeed = 20f;
    public float snapSpeed = 30f;
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


}
