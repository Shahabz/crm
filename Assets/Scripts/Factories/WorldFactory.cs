using UnityEngine;
using System.Collections;

public class WorldFactory : AbstractFactory,ScreenControllerToShow {	
	
	public bool FlagAddExtraObjectsInPull;
	public int numOfTrees;
	public string PreloadTerrains="";
	public string RoadTerrains="";
	public GameObject debugPathIndicator;
	
	public GameObject terrainFactory;
	public GameObject UniqueFactory;
	public GameObject ObstacleFactory;
	public GameObject ObstacleSetFactory;
	public GameObject MoneyFactory;
	public GameObject BoostFactory;
	public GameObject []levelTags;
	
	private ArrayList treeElementFactories=new ArrayList();
	
	private TerrainElementFactory terrainElementFactory;
	
	private AbstractElementFactory uniqueElementFactory;
	private AbstractElementFactory boostElementFactory;
	
	
	private AbstractElementFactory obstacleElementFactory;  
	private AbstractElementFactory obstacleSetElementFactory;  	
	
	private AbstractElementFactory moneyElementFactory;
	
	private bool firstTimeInit=true;
	
	protected string []preloadTerrainsNames;
	protected string []roadTerrainsNames;
	protected int currentRoadPos=0;
	
	protected GameObject pathIndicator;
	
	private int vetexCount=0;
	
	private GameObject curLevelTagGameObject=null;
	private LevelTag curLevelTag;
	
	protected bool flagFirstTime=true;
	
	public string GetCurrentObstacleSet()
	{
		return terrainElementFactory.GetCurrentTerrainForZ().GetComponent<TerrainTag>().obstacleSetName;
	}
	
	public override void init(){
		GameObject curFactoryObject;
		
		//terrain
		curFactoryObject=Instantiate (terrainFactory) as GameObject;
		terrainElementFactory=curFactoryObject.GetComponent("TerrainElementFactory") as TerrainElementFactory;
		
		//uniqueObjects
		curFactoryObject=Instantiate (UniqueFactory) as GameObject;
		uniqueElementFactory=curFactoryObject.GetComponent("AbstractElementFactory") as AbstractElementFactory;
		
		//boostObjects
		curFactoryObject=Instantiate (BoostFactory) as GameObject;
		boostElementFactory=curFactoryObject.GetComponent("AbstractElementFactory") as AbstractElementFactory;
		
		//obstacles
		curFactoryObject=Instantiate (ObstacleFactory) as GameObject;
		obstacleElementFactory=curFactoryObject.GetComponent("AbstractElementFactory") as AbstractElementFactory;
		
		//Obstacle Set
		curFactoryObject=Instantiate (ObstacleSetFactory) as GameObject;
		obstacleSetElementFactory=curFactoryObject.GetComponent("AbstractElementFactory") as AbstractElementFactory;
		
		//money
		curFactoryObject=Instantiate (MoneyFactory) as GameObject;
		moneyElementFactory=curFactoryObject.GetComponent("AbstractElementFactory") as AbstractElementFactory;
		
		numberOfTerrains=2;
		
		//Get current level
		LoadCurrentLevel();
		
		if(debugPathIndicator){
			pathIndicator=Instantiate (debugPathIndicator) as GameObject;
		}
	}
	
	// Screen Controller To Show Methods
	public void ShowOnScreen()
	{
		if(!flagFirstTime){
			Debug.Log ("LoadCurrentLevel");
			LoadCurrentLevel();
			GlobalOptions.GetPlayerScript().Restart();
		}
	}
	
	public void HideOnScreen()
	{
		Debug.Log ("HideOnScreen");
		flagFirstTime=false;
	}
	//end Screen Controller To Show Methods
	
	public string GetNextLevelName()
	{
		string returnstring=GlobalOptions.loadingLevel;
		int i;
		for(i=0;i<levelTags.Length;i++)
		{
			//нашли
			if((levelTags[i] as GameObject).name==GlobalOptions.loadingLevel){
				break;
			}
		}	
		
		if(i<levelTags.Length-1)
		{
			i++;
			returnstring=(levelTags[i] as GameObject).name;
		}
		return returnstring;
	}
	
