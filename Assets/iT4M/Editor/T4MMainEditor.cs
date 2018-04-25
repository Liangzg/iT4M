//Update SC
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.IO;
using System.Text;
/// <summary>
/// T4M 编辑器主入口
/// </summary>
public class T4MMainEditor : EditorWindow {
	
	static public Transform CurrentSelect ;
    public static T4MMainEditor Instance;

    GUIContent[] MenuIcon = new GUIContent[7];
	static public string T4MActived = "Activated";
	T4MMainObj[] _t4MMainObjCounter;
	static int T4MSelectID;
	static public Projector T4MProjectorPlt;
	bool initMaster;
	
	static bool oldActivBillb;
	static bool oldActivLOD;
    
    private ConverterMenuView converterMenuView = new ConverterMenuView();
    private OptimizeView      optimizeView = new OptimizeView();
    private MyT4MView         myT4MView = new MyT4MView();
    private PlantingView      plantingView = new PlantingView();
    private PainterMenuView   painterView  = new PainterMenuView();
    private AFLODView         lodView = new AFLODView();
    private BillboardMenuView bbView = new BillboardMenuView();

    void OnDestroy() 
	{
		T4MCache.T4MMenuToolbar = 0;
        T4MCache.vertexInfo = 0;
        T4MCache.partofT4MObj =0;
        T4MCache.trisInfo =0;
		T4MCache.TexTexture = null;
		T4MSelectID = 0;
		Projector[] projectorObj = FindObjectsOfType(typeof(Projector)) as Projector[];
		foreach(Projector go in projectorObj)
			{
				if(go.hideFlags == HideFlags.HideInHierarchy || go.name == "previewT4M")
				{
					go.hideFlags=0;
					DestroyImmediate (go.gameObject);
				}
			}
    }
	
	[MenuItem ("Window/iT4M %t")]
	static void Initialize () 
	{
		T4MMainEditor window = (T4MMainEditor) EditorWindow.GetWindowWithRect(typeof (T4MMainEditor),new Rect(0,0,386,582),false,"iT4M");
	    Instance = window;
        //window.initlize();
        window.Show();
	}

