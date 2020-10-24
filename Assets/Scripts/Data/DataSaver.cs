using System.IO;
using UnityEngine;

namespace enjoythevibes.Data
{
    public static class DataSaver
    {
        public static PlayerData playerData { private set; get; }

        static DataSaver()
        {
            playerData = new PlayerData();
        }

        private static string GetPath()
        {
            var finalPath = default(string);
            #if !UNITY_EDITOR && UNITY_STANDALONE
            finalPath = Application.dataPath + "/data.enj";
            #endif
            #if UNITY_ANDROID
            finalPath = Application.persistentDataPath + "/data.enj";
            #endif
            #if UNITY_EDITOR
            finalPath = "data.enj";
            #endif
            return finalPath;
        }

        public static void SaveData()
        {
            File.WriteAllBytes(GetPath(), playerData.GetBytesData());
        }

        public static void LoadData()
        {
            var path = GetPath();
            if(File.Exists(path))
            {
                var bytes = File.ReadAllBytes(path);                
                playerData.ReadFromBytes(bytes);
            }
            else
            {
                playerData.SetEmpty();
                SaveData();
            }
        }
    }
}