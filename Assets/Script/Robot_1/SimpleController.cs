using UnityEngine;
using UnityEngine.AI;

public class SimpleController : MonoBehaviour
{
    public Transform randomPos;

    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.SetDestination(randomPos.position);
    }


}
