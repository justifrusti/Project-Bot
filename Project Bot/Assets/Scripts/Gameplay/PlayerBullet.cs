using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public ThirdPersonPlayerController playerController;

    public Rigidbody rb;

    public void AssignPlayer(ThirdPersonPlayerController playerController)
    {
        this.playerController = playerController;
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.velocity = Vector3.zero;

        playerController.DoDamage(playerController.damage, collision);

        if(collision.gameObject.CompareTag("FuseBox"))
        {
            FuseExploder exploder = collision.gameObject.GetComponent<FuseExploder>();

            exploder.ExplosiveForce();

            if (exploder.GetComponent<SwitchComponent>().playCinematic)
            {
                collision.gameObject.GetComponent<SwitchComponent>().CinematicCutscene();
            }

            collision.gameObject.GetComponent<SwitchComponent>().currentAction = SwitchComponent.Action.Disable;
        }

        Destroy(this.gameObject);
    }
}
