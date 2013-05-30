using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(EtherealPC))]

//-------------------------------------------------------------------------------------
// ***** OVRPlayerControllerEditor
//
// OVRPlayerControllerEditor adds extra functionality in the inspector for the currently
// selected OVRPlayerController.
//
public class EtherealPCEditor : Editor {
	// target component
	private EtherealPC m_Component;

	// foldouts
	private static bool m_MotorFoldout;
	private static bool m_PhysicsFoldout;

	// OnEnable
	void OnEnable() {
		m_Component = (EtherealPC) target;
	}

	// OnDestroy
	void OnDestroy() {
	}

	// OnInspectorGUI
	public override void OnInspectorGUI() {
		GUI.color = Color.white;

		{			
			m_Component.turnSensitivity 	= EditorGUILayout.Slider("Turn Sensitivity", m_Component.turnSensitivity, EtherealPC.MIN_TURN_SENS, EtherealPC.MAX_TURN_SENS);
			m_Component.acceleration 		= EditorGUILayout.Slider("Acceleration", m_Component.acceleration, EtherealPC.MIN_ACCEL, EtherealPC.MAX_ACCEL);
			m_Component.jumpPower 			= EditorGUILayout.Slider("Jump Power", m_Component.jumpPower, EtherealPC.MIN_JUMP_POW, EtherealPC.MAX_JUMP_POW);
			m_Component.maxSpeed 			= EditorGUILayout.Slider("Max Speed", m_Component.maxSpeed, EtherealPC.MIN_TOP_SPEED, EtherealPC.MAX_TOP_SPEED);
			m_Component.brakeSpeed 			= EditorGUILayout.Slider("Brake Speed", m_Component.brakeSpeed, EtherealPC.MIN_BRAKE_SPEED, EtherealPC.MAX_BRAKE_SPEED);		
				
			OVREditorGUIUtility.Separator();
		}

		if (GUI.changed)
		{
			EditorUtility.SetDirty(target);
		}
	}		
}
