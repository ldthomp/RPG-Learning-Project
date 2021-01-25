using RPG.core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
    {
        Transform target;
        MovePlayer movePlayer;
        [SerializeField] float weaponRange = 2f;

        NavMeshAgent navMeshAgent;

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            movePlayer = GetComponent<MovePlayer>();
        }
        public void Attack(CombatTarget combatTarget)
        {
            target = combatTarget.transform;
            GetComponent<ActionScheduler>().StartAction(this);
            //move to target
            //stop within range
            //check distance 
            //check it is moving away
        }
        void Update()
        {
            if (target == null) return;
            if (!GetIsInRange())
            {
                movePlayer.MoveTo(target.position);
            }
            else
            {
                movePlayer.Stop(); //stops Navmesh
            }
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.position) < weaponRange;
        }

        public void Cancel()
        {
            target = null;
        }
    }
}