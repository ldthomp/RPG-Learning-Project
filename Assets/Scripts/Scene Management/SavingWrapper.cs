using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";

        [SerializeField] float fadeInTime = 2f;
        private void Awake()
        {
            StartCoroutine(LoadLastScene());
        }
        private IEnumerator LoadLastScene()
        {
            Fader fade = FindObjectOfType<Fader>();

            fade.FadeOutImmediately();
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);

            yield return fade.FadeIn(fadeInTime);
        }
        private void Update()
        {
            
            if(Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                DeleteSaveFile();
            }
        }


        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }
        public void DeleteSaveFile()
        {
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }
    }

}