using JetBrains.Annotations;
using UnityEngine;

namespace Capybutler
{
    public static class PrefsButler
    {
        public static void DeleteAll() => PlayerPrefs.DeleteAll();
        public static void DeleteKey(string key) => PlayerPrefs.DeleteKey(key);
        public static void HasKey(string key) => PlayerPrefs.HasKey(key);

        public static void SetFloat(string key, float value) => PlayerPrefs.SetFloat(key, value);
        public static float GetFloat(string key, float defaultValue = 0f) => PlayerPrefs.GetFloat(key, defaultValue);

        public static void SetInt(string key, int value) => PlayerPrefs.SetInt(key, value);
        public static int GetInt(string key, int defaultValue = 0) => PlayerPrefs.GetInt(key, defaultValue);

        public static void SetBool(string key, bool value) => PlayerPrefs.SetInt(key, value ? 1 : 0);
        public static bool GetBool(string key, bool defaultValue = false) => PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) != 0;

        public static void SetVector2Int(string key, Vector2Int value)
        {
            PlayerPrefs.SetInt($"{key}__x", value.x);
            PlayerPrefs.SetInt($"{key}__y", value.y);
        }

        [UsedImplicitly]
        public static Vector2Int GetVector2Int(string key, Vector2Int defaultValue) => PlayerPrefs.HasKey($"{key}__x") && PlayerPrefs.HasKey($"{key}__y")
            ? new Vector2Int(PlayerPrefs.GetInt($"{key}__x"), PlayerPrefs.GetInt($"{key}__y"))
            : defaultValue;

        public static Vector2Int GetVector2Int(string key) => GetVector2Int(key, Vector2Int.zero);

        public static void SetVector2(string key, Vector2 value)
        {
            PlayerPrefs.SetFloat($"{key}__x", value.x);
            PlayerPrefs.SetFloat($"{key}__y", value.y);
        }

        [UsedImplicitly]
        public static Vector2 GetVector2(string key, Vector2 defaultValue) => PlayerPrefs.HasKey($"{key}__x") && PlayerPrefs.HasKey($"{key}__y")
            ? new Vector2(PlayerPrefs.GetFloat($"{key}__x"), PlayerPrefs.GetFloat($"{key}__y"))
            : defaultValue;


        public static Vector2 GetVector2(string key) => GetVector2(key, Vector2.zero);

        public static void SetVector3Int(string key, Vector3Int value)
        {
            PlayerPrefs.SetInt($"{key}__x", value.x);
            PlayerPrefs.SetInt($"{key}__y", value.y);
            PlayerPrefs.SetInt($"{key}__z", value.z);
        }

        [UsedImplicitly]
        public static Vector3Int GetVector3Int(string key, Vector3Int defaultValue) => PlayerPrefs.HasKey($"{key}__x") && PlayerPrefs.HasKey($"{key}__y") && PlayerPrefs.HasKey($"{key}__z")
            ? new Vector3Int(PlayerPrefs.GetInt($"{key}__x"), PlayerPrefs.GetInt($"{key}__y"), PlayerPrefs.GetInt($"{key}__z"))
            : defaultValue;

        public static Vector3Int GetVector3Int(string key) => GetVector3Int(key, Vector3Int.zero);

        public static void SetVector3(string key, Vector3 value)
        {
            PlayerPrefs.SetFloat($"{key}__x", value.x);
            PlayerPrefs.SetFloat($"{key}__y", value.y);
            PlayerPrefs.SetFloat($"{key}__z", value.z);
        }

        [UsedImplicitly]
        public static Vector3 GetVector3(string key, Vector3 defaultValue) => PlayerPrefs.HasKey($"{key}__x") && PlayerPrefs.HasKey($"{key}__y") && PlayerPrefs.HasKey($"{key}__z")
            ? new Vector3(PlayerPrefs.GetFloat($"{key}__x"), PlayerPrefs.GetFloat($"{key}__y"), PlayerPrefs.GetFloat($"{key}__z"))
            : defaultValue;

        public static Vector3 GetVector3(string key) => GetVector3(key, Vector3.zero);

        public static void SetVector4(string key, Vector4 value)
        {
            PlayerPrefs.SetFloat($"{key}__x", value.x);
            PlayerPrefs.SetFloat($"{key}__y", value.y);
            PlayerPrefs.SetFloat($"{key}__z", value.z);
            PlayerPrefs.SetFloat($"{key}__w", value.w);
        }

        [UsedImplicitly]
        public static Vector4 GetVector4(string key, Vector4 defaultValue) => PlayerPrefs.HasKey($"{key}__x") && PlayerPrefs.HasKey($"{key}__y") && PlayerPrefs.HasKey($"{key}__z") && PlayerPrefs.HasKey($"{key}__w")
            ? new Vector4(PlayerPrefs.GetFloat($"{key}__x"), PlayerPrefs.GetFloat($"{key}__y"), PlayerPrefs.GetFloat($"{key}__z"), PlayerPrefs.GetFloat($"{key}__w"))
            : defaultValue;

        public static Vector4 GetVector4(string key) => GetVector4(key, Vector4.zero);

        public static void SetQuaternion(string key, Quaternion value)
        {
            PlayerPrefs.SetFloat($"{key}__x", value.x);
            PlayerPrefs.SetFloat($"{key}__y", value.y);
            PlayerPrefs.SetFloat($"{key}__z", value.z);
            PlayerPrefs.SetFloat($"{key}__w", value.w);
        }

        [UsedImplicitly]
        public static Quaternion GetQuaternion(string key, Quaternion defaultValue) => PlayerPrefs.HasKey($"{key}__x") && PlayerPrefs.HasKey($"{key}__y") && PlayerPrefs.HasKey($"{key}__z") && PlayerPrefs.HasKey($"{key}__w")
            ? new Quaternion(PlayerPrefs.GetFloat($"{key}__x"), PlayerPrefs.GetFloat($"{key}__y"), PlayerPrefs.GetFloat($"{key}__z"), PlayerPrefs.GetFloat($"{key}__w"))
            : defaultValue;

        public static Quaternion GetQuaternion(string key) => GetQuaternion(key, Quaternion.identity);

        public static void SetColor(string key, Color value)
        {
            PlayerPrefs.SetFloat($"{key}__r", value.r);
            PlayerPrefs.SetFloat($"{key}__g", value.g);
            PlayerPrefs.SetFloat($"{key}__b", value.b);
            PlayerPrefs.SetFloat($"{key}__a", value.a);
        }

        [UsedImplicitly]
        public static Color GetColor(string key, Color defaultValue) => PlayerPrefs.HasKey($"{key}__r") && PlayerPrefs.HasKey($"{key}__g") && PlayerPrefs.HasKey($"{key}__b") && PlayerPrefs.HasKey($"{key}__a")
            ? new Color(PlayerPrefs.GetFloat($"{key}__r"), PlayerPrefs.GetFloat($"{key}__g"), PlayerPrefs.GetFloat($"{key}__b"), PlayerPrefs.GetFloat($"{key}__a"))
            : defaultValue;

        [UsedImplicitly]
        public static Color GetColor(string key) => GetColor(key, Color.black);
    }
}