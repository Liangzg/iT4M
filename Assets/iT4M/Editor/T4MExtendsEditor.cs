//Update SC
using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(T4MMainObj))]
[CanEditMultipleObjects]
public class T4MExtendsEditor : Editor {
	int layerMask = 1073741824;
	bool ToggleF = false;
	Texture2D[] UndoObj;
	static Color[] terrainBay2;
	GameObject PlantObj;
	GameObject PlantObjPreview;
	GameObject currentObjPreview;
	float rotationCorrect;
	T4MPlant[] T4MPlantObjGet;
	int CheckPlacement;
	int State;
	int oldState;
	float RandomDistance;
	Vector3 RandomRotation;
	bool oldRandomRot;
	int plantmodval;
	float oldSize;
	Renderer[] T4MPlantRenderer;
	string OldActivStat = "";
	Collider[] T4MPreviewColl;
	Renderer[] LodObj;
	static ArrayList onPlayModeInstanceFolder= new ArrayList();
	static ArrayList onPlayModeInstanceGroup= new ArrayList();
	static ArrayList onPlayModeInstancePos= new ArrayList();
	static ArrayList onPlayModeInstanceRot= new ArrayList();
	static ArrayList onPlayModeInstanceSize= new ArrayList();
	static ArrayList onPlayModeDestroyed= new ArrayList();
	static bool Play;
	
	void OnSceneGUI  () {
		if (T4MConfig.T4MPreview && T4MCache.T4MMenuToolbar == 3)
			Painter();
		else if (T4MCache.T4MMenuToolbar == 4)
			Planting ();
		else State = 3;
		
		if (oldState != State || !PlantObjPreview && T4MCache.T4MMenuToolbar == 4 ||T4MCache.T4MObjectPlant[T4MConfig.T4MPlantSel] == null && T4MCache.T4MMenuToolbar == 4){
			
			MeshRenderer[] prev = GameObject.FindObjectsOfType(typeof(MeshRenderer)) as MeshRenderer[];
			
			foreach(MeshRenderer go in prev)
			{
				if(go.hideFlags == HideFlags.HideInHierarchy || go.name == "previewT4M")
				{
					go.hideFlags=0;
					DestroyImmediate (go.gameObject);
				}
			}
			oldState = State;
		}
		
	}

