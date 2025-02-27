using System;
using Sirenix.OdinInspector;

namespace Capybutler
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ToggleButtonAttribute : Attribute
    {
        public readonly SdfIconType Icon;
        public readonly SdfIconType IconOff;
        public readonly string Tooltip;

        public ToggleButtonAttribute(string tooltip = "", SdfIconType editorIcon = SdfIconType.None, SdfIconType editorIconOff = SdfIconType.None)
        {
            Tooltip = tooltip;
            Icon = editorIcon;
            IconOff = editorIconOff;
        }
    }
}