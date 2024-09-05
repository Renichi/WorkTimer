namespace WT
{
    using System;
    using UnityEngine;
    using UnityEditor;
    using System.Linq;

    public class WorkTimerWindow : EditorWindow
    {
        static private WTParamaterData data;
        static double previousTime;
        static private double startupTime = 0;
        static private double totalTime = 0;
        static private bool isRest = true;

        [InitializeOnLoadMethod]
        static void AutoStart()
        {
            if (data == null)
            {
                data = LoadData();
                totalTime = data.TotalTime;
            }
            startupTime = 0;
            EditorApplication.update += EditorUpdateCallback;
        }

        static void EditorUpdateCallback()
        {
            double time = EditorApplication.timeSinceStartup;
            double deltaTime = time - previousTime;
            previousTime = time;
            if( isRest ) return;
            startupTime += deltaTime;
            totalTime += deltaTime;
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
            if (data == null) return;
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("StartUp Time", FormatTimeString( startupTime ));
            EditorGUILayout.LabelField("Totla Time", FormatTimeString( totalTime ));
            if( GUILayout.Button(  isRest? "Work Time" : "Break Time" ) ) isRest = !isRest;
        }

        private string FormatTimeString( double time )
        {
            var second = time % 60.0f;
            var minute = (int)(time / 60.0f) % 60;
            var hour = ((int)((int)time / 60.0f)) / 60.0f;
            return String.Format("{0}:{1:00}:{2:00}", (int)hour, (int)minute, (int)second);
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