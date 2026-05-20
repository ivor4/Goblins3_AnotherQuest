using UnityEngine;

namespace Gob3AQ.GameElement.Animation
{

    public class GenericAnimBehavior : StateMachineBehaviour
    {
        private IAnimatorListener listener;
        private bool hasBeenChecked;
        
        // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!hasBeenChecked)
            {
                listener = animator.gameObject.GetComponentInChildren<IAnimatorListener>();
                hasBeenChecked = true;
            }
            listener?.OnAnimationStart(stateInfo);
        }

        // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            listener?.OnAnimationUpdate(stateInfo);
        }

        // OnStateExit is called before OnStateExit is called on any state inside this state machine
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            listener?.OnAnimationEnd(stateInfo);
        }

        // OnStateMove is called before OnStateMove is called on any state inside this state machine
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateIK is called before OnStateIK is called on any state inside this state machine
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateMachineEnter is called when entering a state machine via its Entry Node
        //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        //{
        //    
        //}

        // OnStateMachineExit is called when exiting a state machine via its Exit Node
        //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        //{
        //    
        //}
    }
}