	void Painter (){
		if (State != 1)
			State = 1;
		Event e  = Event.current;
		if (e.type ==  EventType.KeyDown && e.keyCode==KeyCode.T){
			if (T4MMainEditor.T4MActived != "Activated")
				T4MMainEditor.T4MActived = "Activated";
			else T4MMainEditor.T4MActived = "Deactivated";
		}
		
		if (T4MConfig.T4MPreview && T4MMainEditor.T4MActived == "Activated" && T4MConfig.T4MPreview.enabled == false || T4MConfig.T4MPreview.enabled == false){
			 if(
				T4MCache.PaintPrev != PaintHandle.Follow_Normal_Circle && 
				T4MCache.PaintPrev != PaintHandle.Follow_Normal_WireCircle &&
				T4MCache.PaintPrev != PaintHandle.Hide_preview
				){
					T4MConfig.T4MPreview.enabled = true;
			}
		}else if (T4MConfig.T4MPreview && T4MMainEditor.T4MActived == "Deactivated" && T4MConfig.T4MPreview.enabled == true || T4MConfig.T4MPreview.enabled == true){	
			if (T4MCache.PaintPrev != PaintHandle.Classic){ 
				T4MConfig.T4MPreview.enabled = false;
			}
		}
		
		if (T4MMainEditor.T4MActived == "Activated"){
			HandleUtility.AddDefaultControl (0);
		RaycastHit raycastHit = new RaycastHit();
		Ray terrain  = HandleUtility.GUIPointToWorldRay (e.mousePosition);
		if (e.type ==  EventType.KeyDown && e.keyCode==KeyCode.KeypadPlus){
			T4MConfig.brushSize += 1;
		}else if (e.type ==  EventType.KeyDown && e.keyCode==KeyCode.KeypadMinus){
			T4MConfig.brushSize -= 1;
		}
		
		if(Physics.Raycast(terrain, out raycastHit, Mathf.Infinity,layerMask)){
				if (T4MMainEditor.CurrentSelect.gameObject.GetComponent <T4MMainObj>().ConvertType !="UT")
					T4MConfig.T4MPreview.transform.localEulerAngles = new Vector3(90,180+T4MMainEditor.CurrentSelect.localEulerAngles.y,0);
                else T4MConfig.T4MPreview.transform.localEulerAngles = new Vector3(90, T4MMainEditor.CurrentSelect.localEulerAngles.y, 0);
					T4MConfig.T4MPreview.transform.position = raycastHit.point;
					
				if(T4MCache.PaintPrev != PaintHandle.Classic && T4MCache.PaintPrev != PaintHandle.Hide_preview &&  T4MCache.PaintPrev != PaintHandle.Follow_Normal_WireCircle){
					Handles.color = new Color(1f,1f,0f,0.05f);
					Handles.DrawSolidDisc(raycastHit.point, raycastHit.normal, T4MConfig.T4MPreview.orthographicSize*0.9f);
				}else if(T4MCache.PaintPrev != PaintHandle.Classic && T4MCache.PaintPrev != PaintHandle.Hide_preview  && T4MCache.PaintPrev != PaintHandle.Follow_Normal_Circle){
					Handles.color = new Color(1f,1f,0f,1f);
					Handles.DrawWireDisc(raycastHit.point, raycastHit.normal, T4MConfig.T4MPreview.orthographicSize*0.9f);
				}
				
				if ((e.type ==  EventType.mouseDrag && e.alt == false && e.shift == false && e.button == 0) || (e.shift == false && e.alt == false && e.button == 0 && ToggleF == false)){	
					Vector2 pixelUV = raycastHit.textureCoord* T4MConfig.T4MMaskTexUVCoord;//0.14f;
					int PuX = Mathf.FloorToInt (pixelUV.x * T4MCache.T4MMaskTex.width);
					int PuY = Mathf.FloorToInt (pixelUV.y * T4MCache.T4MMaskTex.height);
					int x = Mathf.Clamp ( PuX - T4MCache.T4MBrushSizeInPourcent / 2, 0, T4MCache.T4MMaskTex.width - 1);
					int y = Mathf.Clamp (PuY - T4MCache.T4MBrushSizeInPourcent / 2, 0, T4MCache.T4MMaskTex.height - 1);
					int width = Mathf.Clamp (( PuX + T4MCache.T4MBrushSizeInPourcent / 2) , 0, T4MCache.T4MMaskTex.width) - x;
					int height = Mathf.Clamp ((PuY + T4MCache.T4MBrushSizeInPourcent / 2), 0, T4MCache.T4MMaskTex.height) - y;
					Color[] terrainBay =  T4MCache.T4MMaskTex.GetPixels (x, y, width, height, 0);
					if(T4MCache.T4MMaskTex2)
						terrainBay2 = T4MCache.T4MMaskTex2.GetPixels(x, y, width, height, 0);
					for (int i = 0; i < height; i++) {
						for (int j = 0; j < width; j++) {
							int index = (i * width) + j;
							float Stronger= T4MCache.T4MBrushAlpha[Mathf.Clamp((y + i) - (PuY - T4MCache.T4MBrushSizeInPourcent / 2), 0, T4MCache.T4MBrushSizeInPourcent - 1)*T4MCache.T4MBrushSizeInPourcent + Mathf.Clamp((x + j) - ( PuX - T4MCache.T4MBrushSizeInPourcent / 2), 0, T4MCache.T4MBrushSizeInPourcent - 1)]* T4MConfig.T4MStronger;
							
							if (T4MConfig.T4MselTexture <3){
								terrainBay[index] = Color.Lerp(terrainBay[index], T4MCache.T4MtargetColor,Stronger);
								
							}else{
								terrainBay[index] = Color.Lerp(terrainBay[index], T4MCache.T4MtargetColor,Stronger);//*0.3f);
								if(T4MCache.T4MMaskTex2)
									terrainBay2[index] = Color.Lerp(terrainBay2[index], T4MCache.T4MtargetColor2,Stronger);///0.3f);
							}
						}
					}
					T4MCache.T4MMaskTex.SetPixels(x, y, width,height, terrainBay,0);
					T4MCache.T4MMaskTex.Apply();
					if(T4MCache.T4MMaskTex2){
						T4MCache.T4MMaskTex2.SetPixels(x, y, width,height, terrainBay2, 0);
						T4MCache.T4MMaskTex2.Apply();
						UndoObj = new Texture2D[2];
                        UndoObj[0] = T4MCache.T4MMaskTex;
                        UndoObj[1] = T4MCache.T4MMaskTex2;
					}else{
						UndoObj = new Texture2D[1];
						UndoObj[0] = T4MCache.T4MMaskTex;
					}
                    //Undo.RecordObjects(UndoObj, "T4MMask"); //Unity don't work correctly with this for now
					ToggleF = true;	
					
				}else if (e.type ==  EventType.mouseUp && e.alt == false && e.button == 0 && ToggleF == true){
                    
					T4MMainEditor.SaveTexture();
					ToggleF = false;
				}
			}
		}
	}
	
