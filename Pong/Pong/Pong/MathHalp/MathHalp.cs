using System;
using System.Globalization;
using Pong.Logging;

namespace Pong.MathHalp
{
	public static class MathHalp
	{
		public static double PythagoreanTheorem(float a, float b)
		{
			return Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
		}

		public static double ReturnSideWithKnownHypotenuse(float hypotenuse, float side)
		{
			var returnVal = Math.Sqrt(Math.Pow(hypotenuse, 2) - Math.Pow(side, 2));

			if (double.IsNaN(returnVal))
			{
				Logger.Log(string.Format(CultureInfo.InvariantCulture, "Imaginary number resulted from input. Hypotenuse {0} and side {1}", hypotenuse, side));
			}
			return returnVal;
		}
	}
}
