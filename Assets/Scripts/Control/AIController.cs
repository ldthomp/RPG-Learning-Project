using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 2f;
        [SerializeField] bool gizmosOn = true;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;

        GameObject player;

        Fighter fighter;

        Health health;

        Vector3 guardPosition;

        Mover movePlayer;

        float timeSinceLastSawPlayer = Mathf.Infinity;

        int currentWaypointIndex = 0;

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            health = GetComponent<Health>();
            fighter = GetComponent<Fighter>();
            movePlayer = GetComponent<Mover>();

            guardPosition = transform.position;
        }

        private bool InAttackRangeOfPlayer()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return distanceToPlayer <= chaseDistance;
        }
        private void Update()
        {
            if (health.IsDead()) return;
            if (InAttackRangeOfPlayer()  && fighter.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0f;
                AttackBehaviour();
            }
            else if(timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }
            timeSinceLastSawPlayer += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;
            if(patrolPath != null)
            {
                if (AtWaypoint())
                {
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            movePlayer.StartMoveAction(nextPosition);
        }

        private bool AtWaypoint()
        {
            //return if at a waypoint
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void CycleWaypoint()
        {
            //select next waypoint to move to
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurentAction();
        }

        private void AttackBehaviour()
        {
            fighter.Attack(player);
        }

        // called by unity editor
        private void OnDrawGizmos()
        {
            if(gizmosOn)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, chaseDistance);
            }
        }
    }
}
