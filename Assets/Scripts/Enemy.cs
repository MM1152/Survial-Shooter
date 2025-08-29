using UnityEngine;
using UnityEngine.AI;
public class Enemy : LivingEntity
{
    public enum Status
    {
        IDLE,
        TRACE,
        ATTACK,
        DIE
    }

    private Animator animator;
    private Transform target;
    private NavMeshAgent nav;
    private Rigidbody body;
    private Collider collider;

    public float traceRadious = 5f;
    public float attackRadious = 2f;

    public LayerMask targetMast;
    public Status currentStatus;
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
        collider = GetComponent<CapsuleCollider>();
    }

    public void OnEnable()
    {
        CurrentStatus = Status.IDLE;
        collider.enabled = true;
        nav.enabled = true;
    }

    public void Update()
    {
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
        if (Vector3.Distance(transform.position, target.position) <= attackRadious)
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
        if (Vector3.Distance(transform.position, target.position) > attackRadious)
        {
            CurrentStatus = Status.TRACE;
            return;
        }
    }

    public Transform FindTarget()
    {
        Transform target = null;
        RaycastHit hit;

        Collider[] colliders = Physics.OverlapSphere(transform.position, traceRadious);

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
