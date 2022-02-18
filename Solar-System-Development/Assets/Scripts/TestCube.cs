using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCube : MonoBehaviour
{
    // Start is called before the first frame update

    Rigidbody rb;
    CelestialBody referenceBody;
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        #region Gravity
        //Gravity
        CelestialBody[] bodies = NBodySimulation.Bodies;
        Vector3 gravityOfNearestBody = Vector3.zero;
        float nearestSurfaceDst = float.MaxValue;

        foreach (CelestialBody body in bodies)
        {
            float sqrDst = (body.Position - rb.position).sqrMagnitude;
            Vector3 forceDir = (body.Position - rb.position).normalized;
            Vector3 acceleration = forceDir * Universe.gravitationalConstant * body.mass / sqrDst;
            rb.AddForce(acceleration, ForceMode.Acceleration);

            float dstToSurface = Mathf.Sqrt(sqrDst) - body.radius;

            // Find body with strongest gravitational pull 
            if (dstToSurface < nearestSurfaceDst)
            {
                nearestSurfaceDst = dstToSurface;
                gravityOfNearestBody = acceleration;
                referenceBody = body;
                Debug.Log(body.gameObject.name);
            }


        }

        // Rotate to align with gravity up
        Vector3 gravityUp = -gravityOfNearestBody.normalized;

        Debug.DrawLine(this.gameObject.transform.position, gravityUp, Color.red, 10f);
        //Debug.DrawLine(transform.up, rb.position, Color.green, 10f);
        Debug.Log(gravityUp);
        //groundNormal = gravityUp;
        rb.rotation = Quaternion.FromToRotation(transform.up, gravityUp) * rb.rotation;
        // Move
        //rb.MovePosition(rb.position + Direction() * Time.fixedDeltaTime);
        Debug.Log(rb.rotation);
        #endregion
    }
}
