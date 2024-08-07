using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning($"{gameObject.name} does not have an Animator component.");
        }
    }

    public void SetTrigger(string triggerName)
    {
        if (animator != null)
        {
            animator.SetTrigger(triggerName);
        }
    }

    public void SetBool(string parameterName, bool value)
    {
        if (animator != null)
        {
            animator.SetBool(parameterName, value);
        }
    }

    public void SetFloat(string parameterName, float value)
    {
        if (animator != null)
        {
            animator.SetFloat(parameterName, value);
        }
    }

    public void SetInt(string parameterName, int value)
    {
        if (animator != null)
        {
            animator.SetInteger(parameterName, value);
        }
    }

    public void ResetTrigger(string triggerName)
    {
        if (animator != null)
        {
            animator.ResetTrigger(triggerName);
        }
    }

    public bool GetBool(string parameterName)
    {
        if (animator != null)
        {
            return animator.GetBool(parameterName);
        }
        return false;
    }

    public float GetFloat(string parameterName)
    {
        if (animator != null)
        {
            return animator.GetFloat(parameterName);
        }
        return 0f;
    }

    public int GetInt(string parameterName)
    {
        if (animator != null)
        {
            return animator.GetInteger(parameterName);
        }
        return 0;
    }

    public void Move(bool isMoving)
    {
        if (animator != null)
        {
            animator.SetBool("IsMove", isMoving);
        }
    }

    public void Shoot()
    {
        if (animator != null)
        {
            animator.SetTrigger("Shoot");
        }
    }
    public void SetIdle()
    {
        if (animator != null)
        {
            animator.SetBool("IsMove", false);
            animator.SetTrigger("Idle");

        }
    }
    public void Die()
    {
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
    }
    public void TriggerDieEvent()
    {
        Debug.Log("Trigger Die Event");
    }
    public void TakeDamage()
    {
        //if (animator != null)
        //{
        //    animator.SetTrigger("TakeDamage");
        //}
    }
}