	public void LoadCurrentLevel(){
		int i;
		for(i=0;i<levelTags.Length;i++)
		{
			//нашли
			if((levelTags[i] as GameObject).name==GlobalOptions.loadingLevel){
				curLevelTagGameObject = Instantiate (levelTags[i]) as GameObject;
				break;
			}
		}	
		
		curLevelTag=curLevelTagGameObject.GetComponent("LevelTag")as LevelTag;
		curLevelTag.Parse();
		
		preloadTerrainsNames=curLevelTag.GetPreloadTerrainsNames();
		roadTerrainsNames=curLevelTag.GetRoadTerrainNames();
		
		AddAllObjectsIntoPulls();
	}
	
	public void AddAllObjectsIntoPulls()
	{
		//если раннер
		uniqueElementFactory.DestroyPullObjects();
		boostElementFactory.DestroyPullObjects();
		obstacleElementFactory.DestroyPullObjects();
		obstacleSetElementFactory.DestroyPullObjects();
		moneyElementFactory.DestroyPullObjects();
		terrainElementFactory.DestroyPullObjects();
		
		for(int i=0;i<treeElementFactories.Count;i++)
		{
			(treeElementFactories[i] as GameObject).GetComponent<AbstractElementFactory>().DestroyPullObjects();
		}
		
		//really need this!!!
		//obstacleSetElementFactory.PreloadPullObjects();
		boostElementFactory.PreloadPullObjects();
	}
	
	public override void ReStart(){
		oldObjectPos=initialPos;
		currentRoadPos=0;
		vetexCount=0;
		firstTimeInit=true;
		int i;
		
		terrainElementFactory.ReStart();
		uniqueElementFactory.ReStart();
		boostElementFactory.ReStart();
		obstacleElementFactory.ReStart();
		obstacleSetElementFactory.ReStart();
		for(i=0;i<treeElementFactories.Count;i++)
		{
			(treeElementFactories[i] as GameObject).GetComponent<AbstractElementFactory>().ReStart();
		}
		moneyElementFactory.ReStart();
		
		for(i=0;i<=numberOfTerrains;i++)
		{
			AddNextTerrain(false);
		}
	}
	
	public void TryAddTerrrain() {		
		AddNextTerrain(true);
		//удаляем старый кусочек земли
		DeleteOneFirstTerrain();
    }
	
	void Update () {
		if(currentRoadPos>=roadTerrainsNames.Length)
		{
			firstTimeInit=false;
			currentRoadPos=0;
		}
	}
	
	private void ParseTerrainNames()
	{
		//посчитаем необходимые кусочки
		/*if(drawMode){
			int i,j;
			bool flagFounded;
			ArrayList preloadTerrainList=new ArrayList(); 
			ArrayList terrainList=new ArrayList(); 
			ArrayList removeTerrainList=new ArrayList(); 
			//заполним видимые сначала
			for(i=0;i<numberOfTerrains+1;i++)
			{
				preloadTerrainList.Add(roadTerrainsNames[i]);
				terrainList.Add(roadTerrainsNames[i]);
			}
			
			for(i=numberOfTerrains;i<roadTerrainsNames.Length;i++)
			{
				flagFounded=false;
				for(j=0;j<removeTerrainList.Count;j++){
					if(removeTerrainList[j] as string == roadTerrainsNames[i])
					{
						//нашли
						terrainList.Add(removeTerrainList[j]);
						removeTerrainList.Remove(removeTerrainList[j]);
						flagFounded=true;
						break;
					}
				}
				//ничего не нашли
				if(!flagFounded)
				{
					preloadTerrainList.Add(roadTerrainsNames[i]);
					terrainList.Add(roadTerrainsNames[i]);
				}
				removeTerrainList.Add(terrainList[0]);
				terrainList.Remove(terrainList[0]);
			}
			
			preloadTerrainsNames=new string[preloadTerrainList.Count];
			for(i=0;i<preloadTerrainList.Count;i++){
				preloadTerrainsNames[i]=preloadTerrainList[i] as string;
			}
		}
		else*/
		{//получили массив террейнов
			char []separator={',','\n',' '};
			string []names=PreloadTerrains.Split(separator);
			preloadTerrainsNames=names;
		}
	}
	
