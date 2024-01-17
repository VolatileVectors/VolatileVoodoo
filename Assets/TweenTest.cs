using UnityEngine;
using VolatileVoodoo.Tweens;
using VolatileVoodoo.Utils;

public class TweenTest : MonoBehaviour
{
    public TransformTweener cube;
    public TransformTweener capsule;
    public TransformTweener sphere;

    public void LogCube(bool state)
    {
        gameObject.LogInfo("Cube: " + (state ? "Eased IN" : "Eased OUT"));
    }

    public void LogCapsule(bool state)
    {
        gameObject.LogInfo("Capsule: " + (state ? "Eased IN" : "Eased OUT"));
    }

    public void LogSphere(bool state)
    {
        gameObject.LogInfo("Sphere: " + (state ? "Eased IN" : "Eased OUT"));
    }

    private void OnGUI()
    {
        var area = new Rect(Screen.width / 2f - 630 / 2f, 20, 630, 180);
        GUILayout.BeginArea(area);
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Ease In Cube")) cube.EaseIn();
        if (GUILayout.Button("Ease In Capsule")) capsule.EaseIn();
        if (GUILayout.Button("Ease In Sphere")) sphere.EaseIn();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Ease Out Cube")) cube.EaseOut();
        if (GUILayout.Button("Ease Out Capsule")) capsule.EaseOut();
        if (GUILayout.Button("Ease Out Sphere")) sphere.EaseOut();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}