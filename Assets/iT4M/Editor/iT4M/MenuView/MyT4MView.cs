/********************************************************************************
** Author： LiangZG
** Email :  game.liangzg@foxmail.com
*********************************************************************************/
using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;

/// <summary>
/// 描述：自定义T4M设置
/// <para>创建时间：2016-07-05</para>
/// </summary>
public class MyT4MView
{

    private Transform CurrentSelect;
    int EnumMyT4MV = 0;
    private string[] EnumMyT4M = { "T4M Settings", "ATS Mobile Foliage" };

    private Vector2 scroll;
    private bool isShaderCompatibility = true;
    public void OnGUI()
    {
        CurrentSelect = T4MMainEditor.CurrentSelect;
        T4MMainObj mainObjT4MMain = CurrentSelect.GetComponent<T4MMainObj>();
        if (!mainObjT4MMain)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Please, select the T4M Object", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            return;
        }
        
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EnumMyT4MV = GUILayout.Toolbar(EnumMyT4MV, EnumMyT4M, GUILayout.Width(290), GUILayout.Height(20));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        
        GUILayout.Label("Cleaning Scene", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Gen Terrain ", GUILayout.Height(20)))
        {
            T4MMainObj prev = CurrentSelect.GetComponent<T4MMainObj>();
            if (prev != null)
            {
                GameObject cloneObj = GameObject.Instantiate(CurrentSelect.gameObject);
                GameObject.DestroyImmediate(cloneObj.GetComponent<T4MMainObj>());

                CurrentSelect.gameObject.SetActive(false);

                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                EditorUtility.DisplayDialog("Gen Terrain ", "Gen Terrain Success !", "OK");
            }
        }

        if (GUILayout.Button("Clean MeshRenderers", GUILayout.Height(20)))
        {
            MeshRenderer[] prev = GameObject.FindObjectsOfType(typeof(MeshRenderer)) as MeshRenderer[];
            foreach (MeshRenderer go in prev)
            {
                if (go.hideFlags == HideFlags.HideInHierarchy)
                {
                    go.hideFlags = 0;
                    GameObject.DestroyImmediate(go.gameObject);
                }
            }
            EditorUtility.DisplayDialog("Scene Cleaned", "All MeshRenderer is Clear !", "OK");
        }
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();
        switch (EnumMyT4MV)
        {
            case 0:
                GUILayout.Label("Shader Model", EditorStyles.boldLabel);

                T4MCache.ShaderModel = (SM)EditorGUILayout.EnumPopup("Shader Model", T4MCache.ShaderModel, GUILayout.Width(340));


                if (T4MCache.ShaderModel == SM.ShaderModel1)
                {
                    T4MCache.MenuTextureSM1 = (EnumShaderGLES1)EditorGUILayout.EnumPopup("Shader", T4MCache.MenuTextureSM1, GUILayout.Width(340));
                }
                else if (T4MCache.ShaderModel == SM.ShaderModel2)
                {
                    T4MCache.MenuTextureSM2 = (EnumShaderGLES2)EditorGUILayout.EnumPopup("Shader", T4MCache.MenuTextureSM2, GUILayout.Width(340));
                }
                else if (T4MCache.ShaderModel == SM.ShaderModel3)
                    T4MCache.MenuTextureSM3 = (EnumShaderGLES3)EditorGUILayout.EnumPopup("Shader", T4MCache.MenuTextureSM3, GUILayout.Width(340));
                else T4MCache.CustomShader = EditorGUILayout.ObjectField("Select your Shader", T4MCache.CustomShader, typeof(Shader), true, GUILayout.Width(350)) as Shader;
                EditorGUILayout.Space();


                if (T4MCache.ShaderModel != SM.CustomShader)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Shader Compatibility", EditorStyles.boldLabel);
                    GUILayout.FlexibleSpace();
                    isShaderCompatibility = GUILayout.Toggle(isShaderCompatibility , "");
                    GUILayout.EndHorizontal();

                    if (isShaderCompatibility)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("GLES 1.1", GUILayout.Width(300));
                        if (T4MCache.ShaderModel != SM.ShaderModel3 && T4MCache.ShaderModel != SM.ShaderModel2)
                            GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/ok.png", typeof(Texture)) as Texture);
                        else
                            GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/ko.png", typeof(Texture)) as Texture);
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("GLES 2", GUILayout.Width(300));
                        if ((T4MCache.ShaderModel == SM.ShaderModel1) || (T4MCache.ShaderModel != SM.ShaderModel3) && (T4MCache.ShaderModel == SM.ShaderModel2 && T4MCache.MenuTextureSM2 != EnumShaderGLES2.T4M_6_Textures_HighSpec) && (T4MCache.ShaderModel == SM.ShaderModel2 && T4MCache.MenuTextureSM2 != EnumShaderGLES2.T4M_4_Textures_Bumped)
                                && (T4MCache.ShaderModel == SM.ShaderModel2 && T4MCache.MenuTextureSM2 != EnumShaderGLES2.T4M_5_Textures_HighSpec) && (T4MCache.ShaderModel == SM.ShaderModel2 && T4MCache.MenuTextureSM2 != EnumShaderGLES2.T4M_3_Textures_Bumped_DirectionalLM) && (T4MCache.ShaderModel == SM.ShaderModel2 && T4MCache.MenuTextureSM2 != EnumShaderGLES2.T4M_6_Textures_Unlit_Lightmap_Compatible)
                                && (T4MCache.ShaderModel == SM.ShaderModel2 && T4MCache.MenuTextureSM2 != EnumShaderGLES2.T4M_World_Projection_HighSpec)
                                && (T4MCache.ShaderModel == SM.ShaderModel2 && T4MCache.MenuTextureSM2 != EnumShaderGLES2.T4M_World_Projection_Unlit_Lightmap_Compatible))
                        {

                            GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/ok.png", typeof(Texture)) as Texture);
                        }
                        else {
                            GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/ko.png", typeof(Texture)) as Texture);
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Desktop", GUILayout.Width(300));
                        GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/ok.png", typeof(Texture)) as Texture);
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Unity WebPlayer", GUILayout.Width(300));
                        GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/ok.png", typeof(Texture)) as Texture);

                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Flash", GUILayout.Width(300));
                        if ((T4MCache.ShaderModel == SM.ShaderModel1) || (T4MCache.ShaderModel != SM.ShaderModel3) && (T4MCache.ShaderModel == SM.ShaderModel2 && T4MCache.MenuTextureSM2 != EnumShaderGLES2.T4M_6_Textures_HighSpec)
                                && (T4MCache.ShaderModel == SM.ShaderModel2 && T4MCache.MenuTextureSM2 != EnumShaderGLES2.T4M_5_Textures_HighSpec) && (T4MCache.ShaderModel == SM.ShaderModel2 && T4MCache.MenuTextureSM2 != EnumShaderGLES2.T4M_4_Textures_Bumped)
                                && (T4MCache.ShaderModel == SM.ShaderModel2 && T4MCache.MenuTextureSM2 != EnumShaderGLES2.T4M_3_Textures_Bumped)
                                && (T4MCache.ShaderModel == SM.ShaderModel2 && T4MCache.MenuTextureSM2 != EnumShaderGLES2.T4M_3_Textures_Bumped_SPEC)
                                && (T4MCache.ShaderModel == SM.ShaderModel2 && T4MCache.MenuTextureSM2 != EnumShaderGLES2.T4M_6_Textures_Unlit_Lightmap_Compatible)
                                && (T4MCache.ShaderModel == SM.ShaderModel2 && T4MCache.MenuTextureSM2 != EnumShaderGLES2.T4M_World_Projection_HighSpec)
                                && (T4MCache.ShaderModel == SM.ShaderModel2 && T4MCache.MenuTextureSM2 != EnumShaderGLES2.T4M_World_Projection_Unlit_Lightmap_Compatible)
                                )
                            GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/ok.png", typeof(Texture)) as Texture);
                        else
                            GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/ko.png", typeof(Texture)) as Texture);
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("NaCI", GUILayout.Width(300));
                        if ((T4MCache.ShaderModel == SM.ShaderModel1) || (T4MCache.ShaderModel != SM.ShaderModel3) && (T4MCache.ShaderModel == SM.ShaderModel2 && T4MCache.MenuTextureSM2 != EnumShaderGLES2.T4M_6_Textures_HighSpec)
                                && (T4MCache.ShaderModel == SM.ShaderModel2 && T4MCache.MenuTextureSM2 != EnumShaderGLES2.T4M_5_Textures_HighSpec) && (T4MCache.ShaderModel == SM.ShaderModel2 && T4MCache.MenuTextureSM2 != EnumShaderGLES2.T4M_4_Textures_Bumped) && (T4MCache.ShaderModel == SM.ShaderModel2 && T4MCache.MenuTextureSM2 != EnumShaderGLES2.T4M_6_Textures_Unlit_Lightmap_Compatible))
                            GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/ok.png", typeof(Texture)) as Texture);
                        else
                            GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/ko.png", typeof(Texture)) as Texture);
                        GUILayout.EndHorizontal();
                    }
                }

                EditorGUILayout.Space();


                GUILayout.BeginHorizontal();
                GUILayout.Label("Master T4M Object", EditorStyles.boldLabel, GUILayout.Width(150));


                T4MCache.T4MMaster = EditorGUILayout.Toggle(T4MCache.T4MMaster);

                GUILayout.EndHorizontal();

                if (T4MCache.T4MMaster)
                {
                    masterGUI();
                }
                GUILayout.FlexibleSpace();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("UPDATE", GUILayout.Width(100), GUILayout.Height(25)))
                {
                    MyT4MApplyChange();

                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();
                break;
            case 1:
                atsFoliageWindActivationGUI();
                break;
        }
        
    }

    /// <summary>
    /// Master Obj GUI选项
    /// </summary>
    private void masterGUI()
    {
        
        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        GUILayout.Label("Scene Camera", EditorStyles.boldLabel, GUILayout.Width(190));
        T4MCache.PlayerCam = EditorGUILayout.ObjectField(T4MCache.PlayerCam, typeof(Transform), true) as Transform;
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Activate LOD System  ", EditorStyles.boldLabel, GUILayout.Width(190));
        T4MConfig.ActivatedLOD = EditorGUILayout.Toggle(T4MConfig.ActivatedLOD);
        GUILayout.Label("   Editor Preview", EditorStyles.boldLabel, GUILayout.Width(120));
        CurrentSelect.GetComponent<T4MMainObj>().LODPreview = EditorGUILayout.Toggle(CurrentSelect.GetComponent<T4MMainObj>().LODPreview);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Activate Billboard System  ", EditorStyles.boldLabel, GUILayout.Width(190));
        T4MConfig.ActivatedBillboard = EditorGUILayout.Toggle(T4MConfig.ActivatedBillboard);
        GUILayout.Label("   Editor Preview", EditorStyles.boldLabel, GUILayout.Width(120));
        CurrentSelect.GetComponent<T4MMainObj>().BillboardPreview = EditorGUILayout.Toggle(CurrentSelect.GetComponent<T4MMainObj>().BillboardPreview);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Activate LayerCullDistance  ", EditorStyles.boldLabel, GUILayout.Width(190));
        T4MConfig.ActivatedLayerCul = EditorGUILayout.Toggle(T4MConfig.ActivatedLayerCul);
        GUILayout.Label("   Editor Preview", EditorStyles.boldLabel, GUILayout.Width(120));
        CurrentSelect.GetComponent<T4MMainObj>().LayerCullPreview = EditorGUILayout.Toggle(CurrentSelect.GetComponent<T4MMainObj>().LayerCullPreview);
        GUILayout.EndHorizontal();


        EditorGUILayout.Space();
        if (T4MConfig.ActivatedLayerCul)
        {
            GUILayout.BeginVertical("box");

            GUILayout.Label("Maximum distances of view", EditorStyles.boldLabel, GUILayout.Width(220));
            T4MConfig.CloseDistMaxView = EditorGUILayout.Slider("Close Distance", T4MConfig.CloseDistMaxView, 0, 500);
            T4MConfig.NormalDistMaxView = EditorGUILayout.Slider("Middle Distance", T4MConfig.NormalDistMaxView, 0, 500);
            T4MConfig.FarDistMaxView = EditorGUILayout.Slider("Far Distance", T4MConfig.FarDistMaxView, 0, 500);
            T4MConfig.BGDistMaxView = EditorGUILayout.Slider("BackGround Distance", T4MConfig.BGDistMaxView, 0, 10000);
            GUILayout.EndVertical();
        }

        if (T4MConfig.BGDistMaxView < T4MConfig.FarDistMaxView)
            T4MConfig.BGDistMaxView = T4MConfig.FarDistMaxView;
        else if (T4MConfig.FarDistMaxView < T4MConfig.NormalDistMaxView)
            T4MConfig.FarDistMaxView = T4MConfig.NormalDistMaxView;
        else if (T4MConfig.NormalDistMaxView < T4MConfig.CloseDistMaxView)
            T4MConfig.NormalDistMaxView = T4MConfig.CloseDistMaxView;
        GUILayout.EndVertical();
    }

    private void atsFoliageWindActivationGUI()
    {
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        GUILayout.Label("ATS Foliage Wind Activation", EditorStyles.boldLabel, GUILayout.Width(220));
        CurrentSelect.gameObject.GetComponent<T4MMainObj>().ActiveWind = EditorGUILayout.Toggle(CurrentSelect.gameObject.GetComponent<T4MMainObj>().ActiveWind);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();
        GUILayout.BeginVertical("box");
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Download The Package", GUILayout.Width(160), GUILayout.Height(15)))
        {
            Application.OpenURL("http://u3d.as/content/forst/ats-mobile-foliage/2XM");
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        CurrentSelect.gameObject.GetComponent<T4MMainObj>().TranslucencyColor = EditorGUILayout.ColorField("Translucency Color ", CurrentSelect.gameObject.GetComponent<T4MMainObj>().TranslucencyColor);
        EditorGUILayout.Space();
        CurrentSelect.gameObject.GetComponent<T4MMainObj>().Wind = EditorGUILayout.Vector4Field("Wind Vector", CurrentSelect.gameObject.GetComponent<T4MMainObj>().Wind);
        EditorGUILayout.Space();
        CurrentSelect.gameObject.GetComponent<T4MMainObj>().WindFrequency = EditorGUILayout.Slider("Wind Frequency", CurrentSelect.gameObject.GetComponent<T4MMainObj>().WindFrequency, 0, 5);
        EditorGUILayout.Space();
        CurrentSelect.gameObject.GetComponent<T4MMainObj>().GrassWindFrequency = EditorGUILayout.Slider("Grass Wind Frequency", CurrentSelect.gameObject.GetComponent<T4MMainObj>().GrassWindFrequency, 0, 5);
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Reset", GUILayout.Width(100), GUILayout.Height(15)))
        {
            CurrentSelect.gameObject.GetComponent<T4MMainObj>().TranslucencyColor = new Color(0.73f, 0.85f, 0.4f, 1f);
            CurrentSelect.gameObject.GetComponent<T4MMainObj>().Wind = new Vector4(0.85f, 0.075f, 0.4f, 0.5f);
            CurrentSelect.gameObject.GetComponent<T4MMainObj>().WindFrequency = 0.75f;
            CurrentSelect.gameObject.GetComponent<T4MMainObj>().GrassWindFrequency = 1.5f;
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("UPDATE", GUILayout.Width(100), GUILayout.Height(30)))
        {
            MyT4MApplyChange();

        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("By Forst (Lars)", EditorStyles.boldLabel, GUILayout.Width(105), GUILayout.Height(15));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Others Assets by Forst", "textarea", GUILayout.Width(140), GUILayout.Height(15)))
        {
            Application.OpenURL("http://u3d.as/publisher/forst/1Lf");
        }
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();
    }
    /// <summary>
    /// 应该Shader修改
    /// </summary>
    void MyT4MApplyChange()
    {
        T4MMainObj curMainObjT4MMain = CurrentSelect.gameObject.GetComponent<T4MMainObj>();
        if (T4MCache.ShaderModel == SM.ShaderModel1)
        {
            //Diffuse SM1 
            if (T4MCache.MenuTextureSM1 == EnumShaderGLES1.T4M_2_Textures_Auto_BeastLM_2DrawCall)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel1/T4M 2 Textures Auto BeastLM 2DrawCall");
            }
            else
            if (T4MCache.MenuTextureSM1 == EnumShaderGLES1.T4M_2_Textures_ManualAdd_BeastLM_1DC)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel1/T4M 2 Textures ManualAdd BeastLM_1DC");
            }
            else
            if (T4MCache.MenuTextureSM1 == EnumShaderGLES1.T4M_2_Textures_ManualAdd_CustoLM_1DC)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel1/T4M 2 Textures ManualAdd CustoLM 1DC");
            }
        }
        else if (T4MCache.ShaderModel == SM.ShaderModel2)
        {
            //Unlit SM2
            if (T4MCache.MenuTextureSM2 == EnumShaderGLES2.T4M_2_Textures_Unlit_Lightmap_Compatible)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel2/Unlit/T4M 2 Textures Unlit LM");
            }
            else if (T4MCache.MenuTextureSM2 == EnumShaderGLES2.T4M_3_Textures_Unlit_Lightmap_Compatible)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel2/Unlit/T4M 3 Textures Unlit LM");
            }
            else if (T4MCache.MenuTextureSM2 == EnumShaderGLES2.T4M_4_Textures_Unlit_Lightmap_Compatible)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel2/Unlit/T4M 4 Textures Unlit LM");
            }
            else if (T4MCache.MenuTextureSM2 == EnumShaderGLES2.T4M_5_Textures_Unlit_Lightmap_Compatible)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel2/Unlit/T4M 5 Textures Unlit LM");
            }
            else if (T4MCache.MenuTextureSM2 == EnumShaderGLES2.T4M_6_Textures_Unlit_Lightmap_Compatible)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel2/Unlit/T4M 6 Textures Unlit LM");
            }
            else if (T4MCache.MenuTextureSM2 == EnumShaderGLES2.T4M_6_Textures_Unlit_No_Lightmap)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel2/Unlit/T4M 6 Textures Unlit NoLM");
            }
            else if (T4MCache.MenuTextureSM2 == EnumShaderGLES2.T4M_World_Projection_Unlit_Lightmap_Compatible)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel2/Unlit/T4M World Projection Shader + LM");
            }

            //Diffuse SM2
            else if (T4MCache.MenuTextureSM2 == EnumShaderGLES2.T4M_2_Textures_HighSpec)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel2/Diffuse/T4M 2 Textures");
            }
            else if (T4MCache.MenuTextureSM2 == EnumShaderGLES2.T4M_3_Textures_HighSpec)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel2/Diffuse/T4M 3 Textures");
            }
            else if (T4MCache.MenuTextureSM2 == EnumShaderGLES2.T4M_4_Textures_HighSpec)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel2/Diffuse/T4M 4 Textures");
            }
            else if (T4MCache.MenuTextureSM2 == EnumShaderGLES2.T4M_5_Textures_HighSpec)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel2/Diffuse/T4M 5 Textures");
            }
            else if (T4MCache.MenuTextureSM2 == EnumShaderGLES2.T4M_6_Textures_HighSpec)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel2/Diffuse/T4M 6 Textures");
            }
            else if (T4MCache.MenuTextureSM2 == EnumShaderGLES2.T4M_World_Projection_HighSpec)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel2/Diffuse/T4M World Projection Shader");
            }

            //Specular SM2
            else if (T4MCache.MenuTextureSM2 == EnumShaderGLES2.T4M_2_Textures_Specular)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel2/Specular/T4M 2 Textures Spec");
            }
            else if (T4MCache.MenuTextureSM2 == EnumShaderGLES2.T4M_3_Textures_Specular)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel2/Specular/T4M 3 Textures Spec");
            }
            else if (T4MCache.MenuTextureSM2 == EnumShaderGLES2.T4M_4_Textures_Specular)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel2/Specular/T4M 4 Textures Spec");
            }


            //4 mobile lightmap SM2
            else if (T4MCache.MenuTextureSM2 == EnumShaderGLES2.T4M_2_Textures_4_Mobile)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel2/MobileLM/T4M 2 Textures for Mobile");
            }
            else if (T4MCache.MenuTextureSM2 == EnumShaderGLES2.T4M_3_Textures_4_Mobile)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel2/MobileLM/T4M 3 Textures for Mobile");
            }
            else if (T4MCache.MenuTextureSM2 == EnumShaderGLES2.T4M_4_Textures_4_Mobile)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel2/MobileLM/T4M 4 Textures for Mobile");
            }//else if (T4MCache.MenuTextureSM2 == EnumShaderGLES2.T4M_World_Projection_Mobile){			
             //	CurrentSelect.gameObject.GetComponent <T4MMainObj>().T4MMaterial.shader =  Shader.Find("iT4MShaders/ShaderModel2/MobileLM/T4M World Projection Shader_Mobile");
             //}

            //Toon SM2
            else if (T4MCache.MenuTextureSM2 == EnumShaderGLES2.T4M_2_Textures_Toon)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel2/Toon/T4M 2 Textures Toon");
                Cubemap ToonShade = (Cubemap)AssetDatabase.LoadAssetAtPath(T4MConfig.T4MFolder + "Shaders/Sources/toony lighting.psd", typeof(Cubemap));
                curMainObjT4MMain.T4MMaterial.SetTexture("_ToonShade", ToonShade);
            }
            else if (T4MCache.MenuTextureSM2 == EnumShaderGLES2.T4M_3_Textures_Toon)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel2/Toon/T4M 3 Textures Toon");
                Cubemap ToonShade = (Cubemap)AssetDatabase.LoadAssetAtPath(T4MConfig.T4MFolder + "Shaders/Sources/toony lighting.psd", typeof(Cubemap));
                curMainObjT4MMain.T4MMaterial.SetTexture("_ToonShade", ToonShade);
            }
            else if (T4MCache.MenuTextureSM2 == EnumShaderGLES2.T4M_4_Textures_Toon)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel2/Toon/T4M 4 Textures Toon");
                Cubemap ToonShade = (Cubemap)AssetDatabase.LoadAssetAtPath(T4MConfig.T4MFolder + "Shaders/Sources/toony lighting.psd", typeof(Cubemap));
                curMainObjT4MMain.T4MMaterial.SetTexture("_ToonShade", ToonShade);
            }

            //Bumped SM2
            else if (T4MCache.MenuTextureSM2 == EnumShaderGLES2.T4M_2_Textures_Bumped)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel2/Bump/T4M 2 Textures Bumped");
            }
            else if (T4MCache.MenuTextureSM2 == EnumShaderGLES2.T4M_3_Textures_Bumped)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel2/Bump/T4M 3 Textures Bumped");
            }
            else if (T4MCache.MenuTextureSM2 == EnumShaderGLES2.T4M_4_Textures_Bumped)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel2/Bump/T4M 4 Textures Bumped");
            }

            //Mobile Bumped
            else if (T4MCache.MenuTextureSM2 == EnumShaderGLES2.T4M_2_Textures_Bumped_Mobile)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel2/Bump/T4M 2 Textures Bumped Mobile");
            }
            else if (T4MCache.MenuTextureSM2 == EnumShaderGLES2.T4M_3_Textures_Bumped_Mobile)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel2/Bump/T4M 3 Textures Bumped Mobile");
            }

            //Mobile Bumped Spec
            else if (T4MCache.MenuTextureSM2 == EnumShaderGLES2.T4M_2_Textures_Bumped_SPEC_Mobile)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel2/Bump/T4M 2 Textures Bump Specular Mobile");
            }

            //Mobile Bump LM
            else if (T4MCache.MenuTextureSM2 == EnumShaderGLES2.T4M_2_Textures_Bumped_DirectionalLM_Mobile)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel2/BumpDLM/T4M 2 Textures Bumped DLM Mobile");
            }



            //Bump Spec SM2
            else if (T4MCache.MenuTextureSM2 == EnumShaderGLES2.T4M_2_Textures_Bumped_SPEC)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel2/Bump/T4M 2 Textures Bump Specular");
            }
            else if (T4MCache.MenuTextureSM2 == EnumShaderGLES2.T4M_3_Textures_Bumped_SPEC)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel2/Bump/T4M 3 Textures Bump Specular");
            }
            //Bump LM SM2
            else if (T4MCache.MenuTextureSM2 == EnumShaderGLES2.T4M_2_Textures_Bumped_DirectionalLM)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel2/BumpDLM/T4M 2 Textures Bumped DLM");
            }
            else if (T4MCache.MenuTextureSM2 == EnumShaderGLES2.T4M_3_Textures_Bumped_DirectionalLM)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel2/BumpDLM/T4M 3 Textures Bumped DLM");
            }

        }
        else if (T4MCache.ShaderModel == SM.ShaderModel3)
        {
            //Diffuse SM3
            if (T4MCache.MenuTextureSM3 == EnumShaderGLES3.T4M_2_Textures_Diffuse)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel3/Diffuse/T4M 2 Textures");
            }
            else if (T4MCache.MenuTextureSM3 == EnumShaderGLES3.T4M_3_Textures_Diffuse)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel3/Diffuse/T4M 3 Textures");
            }
            else if (T4MCache.MenuTextureSM3 == EnumShaderGLES3.T4M_4_Textures_Diffuse)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel3/Diffuse/T4M 4 Textures");
            }
            else if (T4MCache.MenuTextureSM3 == EnumShaderGLES3.T4M_5_Textures_Diffuse)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel3/Diffuse/T4M 5 Textures");
            }
            else if (T4MCache.MenuTextureSM3 == EnumShaderGLES3.T4M_6_Textures_Diffuse)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel3/Diffuse/T4M 6 Textures");
            }

            //Specular
            else if (T4MCache.MenuTextureSM3 == EnumShaderGLES3.T4M_2_Textures_Specular)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel3/Specular/T4M 2 Textures Spec");
            }
            else if (T4MCache.MenuTextureSM3 == EnumShaderGLES3.T4M_3_Textures_Specular)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel3/Specular/T4M 3 Textures Spec");
            }
            else if (T4MCache.MenuTextureSM3 == EnumShaderGLES3.T4M_4_Textures_Specular)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel3/Specular/T4M 4 Textures Spec");
            }

            //Bumped SM3
            else if (T4MCache.MenuTextureSM3 == EnumShaderGLES3.T4M_2_Textures_Bumped)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel3/Bump/T4M 2 Textures Bump");
            }
            else if (T4MCache.MenuTextureSM3 == EnumShaderGLES3.T4M_3_Textures_Bumped)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel3/Bump/T4M 3 Textures Bump");
            }
            else if (T4MCache.MenuTextureSM3 == EnumShaderGLES3.T4M_4_Textures_Bumped)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel3/Bump/T4M 4 Textures Bump");
            }

            //Bump Spec SM3
            else if (T4MCache.MenuTextureSM3 == EnumShaderGLES3.T4M_2_Textures_Bumped_SPEC)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel3/BumpSpec/T4M 2 Textures Bump Spec");
            }
            else if (T4MCache.MenuTextureSM3 == EnumShaderGLES3.T4M_3_Textures_Bumped_SPEC)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel3/BumpSpec/T4M 3 Textures Bump Spec");
            }
            else if (T4MCache.MenuTextureSM3 == EnumShaderGLES3.T4M_4_Textures_Bumped_SPEC)
            {
                curMainObjT4MMain.T4MMaterial.shader = Shader.Find("iT4MShaders/ShaderModel3/BumpSpec/T4M 4 Textures Bump Spec");
            }

        }
        else {
            Material temp = new Material(T4MCache.CustomShader);
            if (T4MCache.CustomShader != null && temp.HasProperty("_Control") && temp.HasProperty("_Splat0") && temp.HasProperty("_Splat1"))
            {
                curMainObjT4MMain.T4MMaterial.shader = T4MCache.CustomShader;

            }
            else EditorUtility.DisplayDialog("T4M Message", "This Shader is not compatible", "OK");
        }



        if (curMainObjT4MMain.T4MMaterial.HasProperty("_Control2"))
        {
            Texture Control2;
            if (curMainObjT4MMain.T4MMaterial.GetTexture("_Control") != null)
            {
                Control2 = AssetDatabase.LoadAssetAtPath<Texture>(T4MConfig.T4MPrefabFolder + "Terrains/Texture/" + curMainObjT4MMain.T4MMaterial.GetTexture("_Control").name + "C2.png");
            }
            else Control2 = AssetDatabase.LoadAssetAtPath<Texture>(T4MConfig.T4MPrefabFolder + "Terrains/Texture/" + CurrentSelect.gameObject.name + "C2.png");

            if (Control2 == null)
                CreateControl2Text();
            else curMainObjT4MMain.T4MMaterial.SetTexture("_Control2", Control2);
        }



        if (curMainObjT4MMain.T4MMaterial.HasProperty("_Splat0"))
            curMainObjT4MMain.T4MMaterial.SetTexture("_Splat0", T4MConfig.Layer1);

        if (curMainObjT4MMain.T4MMaterial.HasProperty("_Splat1"))
            curMainObjT4MMain.T4MMaterial.SetTexture("_Splat1", T4MConfig.Layer2);

        if (curMainObjT4MMain.T4MMaterial.HasProperty("_Splat2"))
        {
            curMainObjT4MMain.T4MMaterial.SetTexture("_Splat2", T4MConfig.Layer3);
        }
        if (curMainObjT4MMain.T4MMaterial.HasProperty("_Splat3"))
        {
            curMainObjT4MMain.T4MMaterial.SetTexture("_Splat3", T4MConfig.Layer4);
        }
        if (curMainObjT4MMain.T4MMaterial.HasProperty("_Splat4"))
        {
            curMainObjT4MMain.T4MMaterial.SetTexture("_Splat4", T4MConfig.Layer5);
        }
        if (curMainObjT4MMain.T4MMaterial.HasProperty("_Splat5"))
        {
            curMainObjT4MMain.T4MMaterial.SetTexture("_Splat5", T4MConfig.Layer6);
        }
        if (curMainObjT4MMain.T4MMaterial.HasProperty("_BumpSplat0"))
        {
            curMainObjT4MMain.T4MMaterial.SetTexture("_BumpSplat0", T4MConfig.Layer1Bump);
        }
        if (curMainObjT4MMain.T4MMaterial.HasProperty("_BumpSplat1"))
        {
            curMainObjT4MMain.T4MMaterial.SetTexture("_BumpSplat1", T4MConfig.Layer2Bump);
        }
        if (curMainObjT4MMain.T4MMaterial.HasProperty("_BumpSplat2"))
        {
            curMainObjT4MMain.T4MMaterial.SetTexture("_BumpSplat2", T4MConfig.Layer3Bump);
        }
        if (curMainObjT4MMain.T4MMaterial.HasProperty("_BumpSplat3"))
        {
            curMainObjT4MMain.T4MMaterial.SetTexture("_BumpSplat3", T4MConfig.Layer4Bump);
        }
        if (T4MCache.T4MMaster)
        {
            curMainObjT4MMain.EnabledLODSystem = T4MConfig.ActivatedLOD;
            curMainObjT4MMain.enabledBillboard = T4MConfig.ActivatedBillboard;
            curMainObjT4MMain.enabledLayerCul = T4MConfig.ActivatedLayerCul;
            curMainObjT4MMain.CloseView = T4MConfig.CloseDistMaxView;
            curMainObjT4MMain.NormalView = T4MConfig.NormalDistMaxView;
            curMainObjT4MMain.FarView = T4MConfig.FarDistMaxView;
            curMainObjT4MMain.BackGroundView = T4MConfig.BGDistMaxView;
            curMainObjT4MMain.Master = 1;
            curMainObjT4MMain.PlayerCamera = T4MCache.PlayerCam;
            PrefabUtility.RecordPrefabInstancePropertyModifications(curMainObjT4MMain);
        }
        else {
            curMainObjT4MMain.EnabledLODSystem = false;
            curMainObjT4MMain.enabledBillboard = false;
            curMainObjT4MMain.enabledLayerCul = false;
            curMainObjT4MMain.Master = 0;

            T4MLod[] T4MLodObjGet = GameObject.FindObjectsOfType(typeof(T4MLod)) as T4MLod[];
            for (var i = 0; i < T4MLodObjGet.Length; i++)
            {
                T4MLodObjGet[i].LOD2.enabled = T4MLodObjGet[i].LOD3.enabled = false;
                T4MLodObjGet[i].LOD1.enabled = true;
                if (T4MCache.LODModeControler == LODMod.Mass_Control)
                    T4MLodObjGet[i].Mode = 0;
                else if (T4MCache.LODModeControler == LODMod.Independent_Control)
                    T4MLodObjGet[i].Mode = 0;
                PrefabUtility.RecordPrefabInstancePropertyModifications(T4MLodObjGet[i].GetComponent<T4MLod>());
            }
            curMainObjT4MMain.ObjLodScript = new T4MLod[0];
            curMainObjT4MMain.ObjPosition = new Vector3[0];
            curMainObjT4MMain.ObjLodStatus = new int[0];
            curMainObjT4MMain.Mode = 0;

            T4MBillboard[] T4MBillObjGet = GameObject.FindObjectsOfType(typeof(T4MBillboard)) as T4MBillboard[];
            for (var i = 0; i < T4MBillObjGet.Length; i++)
            {
                T4MBillObjGet[i].Render.enabled = true;
                T4MBillObjGet[i].Transf.LookAt(Vector3.zero, Vector3.up);
            }
            curMainObjT4MMain.BillboardPosition = new Vector3[0];
            curMainObjT4MMain.BillStatus = new int[0];
            curMainObjT4MMain.BillScript = new T4MBillboard[0];
            PrefabUtility.RecordPrefabInstancePropertyModifications(curMainObjT4MMain);
        }
        T4MCache.TexTexture = null;
        T4MMainEditor.Instance.IniNewSelect();
    }


    void CreateControl2Text()
    {
        Texture2D Control2 = new Texture2D(512, 512, TextureFormat.ARGB32, true);
        Color[] ColorBase = new Color[512 * 512];
        for (var t = 0; t < ColorBase.Length; t++)
        {
            ColorBase[t] = new Color(1, 0, 0, 0);
        }

        Control2.SetPixels(ColorBase);
        string path;
        T4MMainObj mainObjT4MMain = CurrentSelect.gameObject.GetComponent<T4MMainObj>();
        if (mainObjT4MMain.T4MMaterial.GetTexture("_Control") != null)
        {
            path = T4MConfig.T4MPrefabFolder + "Terrains/Texture/" + mainObjT4MMain.T4MMaterial.GetTexture("_Control").name + "C2.png";

        }
        else path = T4MConfig.T4MPrefabFolder + "Terrains/Texture/" + CurrentSelect.gameObject.name + "C2.png";
        byte[] data = Control2.EncodeToPNG();
        File.WriteAllBytes(path, data);
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

        TextureImporter TextureI = AssetImporter.GetAtPath(path) as TextureImporter;
        TextureI.textureCompression = TextureImporterCompression.Uncompressed;
        TextureI.isReadable = true;
        TextureI.anisoLevel = 9;
        TextureI.mipmapEnabled = false;
        TextureI.wrapMode = TextureWrapMode.Clamp;
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

        Texture Contr2 = (Texture)AssetDatabase.LoadAssetAtPath(path, typeof(Texture));
        mainObjT4MMain.T4MMaterial.SetTexture("_Control2", Contr2);
        T4MMainEditor.Instance.IniNewSelect();
    }
}
