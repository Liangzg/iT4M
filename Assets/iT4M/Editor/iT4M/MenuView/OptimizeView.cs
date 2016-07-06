/********************************************************************************
** Author： LiangZG
** Email :  game.liangzg@foxmail.com
*********************************************************************************/
using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 描述：优化选项视图
/// <para>创建时间：2016-07-05</para>
/// </summary>
public class OptimizeView
{

    private Transform CurrentSelect;
    private int OptimizeLevel = 0;
    public void OnGUI()
    {
        CurrentSelect = T4MMainEditor.CurrentSelect;

        T4MMainObj mainObjT4MMain = CurrentSelect.GetComponent<T4MMainObj>();
        if (mainObjT4MMain)
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            GUILayout.Label("Optimization of Load Time", EditorStyles.boldLabel);
            OptimizeLevel = (int)EditorGUILayout.Slider("Level", OptimizeLevel, 0, 3);
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            GUILayout.Label("Load Time", EditorStyles.boldLabel);
            if (OptimizeLevel == 0)         GUILayout.Label("Good");
            else if (OptimizeLevel == 1)    GUILayout.Label("Very Good");
            else if (OptimizeLevel == 2)    GUILayout.Label("Impressive");
            else if (OptimizeLevel == 3)    GUILayout.Label("Amazing");
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            GUILayout.Label("Type", EditorStyles.boldLabel);
            if (OptimizeLevel == 0)         GUILayout.Label("Optimize Mesh");
            else if (OptimizeLevel == 1)    GUILayout.Label("Optimize Mesh + Low Compression");
            else if (OptimizeLevel == 2)    GUILayout.Label("Optimize Mesh + Medium Compression");
            else if (OptimizeLevel == 3)    GUILayout.Label("Optimize Mesh + High Compression");
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            GUILayout.Label("Mesh Impact", EditorStyles.boldLabel);
            if (OptimizeLevel == 0)         GUILayout.Label("Nothing");
            else if (OptimizeLevel == 1)    GUILayout.Label("Low Degradation");
            else if (OptimizeLevel == 2)    GUILayout.Label("Medium Degradation");
            else if (OptimizeLevel == 3)    GUILayout.Label("High Degradation");
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Can Take Some Time", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Process", GUILayout.Width(100), GUILayout.Height(30)))
            {
                string AssetName = "";
                int countchild = CurrentSelect.transform.childCount;
                if (countchild > 0)
                {
                    MeshFilter[] T4MOBJPART = CurrentSelect.GetComponentsInChildren<MeshFilter>();
                    AssetName = AssetDatabase.GetAssetPath(T4MOBJPART[0].sharedMesh);
                    Debug.Log(AssetName);

                }
                else {
                    MeshFilter T4MOBJM = CurrentSelect.GetComponent<MeshFilter>();
                    AssetName = AssetDatabase.GetAssetPath(T4MOBJM.sharedMesh);
                }

                ModelImporter OBJI = ModelImporter.GetAtPath(AssetName) as ModelImporter;
                if (OptimizeLevel == 0)
                {
                    OBJI.optimizeMesh = true;
                }
                else if (OptimizeLevel == 1)
                {
                    OBJI.optimizeMesh = true;
                    OBJI.meshCompression = ModelImporterMeshCompression.Low;
                }
                else if (OptimizeLevel == 2)
                {
                    OBJI.optimizeMesh = true;
                    OBJI.meshCompression = ModelImporterMeshCompression.Medium;
                }
                else if (OptimizeLevel == 3)
                {
                    OBJI.optimizeMesh = true;
                    OBJI.meshCompression = ModelImporterMeshCompression.High;
                }
                AssetDatabase.ImportAsset(AssetName, ImportAssetOptions.ForceUpdate);
                PrefabUtility.RevertPrefabInstance(CurrentSelect.gameObject);
                AssetName = "";
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        else
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Please, select the T4M Object", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }

}
