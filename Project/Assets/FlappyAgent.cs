using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class FlappyAgent : Agent
{
    private Rigidbody agentRigidbody;
    public int rewards;
    public GameObject obstacles;

    private void Awake()
    {
        agentRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (obstacles.transform.position.z < -55f)
            EndEpisode();
    }

    public override void OnEpisodeBegin()
    {
        rewards = 0;
        obstacles.transform.position = Vector3.zero;
        transform.position = new Vector3(0, 5, 0);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.localPosition);
        //agent vel
        sensor.AddObservation(agentRigidbody.velocity.y);
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacle in obstacles)
        {
            sensor.AddObservation(obstacle.transform);
        }

        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach (GameObject wall in walls)
        {
            sensor.AddObservation(wall.transform);
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (actions.DiscreteActions[0] == 1)
        {
            agentRigidbody.velocity = Vector3.zero;
            float jumpHeight = 5f;
            agentRigidbody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            discreteActions[0] = 1;
        }
        else
        {
            discreteActions[0] = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Obstacle")
        {
            if (rewards > 0)
                Debug.Log(rewards);
            EndEpisode();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Gap")
        {
            AddReward(1f);
            rewards++;
        }
    }
}

