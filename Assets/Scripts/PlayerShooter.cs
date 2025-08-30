using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public Gun gun;
    private GameManager gm;
    private PlayerHealth health;

    private void Awake()
    {
        health = GetComponent<PlayerHealth>();
    }

    private void Start()
    {
        var find = GameObject.FindWithTag(Define.TAG_GameManager);
        if(find != null)
        {
            gm = find.GetComponent<GameManager>();
        }
    }

    private void LateUpdate()
    {
        if (Input.GetButton(Define.BNT_Fire) && !gm.pause && !health.IsDead)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        gun.Fire();
    }
}
