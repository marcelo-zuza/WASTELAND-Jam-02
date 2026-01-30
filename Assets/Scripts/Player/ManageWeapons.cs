using System;
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
    [SerializeField] float grenadeForce = 200;
    // managing
    private int activeWeapon = WEAPON_GUN;
    private float timer;
    private bool timerStarted;
    private bool canShoot = true;
    private int currentWeapon;
    // Checking
    private bool[] hasWeapon;
    private int[] ammos;
    private int[] reserveAmmos;
    private int[] maxAmmos;
    private float[] reloadTime;
    private string[] weaponName;
    private AudioClip[] weaponFX;



    [Header("Aimming system")]
    Camera playerCamera;
    Ray rayFromPlayer;
    RaycastHit hit;
    private GameObject sparksAtImpact;
    private int gunAmmo = 12;

    [Header("Audio")]
    private AudioSource audioSource;
    [SerializeField] AudioClip laserGunFX;
    [SerializeField] AudioClip automaticGunFX;
    [SerializeField] AudioClip canonFx;
    // UI
    [SerializeField] TextMeshProUGUI gunDisplay;
    // Grenades
    public GameObject grenade;


    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        audioSource = GetComponent<AudioSource>();

        ammos = new int[3];
        hasWeapon = new bool[3];
        maxAmmos = new int[3];
        reserveAmmos = new int[3];
        reloadTime = new float[3];
        weaponName = new string[3];
        weaponFX = new AudioClip[3];


        hasWeapon[WEAPON_GUN] = true;
        hasWeapon[WEAPON_AUTO_GUN] = true;
        hasWeapon[WEAPON_GRENADE] = true;

        weaponName[WEAPON_GUN] = "GUN";
        weaponName[WEAPON_AUTO_GUN] = "AUTOMATIC GUN";
        weaponName[WEAPON_GRENADE] = "GRENADE";

        ammos[WEAPON_GUN] = 10;
        ammos[WEAPON_AUTO_GUN] = 10;
        ammos[WEAPON_GRENADE] = 1;

        reserveAmmos[WEAPON_GUN] = 40;
        reserveAmmos[WEAPON_AUTO_GUN] = 40;
        reserveAmmos[WEAPON_GRENADE] = 0;


        maxAmmos[WEAPON_GUN] = 20;
        maxAmmos[WEAPON_AUTO_GUN] = 20;
        maxAmmos[WEAPON_GRENADE] = 1;

        weaponFX[WEAPON_GUN] = laserGunFX;
        weaponFX[WEAPON_AUTO_GUN] = automaticGunFX;
        weaponFX[WEAPON_GRENADE] = canonFx;

        currentWeapon = WEAPON_GUN;


    }

    void Update()
    {
        rayFromPlayer = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Debug.DrawRay(rayFromPlayer.origin, rayFromPlayer.direction * 100, Color.red);

        if (currentWeapon == WEAPON_GRENADE && ammos[currentWeapon] == 0 && reserveAmmos[currentWeapon] > 0)
        {
            ammos[currentWeapon] = 1;
            reserveAmmos[currentWeapon] -= 1;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame && ammos[currentWeapon] > 0 && canShoot)
        {
            Shoot();
        }
        else if (Mouse.current.leftButton.wasPressedThisFrame && ammos[currentWeapon] <= 0 && reserveAmmos[currentWeapon] > 0) ReloadWeapon();
        else if (Mouse.current.leftButton.wasPressedThisFrame && ammos[currentWeapon] <= 0 && reserveAmmos[currentWeapon] <= 0)
        {
            print(ammos[currentWeapon] + " have no more bullets");
        }


        if (ammos[currentWeapon] < maxAmmos[currentWeapon] && reserveAmmos[currentWeapon] > 0 && Keyboard.current.rKey.wasPressedThisFrame) ReloadWeapon();
        else if (ammos[currentWeapon] < maxAmmos[currentWeapon] && reserveAmmos[currentWeapon] <= 0 && Keyboard.current.rKey.wasPressedThisFrame)
        {
            print("There's no more reserve bullets");
        }


        if (Keyboard.current.tabKey.wasPressedThisFrame) ChangeWeapons();

        if (gunDisplay != null)
        {
            if (currentWeapon == WEAPON_GRENADE)
            {
                if (ammos[currentWeapon] == 0) gunDisplay.text = (weaponName[currentWeapon] + ": " + (reserveAmmos[currentWeapon])).ToString();
                else gunDisplay.text = (weaponName[currentWeapon] + ": " + (reserveAmmos[currentWeapon] + 1)).ToString();
            }
            else
            {
                gunDisplay.text = (weaponName[currentWeapon] + ": " + ammos[currentWeapon] + " / " + reserveAmmos[currentWeapon]).ToString();
            }
        }

        // if(ammos[currentWeapon] > maxAmmos[currentWeapon])
        // {
        //     ammos[currentWeapon] = maxAmmos[currentWeapon];
        // }
    }

    void Shoot()
    {
        if (weaponFX[currentWeapon] != null) audioSource.PlayOneShot(weaponFX[currentWeapon]);


        if (currentWeapon == WEAPON_GRENADE)
        {
            GameObject launcher = GameObject.Find("Launcher");
            GameObject granadeF = (GameObject)Instantiate(grenade, launcher.transform.position, Quaternion.identity);
            granadeF.GetComponent<Rigidbody>().AddForce(launcher.transform.forward * grenadeForce);

        }
        else
        {
            if (Physics.Raycast(rayFromPlayer, out hit, 100))
            {
                print("The object" + hit.collider.gameObject.name + "is in front of player");

                Vector3 positionOfImpact;
                positionOfImpact = hit.point;
                if (sparksAtImpact != null) Instantiate(sparksAtImpact, positionOfImpact, Quaternion.identity);
                Destroy(sparksAtImpact, 1);

                print("You have " + ammos[currentWeapon] + " bullets left\nYou have " + reserveAmmos[currentWeapon] + " reserve bullets");

                GameObject objectTarget;
                if (hit.collider.gameObject.tag == "Target")
                {
                    objectTarget = hit.collider.gameObject;
                    // objectTarget.GetComponent<ManageNPC>().gotHit();
                }
            }
        }

        ammos[currentWeapon]--;
    }

    void ReloadWeapon()
    {
        timer = 0;
        canShoot = false;
        print("You are reloading...");
        timerStarted = true;
        timer += Time.deltaTime;
        if (timer >= reloadTime[currentWeapon])
        {
            canShoot = true;
        }
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
            int reposition = maxAmmos[currentWeapon] - ammos[currentWeapon];

            if (reserveAmmos[currentWeapon] >= reposition)
            {
                int bulltesToComplete = maxAmmos[currentWeapon] - ammos[currentWeapon];
                ammos[currentWeapon] = maxAmmos[currentWeapon];
                reserveAmmos[currentWeapon] -= bulltesToComplete;
            }
            else
            {
                ammos[currentWeapon] += reserveAmmos[currentWeapon];
                reserveAmmos[currentWeapon] = 0;
            }
        }
        print("Reloaded\n your Ammo is: " + ammos[currentWeapon] + "\nReserve Ammo: " + reserveAmmos[currentWeapon]);
    }

    void ChangeWeapons()
    {
        if (hasWeapon[WEAPON_GUN] && hasWeapon[WEAPON_AUTO_GUN] && hasWeapon[WEAPON_GRENADE])
        {
            currentWeapon++;
            if (currentWeapon > 2) currentWeapon = 0;
        }
        else if (hasWeapon[WEAPON_GUN] && hasWeapon[WEAPON_AUTO_GUN])
        {
            if (currentWeapon == WEAPON_GUN) currentWeapon = WEAPON_AUTO_GUN;
            else currentWeapon = WEAPON_GUN;
        }
        else if (hasWeapon[WEAPON_GUN] && hasWeapon[WEAPON_GRENADE])
        {
            if (currentWeapon == WEAPON_GUN) currentWeapon = WEAPON_GRENADE;
            else currentWeapon = WEAPON_GUN;
        }
        else if (hasWeapon[WEAPON_AUTO_GUN] && hasWeapon[WEAPON_GRENADE])
        {
            if (currentWeapon == WEAPON_AUTO_GUN) currentWeapon = WEAPON_GRENADE;
            else currentWeapon = WEAPON_AUTO_GUN;
        }
        else
        {

        }
        print("Current Weapon: " + weaponName[currentWeapon] + "(" + ammos[currentWeapon] + ")");
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







}
