using UnityEngine;
using System.Collections;

public class BearAnimation3D : Abstract{
	
	public Vector3 initPos;
	//walking
	public GameObject[] clothes;
	protected ArrayList clothesList=new ArrayList();
	GameObject walkingBear;
	
	private string curAnimationName="walk";
	
	void Start ()
	{
		GameObject newobj;
		Transform curtransform;
		
		//add all clothes to bear walking
		walkingBear=singleTransform.FindChild("WalkingBear").gameObject;
		curtransform=walkingBear.transform;
		curtransform.parent=singleTransform;
		for (int i=0;i<clothes.Length;i++)
		{
			newobj	= Instantiate (clothes[i]) as GameObject;
			newobj.transform.parent=curtransform;
			newobj.transform.Translate(initPos+singleTransform.position);
			clothesList.Add(newobj);
			
			//make other invisible
			if(i!=0){
				newobj.GetComponent<MeshRenderer>().enabled=false;
			}
		}
		
		for(int i=0;i<clothesList.Count;i++)
		{
			(clothesList[i] as GameObject).animation["jump"].layer=1;
			(clothesList[i] as GameObject).animation["left"].layer=1;
			(clothesList[i] as GameObject).animation["right"].layer=1;
			(clothesList[i] as GameObject).animation["down"].layer=0;
			(clothesList[i] as GameObject).animation["stumble"].layer=1;
			
			(clothesList[i] as GameObject).animation["down"].speed=0.3f;
			(clothesList[i] as GameObject).animation["stumble"].speed=1.3f;
			(clothesList[i] as GameObject).animation["jump"].speed=0.4f;
			(clothesList[i] as GameObject).animation["left"].speed=2f;
			(clothesList[i] as GameObject).animation["right"].speed=2f;
			(clothesList[i] as GameObject).animation["death"].speed=0.9f;
			//(clothesList[i] as GameObject).animation["down"].weight=1;
		}
	}
	
	public void Restart(){
		for (int i=1;i<clothesList.Count;i++)
		{
			GameObject cap=clothesList[i] as GameObject;
			cap.GetComponent<MeshRenderer>().enabled=false;
		}
	}
	
	public void ShowCap(){
		
		for (int i=0;i<clothesList.Count;i++)
		{
			GameObject cap=clothesList[i] as GameObject;
			if(cap.GetComponent<MeshRenderer>().enabled==false)
			{
				cap.GetComponent<MeshRenderer>().enabled=true;
				break;
			}
		}
	}
	
	private void PlayAnimationForName(string inAnimationName)
	{
		if(curAnimationName!=inAnimationName)
		{
			curAnimationName=inAnimationName;
			for(int i=0;i<clothesList.Count;i++)
			{
				(clothesList[i] as GameObject).animation.CrossFade(inAnimationName);
			}
		}
	}
	
	public void Walk () {
		PlayAnimationForName("walk");
	}
	
	public void Right () {
		PlayAnimationForName("right");
	}
	
	public void Left () {
		PlayAnimationForName("left");
	}
	
	public void Dead() {
		PlayAnimationForName("death");
	}
	
	public void Idle() {
		PlayAnimationForName("idle");
	}
	
	public void Jump() {
		PlayAnimationForName("jump");
	}
	
	public void Stumble() {
		Debug.Log("Stumble");
		PlayAnimationForName("stumble");
	}
	
	public void Down() {
		PlayAnimationForName("down");
	}
	
	public void SetWalkSpeed(float inspeed) {
		
		for(int i=0;i<clothesList.Count;i++)
		{
			(clothesList[i] as GameObject).animation["walk"].speed=inspeed*1.5f;
		}
	}
}