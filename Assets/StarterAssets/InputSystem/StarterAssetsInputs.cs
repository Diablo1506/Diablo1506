using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {

		Animator animator;
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
		public bool idle1;
		public bool JabRight;
		public bool JabLeft;
		public bool Dodgeleft;
		public bool DodgeRight;
		public bool Combo3;
		public bool WalkForward;
		public bool Combo1;
		public bool Crouch;
		public bool Bobbing;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            if(cursorInputForLook)
            {
                LookInput(value.Get<Vector2>());
            }
        }
		public void Onidle1(InputValue value)
		{	
            if (Input.GetKey(KeyCode.C))
			{
				idle1 = true;
			}
			else
			{
				idle1 = false;
			}
		}
		public void OnJabRight(InputValue value)
		{	
			if (Input.GetMouseButton(1))
			
			{
				JabRight = true;
			}
			else
			{
				JabRight = false;
			}
		}
		public void OnJabLeft(InputValue value)
		{
			if (Input.GetMouseButton(0))
			{
				JabLeft = true;
			}
			else
			{
				JabLeft = false;
			}
		}
		public void OnDodgeLeft(InputValue value)
		{
			if (Input.GetKey(KeyCode.Q))
			{
				Dodgeleft = true;
			}
			else
			{
				Dodgeleft = false;
			}
		}
		public void OnDodgeRight(InputValue value)
		{
			if (Input.GetKey(KeyCode.E))
			{
				DodgeRight = true;
			}
			else
			{
				DodgeRight = false;
			}
		}
		public void OnCombo3(InputValue value)
		{
			if (Input.GetKey(KeyCode.LeftShift))
			{
				Combo3 = true;
			}
			else
			{
				Combo3 = false;
			}
		}
		public void OnWalkForward(InputValue value)
		{
			if (Input.GetKey(KeyCode.Z))
			{
				WalkForward = true;
			}
			else
			{
				WalkForward = false;
			}
		}

		public void OnCombo1(InputValue value)
		{
			if (Input.GetKey(KeyCode.X))
			{
				Combo1 = true;
			}
			else
			{
				Combo1 = false;
			}
		}
		public void OnCrouch(InputValue value)
		{	
			if (Input.GetKey(KeyCode.LeftControl))
			{
				Crouch = true;
			}	
			else
			{
				Crouch = false;
			}
		}	
		public void OnBobbing(InputValue value)
		{
			if (Input.GetKey(KeyCode.R))
			{
				Bobbing = true;
			}
			else
			{
				Bobbing = false;
			}
		}											

        // Removed OnJump and OnSprint methods
#endif

        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        } 

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        // Removed JumpInput and SprintInput methods

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    

	public void Start()
	{
		animator = GetComponent<Animator>();
		Debug.Log(animator);
	}
	public void Update()
	{   
		// Check if the "W" key is pressed
		if (Input.GetKey(KeyCode.C)) // Use KeyCode.W instead of "W"
		{
			animator.SetBool("isIdle", true);
		}
		else
		{
			// If the "W" key is not pressed, set the "isWalking" parameter to false
			animator.SetBool("isIdle", false);
		}

		 // Check for left mouse button click for Jab Left
            if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button
            {
                animator.SetBool("isJabLeft", true);
            }
            else
            {
                animator.SetBool("isJabLeft", false);
            }

            // Check for right mouse button click for Jab Right
            if (Input.GetMouseButtonDown(1)) // 1 is the right mouse button
            {
                animator.SetBool("isJabRight", true);
            }
            else
            {
                animator.SetBool("isJabRight", false);
            }
		// Press Q to play dodge left
		if (Input.GetKey(KeyCode.Q))
		{
			animator.SetBool("isDodgeleft", true);
		}
		else
		{
			// if Q is not pressed, set the "isDodgeLeft" parameter to false
			animator.SetBool("isDodgeleft", false);
		}
		// Press E to play dodge right
		if (Input.GetKey(KeyCode.E))
		{
			animator.SetBool("isDodgeRight", true);
		}
		else
		{
			// if E is not pressed, set the "isDodgeRight" parameter to false
			animator.SetBool("isDodgeRight", false);
		}
		// Press LeftShift to play combo3
		if (Input.GetKey(KeyCode.LeftShift))
		{
			animator.SetBool("isCombo3", true);
		}
		else
		{
			// if LeftShift is not pressed, set the "isCombo3" parameter to false
			animator.SetBool("isCombo3", false);
		}
		// Press Z to play walk forward
		if (Input.GetKey(KeyCode.Z))
		{
			animator.SetBool("isWalkForward", true);
		}
		else
		{
			// if Z is not pressed, set the "isWalkForward" parameter to false
			animator.SetBool("isWalkForward", false);
		}

		// Press X to play combo1
		if (Input.GetKey(KeyCode.X))
		{
			animator.SetBool("isCombo1", true);
		}
		else
		{
			// if X is not pressed, set the "isCombo1" parameter to false
			animator.SetBool("isCombo1", false);
		}
		// Press LeftControl to play crouch
		if (Input.GetKey(KeyCode.LeftControl))
		{
			animator.SetBool("isCrouch", true);
		}
		else
		{
			// if LeftControl is not pressed, set the "isCrouch" parameter to false
			animator.SetBool("isCrouch", false);
		}
		// Press R to play bobbing
		if (Input.GetKey(KeyCode.R))
		{
			animator.SetBool("isBobbing", true);
		}
		else
		{
			// if R is not pressed, set the "isBobbing" parameter to false
			animator.SetBool("isBobbing", false);
		}

			
	}
}
}
