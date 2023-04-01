using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class RollerAgent : Agent
{
    Rigidbody rBody;
    public Transform target;
    private float forceMultiplier = 10f;
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        // If the Agent fell, zero its momentum; put back in the center
        if (transform.localPosition.y < 0)
        {
            rBody.angularVelocity = Vector3.zero;
            rBody.velocity = Vector3.zero;
            transform.localPosition = new Vector3(0, 0.5f, 0);
        }

        // Agent caught the target, or agent fall
        // Move the target to a new spot
        float x = Random.value * 8 - 4; //  => x is in [-4, 4)
        float z = Random.value * 8 - 4; // => z is in [-4, 4)

        target.localPosition = new Vector3(x, 0.5f, z);
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(target.localPosition);
        sensor.AddObservation(transform.localPosition);

        // Agent velocity
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);

        // 3+3+1+1 = 8 floats
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        //
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.z = actionBuffers.ContinuousActions[1];

        rBody.AddForce(controlSignal * forceMultiplier);
        //rBody.AddForce(controlSignal * forceMultiplier, ForceMode.);
        // 2nd Law of Newton: F=m.A  F/m

        // dv dt => A=dv/dt     db=v1-v0
        // I=m.V     di/dt = m.dv/dt = m.A

        float distanceToTarget = Vector3.Distance(transform.localPosition, target.localPosition);
        // Reached target
        if (distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            EndEpisode();
        }
        // Fell off platform
        else if (transform.localPosition.y < 0)
        {
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //base.Heuristic(actionsOut);
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }
}
