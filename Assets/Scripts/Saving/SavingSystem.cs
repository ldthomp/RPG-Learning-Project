using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public void Save(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            print("saving to " + path);
            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                byte[] bytes = Encoding.UTF8.GetBytes("¡Hola Mundo!");
                stream.Write(bytes, 0, bytes.Length);
            }
                        
        }
        public void Load(string saveFile)
        {

            string path = GetPathFromSaveFile(saveFile);
            print("Loading to " + path);

            using(FileStream stream = File.Open(path, FileMode.Open))
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                string message = Encoding.UTF8.GetString(buffer);
                print(message);
            }

        }
        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}