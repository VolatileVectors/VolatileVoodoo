using UnityEngine;

namespace VolatileVoodoo.Runtime.Utils
{
    public static class VoodooPrefs
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

        public static void SetColor(string key, Color value)
        {
            PlayerPrefs.SetFloat($"{key}__r", value.r);
            PlayerPrefs.SetFloat($"{key}__g", value.g);
            PlayerPrefs.SetFloat($"{key}__b", value.b);
            PlayerPrefs.SetFloat($"{key}__a", value.a);
        }

        public static Color GetColor(string key, Color defaultValue) => new(
            PlayerPrefs.GetFloat($"{key}__r", defaultValue.r),
            PlayerPrefs.GetFloat($"{key}__g", defaultValue.g),
            PlayerPrefs.GetFloat($"{key}__b", defaultValue.b),
            PlayerPrefs.GetFloat($"{key}__a", defaultValue.a)
        );

        public static Color GetColor(string key) => GetColor(key, Color.black);

        public static void SetQuaternion(string key, Quaternion value)
        {
            PlayerPrefs.SetFloat($"{key}__x", value.x);
            PlayerPrefs.SetFloat($"{key}__y", value.y);
            PlayerPrefs.SetFloat($"{key}__z", value.z);
            PlayerPrefs.SetFloat($"{key}__w", value.w);
        }

        public static Quaternion GetQuaternion(string key, Quaternion defaultValue) => new(
            PlayerPrefs.GetFloat($"{key}__x", defaultValue.x),
            PlayerPrefs.GetFloat($"{key}__y", defaultValue.y),
            PlayerPrefs.GetFloat($"{key}__z", defaultValue.z),
            PlayerPrefs.GetFloat($"{key}__w", defaultValue.w)
        );

        public static Quaternion GetQuaternion(string key) => GetQuaternion(key, Quaternion.identity);

        public static void SetVector2(string key, Vector2 value)
        {
            PlayerPrefs.SetFloat($"{key}__x", value.x);
            PlayerPrefs.SetFloat($"{key}__y", value.y);
        }

        public static Vector2 GetVector2(string key, Vector2 defaultValue) => new(
            PlayerPrefs.GetFloat($"{key}__x", defaultValue.x),
            PlayerPrefs.GetFloat($"{key}__y", defaultValue.y)
        );

        public static Vector2 GetVector2(string key) => GetVector2(key, Vector2.zero);

        public static void SetVector3(string key, Vector3 value)
        {
            PlayerPrefs.SetFloat($"{key}__x", value.x);
            PlayerPrefs.SetFloat($"{key}__y", value.y);
            PlayerPrefs.SetFloat($"{key}__z", value.z);
        }


        public static Vector3 GetVector3(string key, Vector3 defaultValue) => new(
            PlayerPrefs.GetFloat($"{key}__x", defaultValue.x),
            PlayerPrefs.GetFloat($"{key}__y", defaultValue.y),
            PlayerPrefs.GetFloat($"{key}__z", defaultValue.z)
        );

        public static Vector3 GetVector3(string key) => GetVector3(key, Vector3.zero);

        public static void SetVector4(string key, Vector4 value)
        {
            PlayerPrefs.SetFloat($"{key}__x", value.x);
            PlayerPrefs.SetFloat($"{key}__y", value.y);
            PlayerPrefs.SetFloat($"{key}__z", value.z);
            PlayerPrefs.SetFloat($"{key}__w", value.w);
        }

        public static Vector4 GetVector4(string key, Vector4 defaultValue) => new(
            PlayerPrefs.GetFloat($"{key}__x", defaultValue.x),
            PlayerPrefs.GetFloat($"{key}__y", defaultValue.y),
            PlayerPrefs.GetFloat($"{key}__z", defaultValue.z),
            PlayerPrefs.GetFloat($"{key}__w", defaultValue.w)
        );

        public static Vector4 GetVector4(string key) => GetVector4(key, Vector4.zero);

        public static void SetVector2Int(string key, Vector2Int value)
        {
            PlayerPrefs.SetInt($"{key}__x", value.x);
            PlayerPrefs.SetInt($"{key}__y", value.y);
        }

        public static Vector2Int GetVector2Int(string key, Vector2Int defaultValue) => new(
            PlayerPrefs.GetInt($"{key}__x", defaultValue.x),
            PlayerPrefs.GetInt($"{key}__y", defaultValue.y)
        );

        public static Vector2Int GetVector2Int(string key) => GetVector2Int(key, Vector2Int.zero);

        public static void SetVector3Int(string key, Vector3Int value)
        {
            PlayerPrefs.SetInt($"{key}__x", value.x);
            PlayerPrefs.SetInt($"{key}__y", value.y);
            PlayerPrefs.SetInt($"{key}__z", value.z);
        }

        public static Vector3Int GetVector3Int(string key, Vector3Int defaultValue) => new(
            PlayerPrefs.GetInt($"{key}__x", defaultValue.x),
            PlayerPrefs.GetInt($"{key}__y", defaultValue.y),
            PlayerPrefs.GetInt($"{key}__z", defaultValue.z)
        );

        public static Vector3Int GetVector3Int(string key) => GetVector3Int(key, Vector3Int.zero);
    }
}