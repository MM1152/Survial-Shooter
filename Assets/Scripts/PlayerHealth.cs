using UnityEngine;

public class PlayerHealth : LivingEntity
{
    public UIManager uiManager;
    public bool IsDead => hp <= 0;

    private Collider collider;
    private Animator animator;

    public AudioClip hitClip;
    public AudioClip dieClip;
    private AudioSource audioSource;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<CapsuleCollider>();
        audioSource = GetComponent<AudioSource>();
    }

    protected override void OnEnable()
    {
        collider.enabled = true;
        maxHp = 300;
        base.OnEnable();
        uiManager.UpdateHpSlide(hp, maxHp);
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (IsDead) return;
        base.OnDamage(damage, hitPoint, hitNormal);
        uiManager.UpdateHpSlide(hp, maxHp);
        audioSource.PlayOneShot(hitClip);
    }

    protected override void Die()
    {
        base.Die();
        collider.enabled = false;
        audioSource.PlayOneShot(dieClip);
        animator.SetTrigger(Define.ANI_Die);
    }
}