	private void ParseRoadTerrainNames()
	{
		//получили массив террейнов
		char []separator={',','\n',' '};
		string []names=RoadTerrains.Split(separator);;
		roadTerrainsNames=names;
	}
	
	public override void AddNextTerrain(bool FlagCoRoutine){
		StartCoroutine(AddObjects(FlagCoRoutine));		
	}
	
	public override void AddObjectsInPulls(bool FlagCoRoutine){
		StartCoroutine(AddExtraObjectsInPullsCoRutine(FlagCoRoutine));	
	}
	
	private IEnumerator AddExtraObjectsInPullsCoRutine(bool FlagCoRoutine){
		
		int i;
		for(i=0;i<preloadTerrainsNames.Length;i++)
		{
			terrainElementFactory.AddExtraObjectInPullWithName(preloadTerrainsNames[i]);
			if(FlagCoRoutine) yield return null;
		}
		
		if(FlagCoRoutine) yield return null;
	}
	
	private IEnumerator addDynamicByMarkers(GameObject inTerrain,TerrainTag interrainTag,bool FlagCoRoutine){
		int i,j;	
		int kolvo;
		int randIndex;
		string curname;
		//tree
		ArrayList markedObjectsTrees=new ArrayList();	
		//uniqueobjects Terrains
		ArrayList markedObjectsUniqueTerrains=new ArrayList();
		
		//uniqueobjects
		ArrayList markedObjectsUnique=new ArrayList();	
		
		//ObstacleSet
		ArrayList markedObjectsObstacleSet=new ArrayList();	
		int neededNumberOfObstacleSet=10;
		
		//find all marks
		Transform[] allChildren = inTerrain.gameObject.GetComponentsInChildren<Transform>();
		for(i=0;i<allChildren.Length;i++)
		{
			//tree
			if(allChildren[i].name=="tree"){
				markedObjectsTrees.Add (allChildren[i]);
			}	
			
			//uniqueObjects
			if(allChildren[i].name=="UniqueObjectPool"){
				markedObjectsUnique.Add (allChildren[i]);
			}	
			
			//uniqueObjects Terrains
			if(allChildren[i].name=="UniqueObjectPoolTerrains"){
				markedObjectsUniqueTerrains.Add (allChildren[i]);
			}	
			
			//berry
			if(currentRoadPos>1||!firstTimeInit)
			{
				//Obstacle Set
				if(allChildren[i].name=="ObstacleSet"){
					markedObjectsObstacleSet.Add (allChildren[i]);
				}	
			}
			
			if(FlagCoRoutine&&i%30==0) yield return null;
		}
		
		//unique terrains
		Transform curUniqueTerrain;
		for(i=0;i<markedObjectsUniqueTerrains.Count;i++){
			Transform[] uniqueMarkers = (markedObjectsUniqueTerrains[i] as Transform).gameObject.GetComponentsInChildren<Transform>();
			for(j=1;j<uniqueMarkers.Length;j++){
				curUniqueTerrain=(uniqueMarkers[j] as Transform);
				curname=curUniqueTerrain.name;
				if(!curname.Contains("Left")&&!curname.Contains("Right"))
				{
					addOneUniqueAtMarker(curUniqueTerrain,interrainTag);
					if(FlagCoRoutine) yield return null;
				}
			}
		}
		
		int jset;
		//obstacles
		if(currentRoadPos>1||!firstTimeInit)
		{
			for(jset=0;jset<interrainTag.ObstacleSetArray.Length&&(MakeObstacleSet||jset==0);jset++)
			{
				//ObstacleSet
				GameObject curSet;
				Transform OneObstacle,marker;
				int randomIndexOfSet;
				for(i=0;i<markedObjectsObstacleSet.Count&&interrainTag.ObstacleSetArray.Length!=0;i++){
					kolvo=neededNumberOfObstacleSet>markedObjectsObstacleSet.Count?markedObjectsObstacleSet.Count:neededNumberOfObstacleSet;
					for(i=0;i<kolvo;i++){
						//случайный индекс маркера
						randIndex=Random.Range(0,markedObjectsObstacleSet.Count);
						//получим марке
						marker=markedObjectsObstacleSet[randIndex]as Transform;
						//теперь выбираем сет
						if(MakeObstacleSet)
						{
							randomIndexOfSet=jset;
						}else
						{
							randomIndexOfSet=Random.Range(0,interrainTag.ObstacleSetArray.Length);
						}
						// получаем препятствие
						curSet=obstacleSetElementFactory.GetNewObjectWithName((interrainTag.ObstacleSetArray[randomIndexOfSet] as GameObject).name);
						//поместим сет препятствий куда надо
						curSet.transform.position=marker.position;
						curSet.transform.rotation=marker.rotation;
						
						//name
						interrainTag.obstacleSetName=curSet.name;
						//получим список препятствий
						Transform[] setMarkers = curSet.GetComponentsInChildren<Transform>();
						
						for(j=1;j<setMarkers.Length;j++){
							OneObstacle=(setMarkers[j] as Transform);
							if(!OneObstacle)
							{
								continue;
							}
							curname=OneObstacle.name;
							if(curname=="money")
							{
								addOneMoneyAtMarker(OneObstacle,curSet.transform,interrainTag);
							}
							else
							if(curname=="boost")
							{
								addOneBoostAtMarker(OneObstacle,curSet.transform,interrainTag);
							}
							else
							{
								addOneObstacleFromSetAtMarker(OneObstacle,curSet.transform,interrainTag,0);
							}
							if(FlagCoRoutine) yield return null;
						}
						//добавим в сет
						interrainTag.PushToAllElements(curSet);
					}
				}
			}
		}
		
		//unique
		Transform curUnique;
		for(i=0;i<markedObjectsUnique.Count;i++){
			Transform[] uniqueMarkers = (markedObjectsUnique[i] as Transform).gameObject.GetComponentsInChildren<Transform>();
			for(j=1;j<uniqueMarkers.Length;j++){
				curUnique=(uniqueMarkers[j] as Transform);
				curname=curUnique.name;
				if(!curname.Contains("Left")&&!curname.Contains("Right"))
				{
					addOneUniqueAtMarker(curUnique,interrainTag);
					if(FlagCoRoutine&&i%2==0) yield return null;
				}
			}
		}
		
		//trees
		for(i=0;i<markedObjectsTrees.Count;i++){
			addOneTreeAtMarker(markedObjectsTrees[i] as Transform,interrainTag);
			if(FlagCoRoutine&&i%2==0) yield return null;
		}
		
		if(FlagCoRoutine) yield return null;
	}
	