    private bool isInitEditor = false;
    public void initlizeEditor()
    {
        if (isInitEditor) return;
        isInitEditor = true;
        MenuIcon[0] = new GUIContent(AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Icons/conv.png", typeof(Texture2D)) as Texture);
        MenuIcon[1] = new GUIContent(AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Icons/optimize.png", typeof(Texture2D)) as Texture);
        MenuIcon[2] = new GUIContent(AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Icons/myt4m.png", typeof(Texture2D)) as Texture);
        MenuIcon[3] = new GUIContent(AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Icons/paint.png", typeof(Texture2D)) as Texture);
        MenuIcon[4] = new GUIContent(AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Icons/plant.png", typeof(Texture2D)) as Texture);
        MenuIcon[5] = new GUIContent(AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Icons/lod.png", typeof(Texture2D)) as Texture);
        MenuIcon[6] = new GUIContent(AssetDatabase.LoadAssetAtPath(T4MConfig.T4MEditorFolder + "Icons/bill.png", typeof(Texture2D)) as Texture);

        T4MUtil.AddLayer("CloseView" , 27);
        T4MUtil.AddLayer("NormalView", 28);
        T4MUtil.AddLayer("FarView", 29);
        T4MUtil.AddLayer("Background", 30);
        T4MUtil.AddLayer("T4MObj", 31);
    }

    void OnInspectorUpdate() 
	{
        Repaint();
    }

    void OnGUI()
    {
        this.initlizeEditor();

        CurrentSelect = Selection.activeTransform;
        T4MCache.nbrT4MObj = 0;
        _t4MMainObjCounter = GameObject.FindObjectsOfType(typeof (T4MMainObj)) as T4MMainObj[];
        for (int i = 0; i < _t4MMainObjCounter.Length; i++)
        {
            if (_t4MMainObjCounter[i].Master == 1)
                T4MCache.nbrT4MObj = +1;
        }

        if (CurrentSelect && Selection.activeInstanceID != T4MSelectID || T4MCache.UnityTerrain &&
            T4MCache.T4MMenuToolbar != 0 || T4MCache.T4MMenuToolbar != 3 && T4MConfig.T4MPreview)
        {
            IniNewSelect();
        }

        GUILayout.BeginHorizontal();
        {
            GUILayout.BeginArea(new Rect(0, 0, 90, 585));
            GUILayout.Label(AssetDatabase.LoadAssetAtPath<Texture2D>(T4MConfig.T4MEditorFolder + "Img/T4MBAN.jpg") , 
                            GUILayout.Width(24), GUILayout.Height(582));
            GUILayout.EndArea();
        }

        { 
        GUILayout.BeginArea(new Rect(25, 0, 363, 585));
        EditorGUILayout.Space();
        {
            GUILayout.BeginHorizontal("box");
            T4MCache.T4MMenuToolbar =   GUILayout.Toolbar(T4MCache.T4MMenuToolbar, MenuIcon, "gridlist", GUILayout.Width(172),GUILayout.Height(18));
            GUILayout.FlexibleSpace();

            GUILayout.Label("Controls", GUILayout.Width(52));
            if (GUILayout.Button(T4MActived, GUILayout.Width(80)))
            {
                if (T4MActived == "Activated")
                {
                    T4MActived = "Deactivated";
                }
                else
                {
                    T4MActived = "Activated";
                }
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.Label(AssetDatabase.LoadAssetAtPath<Texture>(T4MConfig.T4MEditorFolder + "Img/separator.png"));

        #region ------------激活T4M------------------

        if (CurrentSelect != null && T4MActived == "Activated")
        {
//            if (CurrentSelect.gameObject.GetComponent<T4MMainObj>())
//            {
//                Selection.activeTransform = CurrentSelect.parent;
//            }
            
            T4MMainObj mainObjTm  = CurrentSelect.gameObject.GetComponent<T4MMainObj>();
            Renderer[] rendererPart = CurrentSelect.GetComponentsInChildren<Renderer>();
            if (mainObjTm && (!mainObjTm.T4MMaterial || !mainObjTm.T4MMesh))
            {

                if (rendererPart.Length == 0)
                {
                    mainObjTm.T4MMaterial = CurrentSelect.GetComponent<Renderer>().sharedMaterial;
                    mainObjTm.T4MMesh = CurrentSelect.gameObject.GetComponent<MeshFilter>();

                }
                else
                {
                    for (int i = 0; i < rendererPart.Length; i++)
                    {
                        mainObjTm.T4MMaterial = rendererPart[0].sharedMaterial;
                        mainObjTm.T4MMesh = rendererPart[0].gameObject.GetComponent<MeshFilter>();
                    }
                }
            }
            else if (mainObjTm && mainObjTm.T4MMaterial)
            {
                if (T4MCache.nbrT4MObj == 1 && mainObjTm.Master != 1)
                    T4MCache.T4MMaster = false;
                else if (T4MCache.nbrT4MObj == 1 && mainObjTm.Master == 1 && T4MCache.T4MMaster == false &&
                         initMaster == false)
                {
                    T4MCache.T4MMaster = true;
                    initMaster = true;
                }
                if (rendererPart.Length == 0)
                {
                    if (mainObjTm.T4MMaterial != CurrentSelect.GetComponent<Renderer>().sharedMaterial)
                        mainObjTm.T4MMaterial = CurrentSelect.GetComponent<Renderer>().sharedMaterial;
                    EditorUtility.SetSelectedRenderState(CurrentSelect.GetComponent<Renderer>(), EditorSelectedRenderState.Hidden);
                }
                else
                {
                    if (mainObjTm.T4MMaterial != rendererPart[0].sharedMaterial)
                    {
                        mainObjTm.T4MMaterial = rendererPart[0].sharedMaterial;
                    }
                    for (int i = 0; i < rendererPart.Length; i++)
                    {
                        if (rendererPart[i].sharedMaterial != rendererPart[0].sharedMaterial)
                        {
                            rendererPart[i].sharedMaterial = rendererPart[0].sharedMaterial;
                        }
                            EditorUtility.SetSelectedRenderState(CurrentSelect.GetComponent<Renderer>(), EditorSelectedRenderState.Hidden);
                        }
                }
            }
            if (CurrentSelect && !mainObjTm)
            {
                int countchild = CurrentSelect.transform.childCount;
                if (countchild > 0)
                {
                    T4MCache.NbrPartObj = CurrentSelect.transform.GetComponentsInChildren<Renderer>();
                }
            }

            switch (T4MCache.T4MMenuToolbar)
            {
                case 0:
                    converterMenuView.OnGUI();
                    break;
                case 1:
                    optimizeView.OnGUI();
                    break;
                case 2:
                    myT4MView.OnGUI();
                    break;
                case 3:
                    painterView.OnGUI();
                    break;
                case 4:
                    plantingView.OnGUI();
                    break;
                case 5:
                    lodView.OnGUI();
                    break;
                case 6:
                    bbView.OnGUI();
                    break;
            }

            if (oldActivBillb != T4MCache.billActivate)
            {
                oldActivBillb = T4MCache.billActivate;
                if (T4MCache.billActivate == false)
                    bbView.DeactivateBillBByScript();
            }
            if (oldActivLOD != T4MCache.LodActivate)
            {
                oldActivLOD = T4MCache.LodActivate;
                if (T4MCache.LodActivate == false)
                    lodView.DeactivateLODByScript();
            }
        }
            #endregion
        else
        {
            if (CurrentSelect)
            {
                T4MMainObj mainObjTm = CurrentSelect.gameObject.GetComponent<T4MMainObj>();
                if (mainObjTm && mainObjTm.T4MMaterial)
                {
                    Renderer[] rendererPart = CurrentSelect.GetComponentsInChildren<Renderer>();
                    if (rendererPart.Length == 0)
                    {
                        EditorUtility.SetSelectedRenderState(CurrentSelect.GetComponent<Renderer>(), EditorSelectedRenderState.Highlight);
                    }
                    else
                    {
                        for (int i = 0; i < rendererPart.Length; i++)
                        {
                            EditorUtility.SetSelectedRenderState(rendererPart[i] , EditorSelectedRenderState.Highlight);
                        }
                    }
                }                
            }

            GUILayout.FlexibleSpace();
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label(AssetDatabase.LoadAssetAtPath<Texture>(T4MConfig.T4MEditorFolder + "Img/waiting.png"));
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndArea();
    }

    GUILayout.EndHorizontal();
		
		
	}
	
	
	
	void Repair(){
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		GUILayout.Label("This T4M Object is Broken", EditorStyles.boldLabel);
		EditorGUILayout.Space();
		GUILayout.Label("Clean It and Reconvert it ?");
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("CLEANING", GUILayout.Width(100), GUILayout.Height(30))) 
		{
			T4MMainObj ToSuppress = CurrentSelect.GetComponent <T4MMainObj>();
			DestroyImmediate (ToSuppress);
			T4MCache.T4MMenuToolbar = 0;
		}
		GUILayout.FlexibleSpace();
	}


	static public void SaveTexture()
	{
		var path = AssetDatabase.GetAssetPath (T4MCache.T4MMaskTex);
		var bytes   = T4MCache.T4MMaskTex.EncodeToPNG ();
		File.WriteAllBytes (path, bytes);
		if (T4MCache.T4MMaskTex2){
			var path2 = AssetDatabase.GetAssetPath (T4MCache.T4MMaskTex2);
			var bytes2   = T4MCache.T4MMaskTex2.EncodeToPNG ();
			File.WriteAllBytes (path2, bytes2);
		}
		//AssetDatabase.Refresh ();
	}

	
	public void IniNewSelect()
	{
		if (T4MCache.UnityTerrain){
			DestroyImmediate(T4MCache.UnityTerrain);
			if(T4MCache.Child){
				Selection.activeTransform = T4MCache.Child.transform;
				T4MCache.vertexInfo = 0;
                T4MCache.trisInfo = 0;
                T4MCache.partofT4MObj = 0;
				if (T4MCache.nbrT4MObj == 0){
                    T4MMainObj childMainObj = T4MCache.Child.gameObject.GetComponent<T4MMainObj>();
                    childMainObj.EnabledLODSystem = T4MConfig.ActivatedLOD;
                    childMainObj.enabledBillboard = T4MConfig.ActivatedBillboard;
                    childMainObj.enabledLayerCul = T4MConfig.ActivatedLayerCul;
                    childMainObj.CloseView = T4MConfig.CloseDistMaxView;
                    childMainObj.NormalView = T4MConfig.NormalDistMaxView;
                    childMainObj.FarView = T4MConfig.FarDistMaxView;
                    childMainObj.BackGroundView = T4MConfig.BGDistMaxView;
                    childMainObj.Master = 1;
				}
			}
		}

        T4MMainObj mainObjTm = CurrentSelect.gameObject.GetComponent<T4MMainObj>();
        if (CurrentSelect && mainObjTm && mainObjTm.T4MMaterial){
                EditorUtility.SetSelectedRenderState(CurrentSelect.GetComponent<Renderer>() , EditorSelectedRenderState.Hidden);

                initMaster = false;
			
				if (mainObjTm.T4MMaterial.HasProperty("_Splat0")){
					T4MConfig.Layer1 =mainObjTm.T4MMaterial.GetTexture("_Splat0");
					T4MCache.Layer1Tile = mainObjTm.T4MMaterial.GetTextureScale("_Splat0");
				}else T4MConfig.Layer1 =null;
				if (mainObjTm.T4MMaterial.HasProperty("_Splat1")){
					T4MConfig.Layer2 =mainObjTm.T4MMaterial.GetTexture("_Splat1");
					T4MCache.Layer2Tile = mainObjTm.T4MMaterial.GetTextureScale("_Splat1");
				}else T4MConfig.Layer2 =null;
				if (mainObjTm.T4MMaterial.HasProperty("_Splat2")){
					T4MConfig.Layer3 =mainObjTm.T4MMaterial.GetTexture("_Splat2");
					T4MCache.Layer3Tile = mainObjTm.T4MMaterial.GetTextureScale("_Splat2");
				}else T4MConfig.Layer3 =null;
				if (mainObjTm.T4MMaterial.HasProperty("_Splat3")){
					T4MConfig.Layer4 =mainObjTm.T4MMaterial.GetTexture("_Splat3");
                    T4MCache.Layer4Tile = mainObjTm.T4MMaterial.GetTextureScale("_Splat3");
				}else T4MConfig.Layer4 =null;
				if (mainObjTm.T4MMaterial.HasProperty("_Splat4")){
					T4MConfig.Layer5 =mainObjTm.T4MMaterial.GetTexture("_Splat4");
                    T4MCache.Layer5Tile = mainObjTm.T4MMaterial.GetTextureScale("_Splat4");
				}else T4MConfig.Layer5 =null;
				if (mainObjTm.T4MMaterial.HasProperty("_Splat5")){
					T4MConfig.Layer6 =mainObjTm.T4MMaterial.GetTexture("_Splat5");
                    T4MCache.Layer6Tile = mainObjTm.T4MMaterial.GetTextureScale("_Splat5");
				}else T4MConfig.Layer6 =null;
				
				if (mainObjTm.T4MMaterial.HasProperty("_BumpSplat0")){
					T4MConfig.Layer1Bump =mainObjTm.T4MMaterial.GetTexture("_BumpSplat0");
					T4MConfig.Layer2Bump =mainObjTm.T4MMaterial.GetTexture("_BumpSplat1");
					if (mainObjTm.T4MMaterial.HasProperty("_BumpSplat2"))
						T4MConfig.Layer3Bump =mainObjTm.T4MMaterial.GetTexture("_BumpSplat2");
					if (mainObjTm.T4MMaterial.HasProperty("_BumpSplat3"))
						T4MConfig.Layer4Bump =mainObjTm.T4MMaterial.GetTexture("_BumpSplat3");
					
				} 
					if(mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel1/T4M 2 Textures ManualAdd BeastLM_1DC") || mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel1/T4M 2 Textures ManualAdd CustoLM 1DC")){
						T4MConfig.LMMan = mainObjTm.T4MMaterial.GetTexture("_Lightmap");
					}
					CheckShader();
					T4MConfig.ActivatedLOD = mainObjTm.EnabledLODSystem ;
					T4MConfig.ActivatedBillboard = mainObjTm.enabledBillboard ;
					T4MConfig.MaximunView = mainObjTm.MaxViewDistance;
                    T4MConfig.StartLOD2 = mainObjTm.LOD2Start;
                    T4MConfig.StartLOD3 = mainObjTm.LOD3Start;
                    T4MConfig.UpdateInterval = mainObjTm.Interval;
					T4MCache.PlayerCam = mainObjTm.PlayerCamera;
                    T4MConfig.BillInterval = mainObjTm.BillInterval;
					T4MConfig.BillboardDist = mainObjTm.BillMaxViewDistance;
					T4MConfig.ActivatedLayerCul = mainObjTm.enabledLayerCul;
					T4MConfig.BGDistMaxView = mainObjTm.BackGroundView;
					T4MConfig.FarDistMaxView= mainObjTm.FarView;
					T4MConfig.NormalDistMaxView= mainObjTm.NormalView;
					T4MConfig.CloseDistMaxView= mainObjTm.CloseView;
					if (mainObjTm.Mode == 1)
						T4MCache.LODModeControler = LODMod.Mass_Control;
					else T4MCache.LODModeControler = LODMod.Independent_Control;
			
					if(mainObjTm.Axis == 0)
						T4MCache.BillBoardAxis = BillbAxe.Y_Axis;
					else T4MCache.BillBoardAxis = BillbAxe.All_Axis;
			
					if(mainObjTm.LODbasedOnScript == true)
						T4MCache.LODocclusion = OccludeBy.Max_View_Distance;
					else T4MCache.LODocclusion = OccludeBy.Layer_Cull_Distances;
			 
			
					if(mainObjTm.BilBbasedOnScript == true)
                         T4MCache.BilBocclusion = OccludeBy.Max_View_Distance;
					else T4MCache.BilBocclusion = OccludeBy.Layer_Cull_Distances;
					
					
					if (mainObjTm.BillboardPosition != null && mainObjTm.BillboardPosition.Length>0)
						T4MCache.billActivate = true;
					else T4MCache.billActivate = false;
					
					if (mainObjTm.ObjPosition != null && mainObjTm.ObjPosition.Length>0)
						T4MCache.LodActivate = true;
					else T4MCache.LodActivate = false;
			
			
					if (T4MCache.PlayerCam == null && Camera.main)
						T4MCache.PlayerCam = Camera.main.transform;
					else if (T4MCache.PlayerCam == null && !Camera.main){
						Camera[] Cam =  GameObject.FindObjectsOfType(typeof(Camera)) as Camera[];
						for (var b =0; b <Cam.Length;b++){
							if (Cam[b].GetComponent<AudioListener>()){
								T4MCache.PlayerCam = Cam[b].transform; 
							}
						}
					}
			
			
				if (mainObjTm.T4MMaterial.HasProperty("_SpecColor")){
					
					T4MCache.ShinessColor = mainObjTm.T4MMaterial.GetColor("_SpecColor");
					
					if (mainObjTm.T4MMaterial.HasProperty("_ShininessL0")){
                        T4MCache.shiness0 = mainObjTm.T4MMaterial.GetFloat ("_ShininessL0");
					}
					if (mainObjTm.T4MMaterial.HasProperty("_ShininessL1")){
                        T4MCache.shiness1 = mainObjTm.T4MMaterial.GetFloat ("_ShininessL1");
					}
					if (mainObjTm.T4MMaterial.HasProperty("_ShininessL2")){
                        T4MCache.shiness2 = mainObjTm.T4MMaterial.GetFloat ("_ShininessL2");
					}
					if (mainObjTm.T4MMaterial.HasProperty("_ShininessL3")){
                        T4MCache.shiness3 =mainObjTm.T4MMaterial.GetFloat ("_ShininessL3");
					}
				}
				if (mainObjTm.T4MMaterial.HasProperty("_Control2") && mainObjTm.T4MMaterial.GetTexture("_Control2"))
                    T4MCache.T4MMaskTex2 = (Texture2D)mainObjTm.T4MMaterial.GetTexture("_Control2");
				else T4MCache.T4MMaskTex2 = null;
				if (mainObjTm.T4MMaterial.HasProperty("_Control"))
				{
                    T4MConfig.T4MMaskTexUVCoord = mainObjTm.T4MMaterial.GetTextureScale("_Control").x;
                    T4MCache.T4MMaskTex = (Texture2D)mainObjTm.T4MMaterial.GetTexture("_Control");
					T4MCache.intialized=true;
					
				}
			
				
			}
			Projector[] projectorObj = FindObjectsOfType(typeof(Projector)) as Projector[];
			if(projectorObj.Length>0)
			for (var i = 0; i < projectorObj.Length; i++)
			{
				if (projectorObj[i].gameObject.name == "PreviewT4M")
						DestroyImmediate (projectorObj[i].gameObject);
			}
            T4MCache.terrainDat = null;
            T4MCache.vertexInfo = 0;
            T4MCache.trisInfo = 0;
            T4MCache.partofT4MObj = 0;
			T4MCache.TexTexture = null;
			
		T4MSelectID = Selection.activeInstanceID;
		
	}
	
	void CheckShader()
	{
        T4MMainObj mainObjTm = CurrentSelect.gameObject.GetComponent<T4MMainObj>();

        if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel1/T4M 2 Textures Auto BeastLM 2DrawCall")){
			T4MCache.MenuTextureSM1 = EnumShaderGLES1.T4M_2_Textures_Auto_BeastLM_2DrawCall ;
			T4MCache.ShaderModel = SM.ShaderModel1;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel1/T4M 2 Textures ManualAdd BeastLM_1DC")){
			T4MCache.MenuTextureSM1 = EnumShaderGLES1.T4M_2_Textures_ManualAdd_BeastLM_1DC ;
			T4MCache.ShaderModel = SM.ShaderModel1;
		}else	
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel1/T4M 2 Textures ManualAdd CustoLM 1DC")){
			T4MCache.MenuTextureSM1 = EnumShaderGLES1.T4M_2_Textures_ManualAdd_CustoLM_1DC ;
			T4MCache.ShaderModel = SM.ShaderModel1;
		}else	
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel2/Unlit/T4M 2 Textures Unlit LM")){
			T4MCache.MenuTextureSM2 = EnumShaderGLES2.T4M_2_Textures_Unlit_Lightmap_Compatible;
			T4MCache.ShaderModel = SM.ShaderModel2;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel2/Unlit/T4M 3 Textures Unlit LM")){
			T4MCache.MenuTextureSM2 = EnumShaderGLES2.T4M_3_Textures_Unlit_Lightmap_Compatible;
			T4MCache.ShaderModel = SM.ShaderModel2;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel2/Unlit/T4M 4 Textures Unlit LM")){
			T4MCache.MenuTextureSM2 = EnumShaderGLES2.T4M_4_Textures_Unlit_Lightmap_Compatible;
			T4MCache.ShaderModel = SM.ShaderModel2;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel2/Unlit/T4M 5 Textures Unlit LM")){
			T4MCache.MenuTextureSM2 = EnumShaderGLES2.T4M_5_Textures_Unlit_Lightmap_Compatible;
			T4MCache.ShaderModel = SM.ShaderModel2;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel2/Unlit/T4M 6 Textures Unlit LM")){
			T4MCache.MenuTextureSM2 = EnumShaderGLES2.T4M_6_Textures_Unlit_Lightmap_Compatible;
			T4MCache.ShaderModel = SM.ShaderModel2;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel2/Unlit/T4M 6 Textures Unlit NoL")){
			T4MCache.MenuTextureSM2 = EnumShaderGLES2.T4M_6_Textures_Unlit_No_Lightmap;
			T4MCache.ShaderModel = SM.ShaderModel2;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel2/Unlit/T4M World Projection Shader + LM")){
			T4MCache.MenuTextureSM2 = EnumShaderGLES2.T4M_World_Projection_Unlit_Lightmap_Compatible;
			T4MCache.ShaderModel = SM.ShaderModel2;
			T4MConfig.UpSideTile = mainObjTm.T4MMaterial.GetVector ("_Tiling");
            T4MConfig.UpSideF = mainObjTm.T4MMaterial.GetFloat ("_UpSide");
            T4MConfig.BlendFac = mainObjTm.T4MMaterial.GetFloat ("_Blend");
			
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel2/Diffuse/T4M 2 Textures")){
			T4MCache.MenuTextureSM2 = EnumShaderGLES2.T4M_2_Textures_HighSpec;
			T4MCache.ShaderModel = SM.ShaderModel2;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel2/Diffuse/T4M 3 Textures")){
			T4MCache.MenuTextureSM2 = EnumShaderGLES2.T4M_3_Textures_HighSpec;
			T4MCache.ShaderModel = SM.ShaderModel2;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel2/Diffuse/T4M 4 Textures")){
			T4MCache.MenuTextureSM2 = EnumShaderGLES2.T4M_4_Textures_HighSpec;
			T4MCache.ShaderModel = SM.ShaderModel2;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel2/Diffuse/T4M 5 Textures")){
			T4MCache.MenuTextureSM2 = EnumShaderGLES2.T4M_5_Textures_HighSpec;
			T4MCache.ShaderModel = SM.ShaderModel2;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel2/Diffuse/T4M 6 Textures")){
			T4MCache.MenuTextureSM2 = EnumShaderGLES2.T4M_6_Textures_HighSpec;
			T4MCache.ShaderModel = SM.ShaderModel2;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel2/Diffuse/T4M World Projection Shader")){
			T4MCache.MenuTextureSM2 = EnumShaderGLES2.T4M_World_Projection_HighSpec;
			T4MCache.ShaderModel = SM.ShaderModel2;
            T4MConfig.UpSideTile = mainObjTm.T4MMaterial.GetVector ("_Tiling");
            T4MConfig.UpSideF = mainObjTm.T4MMaterial.GetFloat ("_UpSide");
            T4MConfig.BlendFac = mainObjTm.T4MMaterial.GetFloat ("_Blend");
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel2/Specular/T4M 2 Textures Spec")){
			T4MCache.MenuTextureSM2 = EnumShaderGLES2.T4M_2_Textures_Specular;
			T4MCache.ShaderModel = SM.ShaderModel2;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel2/Specular/T4M 3 Textures Spec")){
			T4MCache.MenuTextureSM2 = EnumShaderGLES2.T4M_3_Textures_Specular;
			T4MCache.ShaderModel = SM.ShaderModel2;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel2/Specular/T4M 4 Textures Spec")){
			T4MCache.MenuTextureSM2 = EnumShaderGLES2.T4M_4_Textures_Specular;
			T4MCache.ShaderModel = SM.ShaderModel2;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel2/MobileLM/T4M 2 Textures for Mobile")){
			T4MCache.MenuTextureSM2 = EnumShaderGLES2.T4M_2_Textures_4_Mobile;
			T4MCache.ShaderModel = SM.ShaderModel2;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel2/MobileLM/T4M 3 Textures for Mobile")){
			T4MCache.MenuTextureSM2 = EnumShaderGLES2.T4M_3_Textures_4_Mobile;
			T4MCache.ShaderModel = SM.ShaderModel2;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel2/MobileLM/T4M 4 Textures for Mobile")){
			T4MCache.MenuTextureSM2 = EnumShaderGLES2.T4M_4_Textures_4_Mobile;
			T4MCache.ShaderModel = SM.ShaderModel2;
		}//else
		//if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel2/MobileLM/T4M World Projection Shader_Mobile")){
		//	T4MCache.MenuTextureSM2 = EnumShaderGLES2.T4M_World_Projection_Mobile;
		//	T4MCache.ShaderModel = SM.ShaderModel2;
		//	UpSideTile = mainObjTm.T4MMaterial.GetVector ("_Tiling");
		//	UpSideF = mainObjTm.T4MMaterial.GetFloat ("_UpSide");
		//	BlendFac= mainObjTm.T4MMaterial.GetFloat ("_Blend");
		//}
		else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel2/Toon/T4M 2 Textures Toon")){
			T4MCache.MenuTextureSM2 = EnumShaderGLES2.T4M_2_Textures_Toon;
			T4MCache.ShaderModel = SM.ShaderModel2;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel2/Toon/T4M 3 Textures Toon")){
			T4MCache.MenuTextureSM2 = EnumShaderGLES2.T4M_3_Textures_Toon;
			T4MCache.ShaderModel = SM.ShaderModel2;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel2/Toon/T4M 4 Textures Toon")){
			T4MCache.MenuTextureSM2 = EnumShaderGLES2.T4M_4_Textures_Toon;
			T4MCache.ShaderModel = SM.ShaderModel2;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel2/Bump/T4M 2 Textures Bumped")){
			T4MCache.MenuTextureSM2 = EnumShaderGLES2.T4M_2_Textures_Bumped;
			T4MCache.ShaderModel = SM.ShaderModel2;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel2/Bump/T4M 3 Textures Bumped")){
			T4MCache.MenuTextureSM2 = EnumShaderGLES2.T4M_3_Textures_Bumped;
			T4MCache.ShaderModel = SM.ShaderModel2;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel2/Bump/T4M 4 Textures Bumped")){
			T4MCache.MenuTextureSM2 = EnumShaderGLES2.T4M_4_Textures_Bumped;
			T4MCache.ShaderModel = SM.ShaderModel2;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel2/Bump/T4M 2 Textures Bumped Mobile")){
			T4MCache.MenuTextureSM2 = EnumShaderGLES2.T4M_2_Textures_Bumped_Mobile;
			T4MCache.ShaderModel = SM.ShaderModel2;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel2/Bump/T4M 3 Textures Bumped Mobile")){
			T4MCache.MenuTextureSM2 = EnumShaderGLES2.T4M_3_Textures_Bumped_Mobile;
			T4MCache.ShaderModel = SM.ShaderModel2;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel2/Bump/T4M 2 Textures Bump Specular Mobile")){
			T4MCache.MenuTextureSM2 = EnumShaderGLES2.T4M_2_Textures_Bumped_SPEC_Mobile;
			T4MCache.ShaderModel = SM.ShaderModel2;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel2/BumpDLM/T4M 2 Textures Bumped DLM Mobile")){
			T4MCache.MenuTextureSM2 = EnumShaderGLES2.T4M_2_Textures_Bumped_DirectionalLM_Mobile;
			T4MCache.ShaderModel = SM.ShaderModel2;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel2/Bump/T4M 2 Textures Bump Specular")){
			T4MCache.MenuTextureSM2 = EnumShaderGLES2.T4M_2_Textures_Bumped_SPEC;
			T4MCache.ShaderModel = SM.ShaderModel2;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel2/Bump/T4M 3 Textures Bump Specular")){
			T4MCache.MenuTextureSM2 = EnumShaderGLES2.T4M_3_Textures_Bumped_SPEC;
			T4MCache.ShaderModel = SM.ShaderModel2;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel2/BumpDLM/T4M 2 Textures Bumped DLM")){
			T4MCache.MenuTextureSM2 = EnumShaderGLES2.T4M_2_Textures_Bumped_DirectionalLM;
			T4MCache.ShaderModel = SM.ShaderModel2;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel2/BumpDLM/T4M 3 Textures Bumped DLM")){
			T4MCache.MenuTextureSM2 = EnumShaderGLES2.T4M_3_Textures_Bumped_DirectionalLM;
			T4MCache.ShaderModel = SM.ShaderModel2;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel3/Diffuse/T4M 2 Textures")){
			T4MCache.MenuTextureSM3 = EnumShaderGLES3.T4M_2_Textures_Diffuse;
			T4MCache.ShaderModel = SM.ShaderModel3;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel3/Diffuse/T4M 3 Textures")){
			T4MCache.MenuTextureSM3 = EnumShaderGLES3.T4M_3_Textures_Diffuse;
			T4MCache.ShaderModel = SM.ShaderModel3;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel3/Diffuse/T4M 3 Textures")){
			T4MCache.MenuTextureSM3 = EnumShaderGLES3.T4M_3_Textures_Diffuse;
			T4MCache.ShaderModel = SM.ShaderModel3;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel3/Diffuse/T4M 4 Textures")){
			T4MCache.MenuTextureSM3 = EnumShaderGLES3.T4M_4_Textures_Diffuse;
			T4MCache.ShaderModel = SM.ShaderModel3;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel3/Diffuse/T4M 5 Textures")){
			T4MCache.MenuTextureSM3 = EnumShaderGLES3.T4M_5_Textures_Diffuse;
			T4MCache.ShaderModel = SM.ShaderModel3;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel3/Diffuse/T4M 6 Textures")){
			T4MCache.MenuTextureSM3 = EnumShaderGLES3.T4M_6_Textures_Diffuse;
			T4MCache.ShaderModel = SM.ShaderModel3;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel3/Specular/T4M 2 Textures Spec")){
			T4MCache.MenuTextureSM3 = EnumShaderGLES3.T4M_2_Textures_Specular;
			T4MCache.ShaderModel = SM.ShaderModel3;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel3/Specular/T4M 3 Textures Spec")){
			T4MCache.MenuTextureSM3 = EnumShaderGLES3.T4M_3_Textures_Specular;
			T4MCache.ShaderModel = SM.ShaderModel3;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel3/Specular/T4M 4 Textures Spec")){
			T4MCache.MenuTextureSM3 = EnumShaderGLES3.T4M_4_Textures_Specular;
			T4MCache.ShaderModel = SM.ShaderModel3;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel3/Bump/T4M 2 Textures Bump")){
			T4MCache.MenuTextureSM3 = EnumShaderGLES3.T4M_2_Textures_Bumped;
			T4MCache.ShaderModel = SM.ShaderModel3;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel3/Bump/T4M 3 Textures Bump")){
			T4MCache.MenuTextureSM3 = EnumShaderGLES3.T4M_3_Textures_Bumped;
			T4MCache.ShaderModel = SM.ShaderModel3;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel3/Bump/T4M 4 Textures Bump")){
			T4MCache.MenuTextureSM3 = EnumShaderGLES3.T4M_4_Textures_Bumped;
			T4MCache.ShaderModel = SM.ShaderModel3;
		}
		else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel3/BumpSpec/T4M 2 Textures Bump Spec")){
			T4MCache.MenuTextureSM3 = EnumShaderGLES3.T4M_2_Textures_Bumped_SPEC;
			T4MCache.ShaderModel = SM.ShaderModel3;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel3/BumpSpec/T4M 3 Textures Bump Spec")){
			T4MCache.MenuTextureSM3 = EnumShaderGLES3.T4M_3_Textures_Bumped_SPEC;
			T4MCache.ShaderModel = SM.ShaderModel3;
		}else
		if (mainObjTm.T4MMaterial.shader == Shader.Find("iT4MShaders/ShaderModel3/BumpSpec/T4M 4 Textures Bump Spec")){
			T4MCache.MenuTextureSM3 = EnumShaderGLES3.T4M_4_Textures_Bumped_SPEC;
			T4MCache.ShaderModel = SM.ShaderModel3;
		}else{
			T4MCache.ShaderModel = SM.CustomShader;
            T4MCache.CustomShader =mainObjTm.T4MMaterial.shader;
			if (mainObjTm.T4MMaterial.HasProperty("_Tiling")){
                T4MConfig.UpSideTile = mainObjTm.T4MMaterial.GetVector ("_Tiling");
                T4MConfig.UpSideF = mainObjTm.T4MMaterial.GetFloat ("_UpSide");
                T4MConfig.BlendFac = mainObjTm.T4MMaterial.GetFloat ("_Blend");
				
			}
		}		
					
	}
	
}