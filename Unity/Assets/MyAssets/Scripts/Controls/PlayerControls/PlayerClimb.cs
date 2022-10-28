using UnityEngine;

public class PlayerClimb : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2 ((animator.transform.gameObject.GetComponent<BasicMovement>().flip.facingRight ? 1.0f : -1.0f) * 1.0f, 9.0f);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    animator.transform.gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2(0.0f, 0.5f);
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2((animator.transform.gameObject.GetComponent<BasicMovement>().flip.facingRight ? 1.0f : -1.0f) * 2.0f, 0.0f);
        animator.SetBool("Climb", false);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
