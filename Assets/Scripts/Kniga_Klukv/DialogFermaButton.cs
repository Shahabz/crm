using UnityEngine;
using System.Collections;

public class DialogFermaButton : GuiButtonBase {
	public DialogFerma dialogFerma;
	override protected void MakeOnTouch(){
		dialogFerma.factory.Buy();
	}
}