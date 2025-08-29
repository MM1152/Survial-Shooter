using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public Gun gun;

    private void LateUpdate()
    {
        if (Input.GetButton(Define.BNT_Fire))
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        gun.Fire();
    }
}
