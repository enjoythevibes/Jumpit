using enjoythevibes.Managers;
using UnityEngine;

namespace enjoythevibes.Crystal
{
    public class Crystal : EventMonoBehaviour
    {
        protected override void OnEventTrigger()
        {
            #if UNITY_EDITOR
            Debug.Log("Crystal event " + gameObject.GetInstanceID());
            #endif
            PoolsManager.GetGameObjectsPool(EngineSettings.Crystals.CrystalsPoolTagName).Put(gameObject);
        }
    }
}