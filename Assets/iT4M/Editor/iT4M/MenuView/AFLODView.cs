/********************************************************************************
** Author： LiangZG
** Email :  game.liangzg@foxmail.com
*********************************************************************************/
using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 描述：LOD操作视图
/// <para>创建时间：2016-07-05</para>
/// </summary>
public class AFLODView
{

    private Transform CurrentSelect;

    string[] LODMenu = { "LOD Manager", "LOD Composer" };
    int LODM = 0;
    Mesh LOD1;
    Mesh LOD2;
    Mesh LOD3;

    bool CheckLOD1Collider;
    bool CheckLOD2Collider;
    bool CheckLOD3Collider;

    string CheckStatus;

    LODShaderStatus OldShaderLOD1S;
    LODShaderStatus OldShaderLOD2S;
    LODShaderStatus OldShaderLOD3S;

    public void OnGUI()
    {
        CurrentSelect = T4MMainEditor.CurrentSelect;
        T4MMainObj mainObjT4MMain = CurrentSelect.GetComponent<T4MMainObj>();
        if (mainObjT4MMain)
        {
            if (mainObjT4MMain.T4MMaterial && mainObjT4MMain.T4MMaterial.HasProperty("_Splat0") && 
                mainObjT4MMain.T4MMaterial.HasProperty("_Splat1") && mainObjT4MMain.T4MMaterial.HasProperty("_Control"))
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                LODM = GUILayout.Toolbar(LODM, LODMenu, GUILayout.Width(290), GUILayout.Height(20));
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                switch (LODM)
                {
                    case 0:
                        LODManager();
                        break;
                    case 1:
                        LODObjectC();
                        break;
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



    void LODObjectC()
    {


        EditorGUILayout.Space();

        GUILayout.BeginHorizontal("box");
        GUILayout.Label("LOD Prefab Name", EditorStyles.boldLabel);
        T4MCache.PrefabName = GUILayout.TextField(T4MCache.PrefabName, 25, GUILayout.Width(155));
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();

        GUILayout.Label("LOD Meshes", EditorStyles.boldLabel);
        LOD1 = (Mesh)EditorGUILayout.ObjectField("LOD1 : Close", LOD1, typeof(Mesh), false);
        LOD2 = (Mesh)EditorGUILayout.ObjectField("LOD2 : Medium", LOD2, typeof(Mesh), false);
        LOD3 = (Mesh)EditorGUILayout.ObjectField("LOD3 : Far", LOD3, typeof(Mesh), false);

        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.Label("LOD1 Setup", EditorStyles.boldLabel, GUILayout.Width(223));
        GUILayout.Label("MainTex", EditorStyles.boldLabel, GUILayout.Width(68));
        GUILayout.Label("Bump", EditorStyles.boldLabel, GUILayout.Width(60));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Place the MeshCollider on this LOD", GUILayout.Width(205));
        CheckLOD1Collider = EditorGUILayout.Toggle(CheckLOD1Collider, GUILayout.Width(15));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("LOD1 Shader", GUILayout.Width(103));
        T4MCache.ShaderLOD1S = (LODShaderStatus)EditorGUILayout.EnumPopup(T4MCache.ShaderLOD1S, GUILayout.Width(95));
        GUILayout.EndHorizontal();
        if (T4MCache.ShaderLOD1S == LODShaderStatus.New)
            T4MCache.LOD1S = (Shader)EditorGUILayout.ObjectField(T4MCache.LOD1S, typeof(Shader), true, GUILayout.MaxWidth(220));
        else T4MCache.LOD1Material = (Material)EditorGUILayout.ObjectField(T4MCache.LOD1Material, typeof(Material), false, GUILayout.MaxWidth(220));
        GUILayout.EndVertical();
        if (T4MCache.LOD1S)
            T4MCache.LOD1Material = new Material(Shader.Find(T4MCache.LOD1S.name));

        if (T4MCache.LOD1Material && T4MCache.LOD1Material.HasProperty("_MainTex"))
        {
            if (T4MCache.LOD1Material.GetTexture("_MainTex"))
                T4MCache.LOD1T = T4MCache.LOD1Material.GetTexture("_MainTex");
            T4MCache.LOD1T = EditorGUILayout.ObjectField(T4MCache.LOD1T, typeof(Texture), false, GUILayout.Width(60), GUILayout.Height(60)) as Texture;
            if (T4MCache.LOD1Material && T4MCache.LOD1Material.HasProperty("_BumpMap"))
            {
                if (T4MCache.LOD1Material.GetTexture("_BumpMap"))
                    T4MCache.LOD1B = T4MCache.LOD1Material.GetTexture("_BumpMap");
                T4MCache.LOD1B = EditorGUILayout.ObjectField(T4MCache.LOD1B, typeof(Texture), false, GUILayout.Width(60), GUILayout.Height(60)) as Texture;
            }
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.Label("LOD2 Setup", EditorStyles.boldLabel, GUILayout.Width(223));
        GUILayout.Label("MainTex", EditorStyles.boldLabel, GUILayout.Width(68));
        GUILayout.Label("Bump", EditorStyles.boldLabel, GUILayout.Width(60));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Place the MeshCollider on this LOD", GUILayout.Width(205));
        CheckLOD2Collider = EditorGUILayout.Toggle(CheckLOD2Collider, GUILayout.Width(15));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("LOD2 Shader", GUILayout.Width(103));
        T4MCache.ShaderLOD2S = (LODShaderStatus)EditorGUILayout.EnumPopup(T4MCache.ShaderLOD2S, GUILayout.Width(95));
        GUILayout.EndHorizontal();
        if (T4MCache.ShaderLOD2S == LODShaderStatus.New)
            T4MCache.LOD2S = (Shader)EditorGUILayout.ObjectField(T4MCache.LOD2S, typeof(Shader), false, GUILayout.MaxWidth(220));
        else T4MCache.LOD2Material = (Material)EditorGUILayout.ObjectField(T4MCache.LOD2Material, typeof(Material), false, GUILayout.MaxWidth(220));
        GUILayout.EndVertical();
        if (T4MCache.LOD2S)
            T4MCache.LOD2Material = new Material(Shader.Find(T4MCache.LOD2S.name));
        if (T4MCache.LOD2Material && T4MCache.LOD2Material.HasProperty("_MainTex"))
        {
            if (T4MCache.LOD2Material.GetTexture("_MainTex"))
                T4MCache.LOD2T = T4MCache.LOD2Material.GetTexture("_MainTex");
            T4MCache.LOD2T = EditorGUILayout.ObjectField(T4MCache.LOD2T, typeof(Texture), false, GUILayout.Width(60), GUILayout.Height(60)) as Texture;
            if (T4MCache.LOD2Material && T4MCache.LOD2Material.HasProperty("_BumpMap"))
            {
                if (T4MCache.LOD2Material.GetTexture("_BumpMap"))
                    T4MCache.LOD2B = T4MCache.LOD2Material.GetTexture("_BumpMap");
                T4MCache.LOD2B = EditorGUILayout.ObjectField(T4MCache.LOD2B, typeof(Texture), false, GUILayout.Width(60), GUILayout.Height(60)) as Texture;
            }
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.Label("LOD3 Setup", EditorStyles.boldLabel, GUILayout.Width(223));
        GUILayout.Label("MainTex", EditorStyles.boldLabel, GUILayout.Width(68));
        GUILayout.Label("Bump", EditorStyles.boldLabel, GUILayout.Width(60));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Place the MeshCollider on this LOD", GUILayout.Width(205));
        CheckLOD3Collider = EditorGUILayout.Toggle(CheckLOD3Collider, GUILayout.Width(15));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("LOD3 Shader", GUILayout.Width(103));
        T4MCache.ShaderLOD3S = (LODShaderStatus)EditorGUILayout.EnumPopup(T4MCache.ShaderLOD3S, GUILayout.Width(95));
        GUILayout.EndHorizontal();
        if (T4MCache.ShaderLOD3S == LODShaderStatus.New)
            T4MCache.LOD3S = (Shader)EditorGUILayout.ObjectField(T4MCache.LOD3S, typeof(Shader), false, GUILayout.MaxWidth(220));
        else T4MCache.LOD3Material = (Material)EditorGUILayout.ObjectField(T4MCache.LOD3Material, typeof(Material), false, GUILayout.MaxWidth(220));
        GUILayout.EndVertical();
        if (T4MCache.LOD3S)
            T4MCache.LOD3Material = new Material(Shader.Find(T4MCache.LOD3S.name));
        if (T4MCache.LOD3Material && T4MCache.LOD3Material.HasProperty("_MainTex"))
        {
            if (T4MCache.LOD3Material.GetTexture("_MainTex"))
                T4MCache.LOD3T = T4MCache.LOD3Material.GetTexture("_MainTex");
            T4MCache.LOD3T = EditorGUILayout.ObjectField(T4MCache.LOD3T, typeof(Texture), false, GUILayout.Width(60), GUILayout.Height(60)) as Texture;
            if (T4MCache.LOD3Material && T4MCache.LOD3Material.HasProperty("_BumpMap"))
            {
                if (T4MCache.LOD3Material.GetTexture("_BumpMap"))
                    T4MCache.LOD3B = T4MCache.LOD3Material.GetTexture("_BumpMap");
                T4MCache.LOD3B = EditorGUILayout.ObjectField(T4MCache.LOD3B, typeof(Texture), false, GUILayout.Width(60), GUILayout.Height(60)) as Texture;
            }
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("CONSTRUCT", GUILayout.Width(100), GUILayout.Height(30)))
        {
            if (T4MCache.PrefabName != "" && LOD1 && LOD2 && LOD3 && T4MCache.LOD1Material && T4MCache.LOD2Material && T4MCache.LOD3Material)
                CreatePrefab();
            else EditorUtility.DisplayDialog("T4M Message", "You must complete the formulary before make the construct", "OK");
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();

        if (CheckLOD1Collider == true && CheckStatus != "LOD1")
        {
            CheckStatus = "LOD1";
            CheckLOD2Collider = false;
            CheckLOD3Collider = false;
        }
        if (CheckLOD2Collider == true && CheckStatus != "LOD2")
        {
            CheckStatus = "LOD2";
            CheckLOD1Collider = false;
            CheckLOD3Collider = false;
        }
        if (CheckLOD3Collider == true && CheckStatus != "LOD3")
        {
            CheckStatus = "LOD3";
            CheckLOD1Collider = false;
            CheckLOD2Collider = false;
        }
        if (OldShaderLOD1S != T4MCache.ShaderLOD1S)
        {
            T4MCache.LOD1B = null;
            T4MCache.LOD1T = null;
            T4MCache.LOD1Material = null;
            T4MCache.LOD1S = null;
            OldShaderLOD1S = T4MCache.ShaderLOD1S;
        }
        if (OldShaderLOD2S != T4MCache.ShaderLOD2S)
        {
            T4MCache.LOD2B = null;
            T4MCache.LOD2T = null;
            T4MCache.LOD2Material = null;
            T4MCache.LOD2S = null;
            OldShaderLOD2S = T4MCache.ShaderLOD2S;
        }
        if (OldShaderLOD3S != T4MCache.ShaderLOD3S)
        {
            T4MCache.LOD3B = null;
            T4MCache.LOD3T = null;
            T4MCache.LOD3Material = null;
            T4MCache.LOD3S = null;
            OldShaderLOD3S = T4MCache.ShaderLOD3S;
        }
    }


    void CreatePrefab()
    {
        if (!System.IO.Directory.Exists(T4MConfig.T4MPrefabFolder + "LODObjects/"))
        {
            System.IO.Directory.CreateDirectory(T4MConfig.T4MPrefabFolder + "LODObjects/");
        }
        if (!System.IO.Directory.Exists(T4MConfig.T4MPrefabFolder + "LODObjects/Material/"))
        {
            System.IO.Directory.CreateDirectory(T4MConfig.T4MPrefabFolder + "LODObjects/Material/");
        }
        AssetDatabase.Refresh();

        GameObject LOD1Temp;
        LOD1Temp = new GameObject(T4MCache.PrefabName + "LOD1");
        LOD1Temp.AddComponent<MeshFilter>();
        LOD1Temp.AddComponent<MeshRenderer>();
        LOD1Temp.AddComponent<T4MLod>();
        LOD1Temp.GetComponent<MeshFilter>().mesh = LOD1;
        LOD1Temp.GetComponent<Renderer>().sharedMaterial = T4MCache.LOD1Material;
        LOD1Temp.GetComponent<Renderer>().sharedMaterial.SetTexture("_MainTex", T4MCache.LOD1T);
        LOD1Temp.GetComponent<T4MLod>().LOD1 = LOD1Temp.GetComponent<Renderer>();
        if (CheckLOD1Collider)
        {
            LOD1Temp.AddComponent<MeshCollider>();
            LOD1Temp.GetComponent<MeshCollider>().sharedMesh = LOD1;
        }

        GameObject LOD2Temp;
        LOD2Temp = new GameObject(T4MCache.PrefabName + "LOD2");
        LOD2Temp.AddComponent<MeshFilter>();
        LOD2Temp.AddComponent<MeshRenderer>();
        LOD2Temp.GetComponent<MeshFilter>().mesh = LOD2;
        LOD2Temp.GetComponent<Renderer>().sharedMaterial = T4MCache.LOD2Material;
        LOD2Temp.GetComponent<Renderer>().sharedMaterial.SetTexture("_MainTex", T4MCache.LOD2T);
        LOD1Temp.GetComponent<T4MLod>().LOD2 = LOD2Temp.GetComponent<Renderer>();
        LOD2Temp.GetComponent<MeshRenderer>().enabled = false;
        LOD2Temp.transform.parent = LOD1Temp.transform;
        if (CheckLOD2Collider)
        {
            LOD2Temp.AddComponent<MeshCollider>();
            LOD2Temp.GetComponent<MeshCollider>().sharedMesh = LOD2;
        }

        GameObject LOD3Temp;
        LOD3Temp = new GameObject(T4MCache.PrefabName + "LOD3");
        LOD3Temp.AddComponent<MeshFilter>();
        LOD3Temp.AddComponent<MeshRenderer>();
        LOD3Temp.GetComponent<MeshFilter>().mesh = LOD3;
        LOD3Temp.GetComponent<Renderer>().sharedMaterial = T4MCache.LOD3Material;
        LOD3Temp.GetComponent<Renderer>().sharedMaterial.SetTexture("_MainTex", T4MCache.LOD3T);
        LOD1Temp.GetComponent<T4MLod>().LOD3 = LOD3Temp.GetComponent<Renderer>();
        LOD3Temp.GetComponent<MeshRenderer>().enabled = false;
        LOD3Temp.transform.parent = LOD1Temp.transform;
        if (CheckLOD3Collider)
        {
            LOD3Temp.AddComponent<MeshCollider>();
            LOD3Temp.GetComponent<MeshCollider>().sharedMesh = LOD3;
        }

        bool ExportSuccess = false;
        int num = 1;
        UnityEngine.Object BasePrefab;
        string Next;
        do
        {
            Next = T4MCache.PrefabName + num;

            if (!System.IO.File.Exists(T4MConfig.T4MPrefabFolder + "LODObjects/" + T4MCache.PrefabName + "_LOD.prefab"))
            {

                if (T4MCache.ShaderLOD1S == LODShaderStatus.New)
                    AssetDatabase.CreateAsset(T4MCache.LOD1Material, T4MConfig.T4MPrefabFolder + "LODObjects/Material/" + T4MCache.PrefabName + "LOD1.mat");
                if (T4MCache.ShaderLOD2S == LODShaderStatus.New)
                    AssetDatabase.CreateAsset(T4MCache.LOD2Material, T4MConfig.T4MPrefabFolder + "LODObjects/Material/" + T4MCache.PrefabName + "LOD2.mat");
                if (T4MCache.ShaderLOD3S == LODShaderStatus.New)
                    AssetDatabase.CreateAsset(T4MCache.LOD3Material, T4MConfig.T4MPrefabFolder + "LODObjects/Material/" + T4MCache.PrefabName + "LOD3.mat");
                BasePrefab = PrefabUtility.CreateEmptyPrefab(T4MConfig.T4MPrefabFolder + "LODObjects/" + T4MCache.PrefabName + ".prefab");
                PrefabUtility.ReplacePrefab(LOD1Temp, BasePrefab);
                ExportSuccess = true;
            }
            else if (!System.IO.File.Exists(T4MConfig.T4MPrefabFolder + "LODObjects/" + Next + "_LOD.prefab"))
            {

                if (T4MCache.ShaderLOD1S == LODShaderStatus.New)
                    AssetDatabase.CreateAsset(T4MCache.LOD1Material, T4MConfig.T4MPrefabFolder + "LODObjects/Material/" + Next + "LOD1.mat");
                if (T4MCache.ShaderLOD2S == LODShaderStatus.New)
                    AssetDatabase.CreateAsset(T4MCache.LOD2Material, T4MConfig.T4MPrefabFolder + "LODObjects/Material/" + Next + "LOD2.mat");
                if (T4MCache.ShaderLOD3S == LODShaderStatus.New)
                    AssetDatabase.CreateAsset(T4MCache.LOD3Material, T4MConfig.T4MPrefabFolder + "LODObjects/Material/" + Next + "LOD3.mat");
                BasePrefab = PrefabUtility.CreateEmptyPrefab(T4MConfig.T4MPrefabFolder + "LODObjects/" + Next + ".prefab");
                PrefabUtility.ReplacePrefab(LOD1Temp, BasePrefab);
                ExportSuccess = true;
            }
            num++;
        } while (!ExportSuccess);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        GameObject.DestroyImmediate(LOD1Temp);
        EditorUtility.DisplayDialog("T4M Message", "Construction Completed", "OK");
    }

    void LODManager()
    {
        
        if (T4MCache.T4MMaster)
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            GUILayout.Label("LOD Mode", EditorStyles.boldLabel);
            T4MCache.LODModeControler = (LODMod)EditorGUILayout.EnumPopup("controller", T4MCache.LODModeControler, GUILayout.Width(340));
            EditorGUILayout.Space();
            GUILayout.Label("Culling LOD Object Mode", EditorStyles.boldLabel);
            T4MCache.LODocclusion = (OccludeBy)EditorGUILayout.EnumPopup("Mode", T4MCache.LODocclusion, GUILayout.Width(340));

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Maximum View Distance", EditorStyles.boldLabel, GUILayout.Width(300));
            T4MConfig.MaximunView = EditorGUILayout.FloatField(T4MConfig.MaximunView, GUILayout.Width(50));
            GUILayout.EndHorizontal();


            EditorGUILayout.Space();
            EditorGUILayout.Space();

            GUILayout.Label("LOD Update Interval in Seconde", EditorStyles.boldLabel, GUILayout.Width(400));
            GUILayout.BeginHorizontal();
            GUILayout.Label("(less value = less performance)", GUILayout.Width(300));
            T4MConfig.UpdateInterval = EditorGUILayout.FloatField(T4MConfig.UpdateInterval, GUILayout.Width(50));
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            GUILayout.Label("LOD2 Start", EditorStyles.boldLabel, GUILayout.Width(170));
            GUILayout.FlexibleSpace();
            T4MConfig.StartLOD2 = EditorGUILayout.FloatField(T4MConfig.StartLOD2, GUILayout.Width(50));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            T4MConfig.StartLOD2 = GUILayout.HorizontalScrollbar(T4MConfig.StartLOD2, 0.0f, 10f, T4MConfig.MaximunView, GUILayout.Width(350));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (T4MConfig.StartLOD2 > T4MConfig.StartLOD3 - 5)
                T4MConfig.StartLOD3 = T4MConfig.StartLOD2 + 5;
            if (T4MConfig.StartLOD2 > T4MConfig.MaximunView - 10)
                T4MConfig.StartLOD2 = T4MConfig.MaximunView - 10;
            if (T4MConfig.StartLOD3 > T4MConfig.MaximunView - 5)
                T4MConfig.StartLOD3 = T4MConfig.MaximunView - 5;

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            GUILayout.Label("LOD3 Start", EditorStyles.boldLabel, GUILayout.Width(170));
            GUILayout.FlexibleSpace();
            T4MConfig.StartLOD3 = EditorGUILayout.FloatField(T4MConfig.StartLOD3, GUILayout.Width(50));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            T4MConfig.StartLOD3 = GUILayout.HorizontalScrollbar(T4MConfig.StartLOD3, 0.0f, 10f, T4MConfig.MaximunView, GUILayout.Width(350));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            string buttonLod;
            Texture Swap;
            if (T4MCache.LodActivate == true)
            {
                buttonLod = "DEACTIVATE";
                Swap = (Texture)AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/on.png", typeof(Texture));
            }
            else {
                buttonLod = "ACTIVATE";
                Swap = (Texture)AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/off.png", typeof(Texture));
            }

            if (GUILayout.Button(buttonLod, GUILayout.Width(100), GUILayout.Height(30)))
            {
                ActivateDeactivateLOD();
            }
            GUILayout.Label(Swap, GUILayout.Width(75), GUILayout.Height(30));

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
        }
        else {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Need to be an Master T4M", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }


    void ActivateDeactivateLOD()
    {
        T4MMainObj mainObjT4MMain = CurrentSelect.GetComponent<T4MMainObj>();
        if (T4MCache.LodActivate)
        { //Lod actif
            T4MLod[] T4MLodObjGet = GameObject.FindObjectsOfType(typeof(T4MLod)) as T4MLod[];

            for (var i = 0; i < T4MLodObjGet.Length; i++)
            {
                T4MLodObjGet[i].LOD2.enabled = T4MLodObjGet[i].LOD3.enabled = false;
                T4MLodObjGet[i].LOD1.enabled = true;
                if (T4MCache.LODModeControler == LODMod.Mass_Control)
                    T4MLodObjGet[i].Mode = 0;
                else if (T4MCache.LODModeControler == LODMod.Independent_Control)
                    T4MLodObjGet[i].Mode = 0;
                PrefabUtility.RecordPrefabInstancePropertyModifications(T4MLodObjGet[i]);
            }

            mainObjT4MMain.ObjLodScript = new T4MLod[0];
            mainObjT4MMain.ObjPosition = new Vector3[0];
            mainObjT4MMain.ObjLodStatus = new int[0];
            mainObjT4MMain.Mode = 0;
            PrefabUtility.RecordPrefabInstancePropertyModifications(mainObjT4MMain);
            T4MCache.LodActivate = false;
            Debug.LogWarning("LOD deactivated !");
        }
        else {
            if (!T4MCache.PlayerCam)
                T4MCache.PlayerCam = Camera.main.transform;

            T4MLod[] T4MLodObjGetR = GameObject.FindObjectsOfType(typeof(T4MLod)) as T4MLod[];
            Vector3[] T4MLodVectGetR = new Vector3[T4MLodObjGetR.Length];
            int[] T4MLodValueGetR = new int[T4MLodObjGetR.Length];


            for (int i = 0; i < T4MLodObjGetR.Length; i++)
            {
                T4MLodVectGetR[i] = T4MLodObjGetR[i].transform.position;


                float distanceFromCameraR = Vector3.Distance(new Vector3(T4MLodObjGetR[i].transform.position.x, 
                                                                    T4MCache.PlayerCam.position.y, T4MLodObjGetR[i].transform.position.z),
                                                                    T4MCache.PlayerCam.transform.position);


                if (distanceFromCameraR <= T4MConfig.MaximunView)
                {
                    if (distanceFromCameraR < T4MConfig.StartLOD2)
                    {
                        T4MLodObjGetR[i].LOD2.enabled = T4MLodObjGetR[i].LOD3.enabled = false;
                        T4MLodObjGetR[i].LOD1.enabled = true;
                        T4MLodObjGetR[i].ObjLodStatus = T4MLodValueGetR[i] = 1;
                    }
                    else if (distanceFromCameraR >= T4MConfig.StartLOD2 && distanceFromCameraR < T4MConfig.StartLOD3)
                    {
                        T4MLodObjGetR[i].LOD1.enabled = T4MLodObjGetR[i].LOD3.enabled = false;
                        T4MLodObjGetR[i].LOD2.enabled = true;
                        T4MLodObjGetR[i].ObjLodStatus = T4MLodValueGetR[i] = 2;
                    }
                    else if (distanceFromCameraR >= T4MConfig.StartLOD3)
                    {
                        T4MLodObjGetR[i].LOD1.enabled = T4MLodObjGetR[i].LOD2.enabled = false;
                        T4MLodObjGetR[i].LOD3.enabled = true;
                        T4MLodObjGetR[i].ObjLodStatus = T4MLodValueGetR[i] = 3;
                    }
                }
                else
                {
                    if (T4MCache.LODocclusion == OccludeBy.Max_View_Distance)
                    {
                        T4MLodObjGetR[i].LOD3.enabled = T4MLodObjGetR[i].LOD1.enabled = T4MLodObjGetR[i].LOD2.enabled = false;
                        T4MLodObjGetR[i].ObjLodStatus = T4MLodValueGetR[i] = 0;
                    }
                    else {
                        T4MLodObjGetR[i].LOD1.enabled = T4MLodObjGetR[i].LOD2.enabled = false;
                        T4MLodObjGetR[i].LOD3.enabled = true;
                        T4MLodObjGetR[i].ObjLodStatus = T4MLodValueGetR[i] = 3;
                    }
                }
                //To each LOD
                T4MLodObjGetR[i].Interval = T4MConfig.UpdateInterval;
                T4MLodObjGetR[i].MaxViewDistance = T4MConfig.MaximunView;
                T4MLodObjGetR[i].PlayerCamera = T4MCache.PlayerCam;
                T4MLodObjGetR[i].LOD2Start = T4MConfig.StartLOD2;
                T4MLodObjGetR[i].LOD3Start = T4MConfig.StartLOD3;
                if (T4MCache.LODModeControler == LODMod.Mass_Control)
                    T4MLodObjGetR[i].Mode = 1;
                else if (T4MCache.LODModeControler == LODMod.Independent_Control)
                    T4MLodObjGetR[i].Mode = 2;
                PrefabUtility.RecordPrefabInstancePropertyModifications(T4MLodObjGetR[i]);
            }

            if (T4MCache.LODModeControler == LODMod.Mass_Control)
                mainObjT4MMain.Mode = 1;
            else if (T4MCache.LODModeControler == LODMod.Independent_Control)
                mainObjT4MMain.Mode = 2;


            if (T4MCache.LODocclusion == OccludeBy.Max_View_Distance)
                CurrentSelect.GetComponent<T4MMainObj>().LODbasedOnScript = true;
            else CurrentSelect.GetComponent<T4MMainObj>().LODbasedOnScript = false;

            CurrentSelect.GetComponent<T4MMainObj>().ObjLodScript = T4MLodObjGetR;
            CurrentSelect.GetComponent<T4MMainObj>().ObjPosition = T4MLodVectGetR;
            CurrentSelect.GetComponent<T4MMainObj>().ObjLodStatus = T4MLodValueGetR;
            CurrentSelect.GetComponent<T4MMainObj>().Interval = T4MConfig.UpdateInterval;
            CurrentSelect.GetComponent<T4MMainObj>().MaxViewDistance = T4MConfig.MaximunView;

            CurrentSelect.GetComponent<T4MMainObj>().LOD2Start = T4MConfig.StartLOD2;
            CurrentSelect.GetComponent<T4MMainObj>().LOD3Start = T4MConfig.StartLOD3;
            CurrentSelect.GetComponent<T4MMainObj>().PlayerCamera = T4MCache.PlayerCam;

            PrefabUtility.RecordPrefabInstancePropertyModifications(mainObjT4MMain);

            T4MCache.LodActivate = true;
            Debug.LogWarning("LOD (re)activated !");
        }
        mainObjT4MMain.Awake();
    }

   public void DeactivateLODByScript()
    {
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

        CurrentSelect.gameObject.GetComponent<T4MMainObj>().ObjLodScript = new T4MLod[0];
        CurrentSelect.gameObject.GetComponent<T4MMainObj>().ObjPosition = new Vector3[0];
        CurrentSelect.gameObject.GetComponent<T4MMainObj>().ObjLodStatus = new int[0];
        CurrentSelect.gameObject.GetComponent<T4MMainObj>().Mode = 0;
        PrefabUtility.RecordPrefabInstancePropertyModifications(CurrentSelect.gameObject.GetComponent<T4MMainObj>());
        Debug.Log("The Number of Activated LOD Objects has changed, reactivate the billboards in the 'LOD' Tab.");
        CurrentSelect.gameObject.GetComponent<T4MMainObj>().Awake();
    }
}
