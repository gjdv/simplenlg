/**
 * This class is used to parse numbers that are passed as figures, to determine
 * whether they should take "a" or "an" as determiner.
 * 
 * @author bertugatt
 *
 * Ported to C# by Gert-Jan de Vries
 */


using System;
using System.Text;
using System.Text.RegularExpressions;

namespace SimpleNLG.Main.morphology.english
{


	public class DeterminerAgrHelper
	{

	    /*
	     * An array of strings which are exceptions to the rule that "an" comes
	     * before vowels
	     */
		private static readonly string[] AN_EXCEPTIONS = new string[] {"one", "180", "110"};

	    /*
	     * Start of string involving vowels, for use of "an"
	     */
		private const string AN_AGREEMENT = "\\A(a|e|i|o|u).*";

	    /*
	     * Start of string involving numbers, for use of "an" -- courtesy of Chris
	     * Howell, Agfa healthcare corporation
	     */
	    // private static final String AN_NUMERAL_AGREEMENT =
	    // "^(((8((\\d+)|(\\d+(\\.|,)\\d+))?).*)|((11|18)(\\d{3,}|\\D)).*)$";

	    /**
	     * Check whether this string starts with a number that needs "an" (e.g.
	     * "an 18% increase")
	     * 
	     * @param string
	     *            the string
	     * @return <code>true</code> if this string starts with 11, 18, or 8,
	     *         excluding strings that start with 180 or 110
	     */
		public static bool requiresAn(string @string)
		{
			bool req = false;

			string lowercaseInput = @string.ToLower();

			if (Regex.IsMatch(lowercaseInput, "^"+AN_AGREEMENT+"$") && !isAnException(lowercaseInput))
			{
				req = true;

			}
			else
			{
				string numPref = getNumericPrefix(lowercaseInput);

				if (!ReferenceEquals(numPref, null) && numPref.Length > 0 && Regex.IsMatch(numPref, "^(8|11|18).*$"))
				{
					int? num = int.Parse(numPref);
					req = checkNum(num.Value);
				}
			}

			return req;
		}

	    /*
	     * check whether a string beginning with a vowel is an exception and doesn't
	     * take "an" (e.g. "a one percent change")
	     * 
	     * @return
	     */
		private static bool isAnException(string str)
		{
			foreach (string ex in AN_EXCEPTIONS)
			{
				if (Regex.IsMatch(str,"^" + ex + ".*"))
				{
				    // if (string.equalsIgnoreCase(ex)) {
					return true;
				}
			}

			return false;
		}

	    /*
	     * Returns <code>true</code> if the number starts with 8, 11 or 18 and is
	     * either less than 100 or greater than 1000, but excluding 180,000 etc.
	     */
		private static bool checkNum(int num)
		{
			bool needsAn = false;

		    // eight, eleven, eighty and eighteen
			if (num == 11 || num == 18 || num == 8 || (num >= 80 && num < 90))
			{
				needsAn = true;

			}
			else if (num > 1000)
			{
				num = (int) Math.Round(num / 1000.0);
				needsAn = checkNum(num);
			}

			return needsAn;
		}

	    /*
	     * Retrieve the numeral prefix of a string.
	     */
		private static string getNumericPrefix(string @string)
		{
			StringBuilder numeric = new StringBuilder();

			if (!ReferenceEquals(@string, null))
			{
				@string = @string.Trim();

				if (@string.Length > 0)
				{

					StringBuilder buffer = new StringBuilder(@string);
					char first = buffer[0];

					if (char.IsDigit(first))
					{
						numeric.Append(first);

						for (int i = 1; i < buffer.Length; i++)
						{
							char next = buffer[i];

							if (char.IsDigit(next))
							{
								numeric.Append(next);

							// skip commas within numbers
							}
							else if (next.Equals(','))
							{
								continue;

							}
							else
							{
								break;
							}
						}
					}
				}
			}

			return numeric.Length == 0 ? null : numeric.ToString();
		}


	    /**
	     * Check to see if a string ends with the indefinite article "a" and it agrees with {@code np}. 
	     * @param text
	     * @param np
	     * @return an altered version of {@code text} to use "an" if it agrees with {@code np}, the original string otherwise.
	     */
	    public static string checkEndsWithIndefiniteArticle(string text, string np)
		{

			string[] tokens = text.Split(' ');

			string lastToken = tokens[tokens.Length - 1];

			if (lastToken.Equals("a", StringComparison.OrdinalIgnoreCase) && requiresAn(np))
			{

				tokens[tokens.Length - 1] = "an";

				return stringArrayToString(tokens);

			}

			return text;

		}

	    // Turns ["a","b","c"] into "a b c"
		private static string stringArrayToString(string[] sArray)
		{

			StringBuilder buf = new StringBuilder();

			for (int i = 0; i < sArray.Length; i++)
			{

				buf.Append(sArray[i]);

				if (i != sArray.Length - 1)
				{

					buf.Append(" ");

				}

			}

			return buf.ToString();

		}

	}

}