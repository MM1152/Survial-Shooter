using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : LivingEntity
{
    public enum Status
    {
        IDLE,
        TRACE,
        ATTACK,
        DIE,
        PAUSE
    }
    public EnemyData data;
    public ParticleSystem hitParticle;
    public GameManager gm;

    private Animator animator;
    private Transform target;
    private NavMeshAgent nav;
    private Rigidbody body;
    private Collider collider;

    private float lastAttackTime;
    public LayerMask targetMast;
    public Status currentStatus;
    private Status prevStatus;
    public Status CurrentStatus
    {
        get { return currentStatus; }
        set
        {
            if (currentStatus == value) return;
            currentStatus = value;

            switch (currentStatus)
            {
                case Status.IDLE:
                    nav.isStopped = true;
                    animator.SetBool(Define.ANI_Move, false);
                    break;
                case Status.TRACE:
                    nav.isStopped = false;
                    animator.SetBool(Define.ANI_Move, true);
                    break;
                case Status.ATTACK:
                    nav.isStopped = true;
                    animator.SetBool(Define.ANI_Move, false);
                    break;
                case Status.DIE:
                    animator.SetTrigger(Define.ANI_Die);
                    break;
            }
        }
    }

    public void Awake()
    {
        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        body = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();

    }
    private void Start()
    {
        var find = GameObject.FindWithTag("GameController");
        if(find != null)
        {
            gm = find.GetComponent<GameManager>();
            OnDeath += () => gm.AddScore(data.score);
        }
    }
    protected override void OnEnable()
    {
        CurrentStatus = Status.IDLE;
        collider.enabled = true;
        nav.enabled = true;

        maxHp = data.maxHp;

        base.OnEnable();
    }

    public void Update()
    {
        if (Status.DIE == CurrentStatus) return;

        if (gm.pause)
        {
            if(CurrentStatus != Status.PAUSE)
            {
                nav.isStopped = true;
                prevStatus = CurrentStatus;
                CurrentStatus = Status.PAUSE;
            }

            return;
        }

        switch (currentStatus)
        {
            case Status.IDLE:
                UpdateIDLE();
                break;
            case Status.TRACE:
                UpdateTRACE();
                break;
            case Status.ATTACK:
                UpdateATTACK();
                break;
            case Status.PAUSE:
                CurrentStatus = prevStatus;
                break;
        }
    }

    private void UpdateIDLE()
    {
        if (target != null)
        {
            CurrentStatus = Status.TRACE;
            return;
        }

        target = FindTarget();
    }
    private void UpdateTRACE()
    {
        if(target == null)
        {
            CurrentStatus = Status.IDLE;
            return;
        }
        if (Vector3.Distance(transform.position, target.position) <= data.attackRadious)
        {
            CurrentStatus = Status.ATTACK;
            return;
        }

        nav.SetDestination(target.position);
    }
    private void UpdateATTACK()
    {
        if (target == null)
        {
            CurrentStatus = Status.IDLE;
            return;
        }
        if (Vector3.Distance(transform.position, target.position) > data.attackRadious)
        {
            CurrentStatus = Status.TRACE;
            return;
        }

        if(lastAttackTime + data.attackInterval < Time.time)
        {
            lastAttackTime = Time.time;
            var find = target.GetComponent<IDamageAble>();
            if(find != null)
            {
                find.OnDamage(data.damage, Vector3.zero, Vector3.zero);
            }
        }
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (CurrentStatus == Status.DIE) return;
        base.OnDamage(damage, hitPoint, hitNormal);
        if(hitParticle != null)
        {
            hitParticle.transform.position = hitPoint;
            hitParticle.transform.forward = hitNormal;
            hitParticle.Play();
        }
    }

    public Transform FindTarget()
    {
        Transform target = null;
        RaycastHit hit;

        Collider[] colliders = Physics.OverlapSphere(transform.position, data.traceRadious);

        if(colliders.Length != 0)
        {
            foreach(var collider in colliders)
            {
                if(collider.CompareTag(Define.TAG_Player))
                {
                    target = collider.transform;
                }
            }
        }

        return target;
    }
    protected override void Die()
    {
        base.Die();
        CurrentStatus = Status.DIE;
    }

    public void StartSinking()
    {
        nav.enabled = false;
        collider.enabled = false;
    }
}
