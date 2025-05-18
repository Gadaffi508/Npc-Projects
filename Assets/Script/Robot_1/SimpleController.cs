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

    public void StopAgent()
    {
        agent.isStopped = true;
    }

    public void ContinueAgent()
    {
        agent.isStopped = false;
    }
}
