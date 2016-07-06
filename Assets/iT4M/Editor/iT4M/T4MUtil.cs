/********************************************************************************
** Author： LiangZG
** Email :  game.liangzg@foxmail.com
*********************************************************************************/
using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 描述：T4M工具集
/// <para>创建时间：2016-07-06</para>
/// </summary>
public class T4MUtil  {


    public static void AddLayer(string[] layers)
    {
        UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>("ProjectSettings/TagManager.asset");
        SerializedObject layerMgr = new SerializedObject(obj);
        SerializedProperty it = layerMgr.GetIterator();
        foreach (string layer in layers)
        {
            if (HasLayer(layer)) return;

            it.Reset();
            while (it.NextVisible(true))
            {
                if (it.name.StartsWith("data") && string.IsNullOrEmpty(it.stringValue))
                {
                    it.stringValue = layer;
                    break;
                }
            }
        }
        layerMgr.ApplyModifiedProperties();
    }

    /// <summary>
    /// 添加指定索引的层次，layerIndex 从1开始
    /// </summary>
    /// <param name="newLayer"></param>
    /// <param name="layerIndex">实际位置的索引为layerIndex -1 </param>
    public static void AddLayer(string newLayer, int layerIndex)
    {
        if (HasLayer(newLayer)) return;

        UnityEngine.Object obj = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0];
        SerializedObject layerMgr = new SerializedObject(obj);
        SerializedProperty it = layerMgr.GetIterator();

        int index = 0;
        while (it.NextVisible(true))
        {           
            if (it.name.StartsWith("data"))
            {
                index++;
                if (layerIndex != index) continue;

                it.stringValue = newLayer;
                break;
            }
        }
        layerMgr.ApplyModifiedProperties();
    }


    public static bool HasLayer(string layer)
    {
        string[] layerArr = UnityEditorInternal.InternalEditorUtility.layers;
        foreach (string lay in layerArr)
        {
            if (!string.IsNullOrEmpty(lay) && lay.Trim() == layer.Trim()) return true;
        }
        return false;
    }

    public static bool HasTag(string tagName)
    {
        string[] tagArr = UnityEditorInternal.InternalEditorUtility.tags;
        foreach (string tag in tagArr)
        {
            if (!string.IsNullOrEmpty(tag) && tag.Trim() == tagName.Trim()) return true;
        }
        return false;
    }
}
