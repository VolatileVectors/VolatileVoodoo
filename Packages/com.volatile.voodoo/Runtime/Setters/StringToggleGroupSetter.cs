using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace VolatileVoodoo.Runtime.Setters
{
    public class StringToggleGroupSetter : MonoBehaviour
    {
        [Serializable]
        public struct StringToggleEntry
        {
            public string value;
            public Toggle toggle;
        }

        public List<StringToggleEntry> toggleList;

        public void UpdateWithoutNotify(string category)
        {
            var toggle = toggleList.FirstOrDefault(entry => entry.value.Equals(category)).toggle;
            if (toggle != null) {
                toggle.SetIsOnWithoutNotify(true);
            }
        }
    }
}