using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //Referances
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public ParticleSystem smokeAfterMuzzleFlash;
    public Animator animator;

    //Setting gun stats
    public float damage = 10f;
    public float range = 100f;
    private bool HasShot = false;

    private void Update()
    {
        HasShot = false;

        //Checking for user input on left mouse button.
        if (Input.GetButtonDown("Fire1"))
        {
            HasShot = true;

            animator.SetBool("HasShot", HasShot);
            Shoot();
        }
    }

    private void Shoot()
    {
        //Defining raycast variable
        RaycastHit hit;

        muzzleFlash.Play();

        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
        }
    }
}
