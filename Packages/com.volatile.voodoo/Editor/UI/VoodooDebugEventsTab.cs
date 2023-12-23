using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using VolatileVoodoo.Events.Base;

namespace VolatileVoodoo.Editor.UI
{
    public class VoodooDebugEventsTab : VoodooDebugTab
    {
        public static readonly List<BaseEvent> VoodooEvents = new();
        public static readonly List<BaseEventListener> VoodooEventListeners = new();

        private ListView voodooValuesEventsList;

        public override void CreateGUI(VisualElement rootVisualElement)
        {
            base.CreateGUI(rootVisualElement);

            FindAll(VoodooEvents);
            FindAll(VoodooEventListeners);

            voodooValuesEventsList = rootVisualElement.Q<ListView>("eventsList");
            voodooValuesEventsList.itemsSource = VoodooEvents;
        }

        protected override void OnFilterChanged(ChangeEvent<string> textChange)
        {
            throw new NotImplementedException();
        }
    }
}