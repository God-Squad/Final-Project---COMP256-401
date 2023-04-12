using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class FlappyAgent : Agent
{
    private Rigidbody agentRigidbody;
    public GameObject obstacles;
    public LevelGenerator levelGenerator;
    public GameObject[] walls;

    private void Awake()
    {
        agentRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (obstacles.transform.position.z < -29f)
        {
            Debug.Log(GetCumulativeReward());
            EndEpisode();
        }
    }

    public override void OnEpisodeBegin()
    {
        obstacles.transform.position = Vector3.zero;
        transform.position = new Vector3(0, 5, 0);
        levelGenerator.GenerateObstacles();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(agentRigidbody.velocity.y);
        for (int i = 0; i < obstacles.transform.childCount; i++)
        {
            if (obstacles.transform.GetChild(i).gameObject.tag == "Gap")
            {
                sensor.AddObservation(obstacles.transform.GetChild(i).localPosition.y);
                sensor.AddObservation(Vector3.Distance(obstacles.transform.GetChild(i).localPosition, transform.localPosition));
            }
        }
        foreach (GameObject wall in walls)
        {
            sensor.AddObservation(wall.transform.localPosition);
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
        if (collision.gameObject.tag == "Obstacle" || collision.gameObject.tag == "Wall")
        {
            if (GetCumulativeReward() > 0)
                Debug.Log(GetCumulativeReward());
            EndEpisode();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Gap")
        {
            AddReward(1f);
            EndEpisode();
        }
    }
}

