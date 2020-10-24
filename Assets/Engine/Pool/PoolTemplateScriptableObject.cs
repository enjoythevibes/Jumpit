using UnityEngine;

namespace enjoythevibes.Pool
{
    [CreateAssetMenu(fileName = "PoolTemplate", menuName = "enjoythevibes/PoolTemplate", order = 100)]
    public class PoolTemplateScriptableObject : ScriptableObject
    {
        [SerializeField]
        private string templateTagName = default;
        public string TemplateTagName => templateTagName;
        [SerializeField]
        private GameObject templatePrefab = default;
        public GameObject TemplatePrefab => templatePrefab;
        [SerializeField]
        private int amountToPool = default;
        public int AmountToPool => amountToPool;
    }
}