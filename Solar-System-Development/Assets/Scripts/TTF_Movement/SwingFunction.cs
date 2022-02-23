using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingFunction : MonoBehaviour
{
    SpringJoint joint;
    public float Rope_MaxDist;

    [Header("Grap Gun")]
    public Transform FPSCamera,Player,GunTip;
    public LayerMask whatIsGrappleable;
    Vector3 GrapplePoint;
    Vector3 currentGrapplePosition;

    [Header("Render")]
    LineRenderer RopeLR;
    void Awake()
    {
        RopeLR = GetComponent<LineRenderer>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        //Debug.DrawRay(FPSCamera.position, FPSCamera.forward, Color.green, 10000f);
        //right click to shoot
        if (Input.GetMouseButtonDown(0))
        {
            StartGrapple();
        }
        //left click to cancel
        else if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }
    }
    private void LateUpdate()
    {
        DrawRope();
    }

    void StartGrapple()
    {
        RaycastHit hit;
        if(Physics.Raycast(FPSCamera.position, FPSCamera.forward, out hit,Rope_MaxDist,whatIsGrappleable))
        {
            Debug.Log(hit.transform.name);

            GrapplePoint = hit.point;
            joint = Player.gameObject.AddComponent<SpringJoint>();
            //Should the connectedAnchor be calculated automatically?
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = GrapplePoint;

            float distanceFromPoint = Vector3.Distance(Player.position, GrapplePoint);
            
            //the dist grapple will try to keep from grapple point
            joint.maxDistance = distanceFromPoint*0.8f;
            joint.minDistance = distanceFromPoint*0.25f;

            //adjust these values to fit your game
            //The spring force used to keep the two objects together.
            joint.spring = 45f;
            //The damper force used to dampen the spring force.
            joint.damper = 7f;
            //The scale to apply to the inverse mass and inertia tensor of the body prior to solving the constraints.
            joint.massScale = 4.5f;


            //render the rope
            RopeLR.positionCount = 2;
            currentGrapplePosition = GunTip.position;

        }
    }
    void StopGrapple()
    {
        RopeLR.positionCount = 0;
        Destroy(joint);
    }

    void DrawRope()
    {
        if(!joint) return;
        currentGrapplePosition = Vector3.Lerp(GrapplePoint, currentGrapplePosition, Time.deltaTime*8f);

        RopeLR.SetPosition(0, GunTip.position);
        RopeLR.SetPosition(1, currentGrapplePosition);
    }
    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return GrapplePoint;
    }
}
