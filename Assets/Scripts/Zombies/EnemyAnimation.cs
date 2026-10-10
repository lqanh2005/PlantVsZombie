
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private bool isDead;

    public void ResetState()
    {
        isDead = false;
        animator.Rebind();
        animator.Update(0f);
    }

    public void SetMoving(bool isMoving)
    {
        if (isDead) return;

        animator.SetBool("IsMoving", isMoving);
    }

    public void Attack()
    {
        if (isDead) return;

        animator.SetTrigger("Attack");
    }

    public void TakeDamage()
    {
        if (isDead) return;

        animator.SetTrigger("TakeDamage");
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("TakeDamage");
        animator.SetBool("IsMoving", false);
        animator.SetTrigger("Die");
    }
    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void OnValidate()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }
}
