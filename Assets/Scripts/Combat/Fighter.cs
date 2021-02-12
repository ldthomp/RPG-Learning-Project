using RPG.Core;
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
        Mover movePlayer;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float weaponDamage = 10f;

        float timeSinceLastAttack = Mathf.Infinity;

        private void Start()
        {
            movePlayer = GetComponent<Mover>();
        }
        public void Attack(GameObject combatTarget)
        {
            target = combatTarget.transform.GetComponent<Health>();
            GetComponent<ActionScheduler>().StartAction(this);
            //move to target
            //stop within range
            //check distance 
            //check it is moving away
        }
        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }
        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (target == null) return;
            if (target.IsDead()) return;

            if (!GetIsInRange())
            {
                movePlayer.MoveTo(target.transform.position, 1f);
            }
            else
            {
                Debug.Log(gameObject.name + " mover is being cancelled");
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
            Debug.Log("resetting stop attacking trigger of " + gameObject.name);
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
            movePlayer.Cancel();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("Attack");
            GetComponent<Animator>().SetTrigger("stopAttacking");
            Debug.Log("setting stop attacking trigger of " + gameObject.name);
        }
    }
}