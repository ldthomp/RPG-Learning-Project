﻿using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Resources;
using UnityEngine.AI;
using RPG.Stats;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction , ISaveable, IModifierProvider
    {
        Health target;
        Mover movePlayer;

        [SerializeField] float timeBetweenAttacks = 1f;

        [SerializeField] Weapon defaultWeapon = null;

        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;

        Weapon currentWeapon = null;

        float timeSinceLastAttack = Mathf.Infinity;

        private void Start()
        {
            if(currentWeapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
            movePlayer = GetComponent<Mover>();
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
        public void EquipWeapon(Weapon weapon)
        {
            if (weapon == null) return;
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }
        public Health GetTarget()
        {
            return target;
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
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            }
            else
            {
                target.TakeDamage(gameObject, damage);
            }

            print("Enemy Taking a Hit");
        }

        void Shoot()
        {
            Hit();
        }    

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetRange();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
            movePlayer.Cancel();
        }
        public IEnumerable<float> GetAdditiveModifier(Stat stat)
        {
            if(stat == Stat.Damage)
            {
                yield return currentWeapon.GetDamage();
            }
        }
        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("Attack");
            GetComponent<Animator>().SetTrigger("stopAttacking");
            Debug.Log("setting stop attacking trigger of " + gameObject.name);
        }

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = UnityEngine.Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }


    }
}