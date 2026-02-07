using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ManageWeapons : MonoBehaviour
{
    [Header("Weapon management")]
    // Types of guns
    private const int WEAPON_GUN = 0;
    private const int WEAPON_AUTO_GUN = 1;
    private const int WEAPON_GRENADE = 2;
    private int numberOfWeapons = 3;
    [SerializeField] float grenadeForce = 200;

    // managing
    private int activeWeapon = WEAPON_GUN;
    private float timer;
    private bool timerStarted;
    private bool canShoot = true;
    private int currentWeapon;

    // Checking
    public bool[] hasWeapon;
    private int[] ammos;
    private int[] reserveAmmos;
    private int[] maxAmmos;
    private float[] reloadTime;
    private string[] weaponName;
    private AudioClip[] weaponFX;

    // List of Weapon GameObjects
    [SerializeField] GameObject[] weaponGameObject = new GameObject[3];

    [Header("Aimming system")]
    Camera playerCamera;
    Ray rayFromPlayer;
    RaycastHit hit;
    private GameObject sparksAtImpact;
    private int gunAmmo = 12;

    [Header("Audio")]
    private AudioSource audioSource;
    [SerializeField] AudioClip pistolFX;
    [SerializeField] AudioClip automaticGunFX;
    [SerializeField] AudioClip canonFx;

    // UI
    [SerializeField] TextMeshProUGUI gunDisplay;

    [Header("Grenade Launch Configuration")]
    public GameObject grenadePrefab;
    public Transform grenadeSpawnPosTransform;
    public float grenadeLaunchDuration = 0.3f;
    public float grenadeOutOfViewDistance = 2f;

    [Header("Configuração do coice da arma")]
    public float recoilX = -5f;
    public float recoilY = 2f;
    public float returnSpeed = 20f;
    public float snapSpeed = 30f;
    private Vector3 gunRecoilCurrentPosition;
    private Vector3 gunRecoilTargetPosition;

    //Weapon initial position
    private Vector3 weaponInitialPosition;

    // Display Ammo
    public TextMeshProUGUI ammoDisplay;
    public TextMeshProUGUI reserveAmmoDisplay;

    [Header("Gun Configuration")]
    public float automaticGunFireRateTimer = 0.05f;
    private float fireRateTimer = 0f;
    public float weaponReloadPosition = -0.3f;
    public float reloadDuration = 0.3f;


    void Start()
    {
        // Components
        playerCamera = GetComponentInChildren<Camera>();
        audioSource = GetComponent<AudioSource>();
        // Arrays
        ammos = new int[numberOfWeapons];
        hasWeapon = new bool[numberOfWeapons];
        maxAmmos = new int[numberOfWeapons];
        reserveAmmos = new int[numberOfWeapons];
        reloadTime = new float[numberOfWeapons];
        weaponName = new string[numberOfWeapons];
        weaponFX = new AudioClip[numberOfWeapons];
        // Gun posetions
        hasWeapon[WEAPON_GUN] = true;
        hasWeapon[WEAPON_AUTO_GUN] = true;
        hasWeapon[WEAPON_GRENADE] = true;
        // GUN names
        weaponName[WEAPON_GUN] = "GUN";
        weaponName[WEAPON_AUTO_GUN] = "AUTOMATIC GUN";
        weaponName[WEAPON_GRENADE] = "GRENADE";
        // Ammunation
        ammos[WEAPON_GUN] = 10;
        ammos[WEAPON_AUTO_GUN] = 10;
        ammos[WEAPON_GRENADE] = 10;
        // Reserver Ammunation
        reserveAmmos[WEAPON_GUN] = 40;
        reserveAmmos[WEAPON_AUTO_GUN] = 40;
        reserveAmmos[WEAPON_GRENADE] = 0;
        // Weappon max reserve ammo
        maxAmmos[WEAPON_GUN] = 20;
        maxAmmos[WEAPON_AUTO_GUN] = 20;
        maxAmmos[WEAPON_GRENADE] = 1;
        // Weapons fX
        weaponFX[WEAPON_GUN] = pistolFX;
        weaponFX[WEAPON_AUTO_GUN] = automaticGunFX;
        weaponFX[WEAPON_GRENADE] = canonFx;
        // Gun GameObjects
        currentWeapon = WEAPON_GUN;
        weaponGameObject[WEAPON_GUN].gameObject.SetActive(true);
        weaponGameObject[WEAPON_AUTO_GUN].gameObject.SetActive(false);
        weaponGameObject[WEAPON_GRENADE].gameObject.SetActive(false);

        // Weapon initial position
        weaponInitialPosition = weaponGameObject[currentWeapon].transform.localPosition;
    }

    void Update()
    {
        Commands();
        DisplayAmmo();
        FireRateTimerCounter();
        Checkings();
    }

    void FireRateTimerCounter()
    {
        if (fireRateTimer > 0) fireRateTimer -= Time.deltaTime;
    }

    void Checkings()
    {
        // Ray debug
        rayFromPlayer = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Debug.DrawRay(rayFromPlayer.origin, rayFromPlayer.direction * 100, Color.red);
        // Changing weapons checking
        if (currentWeapon > 2)
        {
            currentWeapon = 0;
        }
        else if (currentWeapon < 0)
        {
            currentWeapon = 2;
        }
        // Grenede always in hand
        if (currentWeapon == WEAPON_GRENADE && ammos[currentWeapon] == 0 && reserveAmmos[currentWeapon] > 0)
        {
            ammos[currentWeapon] = 1;
            reserveAmmos[currentWeapon] -= 1;
        }
        // Recoil system Update weapon to original poisition
        weaponGameObject[currentWeapon].transform.localPosition = Vector3.Lerp(weaponGameObject[currentWeapon].transform.localPosition, weaponInitialPosition, Time.deltaTime * 10f);

        if (ammos[WEAPON_GRENADE] > 0)
        {
            hasWeapon[WEAPON_GRENADE] = true;
        }

        if (ammos[WEAPON_GRENADE] <=0 && hasWeapon[WEAPON_GRENADE] == true)
        {
            hasWeapon[WEAPON_GRENADE] = false;
            ChangeWeapons("+");
        }
    }

    void Commands()
    {
        // Reload Weapon
        if (ammos[currentWeapon] < maxAmmos[currentWeapon] && reserveAmmos[currentWeapon] > 0 && Keyboard.current.rKey.wasPressedThisFrame)
        {
            ReloadWeapon();
        }
        // Shoot Automatic Gun
        if (currentWeapon == WEAPON_AUTO_GUN)
        {
            if (Mouse.current.leftButton.isPressed && ammos[currentWeapon] > 0 && canShoot && fireRateTimer <= 0)
            {
                Shoot();
                fireRateTimer = automaticGunFireRateTimer;
            }
            else if (Mouse.current.leftButton.wasPressedThisFrame && ammos[currentWeapon] <= 0 && reserveAmmos[currentWeapon] > 0) ReloadWeapon();
        }
        // Shoot other weapons
        else
        {
            if(Mouse.current.leftButton.wasPressedThisFrame && ammos[currentWeapon] > 0 && canShoot)
            {
                Shoot();
            }
            else if (Mouse.current.leftButton.isPressed && ammos[currentWeapon] <= 0 && reserveAmmos[currentWeapon] > 0)
            {
                ReloadWeapon();
            }          
        }

        // Change Weapons
       if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            ChangeWeapons("+");
        }

        if(Mouse.current.scroll.y.value > 0)
        {
            ChangeWeapons("+");
        }
        else if(Mouse.current.scroll.y.value < 0)
        {
            ChangeWeapons("-");
        }

    }



    void LaunchGrenade()
    {
        Vector3 grenadeSpawnPos = grenadeSpawnPosTransform.position;
        GameObject grenadeInstance = Instantiate(grenadePrefab, grenadeSpawnPos, Quaternion.identity);

        Rigidbody grenadeRb = grenadeInstance.GetComponent<Rigidbody>();
        if (grenadeRb != null)
        {
            grenadeRb.linearVelocity = playerCamera.transform.forward * grenadeForce;
        }
        audioSource.PlayOneShot(canonFx);
        ammos[currentWeapon]--;
    }

    void Shoot()
    {
        if (currentWeapon == WEAPON_GRENADE)
        {
            LaunchGrenade();
        }
        else
        {
            if (Physics.Raycast(rayFromPlayer, out hit, 100))
            {
                print("The object" + hit.collider.gameObject.name + "is in front of player");

                Vector3 positionOfImpact = hit.point;
                if (sparksAtImpact != null)
                {
                    Instantiate(sparksAtImpact, positionOfImpact, Quaternion.identity);
                }

                print("You have " + ammos[currentWeapon] + " bullets left\nYou have " + reserveAmmos[currentWeapon] + " reserve bullets");

                GameObject objectTarget;
                if (hit.collider.gameObject.tag == "Target")
                {
                    objectTarget = hit.collider.gameObject;
                    // objectTarget.GetComponent<ManageNPC>().gotHit();
                }
            }
            if(audioSource != null && weaponFX[currentWeapon] != null)
            {
                audioSource.PlayOneShot(weaponFX[currentWeapon]);
            }
            GunRecoil();
            ammos[currentWeapon]--;
        }
    }

    private IEnumerator ReloadCoroutine()
    {
        canShoot = false;
        print("You are reloading...");

        Vector3 reloadPosition = weaponInitialPosition + new Vector3(0, -weaponReloadPosition, 0);
        float elapsedTime = 0f;

        // Animar arma descendo e subindo
        while (elapsedTime < reloadDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / reloadDuration);

            // Usar SmoothStep para suavidade na curva
            float smoothProgress = Mathf.SmoothStep(0f, 1f, progress);

            // Desce na primeira metade, sobe na segunda metade
            Vector3 targetPos = smoothProgress < 0.5f
                ? Vector3.Lerp(weaponInitialPosition, reloadPosition, smoothProgress * 2f)
                : Vector3.Lerp(reloadPosition, weaponInitialPosition, (smoothProgress - 0.5f) * 2f);

            weaponGameObject[currentWeapon].transform.localPosition = targetPos;
            yield return null;
        }

        // Garantir posição final correta
        weaponGameObject[currentWeapon].transform.localPosition = weaponInitialPosition;

        // Aplicar munição
        if (ammos[currentWeapon] <= 0)
        {
            if (reserveAmmos[currentWeapon] >= maxAmmos[currentWeapon])
            {
                ammos[currentWeapon] += maxAmmos[currentWeapon];
                reserveAmmos[currentWeapon] -= maxAmmos[currentWeapon];
            }
            else
            {
                ammos[currentWeapon] = reserveAmmos[currentWeapon];
                reserveAmmos[currentWeapon] = 0;
            }
        }
        else
        {
            int bulletsNeeded = maxAmmos[currentWeapon] - ammos[currentWeapon];
            if (reserveAmmos[currentWeapon] >= bulletsNeeded)
            {
                ammos[currentWeapon] = maxAmmos[currentWeapon];
                reserveAmmos[currentWeapon] -= bulletsNeeded;
            }
            else
            {
                ammos[currentWeapon] += reserveAmmos[currentWeapon];
                reserveAmmos[currentWeapon] = 0;
            }
        }

        print("Reloaded\nYour Ammo: " + ammos[currentWeapon] + " / Reserve: " + reserveAmmos[currentWeapon]);
        canShoot = true;
    }

    void ReloadWeapon()
    {
        if (!canShoot) return;
        StartCoroutine(ReloadCoroutine());
    }

    void ChangeWeapons(string numOperator)
    {
        weaponGameObject[currentWeapon].gameObject.SetActive(false);

        // Atualizar índice da arma
        if (numOperator == "+")
        {
            currentWeapon = currentWeapon >= 2 ? 0 : currentWeapon + 1;
        }
        else if (numOperator == "-")
        {
            currentWeapon = currentWeapon <= 0 ? 2 : currentWeapon - 1;
        }

        // Encontrar próxima arma disponível
        int iterações = 0;
        while (!hasWeapon[currentWeapon] && iterações < numberOfWeapons)
        {
            currentWeapon = numOperator == "+" ? (currentWeapon >= 2 ? 0 : currentWeapon + 1) : (currentWeapon <= 0 ? 2 : currentWeapon - 1);
            iterações++;
        }

        // Ativar arma e atualizar posição
        weaponGameObject[currentWeapon].gameObject.SetActive(true);
        weaponInitialPosition = weaponGameObject[currentWeapon].transform.localPosition;

        print("Current Weapon: " + weaponName[currentWeapon] + " (" + ammos[currentWeapon] + ")");
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        string tagOfTheOtherObject = hit.collider.gameObject.tag;
        if (tagOfTheOtherObject == "ammo_gun" || tagOfTheOtherObject == "ammo_automatic_gun" || tagOfTheOtherObject == "ammo_grenade")
        {
            int indexOfAmmoBeingUpdated = 0;
            if (tagOfTheOtherObject == "ammo_gun") indexOfAmmoBeingUpdated = WEAPON_GUN;
            if (tagOfTheOtherObject == "ammo_automatic_gun") indexOfAmmoBeingUpdated = WEAPON_AUTO_GUN;
            if (tagOfTheOtherObject == "ammo_grenade") indexOfAmmoBeingUpdated = WEAPON_GRENADE;
            reserveAmmos[indexOfAmmoBeingUpdated] += 10;
            print(weaponName[indexOfAmmoBeingUpdated] + " got 10 bullets");

            //if(ammos[indexOfAmmoBeingUpdated] > maxAmmos[indexOfAmmoBeingUpdated]) ammos[indexOfAmmoBeingUpdated] = maxAmmos[indexOfAmmoBeingUpdated];
            Destroy(hit.collider.gameObject);
        }
    }
    public void GunRecoil()
    {
        Vector3 kick = new Vector3(Random.Range(-recoilX, recoilX), recoilY, 0);
        weaponGameObject[currentWeapon].transform.localPosition += kick;
    }

    void DisplayAmmo()
    {
        ammoDisplay.text = ammos[currentWeapon].ToString();
        reserveAmmoDisplay.text = " / " + reserveAmmos[currentWeapon].ToString();
    }
    
}
