using UnityEngine;
using System.Collections;

public class DebugData {
	
	public EtherealPC etherealPC;
	
	public delegate float GetValueDelagate();
	public delegate void IncreaseDelagate();
	public delegate void DecreaseDelagate();
	
	private ArrayList keys = new ArrayList();
	private ArrayList getValueDelegates = new ArrayList();
	private ArrayList decreaseDelegates = new ArrayList();
	private ArrayList increaseDelegates = new ArrayList();
	
	public DebugData(){
	}
	
	public DebugData(EtherealPC etherealPC){
		this.etherealPC = etherealPC;
	}
	
	public void AddData(string key, GetValueDelagate getValueDel, IncreaseDelagate increaseDel, DecreaseDelagate decreaseDel){
		keys.Add(key);
		getValueDelegates.Add(getValueDel);
		increaseDelegates.Add(increaseDel);
		decreaseDelegates.Add(decreaseDel);
	}
	
	public int GetSize(){
		return keys.Count;	
	}
	
	public string GetKey(int index){
		return (string) keys[index];	
	}
	
	public void DebugDecrease(int index){
		((DecreaseDelagate) decreaseDelegates[index])();
	}
	
	public void DebugIncrease(int index){
		((IncreaseDelagate) increaseDelegates[index])();
	}
	
	
	public string GetValue(int index){
		return ((GetValueDelagate) getValueDelegates[index])().ToString("F2");
	}
	
}
