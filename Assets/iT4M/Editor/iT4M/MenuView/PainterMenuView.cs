/********************************************************************************
** Author： LiangZG
** Email :  game.liangzg@foxmail.com
*********************************************************************************/
using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 描述：绘制笔刷选项视图
/// <para>创建时间：</para>
/// </summary>
public class PainterMenuView
{

    private Transform CurrentSelect;

    int MyT4MV = 0;
    string[] MyT4MMen = { "Painter", "Materials" };

    int selProcedural = 0;
    ProceduralMaterial PreceduralAdd;
    ProceduralMaterial Precedural;

    Texture MaterialAdd;
    MaterialType MaterialTyp = MaterialType.Classic;
    Vector2 scrollPos;

    int selBrush = 0;
    Texture[] TexBrush;
    
    bool joinTiles = true;

    int oldSelBrush;
    int layerMask = 1 << 30;
    int oldBrushSizeInPourcent;
    int oldselTexture;

    public void OnGUI()
    {
        CurrentSelect = T4MMainEditor.CurrentSelect;
        T4MMainObj selectMainObj = CurrentSelect.GetComponent<T4MMainObj>();
        if (!selectMainObj)
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
        MyT4MV = GUILayout.Toolbar(MyT4MV, MyT4MMen, GUILayout.Width(290), GUILayout.Height(20));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        switch (MyT4MV)
        {
            case 0:
                if (selectMainObj.T4MMaterial.shader != Shader.Find("iT4MShaders/ShaderModel2/Unlit/T4M World Projection Shader + LM") &&
                    selectMainObj.T4MMaterial.shader != Shader.Find("iT4MShaders/ShaderModel2/Diffuse/T4M World Projection Shader") &&
                    selectMainObj.T4MMaterial.shader != Shader.Find("iT4MShaders/ShaderModel2/MobileLM/T4M World Projection Shader_Mobile") &&
                    !selectMainObj.T4MMaterial.HasProperty("_Tiling"))
                {
                    PixelPainterMenu();
                }
                else ProjectionWorldConfig();
                break;
            case 1:
                if (selectMainObj.T4MMaterial && selectMainObj.T4MMaterial.HasProperty("_Splat0") && selectMainObj.T4MMaterial.HasProperty("_Splat1") && 
                    selectMainObj.T4MMaterial.HasProperty("_Control"))
                {

                    EditorGUILayout.Space();
                    InitPincil();
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    GUILayout.BeginHorizontal("box", GUILayout.Width(310));
                    GUILayout.FlexibleSpace();

                    selProcedural = GUILayout.SelectionGrid(selProcedural, T4MCache.TexTexture, 6, "gridlist", GUILayout.Width(340),
                                                            GUILayout.Height(58));

                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.Label("Add / Replace / Substances Update", EditorStyles.boldLabel);
                    EditorGUILayout.BeginVertical("box");
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("", GUILayout.Width(3));
                    if (GUILayout.Button((Texture)AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/up.png", typeof(Texture)), GUILayout.Width(53)))
                    {
                        if (!PreceduralAdd && !MaterialAdd && Precedural)
                            PreceduralAdd = Precedural;

                        if (PreceduralAdd)
                        {
                            selectMainObj.T4MMaterial.SetTexture("_Splat0", PreceduralAdd.GetTexture("_MainTex"));
                            if (selectMainObj.T4MMaterial.HasProperty("_BumpSplat0"))
                                selectMainObj.T4MMaterial.SetTexture("_BumpSplat0", PreceduralAdd.GetTexture("_BumpMap"));
                        }
                        else if (MaterialAdd)
                        {
                            selectMainObj.T4MMaterial.SetTexture("_Splat0", MaterialAdd);
                        }
                        selProcedural = 0;
                        PreceduralAdd = null;
                        MaterialAdd = null;
                        T4MMainEditor.Instance.IniNewSelect();
                    }
                    if (GUILayout.Button((Texture)AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/up.png", typeof(Texture)), GUILayout.Width(53)))
                    {
                        if (!PreceduralAdd && !MaterialAdd && Precedural)
                            PreceduralAdd = Precedural;
                        if (PreceduralAdd)
                        {
                            selectMainObj.T4MMaterial.SetTexture("_Splat1", PreceduralAdd.GetTexture("_MainTex"));
                            if (selectMainObj.T4MMaterial.HasProperty("_BumpSplat1"))
                                selectMainObj.T4MMaterial.SetTexture("_BumpSplat1", PreceduralAdd.GetTexture("_BumpMap"));
                        }
                        else if (MaterialAdd)
                        {
                            selectMainObj.T4MMaterial.SetTexture("_Splat1", MaterialAdd);
                        }
                        selProcedural = 1;
                        PreceduralAdd = null;
                        MaterialAdd = null;
                        T4MMainEditor.Instance.IniNewSelect();
                    }
                    if (selectMainObj.T4MMaterial.HasProperty("_Splat2"))
                        if (GUILayout.Button((Texture)AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/up.png", typeof(Texture)), GUILayout.Width(53)))
                        {
                            if (!PreceduralAdd && !MaterialAdd && Precedural)
                                PreceduralAdd = Precedural;
                            if (PreceduralAdd)
                            {
                                selectMainObj.T4MMaterial.SetTexture("_Splat2", PreceduralAdd.GetTexture("_MainTex"));
                                if (selectMainObj.T4MMaterial.HasProperty("_BumpSplat2"))
                                    selectMainObj.T4MMaterial.SetTexture("_BumpSplat2", PreceduralAdd.GetTexture("_BumpMap"));
                            }
                            else if (MaterialAdd)
                            {
                                selectMainObj.T4MMaterial.SetTexture("_Splat2", MaterialAdd);
                            }
                            selProcedural = 2;
                            PreceduralAdd = null;
                            MaterialAdd = null;
                            T4MMainEditor.Instance.IniNewSelect();
                        }
                    if (selectMainObj.T4MMaterial.HasProperty("_Splat3"))
                        if (GUILayout.Button((Texture)AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/up.png", typeof(Texture)), GUILayout.Width(53)))
                        {
                            if (!PreceduralAdd && !MaterialAdd && Precedural)
                                PreceduralAdd = Precedural;
                            if (PreceduralAdd)
                            {
                                selectMainObj.T4MMaterial.SetTexture("_Splat3", PreceduralAdd.GetTexture("_MainTex"));
                                if (selectMainObj.T4MMaterial.HasProperty("_BumpSplat3"))
                                    selectMainObj.T4MMaterial.SetTexture("_BumpSplat3", PreceduralAdd.GetTexture("_BumpMap"));
                            }
                            else if (MaterialAdd)
                            {
                                selectMainObj.T4MMaterial.SetTexture("_Splat3", MaterialAdd);
                            }
                            selProcedural = 3;
                            PreceduralAdd = null;
                            MaterialAdd = null;
                            T4MMainEditor.Instance.IniNewSelect();
                        }
                    if (selectMainObj.T4MMaterial.HasProperty("_Splat4"))
                        if (GUILayout.Button((Texture)AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/up.png", typeof(Texture)), GUILayout.Width(53)))
                        {
                            if (!PreceduralAdd && !MaterialAdd && Precedural)
                                PreceduralAdd = Precedural;

                            if (PreceduralAdd)
                            {
                                selectMainObj.T4MMaterial.SetTexture("_Splat4", PreceduralAdd.GetTexture("_MainTex"));
                            }
                            else if (MaterialAdd)
                            {
                                selectMainObj.T4MMaterial.SetTexture("_Splat4", MaterialAdd);
                            }
                            selProcedural = 4;
                            PreceduralAdd = null;
                            MaterialAdd = null;
                            T4MMainEditor.Instance.IniNewSelect();
                        }
                    if (selectMainObj.T4MMaterial.HasProperty("_Splat5"))
                        if (GUILayout.Button((Texture)AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/up.png", typeof(Texture)), GUILayout.Width(53)))
                        {
                            if (!PreceduralAdd && !MaterialAdd && Precedural)
                                PreceduralAdd = Precedural;

                            if (PreceduralAdd)
                            {
                                selectMainObj.T4MMaterial.SetTexture("_Splat5", PreceduralAdd.GetTexture("_MainTex"));
                            }
                            else if (MaterialAdd)
                            {
                                selectMainObj.T4MMaterial.SetTexture("_Splat5", MaterialAdd);
                            }
                            selProcedural = 5;
                            PreceduralAdd = null;
                            MaterialAdd = null;
                            T4MMainEditor.Instance.IniNewSelect();
                        }

                    EditorGUILayout.EndHorizontal();

                    string AssetName = AssetDatabase.GetAssetPath(selectMainObj.T4MMaterial.GetTexture("_Splat" + selProcedural));
                    SubstanceImporter SubstanceI = AssetImporter.GetAtPath(AssetName) as SubstanceImporter;
                    if (SubstanceI)
                    {

                        ProceduralMaterial[] ProcMat = SubstanceI.GetMaterials();
                        for (int i = 0; i < ProcMat.Length; i++)
                        {
                            if (ProcMat[i].name + "_Diffuse" == selectMainObj.T4MMaterial.GetTexture("_Splat" + selProcedural).name)
                            {
                                Precedural = ProcMat[i];
                                //SubstanceI.SetTextureAlphaSource(Precedural, Precedural.name+"_Diffuse", ProceduralOutputType.Diffuse);
                            }
                        }
                    }
                    else Precedural = null;

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();


                    MaterialTyp = (MaterialType)EditorGUILayout.EnumPopup("Material Type", MaterialTyp, GUILayout.Width(340));
                    EditorGUILayout.BeginHorizontal();

                    if (MaterialTyp != MaterialType.Classic)
                    {
                        GUILayout.Label("Substances To Add : ");
                        MaterialAdd = null;
                        PreceduralAdd = EditorGUILayout.ObjectField(PreceduralAdd, typeof(ProceduralMaterial), true, GUILayout.Width(220)) as ProceduralMaterial;
                    }
                    else {
                        GUILayout.Label("Texture To Add : ");
                        PreceduralAdd = null;
                        MaterialAdd = EditorGUILayout.ObjectField(MaterialAdd, typeof(Texture2D), true, GUILayout.Width(220)) as Texture;
                    }


                    GUILayout.FlexibleSpace();

                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space();
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space();

                    if (Precedural)
                    {
                        GUILayout.Label("Modify", EditorStyles.boldLabel);
                        EditorGUILayout.BeginHorizontal("box");
                        GUILayout.FlexibleSpace();
                        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(350), GUILayout.Height(296));
                        Substance();
                        EditorGUILayout.EndScrollView();
                        GUILayout.FlexibleSpace();
                        EditorGUILayout.EndHorizontal();
                    }
                    else {

                        ClassicMat();

                    }
                }
                break;

        }


    }



    void ClassicMat()
    {
        T4MMainObj mainObjTm = CurrentSelect.gameObject.GetComponent<T4MMainObj>();
        if (selProcedural == 0)
        {
            if (T4MConfig.Layer1)
            {
                GUILayout.Label("Modify", EditorStyles.boldLabel);
                GUILayout.BeginHorizontal("Box");
                GUILayout.Label((Texture)AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/TDiff.jpg", typeof(Texture)));
                T4MConfig.Layer1 = EditorGUILayout.ObjectField(T4MConfig.Layer1, typeof(Texture2D), true,
                                                                GUILayout.Width(75), GUILayout.Height(75)) as Texture;
                mainObjTm.T4MMaterial.SetTexture("_Splat0", T4MConfig.Layer1);
                if (mainObjTm.T4MMaterial.HasProperty("_BumpSplat0"))
                {
                    GUILayout.Label((Texture)AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/TBump.jpg", typeof(Texture)));
                    T4MConfig.Layer1Bump = EditorGUILayout.ObjectField(T4MConfig.Layer1Bump, typeof(Texture2D), true,
                                                                        GUILayout.Width(75), GUILayout.Height(75)) as Texture;
                    mainObjTm.T4MMaterial.SetTexture("_BumpSplat0", T4MConfig.Layer1Bump);
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

        }
        else if (selProcedural == 1)
        {
            if (T4MConfig.Layer2)
            {
                GUILayout.Label("Modify", EditorStyles.boldLabel);
                GUILayout.BeginHorizontal("Box");
                GUILayout.Label((Texture)AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/TDiff.jpg", typeof(Texture)));
                T4MConfig.Layer2 = EditorGUILayout.ObjectField(T4MConfig.Layer2, typeof(Texture2D), true, 
                                                               GUILayout.Width(75), GUILayout.Height(75)) as Texture;
                mainObjTm.T4MMaterial.SetTexture("_Splat1", T4MConfig.Layer2);
                if (mainObjTm.T4MMaterial.HasProperty("_BumpSplat1"))
                {
                    GUILayout.Label((Texture)AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/TBump.jpg", typeof(Texture)));
                    T4MConfig.Layer2Bump = EditorGUILayout.ObjectField(T4MConfig.Layer2Bump, typeof(Texture2D), true, 
                                                                       GUILayout.Width(75), GUILayout.Height(75)) as Texture;
                    mainObjTm.T4MMaterial.SetTexture("_BumpSplat1", T4MConfig.Layer2Bump);
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

        }
        else if (selProcedural == 2)
        {
            if (T4MConfig.Layer3)
            {
                GUILayout.Label("Modify", EditorStyles.boldLabel);
                GUILayout.BeginHorizontal("Box");
                GUILayout.Label((Texture)AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/TDiff.jpg", typeof(Texture)));
                T4MConfig.Layer3 = EditorGUILayout.ObjectField(T4MConfig.Layer3, typeof(Texture2D), true, GUILayout.Width(75), GUILayout.Height(75)) as Texture;
                mainObjTm.T4MMaterial.SetTexture("_Splat2", T4MConfig.Layer3);
                if (mainObjTm.T4MMaterial.HasProperty("_BumpSplat2"))
                {
                    GUILayout.Label((Texture)AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/TBump.jpg", typeof(Texture)));
                    T4MConfig.Layer3Bump = EditorGUILayout.ObjectField(T4MConfig.Layer3Bump, typeof(Texture2D), true, GUILayout.Width(75), GUILayout.Height(75)) as Texture;
                    mainObjTm.T4MMaterial.SetTexture("_BumpSplat2", T4MConfig.Layer3Bump);
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

        }
        else if (selProcedural == 3)
        {
            if (T4MConfig.Layer4)
            {
                GUILayout.Label("Modify", EditorStyles.boldLabel);
                GUILayout.BeginHorizontal("Box");
                GUILayout.Label((Texture)AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/TDiff.jpg", typeof(Texture)));
                T4MConfig.Layer4 = EditorGUILayout.ObjectField(T4MConfig.Layer4, typeof(Texture2D), true, GUILayout.Width(75), GUILayout.Height(75)) as Texture;
                mainObjTm.T4MMaterial.SetTexture("_Splat3", T4MConfig.Layer4);
                if (mainObjTm.T4MMaterial.HasProperty("_BumpSplat3"))
                {
                    GUILayout.Label((Texture)AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/TBump.jpg", typeof(Texture)));
                    T4MConfig.Layer4Bump = EditorGUILayout.ObjectField(T4MConfig.Layer4Bump, typeof(Texture2D), true, GUILayout.Width(75), GUILayout.Height(75)) as Texture;
                    mainObjTm.T4MMaterial.SetTexture("_BumpSplat3", T4MConfig.Layer4Bump);

                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

        }
        else if (selProcedural == 4)
        {
            if (T4MConfig.Layer5)
            {
                GUILayout.Label("Modify", EditorStyles.boldLabel);
                GUILayout.BeginHorizontal("Box");
                GUILayout.Label((Texture)AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/TDiff.jpg", typeof(Texture)));
                T4MConfig.Layer5 = EditorGUILayout.ObjectField(T4MConfig.Layer5, typeof(Texture2D), true, GUILayout.Width(75), GUILayout.Height(75)) as Texture;
                mainObjTm.T4MMaterial.SetTexture("_Splat4", T4MConfig.Layer5);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
        }
        else if (selProcedural == 5)
        {
            if (T4MConfig.Layer6)
            {
                GUILayout.Label("Modify", EditorStyles.boldLabel);
                GUILayout.BeginHorizontal("Box");
                GUILayout.Label((Texture)AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/TDiff.jpg", typeof(Texture)));
                T4MConfig.Layer6 = EditorGUILayout.ObjectField(T4MConfig.Layer6, typeof(Texture2D), true, GUILayout.Width(75), GUILayout.Height(75)) as Texture;
                mainObjTm.T4MMaterial.SetTexture("_Splat5", T4MConfig.Layer6);
            }
        }

        if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel1/T4M 2 Textures ManualAdd BeastLM_1DC") || mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel1/T4M 2 Textures ManualAdd CustoLM 1DC"))
        {
            GUILayout.Label("Manual Lightmap Add", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal("Box");
            GUILayout.Label((Texture)AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/TLM.jpg", typeof(Texture)));
            T4MConfig.LMMan = EditorGUILayout.ObjectField(T4MConfig.LMMan, typeof(Texture2D), true, GUILayout.Width(75), GUILayout.Height(75)) as Texture;
            mainObjTm.T4MMaterial.SetTexture("_Lightmap", T4MConfig.LMMan);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }


    void PixelPainterMenu()
    {
        T4MMainObj mainObjTm = CurrentSelect.gameObject.GetComponent<T4MMainObj>();
        if (mainObjTm)
        {
            if (mainObjTm.T4MMaterial && mainObjTm.T4MMaterial.HasProperty("_Splat0") && mainObjTm.T4MMaterial.HasProperty("_Splat1") && mainObjTm.T4MMaterial.HasProperty("_Control"))
            {
                IniBrush();
                InitPincil();
                if (!T4MConfig.T4MPreview)
                    InitPreview();
                if (T4MCache.intialized)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/brushes.jpg", typeof(Texture)) as Texture, "label");
                    GUILayout.BeginHorizontal("box", GUILayout.Width(318));
                    GUILayout.FlexibleSpace();
                    selBrush = GUILayout.SelectionGrid(selBrush, TexBrush, 9, "gridlist", GUILayout.Width(290), GUILayout.Height(70));
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    GUILayout.BeginHorizontal("box", GUILayout.Width(340));
                    GUILayout.FlexibleSpace();
                    if (T4MCache.TexTexture.Length > 4)
                        T4MConfig.T4MselTexture = GUILayout.SelectionGrid(T4MConfig.T4MselTexture, T4MCache.TexTexture, 6, "gridlist", GUILayout.Width(340), GUILayout.Height(58));
                    else
                        T4MConfig.T4MselTexture = GUILayout.SelectionGrid(T4MConfig.T4MselTexture, T4MCache.TexTexture, 4, "gridlist", GUILayout.Width(340), GUILayout.Height(86));
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    EditorGUILayout.Space();


                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    GUILayout.BeginVertical("box", GUILayout.Width(347));
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Preview Type", GUILayout.Width(145));
                    T4MCache.PaintPrev = (PaintHandle)EditorGUILayout.EnumPopup(T4MCache.PaintPrev, GUILayout.Width(160));
                    GUILayout.EndHorizontal();
                    T4MConfig.brushSize = (int)EditorGUILayout.Slider("Brush Size", T4MConfig.brushSize, 1, 36);
                    T4MConfig.T4MStronger = EditorGUILayout.Slider("Brush Stronger", T4MConfig.T4MStronger, 0.05f, 1f);
                    GUILayout.EndVertical();
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();


                    if (mainObjTm.T4MMaterial.HasProperty("_SpecColor"))
                    {
                        EditorGUILayout.Space();
                        GUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                        GUILayout.BeginVertical("box", GUILayout.Width(347), GUILayout.Height(96));
                        T4MCache.ShinessColor = EditorGUILayout.ColorField("Shininess Color", T4MCache.ShinessColor);
                        mainObjTm.T4MMaterial.SetColor("_SpecColor", T4MCache.ShinessColor);
                        EditorGUILayout.Space();
                        if (mainObjTm.T4MMaterial.HasProperty("_ShininessL0"))
                        {
                            T4MCache.shiness0 = EditorGUILayout.Slider("Shininess Layer 1", T4MCache.shiness0, 0.00f, 1.0f);
                            mainObjTm.T4MMaterial.SetFloat("_ShininessL0", T4MCache.shiness0);
                        }
                        if (mainObjTm.T4MMaterial.HasProperty("_ShininessL1"))
                        {
                            T4MCache.shiness1 = EditorGUILayout.Slider("Shininess Layer 2", T4MCache.shiness1, 0.00f, 1.0f);
                            mainObjTm.T4MMaterial.SetFloat("_ShininessL1", T4MCache.shiness1);
                        }
                        if (mainObjTm.T4MMaterial.HasProperty("_ShininessL2"))
                        {
                            T4MCache.shiness2 = EditorGUILayout.Slider("Shininess Layer 3", T4MCache.shiness2, 0.00f, 1.0f);
                            mainObjTm.T4MMaterial.SetFloat("_ShininessL2", T4MCache.shiness2);
                        }
                        if (mainObjTm.T4MMaterial.HasProperty("_ShininessL3"))
                        {
                            T4MCache.shiness3 = EditorGUILayout.Slider("Shininess Layer 4", T4MCache.shiness3, 0.00f, 1.0f);
                            mainObjTm.T4MMaterial.SetFloat("_ShininessL3", T4MCache.shiness3);
                        }

                        GUILayout.EndVertical();
                        GUILayout.FlexibleSpace();
                        GUILayout.EndHorizontal();

                    }
                    EditorGUILayout.Space();




                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (mainObjTm.T4MMaterial.HasProperty("_SpecColor"))
                    {
                        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(350), GUILayout.Height(140));
                        GUILayout.BeginVertical("box", GUILayout.Width(320));
                    }
                    else {
                        GUILayout.BeginVertical("box", GUILayout.Width(320));
                        if (T4MCache.TexTexture.Length > 4)
                            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(340), GUILayout.Height(275));
                        else scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(340), GUILayout.Height(240));
                    }

                    joinTiles = EditorGUILayout.Toggle("Tiling : Join X/Y", joinTiles);
                    EditorGUILayout.Space();
                    if (joinTiles)
                    {
                        T4MCache.Layer1Tile.x = T4MCache.Layer1Tile.y = EditorGUILayout.Slider("Layer1 Tiling :", T4MCache.Layer1Tile.x, 1, 500 * T4MConfig.T4MMaskTexUVCoord);
                        mainObjTm.T4MMaterial.SetTextureScale("_Splat0", new Vector2(T4MCache.Layer1Tile.x, T4MCache.Layer1Tile.x));
                        EditorGUILayout.Space();
                        T4MCache.Layer2Tile.x = T4MCache.Layer2Tile.y = EditorGUILayout.Slider("T4MConfig.Layer2 Tiling :", T4MCache.Layer2Tile.x, 1, 500 * T4MConfig.T4MMaskTexUVCoord);
                        mainObjTm.T4MMaterial.SetTextureScale("_Splat1", new Vector2(T4MCache.Layer2Tile.x, T4MCache.Layer2Tile.x));
                        EditorGUILayout.Space();
                        if (mainObjTm.T4MMaterial.HasProperty("_Splat2"))
                        {
                            T4MCache.Layer3Tile.x = T4MCache.Layer3Tile.y = EditorGUILayout.Slider("T4MConfig.Layer3 Tiling :", T4MCache.Layer3Tile.x, 1, 500 * T4MConfig.T4MMaskTexUVCoord);
                            mainObjTm.T4MMaterial.SetTextureScale("_Splat2", new Vector2(T4MCache.Layer3Tile.x, T4MCache.Layer3Tile.x));
                        }
                        EditorGUILayout.Space();
                        if (mainObjTm.T4MMaterial.HasProperty("_Splat3"))
                        {
                            T4MCache.Layer4Tile.x = T4MCache.Layer4Tile.y = EditorGUILayout.Slider("T4MConfig.Layer4 Tiling :", T4MCache.Layer4Tile.x, 1, 500 * T4MConfig.T4MMaskTexUVCoord);
                            mainObjTm.T4MMaterial.SetTextureScale("_Splat3", new Vector2(T4MCache.Layer4Tile.x, T4MCache.Layer4Tile.x));
                        }
                        EditorGUILayout.Space();
                        if (mainObjTm.T4MMaterial.HasProperty("_Splat4"))
                        {
                            T4MCache.Layer5Tile.x = T4MCache.Layer5Tile.y = EditorGUILayout.Slider("T4MConfig.Layer5 Tiling :", T4MCache.Layer5Tile.x, 1, 500 * T4MConfig.T4MMaskTexUVCoord);
                            mainObjTm.T4MMaterial.SetTextureScale("_Splat4", new Vector2(T4MCache.Layer5Tile.x, T4MCache.Layer5Tile.x));
                        }
                        EditorGUILayout.Space();
                        if (mainObjTm.T4MMaterial.HasProperty("_Splat5"))
                        {
                            T4MCache.Layer6Tile.x = T4MCache.Layer6Tile.y = EditorGUILayout.Slider("T4MConfig.Layer6 Tiling :", T4MCache.Layer6Tile.x, 1, 500 * T4MConfig.T4MMaskTexUVCoord);
                            mainObjTm.T4MMaterial.SetTextureScale("_Splat5", new Vector2(T4MCache.Layer6Tile.x, T4MCache.Layer6Tile.x));
                        }
                    }
                    else {
                        T4MCache.Layer1Tile.x = EditorGUILayout.Slider("Layer1 TilingX :", T4MCache.Layer1Tile.x, 1, 500 * T4MConfig.T4MMaskTexUVCoord);
                        T4MCache.Layer1Tile.y = EditorGUILayout.Slider("Layer1 TilingZ :", T4MCache.Layer1Tile.y, 1, 500 * T4MConfig.T4MMaskTexUVCoord);
                        mainObjTm.T4MMaterial.SetTextureScale("_Splat0", new Vector2(T4MCache.Layer1Tile.x, T4MCache.Layer1Tile.y));
                        EditorGUILayout.Space();
                        T4MCache.Layer2Tile.x = EditorGUILayout.Slider("T4MConfig.Layer2 TilingX :", T4MCache.Layer2Tile.x, 1, 500 * T4MConfig.T4MMaskTexUVCoord);
                        T4MCache.Layer2Tile.y = EditorGUILayout.Slider("T4MConfig.Layer2 TilingZ :", T4MCache.Layer2Tile.y, 1, 500 * T4MConfig.T4MMaskTexUVCoord);
                        mainObjTm.T4MMaterial.SetTextureScale("_Splat1", new Vector2(T4MCache.Layer2Tile.x, T4MCache.Layer2Tile.y));
                        EditorGUILayout.Space();
                        if (mainObjTm.T4MMaterial.HasProperty("_Splat2"))
                        {
                            T4MCache.Layer3Tile.x = EditorGUILayout.Slider("T4MConfig.Layer3 TilingX :", T4MCache.Layer3Tile.x, 1, 500 * T4MConfig.T4MMaskTexUVCoord);
                            T4MCache.Layer3Tile.y = EditorGUILayout.Slider("T4MConfig.Layer3 TilingZ :", T4MCache.Layer3Tile.y, 1, 500 * T4MConfig.T4MMaskTexUVCoord);
                            mainObjTm.T4MMaterial.SetTextureScale("_Splat2", new Vector2(T4MCache.Layer3Tile.x, T4MCache.Layer3Tile.y));
                        }
                        EditorGUILayout.Space();
                        if (mainObjTm.T4MMaterial.HasProperty("_Splat3"))
                        {
                            T4MCache.Layer4Tile.x = EditorGUILayout.Slider("T4MConfig.Layer4 TilingX :", T4MCache.Layer4Tile.x, 1, 500 * T4MConfig.T4MMaskTexUVCoord);
                            T4MCache.Layer4Tile.y = EditorGUILayout.Slider("T4MConfig.Layer4 TilingZ :", T4MCache.Layer4Tile.y, 1, 500 * T4MConfig.T4MMaskTexUVCoord);
                            mainObjTm.T4MMaterial.SetTextureScale("_Splat3", new Vector2(T4MCache.Layer4Tile.x, T4MCache.Layer4Tile.y));
                        }
                        EditorGUILayout.Space();
                        if (mainObjTm.T4MMaterial.HasProperty("_Splat4"))
                        {
                            T4MCache.Layer5Tile.x = EditorGUILayout.Slider("T4MConfig.Layer5 TilingX :", T4MCache.Layer5Tile.x, 1, 500 * T4MConfig.T4MMaskTexUVCoord);
                            T4MCache.Layer5Tile.y = EditorGUILayout.Slider("T4MConfig.Layer5 TilingZ :", T4MCache.Layer5Tile.y, 1, 500 * T4MConfig.T4MMaskTexUVCoord);
                            mainObjTm.T4MMaterial.SetTextureScale("_Splat4", new Vector2(T4MCache.Layer5Tile.x, T4MCache.Layer5Tile.y));
                        }
                        EditorGUILayout.Space();
                        if (mainObjTm.T4MMaterial.HasProperty("_Splat5"))
                        {
                            T4MCache.Layer6Tile.x = EditorGUILayout.Slider("T4MConfig.Layer6 TilingX :", T4MCache.Layer6Tile.x, 1, 500 * T4MConfig.T4MMaskTexUVCoord);
                            T4MCache.Layer6Tile.y = EditorGUILayout.Slider("T4MConfig.Layer6 TilingZ :", T4MCache.Layer6Tile.y, 1, 500 * T4MConfig.T4MMaskTexUVCoord);
                            mainObjTm.T4MMaterial.SetTextureScale("_Splat5", new Vector2(T4MCache.Layer6Tile.x, T4MCache.Layer6Tile.y));
                        }
                    }
                    EditorGUILayout.EndScrollView();
                    GUILayout.EndVertical();
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    if (TexBrush.Length > 0)
                    {
                        T4MConfig.T4MPreview.material.SetTexture("_MaskTex", TexBrush[selBrush]);
                        MeshFilter temp = CurrentSelect.GetComponent<MeshFilter>();
                        if (temp == null)
                            temp = CurrentSelect.GetComponent<T4MMainObj>().T4MMesh;
                        T4MConfig.T4MPreview.orthographicSize = (T4MConfig.brushSize * CurrentSelect.localScale.x) * (temp.sharedMesh.bounds.size.x / 200);
                    }

                    float test = T4MConfig.T4MStronger * 200 / 100;
                    T4MConfig.T4MPreview.material.SetFloat("_Transp", Mathf.Clamp(test, 0.4f, 1));

                    T4MCache.T4MBrushSizeInPourcent = (int)Mathf.Round((T4MConfig.brushSize * T4MCache.T4MMaskTex.width) / 100);



                    if (T4MConfig.T4MselTexture == 0)
                        T4MConfig.T4MPreview.material.SetTextureScale("_MainTex", T4MCache.Layer1Tile);
                    else if (T4MConfig.T4MselTexture == 1)
                        T4MConfig.T4MPreview.material.SetTextureScale("_MainTex", T4MCache.Layer2Tile);
                    else if (T4MConfig.T4MselTexture == 2)
                        T4MConfig.T4MPreview.material.SetTextureScale("_MainTex", T4MCache.Layer3Tile);
                    else if (T4MConfig.T4MselTexture == 3)
                        T4MConfig.T4MPreview.material.SetTextureScale("_MainTex", T4MCache.Layer4Tile);
                    else if (T4MConfig.T4MselTexture == 4)
                        T4MConfig.T4MPreview.material.SetTextureScale("_MainTex", T4MCache.Layer5Tile);
                    else if (T4MConfig.T4MselTexture == 5)
                        T4MConfig.T4MPreview.material.SetTextureScale("_MainTex", T4MCache.Layer6Tile);

                    if (selBrush != oldSelBrush || T4MCache.T4MBrushSizeInPourcent != oldBrushSizeInPourcent || T4MCache.T4MBrushAlpha == null || T4MConfig.T4MselTexture != oldselTexture)
                    {
                        if (T4MConfig.T4MselTexture == 0)
                        {
                            T4MConfig.T4MPreview.material.SetTexture("_MainTex", mainObjTm.T4MMaterial.GetTexture("_Splat0") as Texture);
                            T4MCache.T4MtargetColor = new Color(1f, 0f, 0f, 0f);
                            if (T4MCache.T4MMaskTex2)
                                T4MCache.T4MtargetColor2 = new Color(0, 0, 0, 0);
                        }
                        else if (T4MConfig.T4MselTexture == 1)
                        {
                            T4MConfig.T4MPreview.material.SetTexture("_MainTex", mainObjTm.T4MMaterial.GetTexture("_Splat1") as Texture);
                            if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel1/T4M 2 Textures Auto BeastLM 2DrawCall") ||
                                mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel1/T4M 2 Textures ManualAdd BeastLM_1DC") ||
                                mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel1/T4M 2 Textures ManualAdd CustoLM 1DC"))
                                T4MCache.T4MtargetColor = new Color(0, 0, 0, 1);
                            else {
                                T4MCache.T4MtargetColor = new Color(0, 1, 0, 0);
                                if (T4MCache.T4MMaskTex2)
                                    T4MCache.T4MtargetColor2 = new Color(0, 0, 0, 0);
                            }
                        }
                        else if (T4MConfig.T4MselTexture == 2)
                        {
                            T4MConfig.T4MPreview.material.SetTexture("_MainTex", mainObjTm.T4MMaterial.GetTexture("_Splat2") as Texture);
                            T4MCache.T4MtargetColor = new Color(0, 0, 1, 0);
                            if (T4MCache.T4MMaskTex2)
                                T4MCache.T4MtargetColor2 = new Color(0, 0, 0, 0);
                        }
                        else if (T4MConfig.T4MselTexture == 3)
                        {
                            T4MConfig.T4MPreview.material.SetTexture("_MainTex", mainObjTm.T4MMaterial.GetTexture("_Splat3") as Texture);
                            T4MCache.T4MtargetColor = new Color(0, 0, 0, 1);
                            if (T4MCache.T4MMaskTex2)
                                T4MCache.T4MtargetColor2 = new Color(1, 0, 0, 0);
                        }
                        else if (T4MConfig.T4MselTexture == 4)
                        {
                            T4MConfig.T4MPreview.material.SetTexture("_MainTex", mainObjTm.T4MMaterial.GetTexture("_Splat4") as Texture);
                            T4MCache.T4MtargetColor = new Color(0, 0, 0, 1);
                            if (T4MCache.T4MMaskTex2)
                                T4MCache.T4MtargetColor2 = new Color(0, 1, 0, 0);
                        }
                        else if (T4MConfig.T4MselTexture == 5)
                        {
                            T4MConfig.T4MPreview.material.SetTexture("_MainTex", mainObjTm.T4MMaterial.GetTexture("_Splat5") as Texture);
                            T4MCache.T4MtargetColor = new Color(0, 0, 0, 1);
                            if (T4MCache.T4MMaskTex2)
                                T4MCache.T4MtargetColor2 = new Color(0, 0, 1, 0);
                        }
                        Texture2D TBrush = TexBrush[selBrush] as Texture2D;
                        T4MCache.T4MBrushAlpha = new float[T4MCache.T4MBrushSizeInPourcent * T4MCache.T4MBrushSizeInPourcent];
                        for (int i = 0; i < T4MCache.T4MBrushSizeInPourcent; i++)
                        {
                            for (int j = 0; j < T4MCache.T4MBrushSizeInPourcent; j++)
                            {
                                T4MCache.T4MBrushAlpha[j * T4MCache.T4MBrushSizeInPourcent + i] = TBrush.GetPixelBilinear(((float)i) / T4MCache.T4MBrushSizeInPourcent, ((float)j) / T4MCache.T4MBrushSizeInPourcent).a;
                            }
                        }
                        oldselTexture = T4MConfig.T4MselTexture;
                        oldSelBrush = selBrush;
                        oldBrushSizeInPourcent = T4MCache.T4MBrushSizeInPourcent;
                    }
                }
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


    void InitPincil()
    {
        T4MMainObj mainObjTm = CurrentSelect.gameObject.GetComponent<T4MMainObj>();
        if (mainObjTm.T4MMaterial.HasProperty("_Splat5"))
        {
            T4MCache.TexTexture = new Texture[6];
            T4MCache.TexTexture[0] = AssetPreview.GetAssetPreview(mainObjTm.T4MMaterial.GetTexture("_Splat0"));
            T4MCache.TexTexture[1] = AssetPreview.GetAssetPreview(mainObjTm.T4MMaterial.GetTexture("_Splat1"));
            T4MCache.TexTexture[2] = AssetPreview.GetAssetPreview(mainObjTm.T4MMaterial.GetTexture("_Splat2"));
            T4MCache.TexTexture[3] = AssetPreview.GetAssetPreview(mainObjTm.T4MMaterial.GetTexture("_Splat3"));
            T4MCache.TexTexture[4] = AssetPreview.GetAssetPreview(mainObjTm.T4MMaterial.GetTexture("_Splat4"));
            T4MCache.TexTexture[5] = AssetPreview.GetAssetPreview(mainObjTm.T4MMaterial.GetTexture("_Splat5"));
        }
        else if (mainObjTm.T4MMaterial.HasProperty("_Splat4"))
        {
            T4MCache.TexTexture = new Texture[5];
            T4MCache.TexTexture[0] = AssetPreview.GetAssetPreview(mainObjTm.T4MMaterial.GetTexture("_Splat0"));
            T4MCache.TexTexture[1] = AssetPreview.GetAssetPreview(mainObjTm.T4MMaterial.GetTexture("_Splat1"));
            T4MCache.TexTexture[2] = AssetPreview.GetAssetPreview(mainObjTm.T4MMaterial.GetTexture("_Splat2"));
            T4MCache.TexTexture[3] = AssetPreview.GetAssetPreview(mainObjTm.T4MMaterial.GetTexture("_Splat3"));
            T4MCache.TexTexture[4] = AssetPreview.GetAssetPreview(mainObjTm.T4MMaterial.GetTexture("_Splat4"));
        }
        else if (mainObjTm.T4MMaterial.HasProperty("_Splat3"))
        {
            T4MCache.TexTexture = new Texture[4];
            T4MCache.TexTexture[0] = AssetPreview.GetAssetPreview(mainObjTm.T4MMaterial.GetTexture("_Splat0"));
            T4MCache.TexTexture[1] = AssetPreview.GetAssetPreview(mainObjTm.T4MMaterial.GetTexture("_Splat1"));
            T4MCache.TexTexture[2] = AssetPreview.GetAssetPreview(mainObjTm.T4MMaterial.GetTexture("_Splat2"));
            T4MCache.TexTexture[3] = AssetPreview.GetAssetPreview(mainObjTm.T4MMaterial.GetTexture("_Splat3"));
        }
        else if (mainObjTm.T4MMaterial.HasProperty("_Splat2"))
        {
            T4MCache.TexTexture = new Texture[3];
            T4MCache.TexTexture[0] = AssetPreview.GetAssetPreview(mainObjTm.T4MMaterial.GetTexture("_Splat0"));
            T4MCache.TexTexture[1] = AssetPreview.GetAssetPreview(mainObjTm.T4MMaterial.GetTexture("_Splat1"));
            T4MCache.TexTexture[2] = AssetPreview.GetAssetPreview(mainObjTm.T4MMaterial.GetTexture("_Splat2"));
        }
        else {
            T4MCache.TexTexture = new Texture[2];
            T4MCache.TexTexture[0] = AssetPreview.GetAssetPreview(mainObjTm.T4MMaterial.GetTexture("_Splat0"));
            T4MCache.TexTexture[1] = AssetPreview.GetAssetPreview(mainObjTm.T4MMaterial.GetTexture("_Splat1"));

        }
    }


    void IniBrush()
    {
        ArrayList BrushList = new ArrayList();
        Texture BrushesTL;
        int BrushNum = 0;
        do
        {
            BrushesTL = (Texture)AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Brushes/Brush" + BrushNum + ".png", typeof(Texture));
            if (BrushesTL)
            {
                BrushList.Add(BrushesTL);
            }
            BrushNum++;
        } while (BrushesTL);
        TexBrush = BrushList.ToArray(typeof(Texture)) as Texture[];
    }


    void InitPreview()
    {
        T4MMainObj mainObjTm = CurrentSelect.gameObject.GetComponent<T4MMainObj>();
        var ProjectorB = new GameObject("PreviewT4M");
        ProjectorB.AddComponent(typeof(Projector));
        ProjectorB.hideFlags = HideFlags.HideInHierarchy;
        T4MConfig.T4MPreview = ProjectorB.GetComponent(typeof(Projector)) as Projector;
        MeshFilter SizeOfGeo = CurrentSelect.GetComponent<MeshFilter>();
        if (SizeOfGeo == null)
            SizeOfGeo = mainObjTm.T4MMesh;
        Vector2 MeshSize = new Vector2(SizeOfGeo.sharedMesh.bounds.size.x, SizeOfGeo.sharedMesh.bounds.size.z);
        T4MConfig.T4MPreview.nearClipPlane = -20;
        T4MConfig.T4MPreview.farClipPlane = 20;
        T4MConfig.T4MPreview.orthographic = true;
        T4MConfig.T4MPreview.orthographicSize = (T4MConfig.brushSize * CurrentSelect.localScale.x) * (MeshSize.x / 100);
        T4MConfig.T4MPreview.ignoreLayers = ~layerMask;
        T4MConfig.T4MPreview.transform.Rotate(90, -90, 0);
        Shader mShader = Shader.Find("Hidden/iT4MShaders/BrushPreview");
        Material NewPMat = new Material(mShader);
        T4MConfig.T4MPreview.material = NewPMat;
        T4MConfig.T4MPreview.material.SetTexture("_MainTex", T4MCache.TexTexture[T4MConfig.T4MselTexture]);
        T4MConfig.T4MPreview.material.SetTexture("_MaskTex", TexBrush[selBrush]);
        if (T4MConfig.T4MselTexture == 0)
        {
            T4MConfig.T4MPreview.material.SetTextureScale("_MainTex", T4MCache.Layer1Tile);
            T4MConfig.T4MPreview.material.SetTexture("_MainTex", mainObjTm.T4MMaterial.GetTexture("_Splat0"));
        }
        else if (T4MConfig.T4MselTexture == 1)
        {
            T4MConfig.T4MPreview.material.SetTextureScale("_MainTex", T4MCache.Layer2Tile);
            T4MConfig.T4MPreview.material.SetTexture("_MainTex", mainObjTm.T4MMaterial.GetTexture("_Splat1"));
        }
        else if (T4MConfig.T4MselTexture == 2)
        {
            T4MConfig.T4MPreview.material.SetTextureScale("_MainTex", T4MCache.Layer3Tile);
            T4MConfig.T4MPreview.material.SetTexture("_MainTex", mainObjTm.T4MMaterial.GetTexture("_Splat2"));
        }
        else if (T4MConfig.T4MselTexture == 3)
        {
            T4MConfig.T4MPreview.material.SetTextureScale("_MainTex", T4MCache.Layer4Tile);
            T4MConfig.T4MPreview.material.SetTexture("_MainTex", mainObjTm.T4MMaterial.GetTexture("_Splat3"));
        }
        else if (T4MConfig.T4MselTexture == 4)
        {
            T4MConfig.T4MPreview.material.SetTextureScale("_MainTex", T4MCache.Layer5Tile);
            T4MConfig.T4MPreview.material.SetTexture("_MainTex", mainObjTm.T4MMaterial.GetTexture("_Splat4"));
        }
        else if (T4MConfig.T4MselTexture == 5)
        {
            T4MConfig.T4MPreview.material.SetTextureScale("_MainTex", T4MCache.Layer6Tile);
            T4MConfig.T4MPreview.material.SetTexture("_MainTex", mainObjTm.T4MMaterial.GetTexture("_Splat5"));
        }
    }


    void Substance()
    {

        var inputs = Precedural.GetProceduralPropertyDescriptions();

        for (int i = 0; i < inputs.Length; i++)
        {
            var input = inputs[i];
            var type = input.type;

            if (type == ProceduralPropertyType.Boolean)
            {
                var inputBool = Precedural.GetProceduralBoolean(input.name);
                var oldInputBool = inputBool;
                inputBool = EditorGUILayout.Toggle(input.name, inputBool);
                if (inputBool != oldInputBool)
                    Precedural.SetProceduralBoolean(input.name, inputBool);
            }

            else if (type == ProceduralPropertyType.Float)
            {
                if (input.hasRange)
                {

                    GUILayout.Label(input.name, EditorStyles.boldLabel);

                    var inputFloat = Precedural.GetProceduralFloat(input.name);
                    var oldInputFloat = inputFloat;

                    inputFloat = EditorGUILayout.Slider(inputFloat, input.minimum, input.maximum);
                    if (inputFloat != oldInputFloat)
                        Precedural.SetProceduralFloat(input.name, inputFloat);
                }
            }

            else if (type == ProceduralPropertyType.Vector2 ||
                type == ProceduralPropertyType.Vector3 ||
                type == ProceduralPropertyType.Vector4
            )
            {

                if (input.hasRange)
                {
                    GUILayout.Label(input.name, EditorStyles.boldLabel);


                    var vectorComponentAmount = 4;
                    if (type == ProceduralPropertyType.Vector2) vectorComponentAmount = 2;
                    if (type == ProceduralPropertyType.Vector3) vectorComponentAmount = 3;

                    var inputVector = Precedural.GetProceduralVector(input.name);
                    var oldInputVector = inputVector;


                    for (int c = 0; c < vectorComponentAmount; c++)
                        inputVector[c] = EditorGUILayout.Slider(
                            inputVector[c], input.minimum, input.maximum);

                    if (inputVector != oldInputVector)
                        Precedural.SetProceduralVector(input.name, inputVector);
                }
            }

            else if (type == ProceduralPropertyType.Color3 || type == ProceduralPropertyType.Color4)
            {
                GUILayout.Label(input.name, EditorStyles.boldLabel);




                var colorInput = Precedural.GetProceduralColor(input.name);
                var oldColorInput = colorInput;

                colorInput = EditorGUILayout.ColorField("Shader Color", colorInput);

                if (colorInput != oldColorInput)
                    Precedural.SetProceduralColor(input.name, colorInput);
            }


            else if (type == ProceduralPropertyType.Enum)
            {
                GUILayout.Label(input.name, EditorStyles.boldLabel);

                var enumInput = Precedural.GetProceduralEnum(input.name);
                var oldEnumInput = enumInput;
                var enumOptions = input.enumOptions;

                enumInput = GUILayout.SelectionGrid(enumInput, enumOptions, 1);
                if (enumInput != oldEnumInput)
                    Precedural.SetProceduralEnum(input.name, enumInput);
            }
        }
        Precedural.RebuildTexturesImmediately();



    }


    void ProjectionWorldConfig()
    {
        if (T4MConfig.UpSideTile.x != T4MConfig.UpSideTile.y && joinTiles == true || T4MConfig.UpSideTile.z != T4MConfig.UpSideTile.w && joinTiles == true)
        {
            joinTiles = false;
        }
        EditorGUILayout.Space();
        GUILayout.Label("Painting Menu is not available for this shader", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        GUILayout.Label("World Projection Shaders Options", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        T4MConfig.UpSideF = EditorGUILayout.Slider("UP/SIDES Fighting :", T4MConfig.UpSideF, 0, 10);
        T4MConfig.BlendFac = EditorGUILayout.Slider("Blend Factor :", T4MConfig.BlendFac, 0, 20);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        joinTiles = EditorGUILayout.Toggle("Tiling : Join X/Y", joinTiles);
        EditorGUILayout.Space();
        if (joinTiles)
        {
            T4MConfig.UpSideTile.x = T4MConfig.UpSideTile.y = EditorGUILayout.Slider("Up Texture Tiling :", T4MConfig.UpSideTile.x, 0.01f, 10);
            T4MConfig.UpSideTile.z = T4MConfig.UpSideTile.w = EditorGUILayout.Slider("Side Tecture Tiling :", T4MConfig.UpSideTile.z, 0.01f, 10);
        }
        else {
            T4MConfig.UpSideTile.x = EditorGUILayout.Slider("Up Texture Tiling X:", T4MConfig.UpSideTile.x, 0.01f, 2);
            T4MConfig.UpSideTile.y = EditorGUILayout.Slider("Up Texture Tiling Y:", T4MConfig.UpSideTile.y, 0.01f, 2);
            T4MConfig.UpSideTile.z = EditorGUILayout.Slider("Side Tecture Tiling X:", T4MConfig.UpSideTile.z, 0.01f, 2);
            T4MConfig.UpSideTile.w = EditorGUILayout.Slider("Side Tecture Tiling Y:", T4MConfig.UpSideTile.w, 0.01f, 2);
        }

        T4MMainObj mainObjTm = CurrentSelect.gameObject.GetComponent<T4MMainObj>();
        mainObjTm.T4MMaterial.SetVector("_Tiling", new Vector4(T4MConfig.UpSideTile.x, T4MConfig.UpSideTile.y, 
                                                           T4MConfig.UpSideTile.z, T4MConfig.UpSideTile.w));
        mainObjTm.T4MMaterial.SetFloat("_UpSide", T4MConfig.UpSideF);
        mainObjTm.T4MMaterial.SetFloat("_Blend", T4MConfig.BlendFac);
    }

}
