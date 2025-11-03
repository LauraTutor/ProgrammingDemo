using UnityEngine;

public class SandTrigger : MonoBehaviour
{
    private FirstPersonController firstPersonController;

    private float originalWalkSpeed;
    private float originalSprintMultiplier;
    private float originalJumpForce;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Get the player's controller
            firstPersonController = other.GetComponent<FirstPersonController>();
            if (firstPersonController != null)
            {
                // Store original values
                originalWalkSpeed = firstPersonController.walkSpeed;
                originalSprintMultiplier = firstPersonController.sprintMultiplier;
                originalJumpForce = firstPersonController.jumpForce;

                // Set sand values
                firstPersonController.walkSpeed = 2.0f;
                firstPersonController.sprintMultiplier = 2.0f;
                firstPersonController.jumpForce = 3.0f;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && firstPersonController != null)
        {
            // Restore original values
            firstPersonController.walkSpeed = originalWalkSpeed;
            firstPersonController.sprintMultiplier = originalSprintMultiplier;
            firstPersonController.jumpForce = originalJumpForce;
        }
    }
}