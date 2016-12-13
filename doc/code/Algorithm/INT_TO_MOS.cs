using System;
using System.Collections.Generic;

namespace Algorithm
{
	internal class INT_TO_MOS
	{
		public string ChangeMOS(List<int> pnOutNumber)
		{
			string[] array = new string[]
			{
				"-----",
				"·----",
				"··---",
				"···--",
				"····-",
				"·····",
				"-····",
				"--···",
				"---··",
				"----·"
			};
			string text = null;
			foreach (int current in pnOutNumber)
			{
				text += array[current];
			}
			return text;
		}

		public List<int> ChangeNumber(List<string> psMos)
		{
			List<int> list = new List<int>();
			string[] array = new string[]
			{
				"-----",
				"·----",
				"··---",
				"···--",
				"····-",
				"·····",
				"-····",
				"--···",
				"---··",
				"----·"
			};
			for (int i = 0; i < psMos.Count; i++)
			{
				for (int j = 0; j < 10; j++)
				{
					if (array[j] == psMos[i])
					{
						list.Add(j);
					}
				}
			}
			return list;
		}
	}
}
