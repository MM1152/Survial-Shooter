using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class Gun : MonoBehaviour
{
    protected List<LineRenderer> lineRenderer = new List<LineRenderer>();
    private float lastShootTime;
    private bool shootAble;
    private AudioSource audioSource;
    public AudioClip gunShoot;

    public ParticleSystem gunParticle;
    public Transform shootPosition;
    public Transform player;

    public float damage;
    public float range;
    public float shootInterval = 2f;

    public float radius = 1f;

    public Vector3[] shootDir;
    protected virtual void Awake()
    {
        lineRenderer.Add(GetComponent<LineRenderer>());

        for (int i = 0; i < 4; i++)
        {
            GameObject go = new GameObject("LineRenderes", typeof(LineRenderer));
            go.transform.parent = transform;
            go.transform.position = Vector3.zero;

            LineRenderer line = go.GetComponent<LineRenderer>();
            line.material = lineRenderer[0].material;
            line.positionCount = 2;
            line.startWidth = 0.05f;
            line.endWidth = 0.05f;

            lineRenderer.Add(line);
        }

        foreach(var line in lineRenderer)
        {
            line.enabled = false;
        }
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        shootAble = true;
    }

    private void LateUpdate()
    {
        foreach (var line in lineRenderer)
        {
            line.SetPosition(0, shootPosition.position);
        }
    }

    public virtual void Fire()
    {
        if (lastShootTime + shootInterval < Time.time && shootAble)
        {
            shootDir = GetShootDir();
            lastShootTime = Time.time;

            audioSource.PlayOneShot(gunShoot);

            for(int i = 0; i< shootDir.Length; i++)
            {
                Vector3 hitPosition = shootPosition.position;
                RaycastHit hit;

                Vector3 dir = shootDir[i] * range - hitPosition;
                dir.Normalize();

                if (Physics.Raycast(shootPosition.position, dir, out hit, range))
                {
                    hitPosition = hit.point;

                    var find = hit.collider.GetComponent<IDamageAble>();
                    if (find != null)
                    {
                        find.OnDamage(damage, hit.point, hit.normal);
                    }
                }
                else
                {
                    hitPosition += dir * range;
                }
                lineRenderer[i].SetPosition(1, hitPosition);
            }

           
            gunParticle.Play();
            StartCoroutine(CoShoot());
        }
    }

    private IEnumerator CoShoot()
    {
        foreach (var line in lineRenderer)
        {
            line.enabled = true;
        }

        shootAble = false;
        yield return new WaitForSeconds(0.1f);
        foreach (var line in lineRenderer)
        {
            line.enabled = false;
        }
        shootAble = true;
    }

    private Vector3[] GetShootDir()
    {
        Vector3[] dir = new Vector3[lineRenderer.Count];

        for(int i = 0; i < dir.Length; i++)
        {
            Vector3 insideCircle = shootPosition.forward * range + Random.insideUnitSphere;
            dir[i] = insideCircle;
        }

        return dir;
    }
}
