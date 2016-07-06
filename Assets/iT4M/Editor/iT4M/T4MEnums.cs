/********************************************************************************
** Author： LiangZG
** Email :  game.liangzg@foxmail.com
*********************************************************************************/
public enum SM
{
    ShaderModel1,
    ShaderModel2,
    ShaderModel3,
    CustomShader
}
public enum PlantMode
{
    Classic,
    Follow_Normals
}

public enum EnumShaderGLES1
{
    T4M_2_Textures_Auto_BeastLM_2DrawCall,
    T4M_2_Textures_ManualAdd_BeastLM_1DC,
    T4M_2_Textures_ManualAdd_CustoLM_1DC
}

public enum CreaType
{
    Classic_T4M,
    Custom
}
public enum LODMod
{
    Mass_Control,
    Independent_Control
}

public enum ViewD
{
    Close,
    Middle,
    Far,
    BackGround
}
public enum BillbAxe
{
    Y_Axis,
    All_Axis
}

public enum OccludeBy
{
    Max_View_Distance,
    Layer_Cull_Distances
}
public enum EnumShaderGLES2
{
    T4M_2_Textures_Unlit_Lightmap_Compatible,
    T4M_3_Textures_Unlit_Lightmap_Compatible,
    T4M_4_Textures_Unlit_Lightmap_Compatible,
    T4M_5_Textures_Unlit_Lightmap_Compatible,
    T4M_6_Textures_Unlit_Lightmap_Compatible,
    T4M_World_Projection_Unlit_Lightmap_Compatible,
    T4M_6_Textures_Unlit_No_Lightmap,
    T4M_2_Textures_HighSpec,
    T4M_3_Textures_HighSpec,
    T4M_4_Textures_HighSpec,
    T4M_5_Textures_HighSpec,
    T4M_6_Textures_HighSpec,
    T4M_World_Projection_HighSpec,
    T4M_2_Textures_Specular,
    T4M_3_Textures_Specular,
    T4M_4_Textures_Specular,
    T4M_2_Textures_4_Mobile,
    T4M_3_Textures_4_Mobile,
    T4M_4_Textures_4_Mobile,
    T4M_2_Textures_Toon,
    T4M_3_Textures_Toon,
    T4M_4_Textures_Toon,
    T4M_2_Textures_Bumped_Mobile,
    T4M_3_Textures_Bumped_Mobile,
    T4M_2_Textures_Bumped,
    T4M_3_Textures_Bumped,
    T4M_4_Textures_Bumped,
    T4M_2_Textures_Bumped_SPEC_Mobile,
    T4M_2_Textures_Bumped_SPEC,
    T4M_3_Textures_Bumped_SPEC,
    T4M_2_Textures_Bumped_DirectionalLM_Mobile,
    T4M_2_Textures_Bumped_DirectionalLM,
    T4M_3_Textures_Bumped_DirectionalLM
}

public enum EnumShaderGLES3
{
    T4M_2_Textures_Diffuse,
    T4M_3_Textures_Diffuse,
    T4M_4_Textures_Diffuse,
    T4M_5_Textures_Diffuse,
    T4M_6_Textures_Diffuse,
    T4M_2_Textures_Specular,
    T4M_3_Textures_Specular,
    T4M_4_Textures_Specular,
    T4M_2_Textures_Bumped,
    T4M_3_Textures_Bumped,
    T4M_4_Textures_Bumped,
    T4M_2_Textures_Bumped_SPEC,
    T4M_3_Textures_Bumped_SPEC,
    T4M_4_Textures_Bumped_SPEC
}

public enum LODShaderStatus
{
    New,
    AlreadyExist
}

public enum MaterialType
{
    Classic,
    Substances
}

public enum PaintHandle
{
    Classic,
    Follow_Normal_Circle,
    Follow_Normal_WireCircle,
    Hide_preview
}