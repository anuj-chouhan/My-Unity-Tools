using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A simple and type-safe save/load manager using PlayerPrefs.
/// Supports enum-based keys and default fallback values.
/// </summary>
public static class SaveLoadSystem
{
    public enum IntKeys { ExampleKey_Int1, ExampleKey_Int2 }
    public enum FloatKeys { ExampleKey_Float1, ExampleKey_Float2 }
    public enum BoolKeys { ExampleKey_Bool1, ExampleKey_Bool2 }
    public enum StringKeys { ExampleKey_Key1, ExampleKey_Key2 }

    private static readonly Dictionary<IntKeys, int> defaultIntValues = new()
    {
        { IntKeys.ExampleKey_Int1, 100 },
        { IntKeys.ExampleKey_Int2, 10 },
    };

    private static readonly Dictionary<FloatKeys, float> defaultFloatValues = new()
    {
        { FloatKeys.ExampleKey_Float1, 1.5f },
        { FloatKeys.ExampleKey_Float2, 10.2f },
    };

    private static readonly Dictionary<BoolKeys, bool> defaultBoolValues = new()
    {
        { BoolKeys.ExampleKey_Bool1, true },
        { BoolKeys.ExampleKey_Bool2, false },
    };

    private static readonly Dictionary<StringKeys, string> defaultStringValues = new()
    {
        { StringKeys.ExampleKey_Key1, "Anuj" },
        { StringKeys.ExampleKey_Key2, "Chouhan" },
    };

    // =================== Save Methods ===================

    /// <summary>
    /// Saves an integer value to PlayerPrefs using the provided key.
    /// </summary>
    /// <param name="key">The enum key representing the data to be saved.</param>
    /// <param name="value">The integer value to save.</param>
    public static void SaveInt(IntKeys key, int value)
    {
        PlayerPrefs.SetInt(key.ToString(), value);
    }

    /// <summary>
    /// Saves a float value to PlayerPrefs using the provided key.
    /// </summary>
    /// <param name="key">The enum key representing the data to be saved.</param>
    /// <param name="value">The float value to save.</param>
    public static void SaveFloat(FloatKeys key, float value)
    {
        PlayerPrefs.SetFloat(key.ToString(), value);
    }

    /// <summary>
    /// Saves a boolean value to PlayerPrefs using the provided key.
    /// </summary>
    /// <param name="key">The enum key representing the data to be saved.</param>
    /// <param name="value">The boolean value to save.</param>
    public static void SaveBool(BoolKeys key, bool value)
    {
        PlayerPrefs.SetInt(key.ToString(), value ? 1 : 0);
    }

    /// <summary>
    /// Saves a string value to PlayerPrefs using the provided key.
    /// </summary>
    /// <param name="key">The enum key representing the data to be saved.</param>
    /// <param name="value">The string value to save.</param>
    public static void SaveString(StringKeys key, string value)
    {
        PlayerPrefs.SetString(key.ToString(), value);
    }

    // =================== Load Methods ===================

    /// <summary>
    /// Loads an integer value using the given key. Returns the default value if available, otherwise null.
    /// </summary>
    /// <param name="key">The enum key to load from PlayerPrefs.</param>
    /// <returns>The stored int value or null if the key is not mapped in defaults.</returns>
    public static int? LoadInt(IntKeys key)
    {
        return defaultIntValues.ContainsKey(key)
            ? PlayerPrefs.GetInt(key.ToString(), defaultIntValues[key])
            : (int?)null;
    }

    /// <summary>
    /// Loads a float value using the given key. Returns the default value if available, otherwise null.
    /// </summary>
    /// <param name="key">The enum key to load from PlayerPrefs.</param>
    /// <returns>The stored float value or null if the key is not mapped in defaults.</returns>
    public static float? LoadFloat(FloatKeys key)
    {
        return defaultFloatValues.ContainsKey(key)
            ? PlayerPrefs.GetFloat(key.ToString(), defaultFloatValues[key])
            : (float?)null;
    }

    /// <summary>
    /// Loads a boolean value using the given key. Returns the default value if available, otherwise null.
    /// </summary>
    /// <param name="key">The enum key to load from PlayerPrefs.</param>
    /// <returns>The stored bool value or null if the key is not mapped in defaults.</returns>
    public static bool? LoadBool(BoolKeys key)
    {
        return defaultBoolValues.ContainsKey(key)
            ? PlayerPrefs.GetInt(key.ToString(), defaultBoolValues[key] ? 1 : 0) == 1
            : (bool?)null;
    }

    /// <summary>
    /// Loads a string value using the given key. Returns the default value if available, otherwise null.
    /// </summary>
    /// <param name="key">The enum key to load from PlayerPrefs.</param>
    /// <returns>The stored string value or null if the key is not mapped in defaults.</returns>
    public static string LoadString(StringKeys key)
    {
        return defaultStringValues.ContainsKey(key)
            ? PlayerPrefs.GetString(key.ToString(), defaultStringValues[key])
            : null;
    }

    // =================== Delete Methods ===================

    /// <summary>
    /// Deletes a specific int key from PlayerPrefs.
    /// </summary>
    /// <param name="key">The int key to delete.</param>
    public static void DeleteKey(IntKeys key) => PlayerPrefs.DeleteKey(key.ToString());

    /// <summary>
    /// Deletes a specific float key from PlayerPrefs.
    /// </summary>
    /// <param name="key">The float key to delete.</param>
    public static void DeleteKey(FloatKeys key) => PlayerPrefs.DeleteKey(key.ToString());

    /// <summary>
    /// Deletes a specific bool key from PlayerPrefs.
    /// </summary>
    /// <param name="key">The bool key to delete.</param>
    public static void DeleteKey(BoolKeys key) => PlayerPrefs.DeleteKey(key.ToString());

    /// <summary>
    /// Deletes a specific string key from PlayerPrefs.
    /// </summary>
    /// <param name="key">The string key to delete.</param>
    public static void DeleteKey(StringKeys key) => PlayerPrefs.DeleteKey(key.ToString());

    /// <summary>
    /// Deletes all keys and values from PlayerPrefs. Use with caution!
    /// </summary>
    public static void DeleteAll() => PlayerPrefs.DeleteAll();

    // =================== HasKey Methods ===================

    /// <summary>
    /// Checks if the given int key exists in PlayerPrefs.
    /// </summary>
    public static bool HasKey(IntKeys key) => PlayerPrefs.HasKey(key.ToString());

    /// <summary>
    /// Checks if the given float key exists in PlayerPrefs.
    /// </summary>
    public static bool HasKey(FloatKeys key) => PlayerPrefs.HasKey(key.ToString());

    /// <summary>
    /// Checks if the given bool key exists in PlayerPrefs.
    /// </summary>
    public static bool HasKey(BoolKeys key) => PlayerPrefs.HasKey(key.ToString());

    /// <summary>
    /// Checks if the given string key exists in PlayerPrefs.
    /// </summary>
    public static bool HasKey(StringKeys key) => PlayerPrefs.HasKey(key.ToString());
}
