using UnityEngine;

public class WeaponAndThrowableManager : MonoBehaviour
{
    [HideInInspector] public WeaponBehaviour currentWeapon;
    public WeaponBehaviour[] allWeapon;
    private int currentWeaponIndex;


    // Start is called before the first frame update
    void Start()
    {
        currentWeaponIndex = 0;
        currentWeapon = allWeapon[currentWeaponIndex];
        currentWeapon.Equip();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWeapon == null)
            return;

        //if (Input.GetAxis("Mouse ScrollWheel") != 0)
        //{
        //    SwitchWeapon();
        //}

        if (Input.GetMouseButtonDown(0))
        {
            currentWeapon.OnLeftClickDown();
        }
        else if (Input.GetMouseButton(0))
        {
            currentWeapon.OnLeftClickHold();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            currentWeapon.OnLeftClickUp();
        }

        if (Input.GetMouseButtonDown(1))
        {
            currentWeapon.OnRightClickDown();
        }

        else if (Input.GetMouseButtonUp(1))
        {
            currentWeapon.OnRightClickUp();
        }
    }

    public void SwitchWeapon(int index)
    {

    }

    public void SwitchWeapon()
    {
        currentWeapon.Discard();
        currentWeaponIndex++;
        if (currentWeaponIndex >= allWeapon.Length)
            currentWeaponIndex = 0;
        currentWeapon = allWeapon[currentWeaponIndex];
        currentWeapon.Equip();
    }
}
