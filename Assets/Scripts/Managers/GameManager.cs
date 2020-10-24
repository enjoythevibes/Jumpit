using enjoythevibes.Data;
using UnityEngine;

namespace enjoythevibes.Managers
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance { set; get; }
        [SerializeField]
        private GradientSchemeScriptableObject[] gradientSchemes = default;
        public static GradientSchemeScriptableObject[] GradientSchemes => instance.gradientSchemes;

        public static float TimeScale { set; get; } = 1f;

        #if UNITY_EDITOR
        private Data.GradientSchemeScriptableObject colorScheme;
        [Header("Debug")]
        public bool debugEditMode = true;

        private void FixedUpdate() 
        {
            if (!debugEditMode) return;
            if (colorScheme)
                SetColorScehem(colorScheme);    
        }
        #endif

        private void Awake() 
        {
            instance = this;
            EventsManager.AddListener(Events.GameInitialization, OnLoadPlayerData);
            EventsManager.AddListener(Events.LoadPlayerData, OnLoadPlayerData);
            EventsManager.AddListener(Events.SavePlayerData, OnSavePlayerData);
        }

        private void Start()
        {
            EventsManager.CallEvent(Events.GameInitialization);
            #if (UNITY_ANDROID && !UNITY_EDITOR)
            Application.targetFrameRate = 65;
            if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2)
            {
                QualitySettings.SetQualityLevel(2);
            }
            #endif
        }
        
        private void OnLoadPlayerData()
        {
            #if UNITY_EDITOR
            Debug.Log("Load Player Data");
            #endif
            Data.DataSaver.LoadData();
        }

        private void OnSavePlayerData()
        {
            #if UNITY_EDITOR
            Debug.Log("Save Player Data");
            #endif
            Data.DataSaver.SaveData();
        }

        public static void SetColorScehem(GradientSchemeScriptableObject colorScheme)
        {
            RenderSettings.ambientSkyColor = colorScheme.SkyColor;
            RenderSettings.ambientEquatorColor = colorScheme.EquatorColor;
            RenderSettings.ambientGroundColor = colorScheme.GroundColor;
            RenderSettings.fogColor = colorScheme.FogColor;
            var skybox = new Material(RenderSettings.skybox);
            skybox.SetColor("_Color1", colorScheme.SkyboxColor1);
            skybox.SetColor("_Color2", colorScheme.SkyboxColor2);
            RenderSettings.skybox = skybox;
            DynamicGI.UpdateEnvironment();
            #if UNITY_EDITOR
            instance.colorScheme = colorScheme;
            #endif
        }
    }
}