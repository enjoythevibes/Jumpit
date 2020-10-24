#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace enjoythevibes.Build
{
    public static class BuildSettings
    {
        public const string CameraAndroid = "Main Camera_Android";
        public const string CameraPC = "Main Camera_PC";
        public const string PostEffectsAndroid = "Post Processinbg Volume_Android";
        public const string PostEffectsPC = "Post Processinbg Volume_PC";

        public const string AndroidOnly = "AndroidOnly";
        public const string PCOnly = "PCOnly";

        private static GameObject FindGameObjectByName(string name)
        {
            var gameObjects = Resources.FindObjectsOfTypeAll<Transform>();
            foreach (var item in gameObjects)
            {
                if (item.name == name)
                    return item.gameObject;
            }
            return null;
        }

        private static List<GameObject> FindGameObjectsWithTag(this List<GameObject> gameObjectsWithTag, string tag)
        {
            var gameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (var item in gameObjects)
            {
                if (item.tag == tag)
                    gameObjectsWithTag.Add(item);
            }
            return gameObjectsWithTag;
        }

        public class PreBuild : IProcessSceneWithReport
        {
            public int callbackOrder => 0;

            public void OnProcessScene(Scene scene, BuildReport report)
            {
                PlatformOnlyObjects();
            }

            private void PlatformOnlyObjects()
            {
                var objectsWithTag = new List<GameObject>();
                #if !UNITY_STANDALONE
                objectsWithTag = objectsWithTag.FindGameObjectsWithTag(PCOnly);
                #endif
                #if !UNITY_ANDROID
                objectsWithTag = objectsWithTag.FindGameObjectsWithTag(AndroidOnly);
                #endif
                foreach (var gameObject in objectsWithTag)
                {
                    MonoBehaviour.DestroyImmediate(gameObject);
                }
            }
        }

        public class BuildTargetChangeListener : IActiveBuildTargetChanged
        {
            public int callbackOrder => 0;

            public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
            {
                SetSettings(newTarget);
            }
        }

        private static void SetSettings(BuildTarget buildTarget)
        {
            var cameraAndroid = FindGameObjectByName(CameraAndroid);
            var postEffectsAndroid = FindGameObjectByName(PostEffectsAndroid);

            var cameraPC = FindGameObjectByName(CameraPC);
            var postEffectsPC = FindGameObjectByName(PostEffectsPC);

            if (buildTarget == BuildTarget.Android)
            {
                cameraPC.SetActive(false);
                postEffectsPC.SetActive(false);
                cameraAndroid.SetActive(true);
                postEffectsAndroid.SetActive(true);
                QualitySettings.SetQualityLevel(1);
            }
            else
            if (buildTarget == BuildTarget.StandaloneWindows || buildTarget == BuildTarget.StandaloneWindows64)
            {
                cameraAndroid.SetActive(false);
                postEffectsAndroid.SetActive(false);
                cameraPC.SetActive(true);
                postEffectsPC.SetActive(true);
                QualitySettings.SetQualityLevel(0);
            }
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }

        [MenuItem("Engine/Set Android")]
        private static void SetAndroidSettings()
        {
            SetSettings(BuildTarget.Android);
        }

        [MenuItem("Engine/Set PC")]
        private static void SetPCSettings()
        {
            SetSettings(BuildTarget.StandaloneWindows);
        }
    }
}
#endif