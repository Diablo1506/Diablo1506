using UnityEngine;

public class animationStateController : MonoBehaviour
{
    Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log(animator);
    }

    // Update is called once per frame
    void Update()
    {   
        // Check if the "W" key is pressed
        if (Input.GetKey(KeyCode.W)) // Use KeyCode.W instead of "W"
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            // If the "W" key is not pressed, set the "isWalking" parameter to false
            animator.SetBool("isWalking", false);
        }

        if (Input.GetKey(KeyCode.S)) // Use KeyCode.S instead of "S"
        {
            animator.SetBool("IsWalkBack", true);
        }
        else
        {
            // If the "S" key is not pressed, set the "isWalkingBackwards" parameter to false
            animator.SetBool("IsWalkBack", false);
        }
           // Press R to play bobbing
        if (Input.GetKey(KeyCode.R))
        {
            animator.SetBool("isBobbing", true);
        }
        else
        {
            // If the "R" key is not pressed, set the "isBobbing" parameter to false
            animator.SetBool("isBobbing", false);
        }
    }
}
