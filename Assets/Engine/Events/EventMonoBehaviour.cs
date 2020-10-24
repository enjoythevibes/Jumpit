using System.Collections.Generic;
using enjoythevibes.Managers;
using UnityEngine;

namespace enjoythevibes
{
    public abstract class EventMonoBehaviour : MonoBehaviour
    {
        private struct GameObjectEventArgument : IEventArgument
        {
            public int objectID;
            public GameObjectEventArgument(int objectID)
            {
                this.objectID = objectID;
            }
        }

        private static Dictionary<int, EventMonoBehaviour> objects = new Dictionary<int, EventMonoBehaviour>();

        static EventMonoBehaviour()
        {
            EventsManager.AddListener(Events.TriggerObject, OnStaticEventTrigger);
        }

        private static void OnStaticEventTrigger(IEventArgument argument)
        {
            var objectID = ((GameObjectEventArgument)argument).objectID;
            objects[objectID].OnEventTrigger();
        }

        public static void TriggerObject(GameObject gameObject)
        {
            EventsManager.CallEvent(Events.TriggerObject, new GameObjectEventArgument(gameObject.GetInstanceID()));
        }

        private void Start() 
        {
            objects.Add(gameObject.GetInstanceID(), this);
            Init();
        }

        protected virtual void Init() { }
        protected abstract void OnEventTrigger();
    }
}