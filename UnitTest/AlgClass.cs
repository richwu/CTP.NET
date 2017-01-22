using System.Collections.Generic;
using System.Linq;

namespace UnitTest
{
    public class AlgClass
    {
        private Q_TO_A Change = new Q_TO_A();

        private NumberSelect NumSel = new NumberSelect();

        private INT_TO_MOS ToMos = new INT_TO_MOS();

        public string Public_Change_Psw(string sMessage)
        {
            List<char> list = sMessage.ToList<char>();
            List<int> list2 = new List<int>();
            for (int i = 0; i < list.Count; i++)
            {
                list[i] = this.Change.ChangeKeyboard(list[i]);
            }
            foreach (char current in list)
            {
                list2.Add(this.NumSel.Range(current));
            }
            foreach (char current in list)
            {
                list2.Add(this.NumSel.Number(current));
            }
            return this.ToMos.ChangeMOS(list2);
        }

        public string Psw_Change_Public(string sMessage)
        {
            List<string> list = new List<string>();
            string text = null;
            char[] array = sMessage.ToCharArray();
            int num = 0;
            char[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                char c = array2[i];
                text += c;
                num++;
                if (num == 5)
                {
                    list.Add(text);
                    num = 0;
                    text = null;
                }
            }
            List<int> pnMessage = this.ToMos.ChangeNumber(list);
            List<char> list2 = new List<char>();
            foreach (char c in this.NumSel.ChangeChar(pnMessage))
            {
                list2.Add(this.Change.ChangeABC(c));
            }
            string text2 = null;
            foreach (char c in list2)
            {
                text2 += c.ToString();
            }
            return text2;
        }
    }
}