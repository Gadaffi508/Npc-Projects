using UnityEngine;
using UnityEngine.AI;

public class SimpleController : MonoBehaviour
{
    public Transform movePos;

    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.SetDestination(movePos.position);
    }
}
