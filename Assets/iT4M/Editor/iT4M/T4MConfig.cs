/********************************************************************************
** Author： LiangZG
** Email :  game.liangzg@foxmail.com
*********************************************************************************/
using UnityEngine;
using System.Collections;

/// <summary>
/// 描述：T4M配置
/// <para>创建时间：2016-07-05</para>
/// </summary>
public class T4MConfig  {

    public const string T4MEditorFolder = "Assets/iT4M/Editor/";
    public const string T4MFolder = "Assets/iT4M/";
    public const string T4MPrefabFolder = "Assets/iT4MOBJ/";

    public static bool ActivatedLayerCul = true;
    public static bool ActivatedBillboard = true;
    public static bool ActivatedLOD = true;
    public static float CloseDistMaxView = 30f;
    public static float NormalDistMaxView = 60f;
    public static float FarDistMaxView = 200f;
    public static float BGDistMaxView = 10000f;
    public static float BillboardDist = 30f;

    public static Texture Layer1;
    public static Texture Layer2;
    public static Texture Layer3;
    public static Texture Layer4;
    public static Texture Layer5;
    public static Texture Layer6;
    public static Texture LMMan;
    public static Texture Layer1Bump;
    public static Texture Layer2Bump;
    public static Texture Layer3Bump;
    public static Texture Layer4Bump;

    public static Projector T4MPreview;

    static public float T4MMaskTexUVCoord = 1f;
    static public float T4MDistanceMax = 15.0f;
    static public float T4MDistanceMin = 15.0f;

    static public int T4MselTexture = 0;
    public static Vector4 UpSideTile = new Vector4(0.5f, 0.5f, 0.5f, 0.5f);
    public static float UpSideF = 2.5f;
    public static float BlendFac = 4;
    static public int brushSize = 16;
    static public float T4MStronger = 0.5f;

    //Planting
    static public float T4MrandX = 0.0f;
    static public float T4MrandY = 1.0f;
    static public float T4MrandZ = 0.0f;
    static public float T4MYOrigin = 0.02f;

    static public int T4MPlantSel = 0;
    static public float T4MObjSize = 1;


    static public ViewD[] ViewDistance = { ViewD.Middle, ViewD.Middle, ViewD.Middle, ViewD.Middle, ViewD.Middle, ViewD.Middle };


    //LOD
    public static float MaximunView = 60.0f;
    public static float StartLOD2 = 20.0f;
    public static float StartLOD3 = 40.0f;
    public static float UpdateInterval = 1f;

    //Billboard
    public static float BillInterval = 0.1f;

}
