using System.Collections.Generic;
using UnityEngine;

namespace enjoythevibes.Managers
{
    public class PlatformsManager : MonoBehaviour
    {
        private float lastSpawnZPosition;
        private Queue<GameObject> platforms = new Queue<GameObject>();

        private void Awake() 
        {
            lastSpawnZPosition = EngineSettings.Platforms.SpawnEachZ;
            EventsManager.AddListener(Events.PlayGame, OnGeneratePlatforms);
            EventsManager.AddListener(Events.RestartGame, OnDestroyPlatforms);
            EventsManager.AddListener(Events.RestartGame, OnGeneratePlatforms);
            EventsManager.AddListener(Events.BackToMenu, OnDestroyPlatforms);
            EventsManager.AddListener(Events.GenerateNextPlatform, OnGenerateNextPlatform);
        }

        private void OnGeneratePlatforms()
        {
            for (int i = 0; i < EngineSettings.Platforms.SpawnRowsCount; i++)
            {
                OnGenerateNextPlatform();
            }
        }

        private void OnDestroyPlatforms()
        {
            while (platforms.Count > 0)
            {
                var platform = platforms.Dequeue();
                platform.GetComponent<Platforms.Platform>().DestroyPlatform();
            }
            lastSpawnZPosition = EngineSettings.Platforms.SpawnEachZ;
        }

        private void OnGenerateNextPlatform()
        {
            var platformGameObject = PoolsManager.GetGameObjectsPool(EngineSettings.Platforms.PlatformsPoolTagName).Take();
            var xRandomPosition = Random.Range(-EngineSettings.Platforms.MaxMinXPosition, EngineSettings.Platforms.MaxMinXPosition);

            platformGameObject.transform.position = new Vector3(xRandomPosition, 0f, lastSpawnZPosition);
            lastSpawnZPosition += EngineSettings.Platforms.SpawnEachZ;
            platformGameObject.transform.parent = transform;
            if (platforms.Count > EngineSettings.Platforms.SpawnRowsCount + 1)
            {
                var platformToReturn = platforms.Dequeue();
                platformToReturn.GetComponent<Platforms.Platform>().DestroyPlatform();
            }
            platforms.Enqueue(platformGameObject);
            platformGameObject.GetComponent<Platforms.Platform>().GenerateCrystal();
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos() 
        {
            Gizmos.color = Color.green;
            var spawnRowsCount = EngineSettings.Platforms.SpawnRowsCount;
            var platformsAreaSize = new Vector3(1f + (EngineSettings.Platforms.MaxMinXPosition * 2f), 0.5f, EngineSettings.Platforms.SpawnEachZ * (spawnRowsCount - 1) + 1f);
            var center = transform.position + new Vector3(0f, 0f, (EngineSettings.Platforms.SpawnEachZ * (spawnRowsCount - 1)) / 2f);
            Gizmos.DrawWireCube(center, platformsAreaSize);    
        }
        #endif
    }
}