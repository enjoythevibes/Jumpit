using System;
using System.Collections.Generic;
using UnityEngine;

namespace enjoythevibes.Managers
{
    public interface IEventArgument
    {
    }

    public class EventsManager : MonoBehaviour
    {
        private static Queue<Action> actionsToCall = new Queue<Action>();
        private static Queue<ValueTuple<Action<IEventArgument>, IEventArgument>> actionsToCallArgument = new Queue<ValueTuple<Action<IEventArgument>, IEventArgument>>();
        
        private static Dictionary<Events, Action> actions = new Dictionary<Events, Action>();
        private static Dictionary<Events, Action<IEventArgument>> actionsWithArgument = new Dictionary<Events, Action<IEventArgument>>();

        public static void AddListener(Events key, Action action)
        {
            if (actions.ContainsKey(key))
            {
                actions[key] += action;
            }
            else
            {
                actions.Add(key, null);
                actions[key] += action;
            }
        }

        public static void AddListener(Events key, Action<IEventArgument> action)
        {
            if (actionsWithArgument.ContainsKey(key))
            {
                actionsWithArgument[key] += action;
            }
            else
            {
                actionsWithArgument.Add(key, null);
                actionsWithArgument[key] += action;
            }
        }

        public static void RemoveListener(Events key, Action action)
        {
            if (actions.ContainsKey(key))
            {
                actions[key] -= action;
            }
            #if UNITY_EDITOR
            else
            {
                Debug.LogError($"Listener with {key} key not found | Remove");
            }
            #endif
        }

        public static void RemoveListener(Events key, Action<IEventArgument> action)
        {
            if (actionsWithArgument.ContainsKey(key))
            {
                actionsWithArgument[key] -= action;
            }
            #if UNITY_EDITOR
            else
            {
                Debug.LogError($"Listener with {key} key not found | Remove with argument");
            }
            #endif
        }

        public static void CallEvent(Events key)
        {
            if (actions.TryGetValue(key, out var action))
            {
                actionsToCall.Enqueue(action);
            }
            #if UNITY_EDITOR
            else
            {
                Debug.LogError($"Listener with {key} key not found | Call");
            }
            #endif
        }

        public static void CallEvent(Events key, IEventArgument argument)
        {
            if (actionsWithArgument.TryGetValue(key, out var action))
            {
                var valueTuple = new ValueTuple<Action<IEventArgument>, IEventArgument>(action, argument);
                actionsToCallArgument.Enqueue(valueTuple);
            }
            #if UNITY_EDITOR
            else
            {
                Debug.LogError($"Listener with {key} key not found | Call with argument");
            }
            #endif
        }

        private void Update()
        {
            while (actionsToCall.Count > 0)
            {
                var toCall = actionsToCall.Dequeue();
                toCall.Invoke();
            }
            while (actionsToCallArgument.Count > 0)
            {
                var toCall = actionsToCallArgument.Dequeue();
                toCall.Item1.Invoke(toCall.Item2);
            }
        }
    }
}