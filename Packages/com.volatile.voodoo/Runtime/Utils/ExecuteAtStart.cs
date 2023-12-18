using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace VolatileVoodoo.Runtime.Utils
{
    [Serializable]
    public struct StartUpAction
    {
        [LabelText("Delayed by seconds")]
        public float delay;

        public UnityEvent action;
    }

    public class ExecuteAtStart : MonoBehaviour
    {
        public List<StartUpAction> actions;
        private IOrderedEnumerable<StartUpAction> sortedActions;

        private void Awake()
        {
            sortedActions = actions.OrderBy(action => action.delay);
        }

        private IEnumerator Start()
        {
            foreach (var actionItem in sortedActions) {
                var currentTime = Time.timeSinceLevelLoad;
                if (currentTime < actionItem.delay)
                    yield return new WaitForSecondsRealtime(actionItem.delay - currentTime);

                actionItem.action?.Invoke();
            }

            // No children and only Component on the gameObject (+transform) => destroy the whole thing
            // gameObject has children and/or other Components => just remove itself 
            Destroy(transform.childCount == 0 && gameObject.GetComponents<Component>().Length == 2 ? gameObject : this);
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}