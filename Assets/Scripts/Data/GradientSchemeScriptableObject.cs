using UnityEngine;

namespace enjoythevibes.Data
{
    [CreateAssetMenu(fileName = "Gradient Scheme", menuName = "enjoythevibes/GradientScheme", order = 100)]
    public class GradientSchemeScriptableObject : ScriptableObject 
    {
        [Header("Base")]
        
        [SerializeField]
        private int id = 1;
        public int ID => id;

        [Header("Gradient Lighting")]

        [SerializeField]
        [ColorUsage(false, true)]
        private Color skyColor = new Color(1, 1, 1, 1);
        public Color SkyColor => skyColor;

        [SerializeField]
        [ColorUsage(false, true)]
        private Color equatorColor = new Color(1, 1, 1, 1);
        public Color EquatorColor => equatorColor;

        [SerializeField]
        [ColorUsage(false, true)]
        private Color groundColor = new Color(1, 1, 1, 1);
        public Color GroundColor => groundColor;

        [Header("Fog")]

        [SerializeField]
        private Color fogColor = new Color(1, 1, 1, 1);
        public Color FogColor => fogColor;

        [Header("Skybox")]

        [SerializeField]
        private Color skyboxColor1 = new Color(1, 1, 1, 1);
        public Color SkyboxColor1 => skyboxColor1;
        [SerializeField]
        private Color skyboxColor2 = new Color(1, 1, 1, 1);
        public Color SkyboxColor2 => skyboxColor2;

        [Header("Shop Price")]

        [SerializeField]
        private int price = 100;
        public int Price => price;
    }
}