/********************************************************************************
** Author： LiangZG
** Email :  game.liangzg@foxmail.com
*********************************************************************************/

using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using UnityEditor;

/// <summary>
/// 描述：转换目录视图
/// <para>创建时间：2016-07-05</para>
/// </summary>
public class ConverterMenuView
{
    int X;
    int Y;
    string terrainName = "";
    bool NewPref = false;
    bool keepTexture = false;

    int T4MResolution = 90;
    float tRes = 4.1f;

    float HeightmapWidth = 0;
    float HeightmapHeight = 0;

    private Transform CurrentSelect;

    int tCount;
    int counter;
    int totalCount;
    float progressUpdateInterval = 10000;

    string FinalExpName;
    TerrainData terrain;
    public void OnGUI()
    {
        CurrentSelect = T4MMainEditor.CurrentSelect;
        T4MMainObj mainObjT4MMain = CurrentSelect.GetComponent<T4MMainObj>();
         if (T4MCache.vertexInfo == 0 && T4MCache.trisInfo == 0 && T4MCache.partofT4MObj == 0)
        {
            Renderer selectRenderer = CurrentSelect.GetComponent<Renderer>();
            Terrain selectTerrain = CurrentSelect.GetComponent<Terrain>();
            if ((selectRenderer || selectTerrain || T4MCache.NbrPartObj != null && T4MCache.NbrPartObj.Length != 0) && !mainObjT4MMain && 
                !CurrentSelect.GetComponent<T4MPart>())
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (selectRenderer && !selectTerrain || T4MCache.NbrPartObj != null && T4MCache.NbrPartObj.Length != 0 )
                {
                    if (T4MCache.terrainDat)
                        T4MCache.terrainDat = null;
                    GUILayout.Label(">>>>>>>> Object to T4M Terrain <<<<<<<<", EditorStyles.boldLabel);
                }
                else {
                    if (!T4MCache.terrainDat && selectTerrain)
                        GetHeightmap();
                    GUILayout.Label(">> UnityTerrain to T4M Terrain (Experimental) <<", EditorStyles.boldLabel);
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                EditorGUILayout.Space();
                GUILayout.Label("Name", EditorStyles.boldLabel);
                GUILayout.BeginHorizontal("box");

                GUILayout.Label("(empty = Object Name)");
                terrainName = GUILayout.TextField(terrainName, 25, GUILayout.Width(155));
                GUILayout.EndHorizontal();

                if (selectRenderer && !selectTerrain || T4MCache.NbrPartObj != null && T4MCache.NbrPartObj.Length != 0)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("New Prefab", EditorStyles.boldLabel, GUILayout.Width(90));
                    NewPref = EditorGUILayout.Toggle(NewPref, GUILayout.Width(53));
                    EditorGUILayout.HelpBox("if true , while delect original gameobject !" , MessageType.None);
                    GUILayout.EndHorizontal();

                }
                else {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Keep the textures", EditorStyles.boldLabel, GUILayout.Width(150));
                    keepTexture = EditorGUILayout.Toggle(keepTexture, GUILayout.Width(53));
                    GUILayout.EndHorizontal();
                    GUILayout.Label("(Can keep the first 4 splats and first Blend)", GUILayout.Width(300));
                }

                if (selectTerrain)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    GUILayout.Label("T4M Quality", EditorStyles.boldLabel);


                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(" <");
                    GUILayout.FlexibleSpace();
                    T4MResolution = EditorGUILayout.IntField(T4MResolution, GUILayout.Width(30));
                    GUILayout.Label("x " + T4MResolution + " : " + (X * Y).ToString() + " Verts");
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(" >");
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    T4MResolution = (int)GUILayout.HorizontalScrollbar(T4MResolution, 0, 32, 350, GUILayout.Width(350));
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    tRes = (HeightmapWidth) / T4MResolution;
                    X = (int)((HeightmapWidth - 1) / tRes + 1);
                    Y = (int)((HeightmapHeight - 1) / tRes + 1);
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    GUILayout.Label("Vertex Performances (Approximate Indications)", EditorStyles.boldLabel);
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    Texture imgOK = AssetDatabase.LoadAssetAtPath<Texture>(T4MConfig.T4MEditorFolder + "Img/ok.png");
                    Texture imgAvoid = AssetDatabase.LoadAssetAtPath<Texture>(T4MConfig.T4MEditorFolder + "Img/avoid.png");
                    Texture imgKO = AssetDatabase.LoadAssetAtPath<Texture>(T4MConfig.T4MEditorFolder + "Img/ko.png");
                    
                    platformItemGUI("iPhone 3GS", 15000, 30000, imgOK, imgAvoid, imgKO);

                    platformItemGUI("iPad 1", 15000, 30000, imgOK, imgAvoid, imgKO);

                    platformItemGUI("iPhone 4", 20000, 40000, imgOK, imgAvoid, imgKO);

                    platformItemGUI("Tegra 2", 20000, 40000, imgOK, imgAvoid, imgKO);

                    platformItemGUI("iPad 2", 25000, 45000, imgOK, imgAvoid, imgKO);

                    platformItemGUI("iPhone 4S", 25000, 45000, imgOK, imgAvoid, imgKO);

                    platformItemGUI("Flash" , 45000 , 60000 , imgOK , imgAvoid , imgKO);

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Web", GUILayout.Width(300));
                    GUILayout.Label(imgOK);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Desktop", GUILayout.Width(300));
                    GUILayout.Label(imgOK);
                    GUILayout.EndHorizontal();
                }
                GUILayout.FlexibleSpace();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("Can Take Some Time", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (selectRenderer && !selectTerrain || T4MCache.NbrPartObj != null && T4MCache.NbrPartObj.Length != 0)
                {
                    if (GUILayout.Button("PROCESS", GUILayout.Width(100), GUILayout.Height(30)))
                    {
                        Obj2T4M();
                    }
                }
                else
                {
                    if (GUILayout.Button("PROCESS", GUILayout.Width(100), GUILayout.Height(30)))
                    {
                        ConvertUTerrain();
                    }

                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();
            }
            else
            {
                T4MCache.terrainDat = null;
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (mainObjT4MMain)
                    GUILayout.Label("Already T4M Object", EditorStyles.boldLabel);
                else GUILayout.Label("Can't convert that to T4M Object", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
        }
        else
        {
            GUILayout.Label("T4M Final Resolution : ", EditorStyles.boldLabel);
            if (T4MCache.partofT4MObj > 1)
                GUILayout.Label("Vertex : ~" + T4MCache.vertexInfo + " in " + T4MCache.partofT4MObj + " Parts");
            else GUILayout.Label("Vertex : " + T4MCache.vertexInfo + " in " + T4MCache.partofT4MObj + " Part");
            GUILayout.Label("Triangle : " + T4MCache.trisInfo);
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            GUILayout.BeginVertical("Box");
            GUILayout.Label("Since Unity 3.5, some converted objects can be ", EditorStyles.boldLabel);
            GUILayout.Label("no smooth : ", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            GUILayout.Label("Select the New Mesh in the Project window :");
            GUILayout.Label("in T4MOBJ/Meshes/\"yourobject\"");
            EditorGUILayout.Space();
            GUILayout.Label("In Inspector window :");
            GUILayout.Label("Descrease \"Smoothing Angle\", Increase again to 180");
            GUILayout.Label("And \"Apply\"");
            EditorGUILayout.Space();
            GUILayout.Label("Now Select your Object on the scene :");
            GUILayout.Label("Uncheck/check the box the \"Mesh Collider\" in ");
            GUILayout.Label("Inspector window");
            GUILayout.EndVertical();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            if (GUILayout.Button("Keep my Conversion and Destroy Original"))
            {
                GameObject.DestroyImmediate(CurrentSelect.gameObject);
                Selection.activeTransform = T4MCache.Child.transform;
                T4MCache.vertexInfo = 0;
                T4MCache.trisInfo = 0;
                T4MCache.partofT4MObj = 0;
                T4MCache.T4MMenuToolbar = 1;

                if (T4MCache.nbrT4MObj == 0)
                {
                    T4MMainObj mainObjTm = T4MCache.Child.gameObject.GetComponent<T4MMainObj>();
                    mainObjTm.EnabledLODSystem = T4MConfig.ActivatedLOD;
                    mainObjTm.enabledBillboard = T4MConfig.ActivatedBillboard;
                    mainObjTm.enabledLayerCul = T4MConfig.ActivatedLayerCul;
                    mainObjTm.CloseView = T4MConfig.CloseDistMaxView;
                    mainObjTm.NormalView = T4MConfig.NormalDistMaxView;
                    mainObjTm.FarView = T4MConfig.FarDistMaxView;
                    mainObjTm.BackGroundView = T4MConfig.BGDistMaxView;
                    mainObjTm.Master = 1;
                }
            }
            if (GUILayout.Button("Modify Options and Start a New Conversion"))
            {
                GameObject.DestroyImmediate(T4MCache.Child);
                AssetDatabase.DeleteAsset(T4MConfig.T4MPrefabFolder + "Terrains/Meshes/" + FinalExpName + ".obj");
                AssetDatabase.DeleteAsset(T4MConfig.T4MPrefabFolder + "Terrains/" + FinalExpName + ".prefab");
                AssetDatabase.DeleteAsset(T4MConfig.T4MPrefabFolder + "Terrains/Texture/" + FinalExpName + ".png");
                AssetDatabase.DeleteAsset(T4MConfig.T4MPrefabFolder + "Terrains/Material/" + FinalExpName + ".mat");
                CurrentSelect.GetComponent<Terrain>().enabled = true;
                T4MCache.vertexInfo = 0;
                T4MCache.trisInfo = 0;
                T4MCache.partofT4MObj = 0;
                T4MCache.UnityTerrain = null;
                T4MCache.terrainDat = null;
            }
            if (GUILayout.Button("Keep Both and Continue"))
            {
                T4MCache.UnityTerrain.SetActive(false);
                T4MCache.UnityTerrain = null;
                Selection.activeTransform = T4MCache.Child.transform;
                T4MCache.vertexInfo = 0;
                T4MCache.trisInfo = 0;
                T4MCache.partofT4MObj = 0;
                T4MCache.T4MMenuToolbar = 1;

                if (T4MCache.nbrT4MObj == 0)
                {
                    T4MMainObj mainObjTm = T4MCache.Child.gameObject.GetComponent<T4MMainObj>();
                    mainObjTm.EnabledLODSystem = T4MConfig.ActivatedLOD;
                    mainObjTm.enabledBillboard = T4MConfig.ActivatedBillboard;
                    mainObjTm.enabledLayerCul = T4MConfig.ActivatedLayerCul;
                    mainObjTm.CloseView = T4MConfig.CloseDistMaxView;
                    mainObjTm.NormalView = T4MConfig.NormalDistMaxView;
                    mainObjTm.FarView = T4MConfig.FarDistMaxView;
                    mainObjTm.BackGroundView = T4MConfig.BGDistMaxView;
                    mainObjTm.Master = 1;
                }
            }
        }
    }

    /// <summary>
    /// 平台兼容项
    /// </summary>
    /// <param name="platformName"></param>
    /// <param name="minVertex"></param>
    /// <param name="maxVertax"></param>
    /// <param name="imgOK"></param>
    /// <param name="imgAvoid"></param>
    /// <param name="imgKO"></param>
    private void platformItemGUI(string platformName , int minVertex , int maxVertax , Texture imgOK , Texture imgAvoid , Texture imgKO)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(platformName, GUILayout.Width(300));
        if (X * Y <= minVertex) GUILayout.Label(imgOK);
        else if (X * Y > minVertex && X * Y < maxVertax) GUILayout.Label(imgAvoid);
        else if (X * Y >= maxVertax) GUILayout.Label(imgKO);
        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// 获得高度图
    /// </summary>
    private void GetHeightmap()
    {
        T4MCache.terrainDat = CurrentSelect.GetComponent<Terrain>().terrainData;
        HeightmapWidth = T4MCache.terrainDat.heightmapWidth;
        HeightmapHeight = T4MCache.terrainDat.heightmapHeight;
    }


    void Obj2T4M()
    {
        if (terrainName == "")
            terrainName = CurrentSelect.name;

        if (!System.IO.Directory.Exists(T4MConfig.T4MPrefabFolder + "Terrains/"))
        {
            System.IO.Directory.CreateDirectory(T4MConfig.T4MPrefabFolder + "Terrains/");
        }
        if (!System.IO.Directory.Exists(T4MConfig.T4MPrefabFolder + "Terrains/Material/"))
        {
            System.IO.Directory.CreateDirectory(T4MConfig.T4MPrefabFolder + "Terrains/Material/");
        }
        if (!System.IO.Directory.Exists(T4MConfig.T4MPrefabFolder + "Terrains/Texture/"))
        {
            System.IO.Directory.CreateDirectory(T4MConfig.T4MPrefabFolder + "Terrains/Texture/");
        }
        AssetDatabase.Refresh();

        Texture2D NewMaskText = new Texture2D(512, 512, TextureFormat.ARGB32, true);
        Color[] ColorBase = new Color[512 * 512];
        for (var t = 0; t < ColorBase.Length; t++)
        {
            ColorBase[t] = new Color(1, 0, 0, 0);
        }

        NewMaskText.SetPixels(ColorBase);


        bool ExportNameSuccess = false;
        int num = 1;
        string Next;
        do
        {
            Next = terrainName + num;

            if (!System.IO.File.Exists(T4MConfig.T4MPrefabFolder + "Terrains/Material/" + terrainName + ".mat"))
            {
                FinalExpName = terrainName;
                ExportNameSuccess = true;
            }
            else if (!System.IO.File.Exists(T4MConfig.T4MPrefabFolder + "Terrains/Material/" + Next + ".mat"))
            {
                FinalExpName = Next;
                ExportNameSuccess = true;
            }
            num++;
        } while (!ExportNameSuccess);


        var path = T4MConfig.T4MPrefabFolder + "Terrains/Texture/" + FinalExpName + ".png";
        var data = NewMaskText.EncodeToPNG();
        File.WriteAllBytes(path, data);
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        var TextureIm = AssetImporter.GetAtPath(path) as TextureImporter;
        TextureIm.textureFormat = TextureImporterFormat.ARGB32;
        TextureIm.isReadable = true;
        TextureIm.anisoLevel = 9;
        TextureIm.mipmapEnabled = false;
        TextureIm.wrapMode = TextureWrapMode.Clamp;
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        Material Tmaterial = new Material(Shader.Find("iT4MShaders/ShaderModel2/Diffuse/T4M 4 Textures"));
        AssetDatabase.CreateAsset(Tmaterial, T4MConfig.T4MPrefabFolder + "Terrains/Material/" + FinalExpName + ".mat");
        AssetDatabase.ImportAsset(T4MConfig.T4MPrefabFolder + "Terrains/Material/" + FinalExpName + ".mat", ImportAssetOptions.ForceUpdate);
        
        Texture FinalMaterial = (Texture)AssetDatabase.LoadAssetAtPath(path, typeof(Texture));

        T4MMainObj newMainObjT4MMain = CurrentSelect.gameObject.AddComponent<T4MMainObj>();

        int countchild = CurrentSelect.transform.childCount;
        if (countchild > 0)
        {
            Renderer[] T4MOBJPART = CurrentSelect.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < T4MOBJPART.Length; i++)
            {
                if (T4MOBJPART[i].gameObject.GetComponent<Collider>())
                    GameObject.DestroyImmediate(T4MOBJPART[i].gameObject.GetComponent<Collider>());

                T4MOBJPART[i].gameObject.AddComponent<MeshCollider>();

                T4MOBJPART[i].gameObject.isStatic = true;

                T4MOBJPART[i].material = Tmaterial;
                T4MOBJPART[i].gameObject.layer = 30;
                T4MOBJPART[i].gameObject.AddComponent<T4MPart>();
                newMainObjT4MMain.T4MMesh = T4MOBJPART[0].GetComponent<MeshFilter>();

            }
        }
        else {
            if (CurrentSelect.GetComponent<Collider>())
                GameObject.DestroyImmediate(CurrentSelect.GetComponent<Collider>());

            CurrentSelect.gameObject.AddComponent<MeshCollider>();
            CurrentSelect.gameObject.GetComponent<Renderer>().material = newMainObjT4MMain.T4MMaterial = Tmaterial;
            newMainObjT4MMain.T4MMesh = CurrentSelect.gameObject.GetComponent<MeshFilter>();

        }
        newMainObjT4MMain.T4MMaterial = Tmaterial;
        newMainObjT4MMain.T4MMaterial.SetTexture("_Control", FinalMaterial);
        CurrentSelect.gameObject.isStatic = true;
        CurrentSelect.gameObject.layer = 30;

        if (T4MCache.nbrT4MObj == 0)
        {
            T4MMainObj mainObjTm = CurrentSelect.gameObject.GetComponent<T4MMainObj>();
            mainObjTm.EnabledLODSystem = T4MConfig.ActivatedLOD;
            mainObjTm.enabledBillboard = T4MConfig.ActivatedBillboard;
            mainObjTm.enabledLayerCul = T4MConfig.ActivatedLayerCul;
            mainObjTm.CloseView = T4MConfig.CloseDistMaxView;
            mainObjTm.NormalView = T4MConfig.NormalDistMaxView;
            mainObjTm.FarView = T4MConfig.FarDistMaxView;
            mainObjTm.BackGroundView = T4MConfig.BGDistMaxView;
            mainObjTm.Master = 1;
        }


        if (NewPref)
        {
            UnityEngine.Object BasePrefab = PrefabUtility.CreateEmptyPrefab(T4MConfig.T4MPrefabFolder + "Terrains/" + FinalExpName + ".prefab");
            PrefabUtility.ReplacePrefab(CurrentSelect.gameObject, BasePrefab);

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(T4MConfig.T4MPrefabFolder + "Terrains/" + FinalExpName + ".prefab");
            GameObject forRotate = (GameObject)PrefabUtility.InstantiatePrefab(prefab) as GameObject;

            GameObject.DestroyImmediate(CurrentSelect.gameObject);
            Selection.activeTransform = forRotate.transform;
            EditorUtility.SetSelectedWireframeHidden(forRotate.GetComponent<Renderer>(), true);
        }
        else {
            Selection.activeTransform = CurrentSelect.transform;
            EditorUtility.SetSelectedWireframeHidden(CurrentSelect.GetComponent<Renderer>(), true);
        }

        CurrentSelect.gameObject.name = FinalExpName;

        EditorUtility.DisplayDialog("T4M Message", "Conversion Completed !", "OK");
        
        T4MCache.T4MMenuToolbar = 1;
        terrainName = "";
        AssetDatabase.SaveAssets();
    }

    /// <summary>
    /// 转换Unity地形
    /// </summary>
    void ConvertUTerrain()
    {
        if (terrainName == "")
            terrainName = CurrentSelect.name;

        if (!System.IO.Directory.Exists(T4MConfig.T4MPrefabFolder + "Terrains/"))
        {
            System.IO.Directory.CreateDirectory(T4MConfig.T4MPrefabFolder + "Terrains/");
        }
        if (!System.IO.Directory.Exists(T4MConfig.T4MPrefabFolder + "Terrains/Material/"))
        {
            System.IO.Directory.CreateDirectory(T4MConfig.T4MPrefabFolder + "Terrains/Material/");
        }
        if (!System.IO.Directory.Exists(T4MConfig.T4MPrefabFolder + "Terrains/Texture/"))
        {
            System.IO.Directory.CreateDirectory(T4MConfig.T4MPrefabFolder + "Terrains/Texture/");
        }
        if (!System.IO.Directory.Exists(T4MConfig.T4MPrefabFolder + "Terrains/Meshes/"))
        {
            System.IO.Directory.CreateDirectory(T4MConfig.T4MPrefabFolder + "Terrains/Meshes/");
        }
        AssetDatabase.Refresh();

        terrain = CurrentSelect.GetComponent<Terrain>().terrainData;
        int w = terrain.heightmapWidth;
        int h = terrain.heightmapHeight;
        Vector3 meshScale = terrain.size;
        meshScale = new Vector3(meshScale.x / (h - 1) * tRes, meshScale.y, meshScale.z / (w - 1) * tRes);
        Vector2 uvScale = new Vector2((float)(1.0 / (w - 1)), (float)(1.0 / (h - 1)));

        float[,] tData = terrain.GetHeights(0, 0, w, h);
        w = (int)((w - 1) / tRes + 1);
        h = (int)((h - 1) / tRes + 1);
        Vector3[] tVertices = new Vector3[w * h];
        Vector2[] tUV = new Vector2[w * h];
        int[] tPolys = new int[(w - 1) * (h - 1) * 6];
        int y = 0;
        int x = 0;
        for (y = 0; y < h; y++)
        {
            for (x = 0; x < w; x++)
            {
                //tVertices[y*w + x] = Vector3.Scale(meshScale, new Vector3(x, tData[(int)(x*tRes),(int)(y*tRes)], y));
                tVertices[y * w + x] = Vector3.Scale(meshScale, new Vector3(-y, tData[(int)(x * tRes), (int)(y * tRes)], x)); //Thank Cid Newman
                tUV[y * w + x] = Vector2.Scale(new Vector2(y * tRes, x * tRes), uvScale);
            }
        }

        y = 0;
        x = 0;
        int index = 0;
        for (y = 0; y < h - 1; y++)
        {
            for (x = 0; x < w - 1; x++)
            {
                tPolys[index++] = (y * w) + x;
                tPolys[index++] = ((y + 1) * w) + x;
                tPolys[index++] = (y * w) + x + 1;

                tPolys[index++] = ((y + 1) * w) + x;
                tPolys[index++] = ((y + 1) * w) + x + 1;
                tPolys[index++] = (y * w) + x + 1;
            }
        }

        bool ExportNameSuccess = false;
        int num = 1;
        string Next;
        do
        {
            Next = terrainName + num;

            if (!System.IO.File.Exists(T4MConfig.T4MPrefabFolder + "Terrains/" + terrainName + ".prefab"))
            {
                FinalExpName = terrainName;
                ExportNameSuccess = true;
            }
            else if (!System.IO.File.Exists(T4MConfig.T4MPrefabFolder + "Terrains/" + Next + ".prefab"))
            {
                FinalExpName = Next;
                ExportNameSuccess = true;
            }
            num++;
        } while (!ExportNameSuccess);

        //StreamWriter  sw = new StreamWriter(T4MConfig.T4MPrefabFolder+"Terrains/Meshes/"+FinalExpName+".obj");
        StreamWriter sw = new StreamWriter(FinalExpName + ".obj");
        try
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("# T4M File");
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            counter = tCount = 0;
            totalCount = (int)((tVertices.Length * 2 + (tPolys.Length / 3)) / progressUpdateInterval);
            
            for (int i = 0; i < tVertices.Length; i++)
            {
                UpdateProgress();
                sb.AppendFormat("v {0} {1} {2} \n", tVertices[i].x, tVertices[i].y, tVertices[i].z);
            }

            for (int i = 0; i < tUV.Length; i++)
            {
                UpdateProgress();
                sb.AppendFormat("vt {0} {1} \n", tUV[i].x, tUV[i].y);
            }
            for (int i = 0; i < tPolys.Length; i += 3)
            {
                UpdateProgress();

                sb.AppendFormat("f {0}/{0} {1}/{1} {2}/{2}\n", tPolys[i] + 1, tPolys[i + 1] + 1, tPolys[i + 2] + 1);
            }

            sw.Write(sb.ToString());
        }
        catch (Exception err)
        {
            Debug.Log("Error saving file: " + err.Message);
        }
        sw.Close();
        AssetDatabase.SaveAssets();

        string path = T4MConfig.T4MPrefabFolder + "Terrains/Texture/" + FinalExpName + ".png";

        //Control Texture Creation or Recuperation
        string AssetName = AssetDatabase.GetAssetPath(CurrentSelect.GetComponent<Terrain>().terrainData) as string;
        UnityEngine.Object[] AssetName2 = AssetDatabase.LoadAllAssetsAtPath(AssetName);
        if (AssetName2 != null && AssetName2.Length > 1 && keepTexture)
        {
            for (var b = 0; b < AssetName2.Length; b++)
            {
                if (AssetName2[b].name == "SplatAlpha 0")
                {
                    Texture2D texture = AssetName2[b] as Texture2D;
                    byte[] bytes = texture.EncodeToPNG();
                    File.WriteAllBytes(path, bytes);
                    AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                }
            }
        }
        else {
            Texture2D NewMaskText = new Texture2D(512, 512, TextureFormat.ARGB32, true);
            Color[] ColorBase = new Color[512 * 512];
            for (var t = 0; t < ColorBase.Length; t++)
            {
                ColorBase[t] = new Color(1, 0, 0, 0);
            }
            NewMaskText.SetPixels(ColorBase);
            byte[] data = NewMaskText.EncodeToPNG();
            File.WriteAllBytes(path, data);
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

        UpdateProgress();

        //Modification de la Texture 
        TextureImporter TextureI = AssetImporter.GetAtPath(path) as TextureImporter;
        TextureI.textureFormat = TextureImporterFormat.ARGB32;
        TextureI.isReadable = true;
        TextureI.anisoLevel = 9;
        TextureI.mipmapEnabled = false;
        TextureI.wrapMode = TextureWrapMode.Clamp;
        AssetDatabase.Refresh();

        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

        UpdateProgress();

        //Creation du Materiel
        Material Tmaterial = new Material(Shader.Find("iT4MShaders/ShaderModel2/Diffuse/T4M 4 Textures"));
        AssetDatabase.CreateAsset(Tmaterial, T4MConfig.T4MPrefabFolder + "Terrains/Material/" + FinalExpName + ".mat");
        AssetDatabase.ImportAsset(T4MConfig.T4MPrefabFolder + "Terrains/Material/" + FinalExpName + ".mat", ImportAssetOptions.ForceUpdate);
        AssetDatabase.Refresh();

        //Recuperation des Texture du terrain
        if (keepTexture)
        {
            SplatPrototype[] texts = CurrentSelect.GetComponent<Terrain>().terrainData.splatPrototypes;
            for (int e = 0; e < texts.Length; e++)
            {
                if (e < 4)
                {
                    Tmaterial.SetTexture("_Splat" + e, texts[e].texture);
                    Tmaterial.SetTextureScale("_Splat" + e, texts[e].tileSize * 8.9f);
                }
            }
        }

        //Attribution de la Texture Control sur le materiau
        Texture test = (Texture)AssetDatabase.LoadAssetAtPath(path, typeof(Texture));
        Tmaterial.SetTexture("_Control", test);


        UpdateProgress();


        //Deplacement de l'obj dans les repertoire mesh
        FileUtil.CopyFileOrDirectory(FinalExpName + ".obj", T4MConfig.T4MPrefabFolder + "Terrains/Meshes/" + FinalExpName + ".obj");
        FileUtil.DeleteFileOrDirectory(FinalExpName + ".obj");



        //Force Update
        AssetDatabase.ImportAsset(T4MConfig.T4MPrefabFolder + "Terrains/Meshes/" + FinalExpName + ".obj", ImportAssetOptions.ForceUpdate);

        UpdateProgress();

        //Instance du T4M
        GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(T4MConfig.T4MPrefabFolder + "Terrains/Meshes/" + FinalExpName + ".obj", typeof(GameObject));

        AssetDatabase.Refresh();


        GameObject forRotate = (GameObject)GameObject.Instantiate(prefab, CurrentSelect.transform.position, Quaternion.identity) as GameObject;
        Transform childCheck = forRotate.transform.Find("default");
        T4MCache.Child = childCheck.gameObject;
        forRotate.transform.DetachChildren();
        GameObject.DestroyImmediate(forRotate);
        T4MCache.Child.name = FinalExpName;
        T4MCache.Child.AddComponent<T4MMainObj>();
        //T4MCache.Child.transform.rotation= Quaternion.Euler(0, 90, 0);

        UpdateProgress();

        //Application des Parametres sur le Script
        T4MCache.Child.GetComponent<T4MMainObj>().T4MMaterial = Tmaterial;
        T4MCache.Child.GetComponent<T4MMainObj>().ConvertType = "UT";

        //Regalges Divers
        T4MCache.vertexInfo = 0;
        T4MCache.partofT4MObj = 0;
        T4MCache.trisInfo = 0;
        int countchild = T4MCache.Child.transform.childCount;
        if (countchild > 0)
        {
            Renderer[] T4MOBJPART = T4MCache.Child.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < T4MOBJPART.Length; i++)
            {
                if (!T4MOBJPART[i].gameObject.AddComponent<MeshCollider>())
                    T4MOBJPART[i].gameObject.AddComponent<MeshCollider>();
                T4MOBJPART[i].gameObject.isStatic = true;
                T4MOBJPART[i].material = Tmaterial;
                T4MOBJPART[i].gameObject.layer = 30;
                T4MOBJPART[i].gameObject.AddComponent<T4MPart>();
                T4MCache.Child.GetComponent<T4MMainObj>().T4MMesh = T4MOBJPART[0].GetComponent<MeshFilter>();
                T4MCache.partofT4MObj += 1;
                T4MCache.vertexInfo += T4MOBJPART[i].gameObject.GetComponent<MeshFilter>().sharedMesh.vertexCount;
                T4MCache.trisInfo += T4MOBJPART[i].gameObject.GetComponent<MeshFilter>().sharedMesh.triangles.Length / 3;
            }
        }
        else {
            T4MCache.Child.AddComponent<MeshCollider>();
            T4MCache.Child.isStatic = true;
            T4MCache.Child.GetComponent<Renderer>().material = Tmaterial;
            T4MCache.Child.layer = 30;
            T4MCache.vertexInfo += T4MCache.Child.GetComponent<MeshFilter>().sharedMesh.vertexCount;
            T4MCache.trisInfo += T4MCache.Child.GetComponent<MeshFilter>().sharedMesh.triangles.Length / 3;
            T4MCache.partofT4MObj += 1;
        }

        UpdateProgress();


        GameObject BasePrefab2 = PrefabUtility.CreatePrefab(T4MConfig.T4MPrefabFolder + "Terrains/" + FinalExpName + ".prefab", T4MCache.Child);
        AssetDatabase.ImportAsset(T4MConfig.T4MPrefabFolder + "Terrains/" + FinalExpName + ".prefab", ImportAssetOptions.ForceUpdate);
        GameObject forRotate2 = (GameObject)PrefabUtility.InstantiatePrefab(BasePrefab2) as GameObject;

        GameObject.DestroyImmediate(T4MCache.Child.gameObject);

        T4MCache.Child = forRotate2.gameObject;

        CurrentSelect.GetComponent<Terrain>().enabled = false;

        EditorUtility.SetSelectedWireframeHidden(T4MCache.Child.GetComponent<Renderer>(), true);

        T4MCache.UnityTerrain = CurrentSelect.gameObject;

        EditorUtility.ClearProgressBar();

        AssetDatabase.DeleteAsset(T4MConfig.T4MPrefabFolder + "Terrains/Meshes/Materials");
        terrainName = "";
        AssetDatabase.StartAssetEditing();
        //Modification des attribut du mesh avant de le pr茅fabriquer
        ModelImporter OBJI = ModelImporter.GetAtPath(T4MConfig.T4MPrefabFolder + "Terrains/Meshes/" + FinalExpName + ".obj") as ModelImporter;
        OBJI.globalScale = 1;
        OBJI.splitTangentsAcrossSeams = true;
        OBJI.normalImportMode = ModelImporterTangentSpaceMode.Calculate;
        OBJI.tangentImportMode = ModelImporterTangentSpaceMode.Calculate;
        OBJI.generateAnimations = ModelImporterGenerateAnimations.None;
        OBJI.meshCompression = ModelImporterMeshCompression.Off;
        OBJI.normalSmoothingAngle = 180f;
        //AssetDatabase.ImportAsset (T4MConfig.T4MPrefabFolder+"Terrains/Meshes/"+FinalExpName+".obj", ImportAssetOptions.TryFastReimportFromMetaData);
        AssetDatabase.ImportAsset(T4MConfig.T4MPrefabFolder + "Terrains/Meshes/" + FinalExpName + ".obj", ImportAssetOptions.ForceSynchronousImport);
        AssetDatabase.StopAssetEditing();
        PrefabUtility.ResetToPrefabState(T4MCache.Child);
    }

    void UpdateProgress()
    {
        if (counter++ == progressUpdateInterval)
        {
            counter = 0;
            EditorUtility.DisplayProgressBar("Generate...", "", Mathf.InverseLerp(0, totalCount, ++tCount));
        }
    }


    public void OnDestroy()
    {
        

    }
}
