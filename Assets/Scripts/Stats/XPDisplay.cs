using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class XPDisplay : MonoBehaviour
    {
        Experience experience;

        private void Awake()
        {
            experience = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();

        }
        private void Update()
        {
            //GetComponent<Text>().text = experience.GetExperiencePoints().ToString();
            GetComponent<Text>().text = String.Format("{0:0}", experience.GetExperiencePoints());
        }
    }
}