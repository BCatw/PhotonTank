using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TankPlayer : MonoBehaviour
{
    private Complete.TankMovement movement;
    private Complete.TankShooting shooting;
    private PhotonView photonView;

    private void Awake()
    {
        movement = GetComponent<Complete.TankMovement>();
        shooting = GetComponent<Complete.TankShooting>();
        photonView = GetComponent<PhotonView>();

        if (!photonView.IsMine)
        {
            movement.enabled = false;
        }
    }
}
