using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.ValueResolvers;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using VolatileVoodoo.Runtime.Utils;

namespace VolatileVoodoo.Editor.Utils
{
    public class ToggleButtonAttributeDrawer : OdinAttributeDrawer<ToggleButtonAttribute, bool>
    {
        private const float ButtonWidth = 25f;
        private static readonly Color ActiveColor = EditorGUIUtility.isProSkin ? Color.white : new Color(0.802f, 0.802f, 0.802f, 1f);
        private static readonly Color InactiveColor = EditorGUIUtility.isProSkin ? new Color(0.75f, 0.75f, 0.75f, 1f) : Color.white;

        private ValueResolver<string> tooltipResolver;

        private SdfIconType iconOn = SdfIconType.Check;
        private SdfIconType iconOff = SdfIconType.X;

        protected override void Initialize()
        {
            base.Initialize();
            tooltipResolver = ValueResolver.GetForString(Property, Attribute.Tooltip);

            if (Attribute.Icon != SdfIconType.None)
            {
                iconOn = Attribute.Icon;
                iconOff = Attribute.IconOff != SdfIconType.None ? Attribute.IconOff : Attribute.Icon;
            }

            GUIHelper.RequestRepaint();
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            tooltipResolver.DrawError();

            var tooltip = tooltipResolver.GetValue();
            var currentValue = (bool)Property.ValueEntry.WeakSmartValue;
            
            SirenixEditorGUI.GetFeatureRichControlRect(label: label, height: Mathf.CeilToInt(EditorGUIUtility.singleLineHeight), controlId: out _, hasKeyboardFocus: out _, valueRect: out var buttonRect);
            buttonRect.width = ButtonWidth;
            var buttonContent = new GUIContent(" ", tooltip);

            GUIHelper.PushColor((currentValue ? ActiveColor : InactiveColor) * GUI.color);
            if (GUI.Button(buttonRect, buttonContent, SirenixGUIStyles.MiniButton)) Property.ValueEntry.WeakSmartValue = !currentValue;

            var size = buttonRect.height - 2f;
            var iconRect = new Rect(buttonRect) { width = size, height = size, x = buttonRect.center.x - size / 2f, y = buttonRect.center.y - size / 2f };
            SdfIcons.DrawIcon(iconRect, currentValue ? iconOn : iconOff, currentValue ? Color.green : Color.red);

            GUIHelper.PopColor();
        }
    }
}