using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(Oculus_MovementPC))]

//-------------------------------------------------------------------------------------
// ***** OVRPlayerControllerEditor
//
// OVRPlayerControllerEditor adds extra functionality in the inspector for the currently
// selected OVRPlayerController.
//
public class EtherealEditor : Editor
{
	// target component
	private Oculus_MovementPC m_Component;
	private Oculus_MovementPC min;

	// foldouts
	private static bool m_MotorFoldout;
	private static bool m_PhysicsFoldout;

	// OnEnable
	void OnEnable()
	{
		m_Component = (Oculus_MovementPC)target;
	}

	// OnDestroy
	void OnDestroy()
	{
	}

	// OnInspectorGUI
	public override void OnInspectorGUI()
	{
		GUI.color = Color.white;

		{
			m_MotorFoldout = EditorGUILayout.Foldout(m_MotorFoldout, "Motor");

			if (m_MotorFoldout)
			{
				m_Component.turnSensitivity 	= EditorGUILayout.Slider("Turn Sensitivity", 	m_Component.turnSensitivity, Oculus_MovementPC.minTurnSens, Oculus_MovementPC.maxTurnSens);
				m_Component.acceleration 		= EditorGUILayout.Slider("Acceleration", 		m_Component.acceleration, Oculus_MovementPC.minAccel, Oculus_MovementPC.maxAccel);
				m_Component.jumpPower 		= EditorGUILayout.Slider("Jump Power", 		m_Component.jumpPower, Oculus_MovementPC.minJumpPow, Oculus_MovementPC.maxJumpPow);
				m_Component.maxSpeed 		= EditorGUILayout.Slider("Max Speed", 		m_Component.maxSpeed, Oculus_MovementPC.minMaxSpd, Oculus_MovementPC.topMaxSpd);
				m_Component.brakeSpeed 	= EditorGUILayout.Slider("Brake Speed", m_Component.brakeSpeed, Oculus_MovementPC.minBrakeSpd, Oculus_MovementPC.maxBrakeSpd);
				
				
				OVREditorGUIUtility.Separator();
			}
			
			/*
			m_PhysicsFoldout = EditorGUILayout.Foldout(m_PhysicsFoldout, "Physics");
			
			if (m_PhysicsFoldout)
			{
				m_Component.GravityModifier = EditorGUILayout.Slider("Gravity Modifier", m_Component.GravityModifier, 0, 1);

				OVREditorGUIUtility.Separator();
			}
			*/
		}

		if (GUI.changed)
		{
			EditorUtility.SetDirty(target);
		}
	}		
}
