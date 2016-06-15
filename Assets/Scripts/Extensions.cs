﻿using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
	private static System.Random _random = new System.Random(13);

	public static T GetRandomElement<T>(this List<T> list)
	{
		if (list.Count <= 0)
		{
			return default(T);
		}


		return list[_random.Next(0, list.Count)];
	}

	public static void SnapTo(this GameObject self, GameObject staticTarget)
	{
		var newModule = self.transform.parent.parent;

		var forwardVectorToMatch = -staticTarget.transform.forward;
		var correctiveRotation = Azimuth(forwardVectorToMatch) - Azimuth(self.transform.forward);
		newModule.RotateAround(self.transform.position, Vector3.up, correctiveRotation);
		var correctiveTranslation = staticTarget.transform.position - self.transform.position;
		newModule.transform.position += correctiveTranslation;
	}

	private static float Azimuth(Vector3 vector)
	{
		return Vector3.Angle(Vector3.forward, vector) * Mathf.Sign(vector.x);
	}
}