	void Planting (){
		if (State != 2)
				State = 2;
		Event e  = Event.current;
		if (e.type ==  EventType.KeyDown && e.keyCode==KeyCode.T){
			if (T4MMainEditor.T4MActived != "Activated")
				T4MMainEditor.T4MActived = "Activated";
			else T4MMainEditor.T4MActived = "Deactivated";
		}
		
		if (!Play && EditorApplication.isPlaying == true){
			Play=true;
		}else if (Play && EditorApplication.isPlaying == false){
			SaveInPlayingMode();
			Play=false;	
		}
		
		
		if (PlantObjPreview && T4MMainEditor.T4MActived == "Activated" && OldActivStat != T4MMainEditor.T4MActived){
			T4MPlantRenderer =  PlantObjPreview.GetComponentsInChildren <Renderer>();
			for(int i=0;i<T4MPlantRenderer.Length;i++){
					if(T4MPlantRenderer[i].GetComponent<Renderer>().enabled == false)
						T4MPlantRenderer[i].GetComponent<Renderer>().enabled = true;		
			}
			
			OldActivStat = T4MMainEditor.T4MActived;
		}else if (PlantObjPreview && T4MMainEditor.T4MActived == "Deactivated" && OldActivStat != T4MMainEditor.T4MActived){	
			T4MPlantRenderer =  PlantObjPreview.GetComponentsInChildren <Renderer>();
			for(int i=0;i<T4MPlantRenderer.Length;i++){
				if(T4MPlantRenderer[i].GetComponent<Renderer>().enabled == true)
					T4MPlantRenderer[i].GetComponent<Renderer>().enabled = false;		
			}
			OldActivStat = T4MMainEditor.T4MActived;
		}
		if (T4MMainEditor.T4MActived == "Activated"){	
			if(oldRandomRot != T4MCache.T4MRandomRot){
				if(PlantObjPreview)
					PlantObjPreview.transform.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.zero);
				oldRandomRot = T4MCache.T4MRandomRot;
			}
			
			if (PlantObjPreview && PlantObjPreview.transform.localScale != new Vector3 (T4MConfig.T4MObjSize,T4MConfig.T4MObjSize,T4MConfig.T4MObjSize) && T4MCache.T4MSizeVar ==0 ||PlantObjPreview &&  oldSize != T4MConfig.T4MObjSize){
				PlantObjPreview.transform.localScale = new Vector3 (T4MConfig.T4MObjSize,T4MConfig.T4MObjSize,T4MConfig.T4MObjSize);	
				oldSize = T4MConfig.T4MObjSize;	
			}
			
			
				HandleUtility.AddDefaultControl (0);
				RaycastHit raycastHit = new RaycastHit();
				Ray terrain  = HandleUtility.GUIPointToWorldRay (e.mousePosition);
				
				GameObject CurrentObject = T4MCache.T4MObjectPlant[T4MConfig.T4MPlantSel];
				
				if(Physics.Raycast(terrain, out raycastHit, Mathf.Infinity,layerMask)){
					
					if (CurrentObject){
						if(T4MConfig.T4MDistanceMin != T4MConfig.T4MDistanceMax && T4MCache.T4MRandomSpa && RandomDistance ==0 || RandomDistance < T4MConfig.T4MDistanceMin || RandomDistance > T4MConfig.T4MDistanceMax)
							RandomDist();
						
						if (e.type ==  EventType.KeyDown && e.keyCode==KeyCode.H){
							rotationCorrect = -1;
						}else if (e.type ==  EventType.KeyDown && e.keyCode==KeyCode.G){
							rotationCorrect = 1;
						}else rotationCorrect = 0;
					
						if (e.type ==  EventType.KeyDown && e.keyCode==KeyCode.KeypadPlus){
                            T4MConfig.T4MYOrigin += 0.1f;
						}else if (e.type ==  EventType.KeyDown && e.keyCode==KeyCode.KeypadMinus){
							T4MConfig.T4MYOrigin -= 0.1f;
						}
					
					
						if (!PlantObjPreview || currentObjPreview != CurrentObject){
												
							rotationCorrect= 0f;
							if (PlantObjPreview)
								DestroyImmediate (PlantObjPreview);
							PlantObjPreview = Instantiate (CurrentObject, raycastHit.point,Quaternion.identity) as GameObject;
						
							T4MPlantRenderer =  PlantObjPreview.GetComponentsInChildren <Renderer>();
							for(int i=0;i<T4MPlantRenderer.Length;i++){
								if (T4MPlantRenderer[i].GetComponent<Renderer>() && T4MPlantRenderer[i].GetComponent<Renderer>().sharedMaterial.HasProperty("_MainTex")){
									Texture Text = T4MPlantRenderer[i].GetComponent<Renderer>().sharedMaterial.GetTexture("_MainTex");
                                    Shader mShader = Shader.Find("Hidden/iT4MShaders/PlantPreview");
                                    Material NewPMat  = new Material (mShader);
									T4MPlantRenderer[i].GetComponent<Renderer>().sharedMaterial = NewPMat;
									T4MPlantRenderer[i].GetComponent<Renderer>().sharedMaterial.SetTexture("_MainTex", Text);
								}
							}
							T4MPreviewColl =  PlantObjPreview.GetComponentsInChildren <Collider>();
							for(int j=0;j<T4MPreviewColl.Length;j++){
								DestroyImmediate(T4MPreviewColl[j]);
							}
							PlantObjPreview.transform.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.zero);
							PlantObjPreview.name = "previewT4M";
							T4MPlant removeScript = PlantObjPreview.GetComponent <T4MPlant>();
							DestroyImmediate(removeScript);
							PlantObjPreview.hideFlags = HideFlags.HideInHierarchy;
							currentObjPreview = CurrentObject;
						}
						PlantObjPreview.transform.position = raycastHit.point + (raycastHit.normal * T4MConfig.T4MYOrigin);
						if(T4MCache.T4MSizeVar ==0)
							PlantObjPreview.transform.localScale = new Vector3 (T4MConfig.T4MObjSize, T4MConfig.T4MObjSize, T4MConfig.T4MObjSize);
						if (T4MCache.T4MPlantMod == PlantMode.Follow_Normals){
							if (e.type ==  EventType.MouseMove){
								PlantObjPreview.transform.up = PlantObjPreview.transform.up+raycastHit.normal;
								plantmodval = 1;
							}
						}else{
							if (plantmodval ==1){
								PlantObjPreview.transform.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.zero);
								plantmodval = 0;
							}
						}
						if (rotationCorrect !=0){
								PlantObjPreview.transform.Rotate(0,rotationCorrect * 100 * 0.02f,0);
							}
						if (e.shift == false && e.control == false){
							T4MPlantRenderer =  PlantObjPreview.GetComponentsInChildren <Renderer>();
							if (CheckPlacement !=0){
								for(int i=0;i<T4MPlantRenderer.Length;i++){
									T4MPlantRenderer[i].GetComponent<Renderer>().sharedMaterial.SetColor("_TintColor", new Color(1f,0.5f,0.5f,0.071f));		
								}
							}else{ 
								for(int i=0;i<T4MPlantRenderer.Length;i++){
									T4MPlantRenderer[i].GetComponent<Renderer>().sharedMaterial.SetColor("_TintColor", new Color(0.5f,0.5f,0.5f,0.2f));		
								}
							}
							for(int i=0;i<T4MPlantRenderer.Length;i++){
									T4MPlantRenderer[i].GetComponent<Renderer>().enabled = true;		
							}
							Handles.color = new Color(0f,1f,0f,0.03f);
							Handles.DrawSolidDisc(raycastHit.point, raycastHit.normal, T4MConfig.T4MDistanceMin);
							if (T4MConfig.T4MDistanceMin != T4MConfig.T4MDistanceMax && T4MCache.T4MRandomSpa){
								Handles.color = new Color(1f,1f,0f,0.05f);
								Handles.DrawSolidDisc(raycastHit.point, raycastHit.normal, T4MConfig.T4MDistanceMax);
								Handles.color = new Color(1f,1f,0f,0.5f);
								if(RandomDistance > T4MConfig.T4MDistanceMin && RandomDistance < T4MConfig.T4MDistanceMax)
									Handles.DrawWireDisc(raycastHit.point, raycastHit.normal, RandomDistance);
							}
						}else if (e.shift == true && e.control == false){
							T4MPlantRenderer =  PlantObjPreview.GetComponentsInChildren <Renderer>();
							for(int i=0;i<T4MPlantRenderer.Length;i++){
									T4MPlantRenderer[i].GetComponent<Renderer>().enabled = false;		
							}
							Handles.color = new Color(1f,0f,0f,0.1f);
							Handles.DrawSolidDisc(raycastHit.point, raycastHit.normal, T4MConfig.T4MDistanceMin);
						}else if (e.shift == false && e.control == true){
							T4MPlantRenderer =  PlantObjPreview.GetComponentsInChildren <Renderer>();
							for(int i=0;i<T4MPlantRenderer.Length;i++){
									T4MPlantRenderer[i].GetComponent<Renderer>().enabled = false;		
							}
							Handles.color = new Color(1f,1f,0f,0.1f);
							Handles.DrawSolidDisc(raycastHit.point, raycastHit.normal, T4MConfig.T4MDistanceMin);
						}
						
						if (ToggleF == false){
							T4MPlantObjGet =  GameObject.FindObjectsOfType(typeof(T4MPlant)) as T4MPlant[];
							ToggleF = true;
						}
								CheckPlacement = 0;
								Vector3 fix = raycastHit.point;
								if (T4MPlantObjGet !=null && T4MPlantObjGet.Length>0 && ToggleF == true)
								for(int i=0;i<T4MPlantObjGet.Length;i++){
									if (T4MConfig.T4MDistanceMin != T4MConfig.T4MDistanceMax && T4MCache.T4MRandomSpa){
										if(Vector3.Distance(T4MPlantObjGet[i].transform.position, new Vector3(fix.x, T4MPlantObjGet[i].transform.position.y ,fix.z))< RandomDistance)
											CheckPlacement +=1;
									}else{
										if(Vector3.Distance(T4MPlantObjGet[i].transform.position, new Vector3(fix.x, T4MPlantObjGet[i].transform.position.y ,fix.z))< T4MConfig.T4MDistanceMin)
											CheckPlacement +=1;
									}
								}
					}
						if (e.type ==  EventType.mouseDown && e.alt == false && e.button == 0 && e.shift == false && e.control == false && PlantObjPreview|| e.type ==  EventType.mouseDrag && e.alt == false && e.button == 0 && e.shift == false && e.control == false && PlantObjPreview){
							if (CurrentObject){
								
								if (CheckPlacement == 0)
								{
									PlantObj = null;
									PlantObj =  PrefabUtility.InstantiatePrefab(CurrentObject) as GameObject; 
									if (!PlantObj.GetComponent<T4MPlant>())
										PlantObj.AddComponent <T4MPlant>();
									PlantObj.transform.position = PlantObjPreview.transform.position;
									PlantObj.transform.rotation = PlantObjPreview.transform.rotation;
									PlantObj.transform.localScale = PlantObjPreview.transform.localScale;
							
							
									LodObj =  PlantObj.GetComponentsInChildren <Renderer>();
							
									for (int t=0;t<LodObj.Length;t++){
										if(T4MConfig.ViewDistance[T4MConfig.T4MPlantSel]==ViewD.Close)
											LodObj[t].gameObject.layer = 26;
										else if(T4MConfig.ViewDistance[T4MConfig.T4MPlantSel]==ViewD.Middle)
											LodObj[t].gameObject.layer = 27;
										else if(T4MConfig.ViewDistance[T4MConfig.T4MPlantSel]==ViewD.Far)
											LodObj[t].gameObject.layer = 28;
										else if(T4MConfig.ViewDistance[T4MConfig.T4MPlantSel]==ViewD.BackGround)
											LodObj[t].gameObject.layer = 29;
									}
								
									ToggleF =false;
									GameObject Group = GameObject.Find(T4MCache.T4MGroupName);
									if (!Group){
										Group = new GameObject (T4MCache.T4MGroupName);
									}
									PlantObj.transform.parent = Group.transform;
									
									if (PlantObj.GetComponent<T4MLod>()){
                                        T4MCache.LodActivate = false;
										PlantObj.isStatic = T4MCache.T4MStaticObj;
									}else if (PlantObj.GetComponent<T4MBillboard>()){
										PlantObj.isStatic = false;
                                        T4MCache.billActivate = false;
									}else  PlantObj.isStatic = T4MCache.T4MStaticObj;
							
							
									if (T4MCache.T4MCreateColl && !PlantObj.GetComponent<Collider>())
										PlantObj.AddComponent<MeshCollider>();
									if(T4MCache.T4MRandomSpa)
										RandomDist();
									if(T4MCache.T4MRandomRot)
										RandomRot();
									if(T4MCache.T4MSizeVar !=0)
										RandomSize();
									if (T4MCache.T4MselectObj >1)
										RandomObject ();
									if(EditorApplication.isPlaying == true)
										InPlayingAdd(CurrentObject as GameObject, Group.name as string);
										
								}
							}
						}else if(e.type ==  EventType.mouseDown && e.alt == false && e.button == 0 && e.shift == true && e.control == false && PlantObjPreview || e.type ==  EventType.mouseDrag && e.alt == false && e.button == 0 && e.shift == true && e.control == false && PlantObjPreview){ 
							if (ToggleF == false){
								T4MPlantObjGet =  GameObject.FindObjectsOfType(typeof(T4MPlant)) as T4MPlant[];
								ToggleF =true;
							}
							for(int i=0;i<T4MPlantObjGet.Length;i++){
								Vector3 fix = raycastHit.point;
								if(Vector3.Distance(T4MPlantObjGet[i].transform.position, new Vector3(fix.x, T4MPlantObjGet[i].transform.position.y ,fix.z))< T4MConfig.T4MDistanceMin){
									if(EditorApplication.isPlaying == true)
										onPlayModeDestroyed.Add(T4MPlantObjGet[i].gameObject.transform.position); 
							
									if (T4MPlantObjGet[i].GetComponent<T4MBillboard>())
                                        T4MCache.billActivate = false;
									else if (T4MPlantObjGet[i].GetComponent<T4MLod>())
                                        T4MCache.LodActivate = false;
							
									DestroyImmediate (T4MPlantObjGet[i].gameObject);
									ToggleF = false;
								}
							}
						}else if(e.type ==  EventType.mouseDown && e.alt == false && e.button == 0 && e.shift == false && e.control == true && PlantObjPreview || e.type ==  EventType.mouseDrag && e.alt == false && e.button == 0 && e.shift == false && e.control == true && PlantObjPreview){ 
								
								if (ToggleF == false){
									T4MPlantObjGet =  GameObject.FindObjectsOfType(typeof(T4MPlant)) as T4MPlant[];
									
									ToggleF =true;
								}
							for(int i=0;i<T4MPlantObjGet.Length;i++){
								Vector3 fix = raycastHit.point;
								if(Vector3.Distance(T4MPlantObjGet[i].transform.position, new Vector3(fix.x, T4MPlantObjGet[i].transform.position.y ,fix.z))< T4MConfig.T4MDistanceMin){
									Quaternion rot = T4MPlantObjGet[i].gameObject.transform.rotation;
									Vector3 pos = T4MPlantObjGet[i].gameObject.transform.position;
									Vector3 scale = T4MPlantObjGet[i].gameObject.transform.localScale;
									int lay = T4MPlantObjGet[i].gameObject.layer;
									bool Static = T4MPlantObjGet[i].gameObject.isStatic;
									int LODBILL =0;
									if (T4MPlantObjGet[i].gameObject.GetComponent<T4MLod>())
										LODBILL =1;
									else if (T4MPlantObjGet[i].gameObject.GetComponent<T4MBillboard>())
										LODBILL =2;
								
									DestroyImmediate (T4MPlantObjGet[i].gameObject);
									ToggleF = false;
									PlantObj = null;
									PlantObj =  PrefabUtility.InstantiatePrefab(CurrentObject) as GameObject;
									if (!PlantObj.GetComponent<T4MPlant>())
										PlantObj.AddComponent <T4MPlant>();
							
									PlantObj.transform.position = pos;
									PlantObj.transform.rotation = rot;
									PlantObj.transform.localScale = scale;
									
									LodObj =  PlantObj.GetComponentsInChildren <Renderer>();
									for (int t=0;t<LodObj.Length;t++){
										LodObj[t].gameObject.layer = lay;
									}
							
									if (LODBILL==1)
                                        T4MCache.LodActivate = false;
									else if (LODBILL==2)
										T4MCache.billActivate = false;
							
									if (PlantObj.GetComponent<T4MLod>()){
                                        T4MCache.LodActivate = false;
										PlantObj.isStatic = Static;
									}else if (PlantObj.GetComponent<T4MBillboard>()){
										PlantObj.isStatic = false;
                                        T4MCache.billActivate = false;
									}else  PlantObj.isStatic = Static;
									
									GameObject Group = GameObject.Find(T4MCache.T4MGroupName);
									if (!Group){
										Group = new GameObject (T4MCache.T4MGroupName);
									}
									PlantObj.transform.parent = Group.transform;
								}
							}
						}else if (e.type ==  EventType.mouseUp && ToggleF == true){
							ToggleF =false;
						}
			}
		}
	}
	
	void RandomRot(){
		RandomRotation = new Vector3(Random.Range (-T4MConfig.T4MrandX*45, T4MConfig.T4MrandX*45),
                                     Random.Range (-T4MConfig.T4MrandY*180, T4MConfig.T4MrandY*180),
                                     Random.Range (-T4MConfig.T4MrandZ*45, T4MConfig.T4MrandZ*45));
		if(PlantObjPreview){
			PlantObjPreview.transform.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.zero);
			PlantObjPreview.transform.Rotate(RandomRotation);
		}
	}
	
	void RandomObject (){
		int SelObj = Random.Range (1, T4MCache.T4MselectObj+1);
		int MatchSel=0;
		for (int i=0;i< T4MCache.T4MBoolObj.Length;i++){
			if (T4MCache.T4MObjectPlant[i]){	
				if (T4MCache.T4MBoolObj[i] == true)
					MatchSel +=1;
			}
			if (MatchSel == SelObj){
				T4MConfig.T4MPlantSel= i;
				return;
			}
		} 
	}

	void RandomSize(){
		
		if(PlantObjPreview){
			float Var = Random.Range (-T4MCache.T4MSizeVar, T4MCache.T4MSizeVar);
			
			
			float variation = T4MConfig.T4MObjSize*Var;
			
		
			
			
			PlantObjPreview.transform.transform.localScale = new Vector3(T4MConfig.T4MObjSize+variation,T4MConfig.T4MObjSize+variation,T4MConfig.T4MObjSize+variation);
		}
	}
	
	void RandomDist(){
		RandomDistance = Random.Range(T4MConfig.T4MDistanceMin, T4MConfig.T4MDistanceMax);
	}
	
	
	
	
	void InPlayingAdd(GameObject CurrentObject, string Grp){
		onPlayModeInstanceFolder.Add(AssetDatabase.GetAssetPath (CurrentObject) as string); 
		onPlayModeInstanceGroup.Add(Grp as string); 
		onPlayModeInstancePos.Add(PlantObj.transform.position); 
		onPlayModeInstanceRot.Add(PlantObj.transform.rotation); 
		onPlayModeInstanceSize.Add(PlantObj.transform.localScale); 		
		
	}
	
	 void SaveInPlayingMode(){
		
		string[] F = onPlayModeInstanceFolder.ToArray(typeof(string)) as string[];
		string[] G = onPlayModeInstanceGroup.ToArray(typeof(string)) as string[];
		Vector3[] P = onPlayModeInstancePos.ToArray(typeof(Vector3)) as Vector3[];
		Quaternion[]R = onPlayModeInstanceRot.ToArray(typeof(Quaternion)) as Quaternion[];
		Vector3[] S = onPlayModeInstanceSize.ToArray(typeof(Vector3)) as Vector3[];
		Vector3[] D = onPlayModeDestroyed.ToArray(typeof(Vector3)) as Vector3[];
		
		for (int i=0;i<F.Length;i++){
			GameObject test = PrefabUtility.InstantiatePrefab((GameObject)AssetDatabase.LoadAssetAtPath(F[i], typeof(GameObject))) as GameObject;
			test.transform.position = P[i];
			test.transform.rotation = R[i];
			test.transform.localScale = S[i];
			if (!test.GetComponent<T4MPlant>())
					test.AddComponent <T4MPlant>();
			GameObject Group = GameObject.Find(G[i]);
			if (!Group){
				Group = new GameObject (G[i]);
			}
			test.transform.parent = Group.transform;
		}
		T4MPlant[] T4MPlantToErase =  GameObject.FindObjectsOfType(typeof(T4MPlant)) as T4MPlant[];
		for (int j=0;j<D.Length;j++){
			for (int k=0;k<T4MPlantToErase.Length;k++){
				if (T4MPlantToErase[k].gameObject.transform.position == D[j])
					DestroyImmediate(T4MPlantToErase[k].gameObject);
					T4MPlantToErase =  GameObject.FindObjectsOfType(typeof(T4MPlant)) as T4MPlant[];
					
			}
		}
			onPlayModeInstanceFolder = new ArrayList();
			onPlayModeInstanceGroup = new ArrayList();
			onPlayModeInstancePos = new ArrayList();
			onPlayModeInstanceRot = new ArrayList();
			onPlayModeInstanceSize = new ArrayList();
			onPlayModeDestroyed = new ArrayList();
	}
}
