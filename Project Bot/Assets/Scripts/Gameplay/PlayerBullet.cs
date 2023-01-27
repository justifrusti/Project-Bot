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

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("FuseBox"))
        {
            FuseExploder exploder = collision.gameObject.GetComponent<FuseExploder>();

            exploder.ExplosiveForce();

            if (exploder.GetComponent<SwitchComponent>().playCinematic)
            {
                collision.gameObject.GetComponent<SwitchComponent>().CinematicCutscene();
            }

            collision.gameObject.GetComponent<SwitchComponent>().currentAction = SwitchComponent.Action.Disable;
        }

        if(collision.gameObject.CompareTag("Roomba") || collision.gameObject.CompareTag("Turret"))
        {
            playerController.DoDamage(playerController.damageToApply, collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Overload"))
        {
            if (playerController.unlockedHacking)
            {
                playerController.currentInitialized = collision.gameObject.GetComponent<OverloadInitialize>();
                collision.gameObject.GetComponent<OverloadInitialize>().LaunchMinigame();
            }
        }

        if (collision.gameObject.CompareTag("TimeSwitch"))
        {
            collision.gameObject.GetComponent<SwitchManager>().switchActive = true;
        }

        /*Destroy(this.gameObject);*/
    }
}
