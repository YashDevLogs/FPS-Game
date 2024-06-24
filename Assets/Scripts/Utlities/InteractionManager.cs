using UnityEngine;

public class InteractionManager
{
    public Weapon hoveredWeapon = null;
    public AmmoBox hoveredAmmoBox = null;
    public Throwable hoveredThrowable = null;

    public void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject objectHitByRayCast = hit.transform.gameObject;

            //Weapons
            if (objectHitByRayCast.GetComponent<Weapon>() && objectHitByRayCast.GetComponent<Weapon>().isActiveWeapon == false)
            {
                if (hoveredWeapon)
                {
                    hoveredWeapon.GetComponent<Outline>().enabled = false;
                }

                hoveredWeapon = objectHitByRayCast.gameObject.GetComponent<Weapon>();
                hoveredWeapon.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.F))
                {
                    ServiceLocator.Instance.WeaponManager.PickUpWeapon(objectHitByRayCast);
                }
            }
            else if (objectHitByRayCast.GetComponent<AmmoBox>())
            {
                if (hoveredAmmoBox)
                {
                    hoveredAmmoBox.GetComponent<Outline>().enabled = false;
                }

                hoveredAmmoBox = objectHitByRayCast.gameObject.GetComponent<AmmoBox>();
                hoveredAmmoBox.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.F))
                {
                    ServiceLocator.Instance.WeaponManager.PickUpAmmoBox(objectHitByRayCast.GetComponent<AmmoBox>());
                    hoveredAmmoBox.GetComponent<Outline>().enabled = false;
                    hoveredAmmoBox = null;
                    GameObject.Destroy(objectHitByRayCast);
                }
            }
            else if (objectHitByRayCast.GetComponent<Throwable>())
            {
                if (hoveredThrowable)
                {
                    hoveredThrowable.GetComponent<Outline>().enabled = false;
                }

                hoveredThrowable = objectHitByRayCast.GetComponent<Throwable>();
                hoveredThrowable.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.F))
                {
                    ServiceLocator.Instance.WeaponManager.PickUpThrowable(objectHitByRayCast.GetComponent<Throwable>());
                    hoveredThrowable.GetComponent<Outline>().enabled = false;
                    hoveredThrowable = null;
                    GameObject.Destroy(objectHitByRayCast);
                }
            }
            else
            {
                ResetHover();
            }
        }
        else
        {
            ResetHover();
        }
    }

    private void ResetHover()
    {
        if (hoveredWeapon)
        {
            hoveredWeapon.GetComponent<Outline>().enabled = false;
            hoveredWeapon = null;
        }

        if (hoveredAmmoBox)
        {
            hoveredAmmoBox.GetComponent<Outline>().enabled = false;
            hoveredAmmoBox = null;
        }

        if (hoveredThrowable)
        {
            hoveredThrowable.GetComponent<Outline>().enabled = false;
            hoveredThrowable = null;
        }
    }
}
