using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private float lastShootTime;
    private bool shootAble;
    private AudioSource audioSource;
    public AudioClip gunShoot;

    public ParticleSystem gunParticle;
    public Transform shootPosition;
    public Transform player;

    public float damage;
    public float range;
    public float shootInterval = 0.001f;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        shootAble = true;
    }

    private void LateUpdate()
    {
        lineRenderer.SetPosition(0, shootPosition.position);
    }

    public void Fire()
    {
        if (lastShootTime + shootInterval < Time.time && shootAble)
        {
            lastShootTime = Time.time;

            RaycastHit hit;
            Vector3 hitPosition = shootPosition.position;
            audioSource.PlayOneShot(gunShoot);

            if (Physics.Raycast(shootPosition.position , shootPosition.forward , out hit , range))
            {
                hitPosition = hit.point;

                var find = hit.collider.GetComponent<IDamageAble>();
                if(find != null)
                {
                    find.OnDamage(damage, hit.point, hit.normal);
                }
            }
            else
            {
                hitPosition += shootPosition.forward * range;
            }

            lineRenderer.SetPosition(1, hitPosition);
            gunParticle.Play();
            StartCoroutine(CoShoot());
        }
    }

    private IEnumerator CoShoot()
    {
        lineRenderer.enabled = true;
        shootAble = false;
        yield return new WaitForSeconds(shootInterval);
        lineRenderer.enabled = false;
        shootAble = true;
    }
}
