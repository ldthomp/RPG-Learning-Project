using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
using UnityEngine;

namespace RPG.Resources

{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenerationPercentage= 70f;
        LazyValue<float> healthPoints;

        bool isDead = false;
        BaseStats baseStats;

        private void Awake()
        {
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }
        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }
        private void Start()
        {
            healthPoints.ForceInit();
        }
        private void OnEnable()
        {
            baseStats = GetComponent<BaseStats>();
            baseStats.onLevelUp += LevelUp;
        }

        private void OnDisable()
        {
            baseStats = GetComponent<BaseStats>();
            baseStats.onLevelUp -= LevelUp;
        }
        public bool IsDead()
        {
            return isDead;
        }
        public float GetPercentage()
        {
            //percentage
            return 100 * (healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health));
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;
            if (healthPoints.value <= 0)
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
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
            print(gameObject.name + " took damage " + damage);


            if (healthPoints.value == 0)
            {
                AwardExperience(instigator);
                Death();
            }
        }
        public float GetHealthPoints()
        {
            return healthPoints.value;
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
            healthPoints.value = Mathf.Max(healthPoints.value, regenerateHealthPoints);
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
