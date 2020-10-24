using UnityEngine;

namespace enjoythevibes
{
    public static class EngineSettings
    {
        #region Platforms
        public static class Platforms
        {
            public const float SpawnEachZ = 5f;
            public const int SpawnRowsCount = 15;

            public const string PlatformsPoolTagName = "Platforms";

            public const float MaxMinXPosition = 2f;
        }
        #endregion

        #region UI
        public static class ShopUI
        {
            public static Color DeselectedColor = new Color(0.35f, 0.35f, 0.35f);
            public static Color PurchasedDeselectedColor = new Color(0.75f, 0.75f, 0.75f);
            public static Color SelectedColor = new Color(1f, 1f, 1f);
        }
        #endregion

        #region Crystals
        public static class Crystals
        {
            public const string CrystalsPoolTagName = "Crystals";
            public const string CrystalParticlesTagName = "CrystalParticles";
            public const int SpawnRandomChance = 50;
        }
        #endregion

        #region GameManager
        public static class GameManager
        {
            public const float TimeScaleMultiplier = 0.1f;
            public const float MaxTimeScale = 5f;
        }
        # endregion
    }
}