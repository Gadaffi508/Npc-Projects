using UnityEngine;
using UnityEngine.AI;

public class RobotController : MonoBehaviour
{
    public Transform examplePos;

    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.SetDestination(examplePos.position);
    }
}
