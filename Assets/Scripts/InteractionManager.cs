using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public Weapon hoveredWeapon = null;
    public AmmoBox hoveredAmmoBox = null;
    public Throwable hoveredThrowable = null;

    private void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            GameObject objectHitByRayCast = hit.transform.gameObject;

            //Weapons
            if(objectHitByRayCast.GetComponent<Weapon>() && objectHitByRayCast.GetComponent<Weapon>().isActiveWeapon == false )
            {
                hoveredWeapon = objectHitByRayCast.gameObject.GetComponent<Weapon>();
                hoveredWeapon.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.F))
                {
                    GameManager.Instance.WeaponManager.PickUpWeapon(objectHitByRayCast.gameObject); 
                }
            }
            else
            {
                if (hoveredWeapon)
                {
                    hoveredWeapon.GetComponent<Outline>().enabled = false;
                }
            }

            //Ammo
            if (objectHitByRayCast.GetComponentInChildren<AmmoBox>())
            {
                hoveredAmmoBox = objectHitByRayCast.gameObject.GetComponent<AmmoBox>();
                hoveredAmmoBox.GetComponentInChildren<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.F))
                {
                    GameManager.Instance.WeaponManager.PickUpAmmoBox(hoveredAmmoBox);
                    Destroy(objectHitByRayCast.gameObject);
                }
            }
            else
            {
                if (hoveredAmmoBox)
                {
                    hoveredAmmoBox.GetComponent<Outline>().enabled = false;
                }
            }

            // Throwables
            if (objectHitByRayCast.GetComponentInChildren<Throwable>())
            {
                hoveredThrowable = objectHitByRayCast.gameObject.GetComponent<Throwable>();
                hoveredThrowable.GetComponentInChildren<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.F))
                {
                    GameManager.Instance.WeaponManager.PickUpThrowable(hoveredThrowable);
                    Destroy(objectHitByRayCast.gameObject);
                }
            }
            else
            {
                if (hoveredThrowable)
                {
                    hoveredThrowable.GetComponent<Outline>().enabled = false;
                }
            }

        }
    }
}
