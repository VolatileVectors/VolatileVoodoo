using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using VolatileVoodoo.Runtime.Audio;

namespace VolatileVoodoo.Editor.Audio
{
    [CustomEditor(typeof(AudioEffectPlayer))]
    public class AudioEffectPlayerEditor : OdinEditor
    {
        private void OnSceneGUI()
        {
            var audioEffectPlayer = target as AudioEffectPlayer;
            if (audioEffectPlayer == null) return;

            var position = audioEffectPlayer.transform.position;

            Handles.color = Color.yellow;

            EditorGUI.BeginChangeCheck();
            var minDistance = Handles.RadiusHandle(Quaternion.identity, position, audioEffectPlayer.minDistance);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(target, "Changed minDistance");
                audioEffectPlayer.minDistance = minDistance;
                audioEffectPlayer.OnMinDistanceChanged();
            }

            EditorGUI.BeginChangeCheck();
            var maxDistance = Handles.RadiusHandle(Quaternion.identity, position, audioEffectPlayer.maxDistance);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(target, "Changed maxDistance");
                audioEffectPlayer.maxDistance = maxDistance;
                audioEffectPlayer.OnMaxDistanceChanged();
            }
        }
    }
}