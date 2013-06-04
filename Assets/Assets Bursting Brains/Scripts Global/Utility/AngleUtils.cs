using UnityEngine;
using System.Collections;

public static class AngleUtils {
	
	public static float GetSignedAngle(Vector3 fwd, Vector3 targetDir, Vector3 up){
		float unsignedAngle = Vector3.Angle(fwd, targetDir);
		
		float signedAngle = unsignedAngle * AngleDir(fwd, targetDir, up);
		
		return signedAngle;
	}
	
	public static float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up){
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0.0f) {
            return 1.0f;

        } else if (dir < 0.0f) {
            return -1.0f;

        } else {
            return 0.0f;
        }

    }
}
