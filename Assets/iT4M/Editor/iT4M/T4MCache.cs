/********************************************************************************
** Author： LiangZG
** Email :  game.liangzg@foxmail.com
*********************************************************************************/
using UnityEngine;
using System.Collections;

/// <summary>
/// 描述：T4M缓存数据中转
/// <para>创建时间：2016-07-05</para>
/// </summary>
public class T4MCache  {

	public static string PrefabName = "Name";

    public static GameObject Child;
    public static GameObject UnityTerrain;

    public static int nbrT4MObj;
    public static Renderer[] NbrPartObj;

    public static  int T4MMenuToolbar = 0;

    public static int partofT4MObj = 0;
    public static int vertexInfo;
    public static int trisInfo;

    public static TerrainData terrainDat;

    public static SM ShaderModel = SM.ShaderModel2;
    public static EnumShaderGLES2 MenuTextureSM2 = EnumShaderGLES2.T4M_4_Textures_HighSpec;
    public static EnumShaderGLES1 MenuTextureSM1 = EnumShaderGLES1.T4M_2_Textures_Auto_BeastLM_2DrawCall;
    public static EnumShaderGLES3 MenuTextureSM3 = EnumShaderGLES3.T4M_4_Textures_Diffuse;
    public static Shader CustomShader;

    public static bool T4MMaster = true;
    public static Transform PlayerCam;

    public static LODMod LODModeControler = LODMod.Mass_Control;
    public static Texture[] TexTexture;

    public static bool intialized = false;
    public static float shiness0;
    public static float shiness1;
    public static float shiness2;
    public static float shiness3;
    public static Color ShinessColor;

    public static Vector2 Layer1Tile;
    public static Vector2 Layer2Tile;
    public static Vector2 Layer3Tile;
    public static Vector2 Layer4Tile;
    public static Vector2 Layer5Tile;
    public static Vector2 Layer6Tile;

    static public Color T4MtargetColor;
    static public Color T4MtargetColor2;
    static public Texture2D T4MMaskTex2;
    static public Texture2D T4MMaskTex;

    static public int T4MBrushSizeInPourcent;
    static public PaintHandle PaintPrev = PaintHandle.Follow_Normal_Circle;

    static public float[] T4MBrushAlpha;

    //Planting

    static public PlantMode T4MPlantMod = PlantMode.Classic;
    static public bool T4MRandomRot = true;
    static public bool T4MRandomSpa;
    static public float T4MSizeVar;
    static public string T4MGroupName = "Group1";
    static public bool T4MCreateColl;
    static public bool T4MStaticObj = true;
    static public int T4MselectObj;

    static public GameObject[] T4MObjectPlant = new GameObject[6];
    static public bool[] T4MBoolObj = new bool[6];
    //LOD
    public static Texture LOD1T;
    public static Texture LOD1B;
    public static Texture LOD3T;
    public static Texture LOD3B;
    public static Texture LOD2T;
    public static Texture LOD2B;

    public static Shader LOD1S;
    public static Shader LOD2S;
    public static Shader LOD3S;

    public static Material LOD1Material;
    public static Material LOD2Material;
    public static Material LOD3Material;

    public static LODShaderStatus ShaderLOD1S = LODShaderStatus.New;
    public static LODShaderStatus ShaderLOD2S = LODShaderStatus.New;
    public static LODShaderStatus ShaderLOD3S = LODShaderStatus.New;

    static public bool LodActivate = false;

    public static OccludeBy LODocclusion = OccludeBy.Layer_Cull_Distances;

    //Billboard
    public static OccludeBy BilBocclusion = OccludeBy.Layer_Cull_Distances;
    public static BillbAxe BillBoardAxis = BillbAxe.Y_Axis;
    static public bool billActivate = false;

}
