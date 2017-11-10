using UnityEngine;					// For Unity classes

// Holds generic static functions
public static class Functions {

	public const int POWER_MULT = 3;		// Multiplier to calculate force applied to ball


/// -----------------------------------------------------------------------------------------------
/// Game Specific ---------------------------------------------------------------------------------

	// Gets velocity to be added based on current aim
	public static Vector2 GetVelocity(float power, float angle) {
		 float angleRad = Mathf.PI * angle / 180f;
		return new Vector2(Mathf.Cos(angleRad),Mathf.Sin(angleRad)) * power * POWER_MULT;
	}

/// -----------------------------------------------------------------------------------------------
/// General Methods -------------------------------------------------------------------------------

	// Used to covert a hex string into a Unity Color
	public static Color HexToColor(string hex) {
		hex = hex.Replace ("0x", ""); 	// In case the string is formatted 0xFFFFFF
		hex = hex.Replace ("#", ""); 	// In case the string is formatted #FFFFFF
		byte a = 255;					// Assume fully visible unless specified in hex
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);

		// Only use alpha if the string has enough characters
		if(hex.Length == 8){
			a = byte.Parse(hex.Substring(6,2), System.Globalization.NumberStyles.HexNumber);
		}
		
		return new Color32(r,g,b,a);
	}

	// Provides an easy way of updating a single piece of a color
	// Returns Color of object after setting a specific piece of the color
	public static Color UpdateColor(Color orig, float r = -1f, float g = -1f, float b = -1f, float a = -1f) {
		byte newR = (byte)(255f * ((r == -1f)? orig.r : r));
		byte newG = (byte)(255f * ((g == -1f)? orig.g : g));
		byte newB = (byte)(255f * ((b == -1f)? orig.b : b));
		byte newA = (byte)(255f * ((a == -1f)? orig.a : a));

		return new Color32(newR,newG,newB,newA);
	}
	
}
