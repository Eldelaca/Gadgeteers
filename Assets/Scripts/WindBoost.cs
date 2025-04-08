using UnityEngine;

public class WindBoost : MonoBehaviour
{
    public CharacterController playerController;
    public float boostedJumpSpeed = 20f;

    private float originalJumpSpeed;

    private void Start()
    {
        if (playerController != null)
        {
            originalJumpSpeed = playerController.jumpSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == playerController.gameObject)
        {
            playerController.jumpSpeed = boostedJumpSpeed;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == playerController.gameObject)
        {
            playerController.jumpSpeed = originalJumpSpeed;
        }
    }
}
