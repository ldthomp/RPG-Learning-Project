using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Resources

{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthPoints;

        bool isDead = false;

        private void Start()
        {
            //know bug that will cause issues with enemies that get killed coming back to life. Start can get called after save file is loaded so the health is reset
            healthPoints = GetComponent<BaseStats>().GetHealth();
        }
        public bool IsDead()
        {
            return isDead;
        }


        public void TakeDamage (float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            print("Health =" + healthPoints);
            if (healthPoints == 0)
            {
                Death();
            }
        }
        public float GetPercentage()
        {
            //percentage
            return 100 * (healthPoints / GetComponent<BaseStats>().GetHealth());
        }

        private void Death()
        {
            if (isDead) { return; }
            isDead = true;
            GetComponent<Animator>().SetTrigger("Die");
            GetComponent<ActionScheduler>().CancelCurentAction();
        }
        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;
            if (healthPoints <= 0)
            {
                Death();
            }
            else
            {
                GetComponent<Animator>().Rebind();
            }
        }
    }
}
