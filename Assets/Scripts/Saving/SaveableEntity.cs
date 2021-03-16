using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";
        public string GetUniqueIdentifier()
        {
            return " ";
        }
        public object CaptureState()
        {
            print("Capturing state" + GetUniqueIdentifier());
            return null;
        }
        public void RestoreStates(object state)
        {
            print("restoring state for " + GetUniqueIdentifier());
        }

        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;
            
            print("Editing");
            
        }
    }
}
