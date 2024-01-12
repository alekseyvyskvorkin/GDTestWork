using UnityEngine;
using TestWork.Units;

public class UnitAnimationStateHelper : StateMachineBehaviour
{
    private BaseUnit _baseUnit;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_baseUnit == null) _baseUnit = animator.GetComponent<BaseUnit>();
        _baseUnit.IsAbilityAnimationCompleted = false;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _baseUnit.IsAbilityAnimationCompleted = true;
    }
}
