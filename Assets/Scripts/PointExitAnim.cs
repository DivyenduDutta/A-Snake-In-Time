using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointExitAnim : StateMachineBehaviour
{
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        Transform pointToDestroy = animator.GetComponent<Transform>();
        Destroy(pointToDestroy.gameObject);
    }

}