	private GameObject addOneObstacleFromSetAtMarker(Transform marker,Transform inparent,TerrainTag interrainTag, int recursion){
		GameObject newObject,vspObject;
	
		newObject = obstacleElementFactory.GetNewObjectWithName(marker.name);
		
		//Debug.Log (marker.name);
		
		if(!newObject)
		{
			Debug.Log (marker.name+" NOT FOUND!!!");
			return null;
		}
		
		//set position & rotation
		newObject.transform.position=marker.position;
		
		newObject.transform.rotation=marker.rotation;
	
		if(interrainTag){
			interrainTag.PushToAllElements(newObject);
		}
		
		//if compiled object
		if(marker.name.Contains("Compiled"))
		{
			int j;
			GameObject newObjectInContainer;
			//ищем контейнер
			Transform Container=newObject.transform.FindChild("ContainerOfObjects");
			if(Container)
			{
				Transform[] allChildren = Container.gameObject.GetComponentsInChildren<Transform>();
				//обрабатываем все трансформы
				for(j=1;j<allChildren.Length;j++){
					//reqursively
					newObjectInContainer=addOneObstacleFromSetAtMarker(allChildren[j],inparent,interrainTag,recursion+1);
					if(newObjectInContainer)
					{
						newObjectInContainer.transform.parent=Container;
					}
				}
			}
		}
		
		if(MakeObstacleSet&&recursion==0)
		{
			vspObject=new GameObject();
			if(marker.name=="MonetaContainer")
			{
				vspObject.name="money";
			}
			else if(marker.name=="VodkaContainer"||marker.name=="MagnitContainer"||marker.name=="PostalContainer"||marker.name=="KopilkaContainer")
			{
				vspObject.name="boost";
			}
			else
			{
				vspObject.name=marker.name;
			}
			vspObject.transform.position=marker.position;
			vspObject.transform.rotation=marker.rotation;
			vspObject.transform.parent=marker.parent;
			DestroyImmediate(marker.gameObject);
			Debug.Log ("Deleted All UnUsed");
		}
		
		if(recursion==0&&!MakeObstacleSet)
		{
			newObject.transform.parent=inparent;
		}
		
		return newObject;
	}
	
