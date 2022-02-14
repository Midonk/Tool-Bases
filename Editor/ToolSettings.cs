using UnityEngine;
using UnityEditor;
using System.IO;

namespace TF.Tool
{
    public abstract class ToolSettings<T> : ScriptableObject where T : ScriptableObject
    {    
        #region Properties

        public static string SettingAssetPath => $"{SETTINGSPATH}/{typeof(T).Name}.asset";
            
        #endregion


        #region Main

        /// <summary>
        /// Try to get an existing settings file or create one and returns it if it doesn't
        /// </summary>
        /// <returns>The found settings file</returns>
        public static T GetOrCreate()
        {
            var settings = GetSettings();

            if (settings is null)
            {
                settings = CreateSettings();            
                Debug.LogWarning($"New settings created '<color=cyan>{typeof(T).Name}</color>' at \n" +
                                $"'<color=cyan>{SETTINGSPATH}</color>'");
            }

            return settings;
        }

        #endregion

        
        #region Plumbery

        private static T GetSettings()
        {
            return AssetDatabase.LoadAssetAtPath<T>(SettingAssetPath);
        }

        private static T CreateSettings()
        {
            if (!Directory.Exists(SETTINGSPATH))
            {
                Directory.CreateDirectory(SETTINGSPATH);
                AssetDatabase.Refresh();
            }

            var settings = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(settings, SettingAssetPath);
            AssetDatabase.SaveAssets();

            return settings;
        }

        #endregion


        #region Private Fields

        private const string SETTINGSPATH = "Assets/Editor/Settings";

        #endregion
    }
}