using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIMoveTo : MonoBehaviour
{
	// Положение точки назначения
    public Transform goal;
    private NavMeshAgent _agent;
    [SerializeField] float destinationReachedTreshold = 5;
    private void Start()
    {
        // Получение компонента агента
        _agent = GetComponent<NavMeshAgent>();
        // Указание точки назначения
    }

    public void ChangeGoal(Transform newGoal)
    {
        if((goal != null) && (newGoal != null))
        {
            goal = newGoal;
            _agent.SetDestination(goal.position);
        }
    }

    public bool CheckDestinationReached()
    {
        if (goal != null)
        {
            return true;
        }
        float distanceToTarget = Vector3.Distance(transform.position, goal.position);
        //Debug.Log(transform.position + ", " + goal.position);
        if (distanceToTarget <= destinationReachedTreshold)
        {
            //print("Destination reached");

            return true;
        }

        return false;
    }
}
