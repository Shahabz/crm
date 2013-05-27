using UnityEngine;
using System.Collections;


public class MonetaEnemy : AbstractEnemy {	
	
	private bool effectMade=false;
	public int numberOfMoney=1;
	private float rasstChuvstv=155;
	private Transform parentTransform;
	
	private bool flagPlusPlayerSpeed=false;
	
	//private Transform oldParent;
	
	public override void OnHit(Collider other)
	{
		GuiLayer.AddMoney(numberOfMoney);
		PlayClipSound();
		MakeInactive();
		effectMade=false;
	}
	
	void Update () {
		Rotate();
		TestPlayer();
		
	}
	
	void LateUpdate()
	{
		if(GlobalOptions.gameState==GameStates.PAUSE_MENU||GlobalOptions.gameState==GameStates.GAME_OVER)
		{
			return;
		}
		MakeMagnit();
	}
	
	public void Rotate()
	{
		if(GlobalOptions.gameState==GameStates.PAUSE_MENU)
		{
			return;
		}
		singleTransform.Rotate(new Vector3(0,Time.deltaTime*200,0));
	}
	
	public override void initEnemy()
	{
		parentTransform=singleTransform.parent.parent;
	}
	
	public override void ReStart()
	{
		UnMakeEffect();
		gameObject.SetActiveRecursively(true);	
		singleTransform.rotation=Quaternion.Euler(0, 0, 0);
	}
	
	public void TestPlayer()
	{
		if(!effectMade&&playerScript.GetMagnitFlag())
		{
			float raznx,razny,raznz;
			raznx=parentTransform.position.x-walkingBearTransform.position.x;
			raznz=parentTransform.position.z-walkingBearTransform.position.z;
			razny=parentTransform.position.y-walkingBearTransform.position.y;
			if(raznx*raznx+raznz*raznz<=rasstChuvstv&&Mathf.Abs (razny)<20)
			{
				MakeEffect();
			}
		}
	}
	
	private void MakeMagnit()
	{
		if(effectMade)
		{
			float raznx,raznz,razny,vspz;
			float smex=0.12f;
			raznx=-parentTransform.position.x+walkingBearTransform.position.x;
			razny=-parentTransform.position.y+walkingBearTransform.position.y+1;
			raznz=-parentTransform.position.z+walkingBearTransform.position.z;
	
			if(Mathf.Abs(raznx)>smex)
			{
				raznx=Mathf.Sign(raznx)*smex;
			}
			if(Mathf.Abs(razny)>smex)
			{
				razny=Mathf.Sign(razny)*smex;
			}
			
			if(raznz>smex||flagPlusPlayerSpeed)
			{
				flagPlusPlayerSpeed=true;
				vspz=playerScript.GetRealVelocity();
			}
			else
			{
				vspz=0;
			}
				
			if(Mathf.Abs(raznz)>smex)
			{
				
				raznz=Mathf.Sign(raznz)*smex;
			}
			
			raznz+=vspz;
			
			parentTransform.position+=new Vector3(raznx,razny,raznz);	
		}
	}
	
	private void MakeEffect()
	{
		effectMade=true;
	}
	
	
	private void UnMakeEffect()
	{
		effectMade=false;
		flagPlusPlayerSpeed=false;
	}
}