using UnityEngine;

public class AnimationStateChanger : MonoBehaviour
{
    public string currentState = "Breathing Idle";
    public Animator animator;

    public void ChangeState(string newState, float animationSpeed = 1f)
    {
        animator.speed = animationSpeed;
        if (currentState == newState)
        {
            return;
        }
        animator.CrossFade(newState, .2f);
        currentState = newState;
    }
}
