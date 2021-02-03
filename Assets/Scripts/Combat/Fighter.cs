using RPG.core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        Health target;
        MovePlayer movePlayer;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float weaponDamage = 10f;

        NavMeshAgent navMeshAgent;

        float timeSinceLastAttack = 0f;

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            movePlayer = GetComponent<MovePlayer>();
        }
        public void Attack(CombatTarget combatTarget)
        {
            target = combatTarget.transform.GetComponent<Health>();
            GetComponent<ActionScheduler>().StartAction(this);
            //move to target
            //stop within range
            //check distance 
            //check it is moving away
        }
        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (target == null) return;
            if (target.IsDead()) return;

            if (!GetIsInRange())
            {
                movePlayer.MoveTo(target.transform.position);
            }
            else
            {
                movePlayer.Cancel(); //stops Navmesh
                AttackBehaviour(); 
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                //this will trigger Hit() event
                TriggerAttack();
                timeSinceLastAttack = 0f;
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttacking");
            GetComponent<Animator>().SetTrigger("Attack");
        }

        //Animation Event on Attack
        void Hit()
        {
            if (target == null) return;
            target.TakeDamage(weaponDamage);
            print("Enemy Taking a Hit");
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("Attack");
            GetComponent<Animator>().SetTrigger("stopAttacking");
        }

        public bool CanAttack(CombatTarget combatTarget)
        {
            if (combatTarget == null) { return false; }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }   
    }
}