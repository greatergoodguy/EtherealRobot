using UnityEngine;
using System.Collections;

public class DebugData {
	
	private ArrayList keys = new ArrayList();
	private ArrayList values = new ArrayList();
	
	public DebugData(){
	}
	
	public void AddData(string key, ref float fValue){
		keys.Add(key);
		values.Add(fValue);
	}
	
	public int GetSize(){
		return keys.Count;	
	}
	
	public string GetKeyAt(int index){
		return (string) keys[index];	
	}
	
	
	public float GetValueAt(int index){
		return (float) values[index];	
	}
	
}
