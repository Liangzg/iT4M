/********************************************************************************
** Author： LiangZG
** Email :  game.liangzg@foxmail.com
*********************************************************************************/
using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 描述：植物物件视图
/// <para>创建时间：2016-07-05</para>
/// </summary>
public class PlantingView  {

    private Transform CurrentSelect;

    GameObject AddObject;
    Texture[] TexObject = new Texture[6];

    public void OnGUI()
    {
        CurrentSelect = T4MMainEditor.CurrentSelect;
        T4MMainObj mainObjT4MMain = CurrentSelect.GetComponent<T4MMainObj>();
        if (mainObjT4MMain)
        {
            if (CurrentSelect.gameObject.GetComponent<T4MMainObj>().T4MMaterial && CurrentSelect.gameObject.GetComponent<T4MMainObj>().T4MMaterial.HasProperty("_Splat0") && CurrentSelect.gameObject.GetComponent<T4MMainObj>().T4MMaterial.HasProperty("_Splat1") && CurrentSelect.gameObject.GetComponent<T4MMainObj>().T4MMaterial.HasProperty("_Control"))
            {
                if (T4MCache.T4MRandomRot && T4MCache.T4MPlantMod == PlantMode.Follow_Normals)
                    T4MCache.T4MRandomRot = false;

                GUILayout.BeginVertical("box");
                GUILayout.BeginHorizontal();
                GUILayout.Label("Add to List", GUILayout.Width(105));
                GUILayout.FlexibleSpace();
                AddObject = (GameObject)EditorGUILayout.ObjectField("", AddObject, typeof(GameObject), true, GUILayout.Width(190));
                GUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();

                GUILayout.Label("", GUILayout.Width(1));
                if (GUILayout.Button((Texture)AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/down.png", typeof(Texture)), GUILayout.Width(53)))
                {
                    if (AddObject)
                        T4MCache.T4MObjectPlant[0] = AddObject;
                    else {
                        T4MCache.T4MObjectPlant[0] = null;
                    }
                    AddObject = null;
                }

                if (GUILayout.Button((Texture)AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/down.png", typeof(Texture)), GUILayout.Width(53)))
                {
                    if (AddObject)
                        T4MCache.T4MObjectPlant[1] = AddObject;
                    else {
                        T4MCache.T4MObjectPlant[1] = null;
                    }
                    AddObject = null;
                }

                if (GUILayout.Button((Texture)AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/down.png", typeof(Texture)), GUILayout.Width(53)))
                {
                    if (AddObject)
                        T4MCache.T4MObjectPlant[2] = AddObject;
                    else {
                        T4MCache.T4MObjectPlant[2] = null;
                    }
                    AddObject = null;
                }

                if (GUILayout.Button((Texture)AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/down.png", typeof(Texture)), GUILayout.Width(53)))
                {
                    if (AddObject)
                        T4MCache.T4MObjectPlant[3] = AddObject;
                    else {
                        T4MCache.T4MObjectPlant[3] = null;
                    }
                    AddObject = null;
                }

                if (GUILayout.Button((Texture)AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/down.png", typeof(Texture)), GUILayout.Width(53)))
                {
                    if (AddObject)
                        T4MCache.T4MObjectPlant[4] = AddObject;
                    else {
                        T4MCache.T4MObjectPlant[4] = null;
                    }
                    AddObject = null;
                }

                if (GUILayout.Button((Texture)AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/down.png", typeof(Texture)), GUILayout.Width(53)))
                {
                    if (AddObject)
                        T4MCache.T4MObjectPlant[5] = AddObject;
                    else {
                        T4MCache.T4MObjectPlant[5] = null;
                    }
                    AddObject = null;
                }

                EditorGUILayout.EndHorizontal();

                GUILayout.EndVertical();
                if (T4MCache.T4MObjectPlant[0] != null)
                    TexObject[0] = AssetPreview.GetAssetPreview(T4MCache.T4MObjectPlant[0]) as Texture;
                else TexObject[0] = null;
                if (T4MCache.T4MObjectPlant[1] != null)
                    TexObject[1] = AssetPreview.GetAssetPreview(T4MCache.T4MObjectPlant[1]) as Texture;
                else TexObject[1] = null;
                if (T4MCache.T4MObjectPlant[2] != null)
                    TexObject[2] = AssetPreview.GetAssetPreview(T4MCache.T4MObjectPlant[2]) as Texture;
                else TexObject[2] = null;
                if (T4MCache.T4MObjectPlant[3] != null)
                    TexObject[3] = AssetPreview.GetAssetPreview(T4MCache.T4MObjectPlant[3]) as Texture;
                else TexObject[3] = null;
                if (T4MCache.T4MObjectPlant[4] != null)
                    TexObject[4] = AssetPreview.GetAssetPreview(T4MCache.T4MObjectPlant[4]) as Texture;
                else TexObject[4] = null;
                if (T4MCache.T4MObjectPlant[5] != null)
                    TexObject[5] = AssetPreview.GetAssetPreview(T4MCache.T4MObjectPlant[5]) as Texture;
                else TexObject[5] = null;

                GUILayout.BeginHorizontal("box");
                GUILayout.FlexibleSpace();
                T4MConfig.T4MPlantSel = GUILayout.SelectionGrid(T4MConfig.T4MPlantSel, TexObject, 6, "gridlist", GUILayout.Width(340), GUILayout.Height(58));
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                //LayerMask	
                GUILayout.BeginVertical("box");
                GUILayout.BeginHorizontal();
                GUILayout.Label("", GUILayout.Width(22));
                T4MCache.T4MselectObj = 0;
                for (int i = 0; i < T4MCache.T4MBoolObj.Length; i++)
                {
                    if (T4MCache.T4MObjectPlant[i])
                    {
                        T4MCache.T4MBoolObj[i] = EditorGUILayout.Toggle(T4MCache.T4MBoolObj[i], GUILayout.Width(53));
                        if (T4MCache.T4MBoolObj[i] == true)
                            T4MCache.T4MselectObj += 1;
                    }
                    else GUILayout.Label("", GUILayout.Width(53));
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("", GUILayout.Width(2));
                for (int j = 0; j < T4MCache.T4MObjectPlant.Length; j++)
                {
                    if (T4MCache.T4MObjectPlant[j])
                    {
                        T4MConfig.ViewDistance[j] = (ViewD)EditorGUILayout.EnumPopup(T4MConfig.ViewDistance[j], GUILayout.Width(53));
                    }
                    else GUILayout.Label("", GUILayout.Width(53));
                }
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();


                T4MCache.T4MPlantMod = (PlantMode)EditorGUILayout.EnumPopup("Plant Mode", T4MCache.T4MPlantMod, GUILayout.Width(340));

                GUILayout.BeginVertical("box");
                GUILayout.Label("General", EditorStyles.boldLabel);
                EditorGUILayout.Space();
                T4MConfig.T4MObjSize = EditorGUILayout.Slider("LocalSize", T4MConfig.T4MObjSize, 0.1f, 4);
                T4MCache.T4MSizeVar = EditorGUILayout.Slider("Size Var.(Random +/-)", T4MCache.T4MSizeVar, 0, 0.5f);

                T4MConfig.T4MYOrigin = EditorGUILayout.Slider("Y Origin Corrector ", T4MConfig.T4MYOrigin, -10, 10);
                EditorGUILayout.Space();
                GUILayout.BeginHorizontal();
                GUILayout.Label("Create Collider", GUILayout.Width(120));
                T4MCache.T4MCreateColl = EditorGUILayout.Toggle(T4MCache.T4MCreateColl, GUILayout.Width(15));
                GUILayout.FlexibleSpace();
                GUILayout.Label("Static Object", GUILayout.Width(120));
                T4MCache.T4MStaticObj = EditorGUILayout.Toggle(T4MCache.T4MStaticObj, GUILayout.Width(30));
                GUILayout.EndHorizontal();
                EditorGUILayout.Space();
                GUILayout.BeginHorizontal();
                GUILayout.Label("Group Name", GUILayout.Width(150));
                T4MCache.T4MGroupName = GUILayout.TextField(T4MCache.T4MGroupName, 20, GUILayout.Width(120));
                GUILayout.EndHorizontal();
                EditorGUILayout.Space();
                GUILayout.EndVertical();
                EditorGUILayout.Space();
                GUILayout.BeginVertical("box");
                GUILayout.Label("Spacing Distances", EditorStyles.boldLabel, GUILayout.Width(150));
                EditorGUILayout.Space();
                GUILayout.BeginHorizontal();
                GUILayout.Label("Random Spacing", GUILayout.Width(150));
                T4MCache.T4MRandomSpa = EditorGUILayout.Toggle(T4MCache.T4MRandomSpa, GUILayout.Width(15));
                GUILayout.EndHorizontal();
                T4MConfig.T4MDistanceMin = EditorGUILayout.Slider("Safe Zone", T4MConfig.T4MDistanceMin, 0.1f, 50.0f);
                if (T4MCache.T4MRandomSpa)
                    T4MConfig.T4MDistanceMax = EditorGUILayout.Slider("Random Instance Zone", T4MConfig.T4MDistanceMax, 0.1f, 50.0f);
                GUILayout.EndVertical();

                if (T4MConfig.T4MDistanceMin > T4MConfig.T4MDistanceMax)
                    T4MConfig.T4MDistanceMax = T4MConfig.T4MDistanceMin;
                EditorGUILayout.Space();

                GUILayout.BeginVertical("box");
                GUILayout.BeginHorizontal();
                GUILayout.Label("Random Rotation(s)", EditorStyles.boldLabel, GUILayout.Width(150));

                T4MCache.T4MRandomRot = EditorGUILayout.Toggle(T4MCache.T4MRandomRot, GUILayout.Width(15));
                GUILayout.EndHorizontal();
                EditorGUILayout.Space();
                if (T4MCache.T4MRandomRot)
                {
                    T4MConfig.T4MrandX = EditorGUILayout.Slider("Random X:", T4MConfig.T4MrandX, 0, 1);
                    T4MConfig.T4MrandY = EditorGUILayout.Slider("Random Y:", T4MConfig.T4MrandY, 0, 1);
                    T4MConfig.T4MrandZ = EditorGUILayout.Slider("Random Z:", T4MConfig.T4MrandZ, 0, 1);
                }
                GUILayout.EndVertical();
            }
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
