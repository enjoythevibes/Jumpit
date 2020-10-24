using enjoythevibes.Managers;
using UnityEngine;

namespace enjoythevibes.Platforms
{
    public class Platform : MonoBehaviour
    {
        [SerializeField]
        private float centerRadiusDetection = 0.1f;
        public float СenterRadiusDetection => centerRadiusDetection;

        [SerializeField]
        private Vector3 centerPosition = Vector3.zero;
        public Vector3 CenterPosition => centerPosition;

        private GameObject crystalRelated;

        public void DestroyPlatform()
        {
            if (crystalRelated != null)
            {
                EventMonoBehaviour.TriggerObject(crystalRelated);
                crystalRelated = null;
            }
            PoolsManager.GetGameObjectsPool(EngineSettings.Platforms.PlatformsPoolTagName).Put(gameObject);
        }

        public void TakeCrystal()
        {
            if (crystalRelated != null)
            {
                var particle = PoolsManager.GetGameObjectsPool(EngineSettings.Crystals.CrystalParticlesTagName).Take();
                particle.transform.position = crystalRelated.transform.position + new Vector3(0f, 0.5f, 0f);
                particle.GetComponent<Particles.Particle>().PlayParticle();
                
                EventMonoBehaviour.TriggerObject(crystalRelated);
                crystalRelated = null;
                EventsManager.CallEvent(Events.AddCrystal);
            }
        }

        public void GenerateCrystal() 
        {
            var crystalChance = Random.Range(0, 100);
            if (crystalChance <= EngineSettings.Crystals.SpawnRandomChance)
            {
                var crystalGameObject = PoolsManager.GetGameObjectsPool(EngineSettings.Crystals.CrystalsPoolTagName).Take();
                crystalGameObject.transform.position = transform.position + centerPosition;
                crystalGameObject.transform.parent = transform;                
                crystalRelated = crystalGameObject;
            }
        }
        
        #if UNITY_EDITOR
        private void OnDrawGizmos() 
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position + centerPosition, centerRadiusDetection);    
        }
        #endif
    }
}