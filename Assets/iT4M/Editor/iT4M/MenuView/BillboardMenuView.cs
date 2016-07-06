/********************************************************************************
** Author： LiangZG
** Email :  game.liangzg@foxmail.com
*********************************************************************************/
using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 描述：公告板视图
/// <para>创建时间：2016-07-05</para>
/// </summary>
public class BillboardMenuView  {

    private Transform CurrentSelect;

    string[] BillMenu = { "Billboard Manager", "Billboard Creator" };

    int BillM;
    CreaType CreationBB = CreaType.Classic_T4M;
    Mesh BillMesh;

    public void OnGUI()
    {
        CurrentSelect = T4MMainEditor.CurrentSelect;
        T4MMainObj mainObjT4MMain = CurrentSelect.GetComponent<T4MMainObj>();
        if (mainObjT4MMain)
        {
            if (mainObjT4MMain.T4MMaterial && mainObjT4MMain.T4MMaterial.HasProperty("_Splat0") && mainObjT4MMain.T4MMaterial.HasProperty("_Splat1") && 
                mainObjT4MMain.T4MMaterial.HasProperty("_Control"))
            {

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                BillM = GUILayout.Toolbar(BillM, BillMenu, GUILayout.Width(290), GUILayout.Height(20));
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                switch (BillM)
                {
                    case 0:
                        if (T4MCache.T4MMaster)
                        {
                            Billboard();
                        }
                        else {
                            GUILayout.BeginHorizontal();
                            GUILayout.FlexibleSpace();
                            GUILayout.Label("Need to be an Master T4M", EditorStyles.boldLabel);
                            GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();
                        }
                        break;
                    case 1:
                        BillboardCreator();
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

    void BillboardCreator()
    {
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal("box");
        GUILayout.Label("BillBoard Prefab Name", EditorStyles.boldLabel);
        T4MCache.PrefabName = GUILayout.TextField(T4MCache.PrefabName, 25, GUILayout.Width(155));
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        CreationBB = (CreaType)EditorGUILayout.EnumPopup("New Billboard Type", CreationBB, GUILayout.Width(340));

        if (CreationBB == CreaType.Custom)
        {
            GUILayout.Label("Billboard Meshes", EditorStyles.boldLabel);
            BillMesh = (Mesh)EditorGUILayout.ObjectField("Mesh", BillMesh, typeof(Mesh), false);
        }
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();

        GUILayout.Label("BillBoard Setup", EditorStyles.boldLabel, GUILayout.Width(223));
        GUILayout.Label("MainTex", EditorStyles.boldLabel, GUILayout.Width(68));
        GUILayout.Label("Bump", EditorStyles.boldLabel, GUILayout.Width(60));
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.Label("BillBoard Shader", GUILayout.Width(103));
        T4MCache.ShaderLOD1S = (LODShaderStatus)EditorGUILayout.EnumPopup(T4MCache.ShaderLOD1S, GUILayout.Width(95));
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();
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
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("CONSTRUCT", GUILayout.Width(100), GUILayout.Height(30)))
        {
            if (T4MCache.PrefabName != "" && T4MCache.LOD1Material)
                CreatePrefabBB();
            else EditorUtility.DisplayDialog("T4M Message", "You must complete the formulary before make the construct", "OK");
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();

    }
    void CreatePrefabBB()
    {
        if (!System.IO.Directory.Exists(T4MConfig.T4MPrefabFolder + "BillBObjects/"))
        {
            System.IO.Directory.CreateDirectory(T4MConfig.T4MPrefabFolder + "BillBObjects/");
        }
        if (!System.IO.Directory.Exists(T4MConfig.T4MPrefabFolder + "BillBObjects/Material/"))
        {
            System.IO.Directory.CreateDirectory(T4MConfig.T4MPrefabFolder + "BillBObjects/Material/");
        }
        AssetDatabase.Refresh();
        GameObject LOD1Temp;
        LOD1Temp = new GameObject(T4MCache.PrefabName);
        LOD1Temp.AddComponent<MeshFilter>();
        LOD1Temp.AddComponent<MeshRenderer>();

        if (CreationBB == CreaType.Custom)
        {
            LOD1Temp.GetComponent<MeshFilter>().mesh = BillMesh;
        }
        else {
            GameObject Temp = (GameObject)AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "MeshBillb/Billboard.fbx", typeof(GameObject));
            Mesh Test2 = Temp.GetComponent<MeshFilter>().sharedMesh;
            LOD1Temp.GetComponent<MeshFilter>().mesh = Test2;
        }
        LOD1Temp.AddComponent<T4MBillboard>();
        LOD1Temp.GetComponent<Renderer>().sharedMaterial = T4MCache.LOD1Material;
        LOD1Temp.GetComponent<Renderer>().sharedMaterial.SetTexture("_MainTex", T4MCache.LOD1T);
        LOD1Temp.GetComponent<T4MBillboard>().Render = LOD1Temp.GetComponent<Renderer>();
        LOD1Temp.GetComponent<T4MBillboard>().Transf = LOD1Temp.transform;

        bool ExportSuccess = false;
        int num = 1;
        UnityEngine.Object BasePrefab;
        string Next;
        do
        {
            Next = T4MCache.PrefabName + num;

            if (!System.IO.File.Exists(T4MConfig.T4MPrefabFolder + "BillBObjects/" + T4MCache.PrefabName + ".prefab"))
            {
                if (T4MCache.ShaderLOD1S == LODShaderStatus.New)
                    AssetDatabase.CreateAsset(T4MCache.LOD1Material, T4MConfig.T4MPrefabFolder + "BillBObjects/Material/" + T4MCache.PrefabName + ".mat");

                BasePrefab = PrefabUtility.CreateEmptyPrefab(T4MConfig.T4MPrefabFolder + "BillBObjects/" + T4MCache.PrefabName + ".prefab");
                PrefabUtility.ReplacePrefab(LOD1Temp, BasePrefab);
                ExportSuccess = true;
            }
            else if (!System.IO.File.Exists(T4MConfig.T4MPrefabFolder + "BillBObjects/" + Next + ".prefab"))
            {
                if (T4MCache.ShaderLOD1S == LODShaderStatus.New)
                    AssetDatabase.CreateAsset(T4MCache.LOD1Material, T4MConfig.T4MPrefabFolder + "BillBObjects/Material/" + Next + ".mat");
                BasePrefab = PrefabUtility.CreateEmptyPrefab(T4MConfig.T4MPrefabFolder + "BillBObjects/" + Next + ".prefab");
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

    void Billboard()
    {

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        GUILayout.Label("Culling BillBoard Mode", EditorStyles.boldLabel);
        T4MCache.BilBocclusion = (OccludeBy)EditorGUILayout.EnumPopup("Mode", T4MCache.BilBocclusion, GUILayout.Width(340));

        EditorGUILayout.Space();
        EditorGUILayout.Space();


        GUILayout.Label("BillBoard Rotation Axis", EditorStyles.boldLabel);
        T4MCache.BillBoardAxis = (BillbAxe)EditorGUILayout.EnumPopup("Axis", T4MCache.BillBoardAxis, GUILayout.Width(340));
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        GUILayout.Label("BillBoard Update Interval in Seconde", EditorStyles.boldLabel, GUILayout.Width(400));
        GUILayout.BeginHorizontal();
        GUILayout.Label("(less value = less performance)", GUILayout.Width(300));
        T4MConfig.BillInterval = EditorGUILayout.FloatField(T4MConfig.BillInterval, GUILayout.Width(50));
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Maximum View Distance", EditorStyles.boldLabel, GUILayout.Width(298));
        T4MConfig.BillboardDist = EditorGUILayout.FloatField(T4MConfig.BillboardDist, GUILayout.Width(50));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        T4MConfig.BillboardDist = GUILayout.HorizontalScrollbar(T4MConfig.BillboardDist, 0.0f, 0f, 200f, GUILayout.Width(350));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        Texture Swap;
        string buttonBill;
        if (T4MCache.billActivate)
        {
            buttonBill = "DEACTIVATE";
            Swap = (Texture)AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/on.png", typeof(Texture));
        }
        else {
            buttonBill = "ACTIVATE";
            Swap = (Texture)AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Img/off.png", typeof(Texture));
        }


        if (GUILayout.Button(buttonBill, GUILayout.Width(100), GUILayout.Height(30)))
        {
            ActivateDeactivateBillBoard();
        }
        GUILayout.Label(Swap, GUILayout.Width(75), GUILayout.Height(30));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();

    }

   public void DeactivateBillBByScript()
    {
        T4MBillboard[] T4MBillObjGet = GameObject.FindObjectsOfType(typeof(T4MBillboard)) as T4MBillboard[];

        for (var i = 0; i < T4MBillObjGet.Length; i++)
        {
            T4MBillObjGet[i].Render.enabled = true;
            T4MBillObjGet[i].Transf.LookAt(Vector3.zero, Vector3.up);
        }

        T4MMainObj mainObjT4MMain = CurrentSelect.GetComponent<T4MMainObj>();
        mainObjT4MMain.BillboardPosition = new Vector3[0];
        mainObjT4MMain.BillStatus = new int[0];
        mainObjT4MMain.BillScript = new T4MBillboard[0];
        PrefabUtility.RecordPrefabInstancePropertyModifications(mainObjT4MMain);
        Debug.LogWarning("The Number of Activated Billboard Objects has changed, reactivate the billboards in the 'Billboard' Tab.");
    }

    void ActivateDeactivateBillBoard()
    {
        T4MMainObj mainObjT4MMain = CurrentSelect.GetComponent<T4MMainObj>();
        if (T4MCache.billActivate)
        { //si le billboard est actif
            T4MBillboard[] T4MBillObjGet = GameObject.FindObjectsOfType(typeof(T4MBillboard)) as T4MBillboard[];

            for (var i = 0; i < T4MBillObjGet.Length; i++)
            {
                T4MBillObjGet[i].Render.enabled = true;
                T4MBillObjGet[i].Transf.LookAt(Vector3.zero, Vector3.up);
            }


            mainObjT4MMain.BillboardPosition = new Vector3[0];
            mainObjT4MMain.BillStatus = new int[0];
            mainObjT4MMain.BillScript = new T4MBillboard[0];
            PrefabUtility.RecordPrefabInstancePropertyModifications(mainObjT4MMain);
            T4MCache.billActivate = false;
            Debug.LogWarning("Billboard deactivated !");
        }
        else {
            if (!T4MCache.PlayerCam)
                T4MCache.PlayerCam = Camera.main.transform;

            T4MBillboard[] T4MBillObjGet = GameObject.FindObjectsOfType(typeof(T4MBillboard)) as T4MBillboard[];
            Vector3[] T4MBillVectGetR = new Vector3[T4MBillObjGet.Length];
            int[] T4MBillValueGetR = new int[T4MBillObjGet.Length];

            for (var j = 0; j < T4MBillObjGet.Length; j++)
            {
                T4MBillVectGetR[j] = T4MBillObjGet[j].transform.position;
                if (Vector3.Distance(T4MBillObjGet[j].transform.position, T4MCache.PlayerCam.transform.position) <= T4MConfig.BillboardDist)
                {
                    T4MBillObjGet[j].Render.enabled = true;
                    T4MBillValueGetR[j] = 1;

                }
                else {
                    if (T4MCache.BilBocclusion == OccludeBy.Max_View_Distance)
                    {
                        T4MBillObjGet[j].Render.enabled = false;
                        T4MBillValueGetR[j] = 0;
                    }
                    else {
                        T4MBillObjGet[j].Render.enabled = true;
                        T4MBillValueGetR[j] = 1;
                    }
                }
                if (T4MCache.BillBoardAxis == BillbAxe.Y_Axis)
                    T4MBillObjGet[j].Transf.LookAt(new Vector3(T4MCache.PlayerCam.transform.position.x, T4MBillObjGet[j].Transf.position.y, T4MCache.PlayerCam.transform.position.z), Vector3.up);
                else
                    T4MBillObjGet[j].Transf.LookAt(T4MCache.PlayerCam.transform.position, Vector3.up);

            }
            if (T4MCache.BilBocclusion == OccludeBy.Max_View_Distance)
                mainObjT4MMain.BilBbasedOnScript = true;
            else mainObjT4MMain.BilBbasedOnScript = false;

            mainObjT4MMain.BillboardPosition = T4MBillVectGetR;
            mainObjT4MMain.BillStatus = T4MBillValueGetR;
            mainObjT4MMain.BillScript = T4MBillObjGet;
            mainObjT4MMain.BillMaxViewDistance = T4MConfig.BillboardDist;
            mainObjT4MMain.BillInterval = T4MConfig.BillInterval;
            if (T4MCache.BillBoardAxis == BillbAxe.Y_Axis)
                mainObjT4MMain.Axis = 0;
            else mainObjT4MMain.Axis = 1;
            PrefabUtility.RecordPrefabInstancePropertyModifications(mainObjT4MMain);

            T4MCache.billActivate = true;
            Debug.LogWarning("Billboard (re)activated !");
        }
    }

}