	private void addOneUniqueAtMarker(Transform marker,TerrainTag interrainTag){
		GameObject newObject;
		
		newObject = uniqueElementFactory.GetNewObjectWithName(marker.name);
		
		if(!newObject)
		{
			Debug.Log ("Object Not Found - "+marker.name);
			return;
		}
		
		//set position & rotation
		newObject.transform.position=marker.position;
		
		MarkerTag marderTag=newObject.GetComponent<MarkerTag>();
		if(marderTag)
		{
			marderTag.ApplyRotation(marker.rotation,interrainTag.singleTransform.rotation);
		}
		else
		{
			newObject.transform.rotation=marker.rotation;
		}
	
		if(interrainTag){
			interrainTag.PushToAllElements(newObject);
		}
	}
	
	private void addOneBoostAtMarker(Transform marker,Transform inparent,TerrainTag interrainTag){
		GameObject newObject;
		
		newObject = boostElementFactory.GetNewObject();
		
		//set position & rotation
		newObject.transform.position=marker.position;
		
		newObject.transform.rotation=marker.rotation;
	
		if(interrainTag){
			interrainTag.PushToAllElements(newObject);
		}
		
		if(!MakeObstacleSet)
		{
			newObject.transform.parent=inparent;
		}
	}
	
	private void addOneTreeAtMarker(Transform marker,TerrainTag interrainTag){
		int i;
		GameObject newObject;
		AbstractElementFactory treeElementFactory=null;
		
		//findTreeFactory
		for(i=0;i<treeElementFactories.Count;i++)
		{
			//нашли
			if((treeElementFactories[i] as GameObject).name==interrainTag.treeElementFactory.name){
				treeElementFactory=(treeElementFactories[i] as GameObject).GetComponent<AbstractElementFactory>();
				break;
			}
		}	
		
		if(!treeElementFactory)
		{
			GameObject newTreeFactory;
			newTreeFactory	= Instantiate (interrainTag.treeElementFactory) as GameObject;
			treeElementFactories.Add(newTreeFactory);
			newTreeFactory.name=interrainTag.treeElementFactory.name;
			treeElementFactory=newTreeFactory.GetComponent<AbstractElementFactory>();
		}
		
		
		newObject	= treeElementFactory.GetNewObject();
			
		//set position & rotation
		newObject.transform.position=marker.position;
		
		MarkerTag marderTag=newObject.GetComponent<MarkerTag>();
		if(marderTag)
		{
			marderTag.ApplyRotation(marker.rotation,interrainTag.singleTransform.rotation);
		}
		else
		{
			newObject.transform.rotation=marker.rotation;
		}
	
		if(interrainTag){
			interrainTag.PushToAllElements(newObject);
		}
	}
	
	
	private void addOneMoneyAtMarker(Transform marker,Transform inparent,TerrainTag interrainTag){
		GameObject newObject;
		newObject	= moneyElementFactory.GetNewObject();
			
		//set position & rotation
		newObject.transform.position=marker.position;
		
		if(interrainTag){
			interrainTag.PushToAllElements(newObject);
		}
		
		if(!MakeObstacleSet)
		{
			newObject.transform.parent=inparent;
		}
	}
	
