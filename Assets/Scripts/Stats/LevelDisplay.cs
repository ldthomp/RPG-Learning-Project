﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {  
        BaseStats level;

        private void Awake()
        {
            level = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();

        }
        private void Update()
        {
            GetComponent<Text>().text = string.Format("{0}", level.CalculateLevel());
        }
    
    }

}