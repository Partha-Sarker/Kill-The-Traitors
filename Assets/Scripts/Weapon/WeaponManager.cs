using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public IWeapon currentWeapon;
    public IWeapon[] allWeapon;


    // Start is called before the first frame update
    void Start()
    {
        currentWeapon.Equip();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWeapon == null)
            return;

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
}
