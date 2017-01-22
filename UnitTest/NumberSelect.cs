using System;
using System.Collections.Generic;

namespace UnitTest
{
    public class NumberSelect
    {
        public int Number(char r)
        {
            int result;
            if (r == 'a' || r == 'd' || r == 'g' || r == 'j' || r == 'm' || r == 'p' || r == 't' || r == 'w')
            {
                result = 1;
            }
            else if (r == 'b' || r == 'e' || r == 'h' || r == 'k' || r == 'n' || r == 'q' || r == 'u' || r == 'x')
            {
                result = 2;
            }
            else if (r == 'c' || r == 'f' || r == 'i' || r == 'l' || r == 'o' || r == 'r' || r == 'v' || r == 'y')
            {
                result = 3;
            }
            else if (r == 's' || r == 'z')
            {
                result = 4;
            }
            else if (r == ' ')
            {
                result = 0;
            }
            else if (r >= '0' && r <= '9')
            {
                result = 6;
            }
            else
            {
                result = 5;
            }
            return result;
        }

        public int Range(char r)
        {
            char[,] array = new char[,]
            {
                {
                    'a',
                    'b',
                    'c',
                    ' '
                },
                {
                    'd',
                    'e',
                    'f',
                    ' '
                },
                {
                    'g',
                    'h',
                    'i',
                    ' '
                },
                {
                    'j',
                    'k',
                    'l',
                    ' '
                },
                {
                    'm',
                    'n',
                    'o',
                    ' '
                },
                {
                    'p',
                    'q',
                    'r',
                    's'
                },
                {
                    't',
                    'u',
                    'v',
                    ' '
                },
                {
                    'w',
                    'x',
                    'y',
                    'z'
                }
            };
            int result;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (r == array[i, j])
                    {
                        result = i + 2;
                        return result;
                    }
                }
            }
            if (r == ' ')
            {
                result = 0;
                return result;
            }
            if (Convert.ToInt32(r) >= 48 && Convert.ToInt32(r) <= 57)
            {
                result = Convert.ToInt32(r) - 48;
                return result;
            }
            result = 1;
            return result;
        }

        public List<char> ChangeChar(List<int> pnMessage)
        {
            char[,] array = new char[,]
            {
                {
                    'a',
                    'b',
                    'c',
                    ' '
                },
                {
                    'd',
                    'e',
                    'f',
                    ' '
                },
                {
                    'g',
                    'h',
                    'i',
                    ' '
                },
                {
                    'j',
                    'k',
                    'l',
                    ' '
                },
                {
                    'm',
                    'n',
                    'o',
                    ' '
                },
                {
                    'p',
                    'q',
                    'r',
                    's'
                },
                {
                    't',
                    'u',
                    'v',
                    ' '
                },
                {
                    'w',
                    'x',
                    'y',
                    'z'
                }
            };
            List<int> list = new List<int>();
            List<int> list2 = new List<int>();
            List<char> list3 = new List<char>();
            for (int i = 0; i < pnMessage.Count / 2; i++)
            {
                list.Add(pnMessage[i]);
                list2.Add(pnMessage[pnMessage.Count / 2 + i]);
            }
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] >= 2 && list[i] <= 9 && list2[i] != 6)
                {
                    list3.Add(array[list[i] - 2, list2[i] - 1]);
                }
                if (list[i] == 0 && list2[i] == 0)
                {
                    list3.Add(' ');
                }
                if (list2[i] == 6)
                {
                    list3.Add(Convert.ToChar(list[i] + 48));
                }
                if (list[i] == 1 && list2[i] == 5)
                {
                    list3.Add(',');
                }
            }
            return list3;
        }
    }
}