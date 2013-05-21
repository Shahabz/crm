using UnityEngine;
using System.Collections;

public interface IMissionNotify{
	//run
	void NotifyMetersRunned (int meters);
	void NotifyJumpOverCaw (int caws);
	void NotifyJumpOverDrova (int drova);
	void NotifyJumpOverHaystack (int haystack);
	void NotifySlideUnderRope (int rope);
	void NotifyDodgeBaran (int baran);
	void NotifyDodgeTractor (int tractor);
	//collect
	void NotifyCoinsCollected(int coins);
	void NotifyPostCollected(int post);
	void NotifyVodkaCollected(int vodka);
	void NotifyMagnitCollected(int magnit);
	void NotifyX2Collected(int x2);
	void NotifySenoDeath(int senoDeath);
	void NotifyTraktorDeath(int traktorDeath);
}