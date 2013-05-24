/************************************************************************************

Filename    :   OVRPlayerControllerEditor.cs
Content     :   Player controller interface. 
				This script adds editor functionality to the OVRPlayerController
Created     :   January 17, 2013
Authors     :   Peter Giokaris

Copyright   :   Copyright 2013 Oculus VR, Inc. All Rights reserved.

Use of this software is subject to the terms of the Oculus LLC license
agreement provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.

************************************************************************************/
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(ChadBallPC))]

//-------------------------------------------------------------------------------------
// ***** OVRPlayerControllerEditor
//
// OVRPlayerControllerEditor adds extra functionality in the inspector for the currently
// selected OVRPlayerController.
//
public class BallPCEditor : Editor
{
	// target component
	private ChadBallPC m_Component;
	
    private SerializedObject m_object;
    private SerializedProperty m_propPosition;

	// OnEnable
	void OnEnable(){
		m_Component = (ChadBallPC) target;
	}

	// OnDestroy
	void OnDestroy(){
	}

	// OnInspectorGUI
	public override void OnInspectorGUI(){
		GUI.color = Color.white;

		//m_Component.RotationAmount 	= EditorGUILayout.Slider("Rotation Amount", m_Component.RotationAmount, 0, 80);	
		m_Component.driftFactor 	= EditorGUILayout.IntSlider("Drift Factor", m_Component.driftFactor, 0, 80);	
		OVREditorGUIUtility_BB.Separator();

		if (GUI.changed){
			EditorUtility.SetDirty(target);
		}
	}		
}

