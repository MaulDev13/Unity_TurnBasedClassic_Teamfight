using UnityEngine;

public class BattleUnitAnimation : MonoBehaviour
{
    [SerializeField] private BattleUnit battleUnit;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animControl;

    private void OnDisable()
    {
        battleUnit.iAction2 -= AnimAct;
        battleUnit.iTakeDamage2 -= AnimTakeDamage;
        battleUnit.iDead -= AnimDead;
    }

    public void Init(Sprite art)
    {
        battleUnit.iAction2 += AnimAct;
        battleUnit.iTakeDamage2 += AnimTakeDamage;
        battleUnit.iDead += AnimDead;

        spriteRenderer.sprite = art;

        if(!battleUnit.isPlayerUnit)
        {
            //spriteRenderer.flipX = true;
        } else
        {
            //spriteRenderer.flipX = false;
        }
    }

    public void AnimAct()
    {
        animControl.SetTrigger("Attack");
    }

    public void AnimAct(Skill _skill)
    {
        if(_skill._animator != null)
            animControl.runtimeAnimatorController = _skill._animator;

        if (_skill.actOnSelft_Effect != null)
            Instantiate(_skill.actOnSelft_Effect, transform.position, Quaternion.identity);

        //if(_skill.actOnTarget_Effect != null)
            //Instantiate(_skill.actOnTarget_Effect, transform.position, Quaternion.identity);

        animControl.SetTrigger("Attack");
    }

    public void AnimTakeDamage(Skill _skill)
    {
        animControl.SetTrigger("TakeDamage");
    }

    public void AnimDead()
    {
        animControl.SetTrigger("Dead");
        animControl.SetBool("IsDie", true);
    }
}
