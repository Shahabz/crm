using UnityEngine;
using System.Collections;


public class WoodEnemy : AbstractEnemy {	
	// Use this for initialization
	
	/*public override void OnHit(Collider other)
	{
		playerScript.StumbleTrigger();
		GuiLayer.AddToLife(-3,singleTransform);
		GuiLayer.AddHeadStars();
		PlayClipSound();
	}*/
	
	
	
	
	public override void OnHit(Collider other)
	{
		GuiLayer.AddToLife(-50, singleTransform);
		GuiLayer.AddHeadStars();
		PlayClipSound();
		if(playerScript.isVodka())
			MakeInactiveParent();
	}
	
	public override void ReStart()
	{
		transform.parent.gameObject.SetActiveRecursively(true);
	}
	
}