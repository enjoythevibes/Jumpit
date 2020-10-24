namespace enjoythevibes.Managers
{
    public enum Events
    {
        GameInitialization,
        LoadPlayerData,
        SavePlayerData,

        PlayGame,
        RestartGame,
        GameOver,
        BackToMenu,
    
        AddScore,
        AddExtraScore,
        AddCrystal,
        PlayNote,
        GenerateNextPlatform,

        OpenSettings,
        CloseSettings,

        EnableAudio,
        DisableAudio,

        OpenShop,
        CloseShop,

        TriggerObject
    }
}