	private void addSomeMoneyAtMarker(Transform marker,TerrainTag interrainTag)
	{
		float angletesttransform;
		Vector3 right;
		MoneyMarker moneyMarker;
		
		moneyMarker=marker.GetComponent<MoneyMarker>();
		
		int pathnumber=moneyMarker.pathnumber;
		int kolvo=moneyMarker.numberOfCoins;
		int startPoint=moneyMarker.startPoint;

		GameObject newObject;
		int i;
		interrainTag.SetCustomDotIndex(startPoint,0);
		Vector3 XandYandAngleSmexForz,oldXandYandAngleSmexForz;
		Vector3 angle=Vector3.zero;
		XandYandAngleSmexForz=interrainTag.GetXandYandAngleSmexForZ(new Vector3(0,0,0.1f),true);
		for(i=0;i<kolvo;i++)
		{	
			if(interrainTag.GetflagNextTerrainCustom())
			{
				//Debug.Log ("interrainTag.GetflagNextTerrainCustom()");
				break;
			}
			
			oldXandYandAngleSmexForz=XandYandAngleSmexForz;
			XandYandAngleSmexForz=interrainTag.GetXandYandAngleSmexForZ(new Vector3(0,0,2f),true);
			angletesttransform=GlobalOptions.GetAngleOfRotation(oldXandYandAngleSmexForz,XandYandAngleSmexForz);
			marker.rotation=Quaternion.Euler(0,angletesttransform,0);
			right=marker.TransformDirection(Vector3.right);
			
			
			if(!interrainTag.GetflagNextTerrainCustom())
			{
				newObject = moneyElementFactory.GetNewObject();
				//set position & rotation
				newObject.transform.position=new Vector3(XandYandAngleSmexForz.x,marker.position.y,XandYandAngleSmexForz.z)+right*GlobalOptions.GetPlayerScript().meshPath*pathnumber;
				newObject.transform.rotation=Quaternion.Euler(angle.x,angle.y,angle.z);
				angle.y+=15;
			
		
				if(interrainTag){
					interrainTag.PushToAllElements(newObject);
				}
			}
		}
	}
	
