using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        bool cinematicTriggered = false;
        private void OnTriggerEnter(Collider player)
        {
            if(!cinematicTriggered && player.gameObject.tag == "Player")
            { 
                GetComponent<PlayableDirector>().Play();
                cinematicTriggered = true;
            }
        }
    }
}
