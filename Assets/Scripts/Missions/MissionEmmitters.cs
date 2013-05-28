using UnityEngine;
using System.Collections;

public class MissionEmmitters : Abstract, IMissionEmmitter, IMissionNotify
{
	public BaseMissionEmmitter[] missionEmmitters;
	
	void Awake ()
	{
		//PlayerPrefs.DeleteAll();Debug.LogWarning("PlayerPrefs.DeleteAll()");//TODO: delete this
	}
	
	public void LevelBegin ()
	{
		for(int i=0;i<missionEmmitters.Length;i++){
			missionEmmitters[i].LevelBegin();
		}
	}	
	
	public void RestartActiveMissions ()
	{
		for(int i=0;i<missionEmmitters.Length;i++){
			missionEmmitters[i].RestartActiveMissions();
		}
	}
	
	public void NotifyCoinsCollected(int coins){
		for(int i=0;i<missionEmmitters.Length;i++){
			missionEmmitters[i].NotifyCoinsCollected(coins);
		}
	}
	
	public void NotifyPostCollected (int post)
	{
		for(int i=0;i<missionEmmitters.Length;i++){
			missionEmmitters[i].NotifyPostCollected(post);
		}
	}
	
	public void NotifyVodkaCollected (int vodka)
	{
		for(int i=0;i<missionEmmitters.Length;i++){
			missionEmmitters[i].NotifyVodkaCollected(vodka);
		}
	}
	
	public void NotifyMagnitCollected (int magnit)
	{
		for(int i=0;i<missionEmmitters.Length;i++){
			missionEmmitters[i].NotifyMagnitCollected(magnit);
		}
	}
	
	public void NotifyX2Collected (int x2)
	{
		for(int i=0;i<missionEmmitters.Length;i++){
			missionEmmitters[i].NotifyX2Collected(x2);
		}
	}
	
	public void NotifySenoDeath (int senoDeath)
	{
		for(int i=0;i<missionEmmitters.Length;i++){
			missionEmmitters[i].NotifySenoDeath(senoDeath);
		}
	}
	
	public void NotifyTraktorDeath (int traktorDeath)
	{
		for(int i=0;i<missionEmmitters.Length;i++){
			missionEmmitters[i].NotifyTraktorDeath(traktorDeath);
		}
	}
	
	public ArrayList GetCurrentMissions(){
		ArrayList currentMissions = new ArrayList();
		for(int i=0;i<missionEmmitters.Length;i++){
			currentMissions.AddRange(missionEmmitters[i].GetCurrentMissions());
		}
		return currentMissions;
	}
	
	public ArrayList GetThisLifeFinishedMissions ()
	{
		ArrayList thisLifeFinishedMissions = new ArrayList();
		for(int i=0;i<missionEmmitters.Length;i++){
			thisLifeFinishedMissions.AddRange(missionEmmitters[i].GetThisLifeFinishedMissions());
		}
		return thisLifeFinishedMissions;
	}
	
	public void NotifyScarecrowDeath (int scarecrowDeath)
	{
		for(int i=0;i<missionEmmitters.Length;i++){
			missionEmmitters[i].NotifyScarecrowDeath(scarecrowDeath);
		}
	}
	
	//run
	public void NotifyMetersRunned(int meter){
		for(int i=0;i<missionEmmitters.Length;i++){
			missionEmmitters[i].NotifyMetersRunned(meter);
		}
	}
	
	public void NotifySlideUnderSomething (int something)
	{
		for(int i=0;i<missionEmmitters.Length;i++){
			missionEmmitters[i].NotifySlideUnderSomething(something);
		}
	}
	
	public void NotifyJump (int jump)
	{
		for(int i=0;i<missionEmmitters.Length;i++){
			missionEmmitters[i].NotifyJump(jump);
		}
	}
	
	public void NotifyPointsAdded (int points)
	{
		for(int i=0;i<missionEmmitters.Length;i++){
			missionEmmitters[i].NotifyPointsAdded(points);
		}
	}
	
	public void NotifyJumpOverCaw (int caws)
	{
		for(int i=0;i<missionEmmitters.Length;i++){
			missionEmmitters[i].NotifyJumpOverCaw(caws);
		}
	}
	
	public void NotifyJumpOverDrova (int drova)
	{
		for(int i=0;i<missionEmmitters.Length;i++){
			missionEmmitters[i].NotifyJumpOverDrova(drova);
		}
	}
	
	public void NotifySlideUnderRope (int rope)
	{
		for(int i=0;i<missionEmmitters.Length;i++){
			missionEmmitters[i].NotifySlideUnderRope(rope);
		}
	}
	
	public void NotifyJumpOverHaystack (int haystack)
	{
		for(int i=0;i<missionEmmitters.Length;i++){
			missionEmmitters[i].NotifyJumpOverHaystack(haystack);
		}
	}
	
	public void NotifyDodgeBaran (int baran)
	{
		for(int i=0;i<missionEmmitters.Length;i++){
			missionEmmitters[i].NotifyDodgeBaran(baran);
		}
	}
	
	public void NotifyDodgeTractor (int tractor)
	{
		for(int i=0;i<missionEmmitters.Length;i++){
			missionEmmitters[i].NotifyDodgeTractor(tractor);
		}
	}
	
	public int GetCountFinishedMissions ()
	{
		int finishedMissionsNumber = 0;
		for(int i=0;i<missionEmmitters.Length;i++){
			finishedMissionsNumber+=missionEmmitters[i].GetCountFinishedMissions();
		}
		return finishedMissionsNumber;
	}
	
	public int GetCountMissions ()
	{
		int missionsNumber = 0;
		for(int i=0;i<missionEmmitters.Length;i++){
			missionsNumber+=missionEmmitters[i].GetCountMissions();
		}
		return missionsNumber;
	}
	
	public float GetProgress ()
	{
		int missionsNumber = 0;
		int finishedMissionsNumber = 0;
		for(int i=0;i<missionEmmitters.Length;i++){
			missionsNumber+=missionEmmitters[i].GetCountMissions();
			finishedMissionsNumber+=missionEmmitters[i].GetCountFinishedMissions();
		}
		return finishedMissionsNumber/(float)missionsNumber;
	}
}
