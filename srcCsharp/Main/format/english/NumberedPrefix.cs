/*
 * Ported to C# by Gert-Jan de Vries
 */
 
using System;

namespace SimpleNLG.Main.format.english
{

    /**
     * This class keeps track of the prefix for numbered lists.
     */
    public class NumberedPrefix
	{
		internal string prefix;

		public NumberedPrefix()
		{
			prefix = "0";
		}

		public virtual void increment()
		{
			int dotPosition = prefix.LastIndexOf('.');
			if (dotPosition == -1)
			{
				int counter = Convert.ToInt32(prefix);
				counter++;
				prefix = counter.ToString();

			}
			else
			{
				string subCounterStr = prefix.Substring(dotPosition + 1);
				int subCounter = Convert.ToInt32(subCounterStr);
				subCounter++;
				prefix = prefix.Substring(0, dotPosition) + "." + subCounter.ToString();
			}
		}

        /**
         * This method starts a new level to the prefix (e.g., 1.1 if the current is 1, 2.3.1 if current is 2.3, or 1 if the current is 0).
         */
		public virtual void upALevel()
		{
			if (prefix.Equals("0"))
			{
				prefix = "1";
			}
			else
			{
				prefix = prefix + ".1";
			}
		}

        /**
         * This method removes a level from the prefix (e.g., 0 if current is a plain number, say, 7, or 2.4, if current is 2.4.1).
         */
	    public virtual void downALevel()
		{
			int dotPosition = prefix.LastIndexOf('.');
			if (dotPosition == -1)
			{
				prefix = "0";
			}
			else
			{
				prefix = prefix.Substring(0, dotPosition);
			}
		}

		public virtual string Prefix
		{
			get
			{
				return prefix;
			}
			set
			{
				prefix = value;
			}
		}

	}

}