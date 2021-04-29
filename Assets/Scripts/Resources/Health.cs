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
        [SerializeField] float regenerationPercentage= 70f;

        bool isDead = false;
        BaseStats baseStats;

        private void Start()
        {
            if(healthPoints < 0 )
            {
                healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
            baseStats = GetComponent<BaseStats>();
            baseStats.onLevelUp += LevelUp;
        }
        public bool IsDead()
        {
            return isDead;
        }
        public float GetPercentage()
        {
            //percentage
            return 100 * (healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health));
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
        public void TakeDamage (GameObject instigator, float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            print(gameObject.name + " took damage " + damage);


            if (healthPoints == 0)
            {
                AwardExperience(instigator);
                Death();
            }
        }
        public float GetHealthPoints()
        {
            return healthPoints;
        }
        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }
        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }
        private void LevelUp()
        {
            float regenerateHealthPoints = baseStats.GetStat(Stat.Health) * (regenerationPercentage / 100);
            healthPoints = Mathf.Max(healthPoints,regenerateHealthPoints);
        }

        private void Death()
        {
            if (isDead) { return; }
            isDead = true;
            GetComponent<Animator>().SetTrigger("Die");
            GetComponent<ActionScheduler>().CancelCurentAction();
        }

    }
}
