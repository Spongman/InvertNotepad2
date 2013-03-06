using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace InvertNotepad2
{
	/*
	 * Inverts the brightness of colors in a notepad2 theme .ini file
	 * 
	 * requirements:
	 *		.NET framework v2.0 (
	 * 
	 * build:
	 *		csc.exe InvertNodepad2.cs
	 * 
	 */

	class InvertNotepad2
	{
		static void Main (string [] args)
		{
			if (args.Length == 0)
			{
				Invert (Console.In, Console.Out);
			}
			else if (args.Length == 1)
			{
				string strTemp = Path.GetTempFileName ();
				using (var input = new StreamReader (args [0], true))
				using (var output = new StreamWriter (strTemp, false))
					Invert (input, output);

				File.Move (strTemp, args [0]);
			}
			else if (args.Length == 2)
			{
				using (var input = new StreamReader (args [0], true))
				using (var output = new StreamWriter (args [1], false))
					Invert (input, output);
			}
			else
			{
				Console.Error.WriteLine ("InvertNotepad2 [ <input path> [ <output path> ] ]");
			}
		}

		static Regex _reColor = new Regex ("(fore|back):(#[0-9a-f]{6})", RegexOptions.Compiled | RegexOptions.IgnoreCase);

		static void Invert (TextReader input, TextWriter output)
		{
			string strLine;
			while ((strLine = input.ReadLine ()) != null)
				output.WriteLine (_reColor.Replace (strLine, InvertColor));
		}

		static string InvertColor (Match m)
		{
			Color color = ColorTranslator.FromHtml (m.Groups [2].Value);
			Color inverted = FromHSLA (
				color.GetHue () / 360.0,
				color.GetSaturation (),
				1.0 - color.GetBrightness (),
				color.A / 255.0
			);

			return string.Format ("{0}:#{1:X6}", m.Groups [1].Value, inverted.ToArgb () & 0xffffff);
		}


		public static Color FromHSLA (double H, double S, double L, double A)
		{
			if (A > 1.0)
				A = 1.0;

			double r = L;   // default to gray
			double g = L;
			double b = L;
			double v = L + (L <= 0.5 ? (L * S) : (S - L * S));

			if (v > 0)
			{
				H *= 6.0;

				double m = L + L - v;
				int sextant = (int) H;
				double fract1 = H - sextant;
				double fract2 = 1 - fract1;

				double mid = ((sextant & 1) == 0) ? fract2 * v + fract1 * m : fract1 * v + fract2 * m;

				switch (sextant)
				{
					case 0:
						r = v;
						g = mid;
						b = m;
						break;
					case 1:
						r = mid;
						g = v;
						b = m;
						break;
					case 2:
						r = m;
						g = v;
						b = mid;
						break;
					case 3:
						r = m;
						g = mid;
						b = v;
						break;
					case 4:
						r = mid;
						g = m;
						b = v;
						break;
					case 5:
						r = v;
						g = m;
						b = mid;
						break;
				}
			}
			return Color.FromArgb ((int) (255 * A), (int) (r * 255), (int) (g * 255), (int) (b * 255));
		}
	}
}
