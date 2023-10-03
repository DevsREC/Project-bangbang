using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class AimLine : NetworkBehaviour
{
    private LineRenderer lineRenderer;
    private VariableJoystick joystick;
    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 endDir;
    [SerializeField] private float lineMax=20;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        lineRenderer = GetComponent<LineRenderer>();
        joystick = FindObjectOfType<ShootButton>().GetComponent<VariableJoystick>();
    }

    private void Update()
    {
        if (!IsOwner) return;
        AimLineRenderer();
    }
    private void AimLineRenderer()
    {
        lineRenderer.enabled = true;
        startPos = transform.position;
        startPos.z = 0;
        lineRenderer.SetPosition(0, startPos); //First point of the line's position as player's position
        if (joystick.Vertical != 0 || joystick.Horizontal != 0)
        {
            endDir = new Vector3(joystick.Horizontal, joystick.Vertical, 0f);
        }       
        endDir.Normalize();
        endPos = startPos +  endDir * lineMax; //Second points positions moves according to player's position
        lineRenderer.SetPosition(1, endPos);
    }
}
