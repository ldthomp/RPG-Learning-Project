using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] int sceneToLoad = 1;
        private void Start()
        {
            print(SceneManager.GetActiveScene());
        }
        private void OnTriggerEnter(Collider other)
        {
            //old code used to load scenes (project sub)
            //int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            //int nextSceneIndex = currentSceneIndex + 1;
            //if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
            //{
            //    nextSceneIndex = 0;
            //}
            if (other.tag == "Player")
            {

                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}
