using enjoythevibes.Managers;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 0414

namespace enjoythevibes.UI
{
    public class SettingsUI : MonoBehaviour
    {
        [SerializeField]
        private Slider sensitivitySlider = default;
        private float sensitivityValue;

        [SerializeField]
        private GameObject soundsButtonOn = default;
        [SerializeField]
        private GameObject soundsButtonOff = default;

        #if !UNITY_EDITOR && UNITY_STANDALONE
        private int minResolutionHeight = 300;
        private int maxResolutionHeight;
        #endif

        [SerializeField]
        private GameObject resolutionLabel = default;
        [SerializeField]
        private Slider resolutionSlider = default;
        private int resolutionValue;

        private void Awake()
        {
            EventsManager.AddListener(Events.GameInitialization, OnLoadSettingsData);
            EventsManager.AddListener(Events.OpenSettings, OnEnableSettingsUI);
            EventsManager.AddListener(Events.OpenSettings, OnLoadSettingsData);
            EventsManager.AddListener(Events.CloseSettings, OnDisableSettingsUI);

            #if UNITY_ANDROID
            resolutionLabel.SetActive(false);
            resolutionSlider.gameObject.SetActive(false);
            #endif

            #if !UNITY_EDITOR && UNITY_STANDALONE
            var currentHeightResolution = Screen.currentResolution.height;
            maxResolutionHeight = currentHeightResolution - 100;
            resolutionSlider.minValue = minResolutionHeight;
            resolutionSlider.maxValue = maxResolutionHeight;
            #endif

            OnDisableSettingsUI();
        }

        private void OnEnableSettingsUI()
        {
            gameObject.SetActive(true);
        }

        private void OnDisableSettingsUI()
        {
            gameObject.SetActive(false);
        }

        public void OnSensitivityValueChanged(Slider slider)
        {
            sensitivityValue = slider.value;
        }

        public void OnEnableAudioButton()
        {
            soundsButtonOff.SetActive(false);
            soundsButtonOn.SetActive(true);
            EventsManager.CallEvent(Events.EnableAudio);
            Data.DataSaver.playerData.audioEnabled = true;
        }

        public void OnDisableAudioButton()
        {
            soundsButtonOn.SetActive(false);
            soundsButtonOff.SetActive(true);
            EventsManager.CallEvent(Events.DisableAudio);
            Data.DataSaver.playerData.audioEnabled = false;
        }

        private void OnLoadSettingsData()
        {
            var playerData = Data.DataSaver.playerData;
            sensitivitySlider.value = playerData.sensitivity;
            sensitivityValue = playerData.sensitivity;
            #if !UNITY_EDITOR && UNITY_STANDALONE
            resolutionSlider.value = playerData.resolutionHeight;
            resolutionValue = playerData.resolutionHeight;
            if (Screen.height != resolutionValue)
            {
                SetResolution(resolutionValue);
            }
            #endif
            if (playerData.audioEnabled)
            {
                soundsButtonOn.SetActive(true);                
                soundsButtonOff.SetActive(false);
            }
            else
            {
                soundsButtonOff.SetActive(true);
                soundsButtonOn.SetActive(false);
            }
        }

        public void OnBackToMenuButton()
        {
            var playerData = Data.DataSaver.playerData;
            playerData.sensitivity = sensitivityValue;
            #if !UNITY_EDITOR && UNITY_STANDALONE
            playerData.resolutionHeight = resolutionValue;
            if (Screen.height != resolutionValue)
            {
                SetResolution(resolutionValue);
            }
            #endif
            EventsManager.CallEvent(Events.SavePlayerData);
            EventsManager.CallEvent(Events.CloseSettings);
        }

        public void OnSetDefaultsButton()
        {
            var playerData = Data.DataSaver.playerData;
            playerData.SetDefault();
            OnLoadSettingsData();            
        }

        public void OnResolutionValueChanged(Slider slider)
        {
            resolutionValue = (int)slider.value;
        }

        #if !UNITY_EDITOR && UNITY_STANDALONE
        private void SetResolution(int height)
        {
            var ratio = 9f/18f;
            var resolutionToSet = new Vector2Int(Mathf.RoundToInt(ratio * height), height);
            Screen.SetResolution(resolutionToSet.x, resolutionToSet.y, FullScreenMode.Windowed);
        }
        #endif
    }
}