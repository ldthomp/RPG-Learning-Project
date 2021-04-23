using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;

        private void Update()
        {
            if(gameObject.tag == "Player")
            {
                print(GetLevel());
            }

        }
        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }    
        public int GetLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null) return startingLevel;
            float currentXP = experience.GetExperiencePoints();

            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            for (int level = 1; level <= penultimateLevel; level++)
            {
                float XPtoLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if(XPtoLevelUp > currentXP)
                {
                    return level;
                }
            }
            return penultimateLevel + 1;
        }

    }
}