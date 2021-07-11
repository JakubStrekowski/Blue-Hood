using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrolling : MonoBehaviour
{
    private readonly float TARGET_REACH_DISTANCE = 0.5f; // how close to the target enemy has to be to reach waypoint

    [SerializeField]
    Transform[] patrolWaypoints;


    int _targetWaypointId = 0;


    public bool isPatrollingActive()
    {
        return patrolWaypoints.Length > 0 ? true : false;
    }

    public Vector2 GetNextWaypoint()
    {
        return patrolWaypoints[_targetWaypointId].position;
    }

    public void GoToNextWaypoint()
    {
        _targetWaypointId++;
        _targetWaypointId = _targetWaypointId % patrolWaypoints.Length;
    }

    public bool isWaypointReached()
    {
        return ((Vector2)transform.position - (Vector2)patrolWaypoints[_targetWaypointId].position).magnitude < TARGET_REACH_DISTANCE ? true : false;
    }
}
