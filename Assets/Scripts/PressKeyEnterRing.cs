using UnityEngine;
using UnityEngine.SceneManagement;

public class PressKeyEnterRing : MonoBehaviour
{
    public GameObject Instruction;
    public bool Action = false;

    void Start()
    {
        // Ensure the instruction is hidden at the start
        Instruction.SetActive(false);
    }

    void OnTriggerEnter(Collider collision)
    {
        // Check if the player enters the trigger
        if (collision.CompareTag("Player"))
        {
            Instruction.SetActive(true);
            Action = true;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        // Check if the player exits the trigger
        if (collision.CompareTag("Player"))
        {
            Instruction.SetActive(false);
            Action = false;
        }
    }

    void Update()
    {
        // Check if the "E" key is pressed and the action is allowed
        if (Input.GetKeyDown(KeyCode.E) && Action)
        {
            Instruction.SetActive(false);
            Action = false;

            // Load the boxing ring scene
            SceneManager.LoadScene("BoxingRing");
        }
    }
}




