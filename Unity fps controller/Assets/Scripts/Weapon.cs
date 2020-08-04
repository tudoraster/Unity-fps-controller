using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    //Referances
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public ParticleSystem smokeAfterMuzzleFlash;
    public Animator animator;
    public Text ammoInfo;

    //Setting gun stats
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;

    //Ammo settings
    private int maxAmmo = 7;
    private int currentAmmo;
    private float reloadTime = 1f;
    private bool CanReload = false;
    private bool isReloading = false;

    private float nextTimeToFire = 0f;

    private void Start()
    {
        ammoInfo.text = "7 / 7";
        currentAmmo = maxAmmo;
    }

    private void OnEnable()
    {
        isReloading = false;
        animator.SetBool("CanReload", false);
    }

    private void Update()
    {
        if (isReloading)
            return;

        if(currentAmmo <= 0)
        {
            StartCoroutine(Reload());
        }

        //Checking for user input on left mouse button.
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f/fireRate;

            Shoot();
        }
        else { animator.SetBool("HasShot", false); }
    }

    IEnumerator Reload()
    {
        isReloading = true;

        animator.SetBool("CanReload", true);

        yield return new WaitForSeconds(reloadTime);

        animator.SetBool("CanReload", false);

        currentAmmo = maxAmmo;

        ammoInfo.text = "7 / 7";

        isReloading = false;
    }

    private void Shoot()
    {
        //Everyhit the ammo is 
        currentAmmo--;
        ammoInfo.text = currentAmmo.ToString() + " / 7";

        //Defining raycast variable
        RaycastHit hit;

        muzzleFlash.Play();

        animator.SetBool("HasShot", true);

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
        }
    }
}