	private IEnumerator AddObjects(bool FlagCoRoutine){	
		
		GameObject newTerrain=null;
		TerrainTag terrainTag=null;
		Vector3 newpos=new Vector3(0,0,0);
		
		Vector3 oldWhereToBuild=GlobalOptions.whereToBuild;
		
		
		if(terrainElementFactory.flagGenerate){
			Vector3 lastAddedEndOfTerrain;
			
			if(terrainElementFactory.GetLastAddedObject())
			{
				lastAddedEndOfTerrain=terrainElementFactory.GetLastAddedObject().GetComponent<TerrainTag>().GetEndOfTerrain();
			}
			else
			{
				lastAddedEndOfTerrain=new Vector3(0,0,0);
			}
			
			newTerrain=AddTerrain();
			terrainTag=newTerrain.GetComponent("TerrainTag") as TerrainTag;
			
			if(terrainTag.isEndOfTerrain())
			{
				newpos=new Vector3(lastAddedEndOfTerrain.x+terrainTag.sizeOfPlane/2*oldWhereToBuild.x,lastAddedEndOfTerrain.y,lastAddedEndOfTerrain.z+terrainTag.sizeOfPlane/2*oldWhereToBuild.z);
			}
			else
			{
				newpos=new Vector3(oldObjectPos.x+terrainTag.sizeOfPlane/2*oldWhereToBuild.x,oldObjectPos.y+lastAddedEndOfTerrain.y,oldObjectPos.z+terrainTag.sizeOfPlane/2*oldWhereToBuild.z);
				oldObjectPos=new Vector3(oldObjectPos.x+terrainTag.sizeOfPlane*oldWhereToBuild.x,oldObjectPos.y+lastAddedEndOfTerrain.y,oldObjectPos.z+terrainTag.sizeOfPlane*oldWhereToBuild.z);
			}
			
			//if rotation
			if(terrainTag.nextGoingTo!=TerrainTagNextGoingTo.FORWARD)
			{
				if(terrainTag.nextGoingTo==TerrainTagNextGoingTo.LEFT)
				{
					GlobalOptions.whereToBuild=GlobalOptions.TurnLeftRightVector(oldWhereToBuild,true);
				}else
				{
					GlobalOptions.whereToBuild=GlobalOptions.TurnLeftRightVector(oldWhereToBuild,false);
				}
			}
			
			
			newTerrain.transform.position=newpos;
			
			//rotate terrain
			GlobalOptions.rotateTransformForWhere(newTerrain.transform,oldWhereToBuild);
				
			if(FlagCoRoutine) yield return null;
		}
		//а надо ли что нибудь ещё добавлять?
		//динамические объекты
		StartCoroutine(addDynamicByMarkers(newTerrain,terrainTag,FlagCoRoutine));	
		if(FlagCoRoutine) yield return null;
		
	}
	
	
	public GameObject AddTerrain()
	{
		GameObject newTerrain=null;
		if(firstTimeInit)
		{
			newTerrain=terrainElementFactory.GetNewObjectWithName(roadTerrainsNames[currentRoadPos]);
			currentRoadPos++;
		}
		else
		{
			newTerrain=terrainElementFactory.GetNewObject();
		}
		return newTerrain;
	}
	
	public override void DeleteOneFirstTerrain()
	{		
		if(terrainElementFactory.flagGenerate){
			terrainElementFactory.DeleteOneFirstTerrain();
		}
	}
	
	//get xsmex
	public Vector3 GetXandYandAngleSmexForZ(Vector3 inposition){
		Vector3 returnXandYandAngle=terrainElementFactory.GetXandYandAngleSmexForZ(inposition);
		//addDotToPathIndicator(new Vector3(returnXandYandAngle.x,returnXandYandAngle.y,inposition.z));
		
		return returnXandYandAngle;
	}
	
	public float GetCurTerrainCenter(){
		float returny=terrainElementFactory.GetCurTerrainCenter();
		
		return returny;
	}
	
	private void addDotToPathIndicator(Vector3 indot)
	{
		if(pathIndicator)
		{
			LineRenderer pathRenderer = pathIndicator.GetComponent<LineRenderer>();
			
			vetexCount++;
			
			pathRenderer.SetVertexCount(vetexCount);
			
			pathRenderer.SetPosition(vetexCount-1, indot);
		}
	}
	
	public override Vector3 GetPrevTerrainPos()
	{
		TerrainTag prev=terrainElementFactory.GetCurrentTerrainForZ().GetComponent<TerrainTag>().GetPrevTerrain();
		
		if(prev)
		{
			return prev.transform.position;
		}
		
		return GetCurTerrainPos();
	}
	
	public override Vector3 GetLastTerrainPos()
	{
		return terrainElementFactory.GetLastObject().transform.position;
	}
	
	
	public override Vector3 GetCurTerrainPos()
	{
		return terrainElementFactory.GetCurrentTerrainForZ().transform.position;
	}
	
	public override float GetTerrainLength()
	{
		return terrainElementFactory.GetCurrentTerrainForZ().GetComponent<TerrainTag>().sizeOfPlane;
	}
	
	public override float GetLastTerrainLength()
	{
		return terrainElementFactory.GetLastObject().GetComponent<TerrainTag>().sizeOfPlane;
	}
	
}


