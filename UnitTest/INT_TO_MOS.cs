using System.Collections.Generic;

namespace UnitTest
{
    public class INT_TO_MOS
    {
        private readonly string[] _array;

        public INT_TO_MOS()
        {
            _array = new []
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
        }

        public string ChangeMOS(List<int> pnOutNumber)
        {
            string text = null;
            foreach (var current in pnOutNumber)
            {
                text += _array[current];
            }
            return text;
        }

        public List<int> ChangeNumber(List<string> psMos)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < psMos.Count; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (_array[j] == psMos[i])
                    {
                        list.Add(j);
                    }
                }
            }
            return list;
        }
    }
}