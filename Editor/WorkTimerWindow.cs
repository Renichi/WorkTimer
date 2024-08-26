namespace WT
{
    using System;
    using UnityEngine;
    using UnityEditor;
    using System.Linq;

    public class WorkTimerWindow : EditorWindow
    {
        static private WTParamaterData data;
        const double INTERVAL = 10.0;
        static double previousTime;
        static private double startupTime = 0;
        static private double totalTime = 0;

        [InitializeOnLoadMethod]
        static void AutoStart()
        {
            if (data == null)
            {
                data = LoadData();
                totalTime += data.TotalTime;
            }
            startupTime = 0;
            EditorApplication.update += EditorUpdateCallback;
        }

        static void EditorUpdateCallback()
        {
            double time = EditorApplication.timeSinceStartup;
            double deltaTime = time - previousTime;
            startupTime += deltaTime;
            totalTime += deltaTime;
            previousTime = time;
            data.TotalTime = totalTime;
            EditorUtility.SetDirty(data);
        }

        [MenuItem("Extra/WorkTimer")]
        public static void Create()
        {
            EditorWindow wnd = GetWindow<WorkTimerWindow>();
            wnd.titleContent = new GUIContent("WorkTimer");
        }

        void Update()
        {
            Repaint();
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("StartUp Time", FormatTimeString( startupTime ));
            EditorGUILayout.LabelField("Totla Time", FormatTimeString( totalTime ));
        }

        private string FormatTimeString( double time )
        {
            var second = time % 60.0f;
            var minute = time / 60.0f;
            var hour = time / 60.0f / 60.0f;
            return String.Format("{0}:{1:00}:{2:00}", (int)hour, (int)minute % 60, (int)second);
        }

        static WTParamaterData LoadData()
        {
            return (WTParamaterData)AssetDatabase.FindAssets("t:ScriptableObject")
               .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
               .Select(path => AssetDatabase.LoadAssetAtPath(path, typeof(WTParamaterData)))
               .Where(obj => obj != null)
               .FirstOrDefault();
        }
    }
}