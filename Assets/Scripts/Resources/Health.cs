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
        [SerializeField] float healthPoints= -1f;

        bool isDead = false;

        private void Start()
        {
            if(healthPoints <0 )
            {
                healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
            
        }
        public bool IsDead()
        {
            return isDead;
        }


        public void TakeDamage (GameObject instigator, float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            print("Health =" + healthPoints);


            if (healthPoints == 0)
            {
                AwardExperience(instigator);
                Death();
            }
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        public float GetPercentage()
        {
            //percentage
            return 100 * (healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health));
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
