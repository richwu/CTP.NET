using System;

namespace Algorithm
{
	internal class Q_TO_A
	{
		public char ChangeKeyboard(char r)
		{
			char c = r;
			if (c != ' ')
			{
				switch (c)
				{
				case 'a':
					r = 'q';
					break;
				case 'b':
					r = 'w';
					break;
				case 'c':
					r = 'e';
					break;
				case 'd':
					r = 'r';
					break;
				case 'e':
					r = 't';
					break;
				case 'f':
					r = 'y';
					break;
				case 'g':
					r = 'u';
					break;
				case 'h':
					r = 'i';
					break;
				case 'i':
					r = 'o';
					break;
				case 'j':
					r = 'p';
					break;
				case 'k':
					r = 'a';
					break;
				case 'l':
					r = 's';
					break;
				case 'm':
					r = 'd';
					break;
				case 'n':
					r = 'f';
					break;
				case 'o':
					r = 'g';
					break;
				case 'p':
					r = 'h';
					break;
				case 'q':
					r = 'j';
					break;
				case 'r':
					r = 'k';
					break;
				case 's':
					r = 'l';
					break;
				case 't':
					r = 'z';
					break;
				case 'u':
					r = 'x';
					break;
				case 'v':
					r = 'c';
					break;
				case 'w':
					r = 'v';
					break;
				case 'x':
					r = 'b';
					break;
				case 'y':
					r = 'n';
					break;
				case 'z':
					r = 'm';
					break;
				}
			}
			else
			{
				r = '0';
			}
			return r;
		}

		public char ChangeABC(char r)
		{
			char c = r;
			if (c != '0')
			{
				switch (c)
				{
				case 'a':
					r = 'k';
					break;
				case 'b':
					r = 'x';
					break;
				case 'c':
					r = 'v';
					break;
				case 'd':
					r = 'm';
					break;
				case 'e':
					r = 'c';
					break;
				case 'f':
					r = 'n';
					break;
				case 'g':
					r = 'o';
					break;
				case 'h':
					r = 'p';
					break;
				case 'i':
					r = 'h';
					break;
				case 'j':
					r = 'q';
					break;
				case 'k':
					r = 'r';
					break;
				case 'l':
					r = 's';
					break;
				case 'm':
					r = 'z';
					break;
				case 'n':
					r = 'y';
					break;
				case 'o':
					r = 'i';
					break;
				case 'p':
					r = 'j';
					break;
				case 'q':
					r = 'a';
					break;
				case 'r':
					r = 'd';
					break;
				case 's':
					r = 'l';
					break;
				case 't':
					r = 'e';
					break;
				case 'u':
					r = 'g';
					break;
				case 'v':
					r = 'w';
					break;
				case 'w':
					r = 'b';
					break;
				case 'x':
					r = 'u';
					break;
				case 'y':
					r = 'f';
					break;
				case 'z':
					r = 't';
					break;
				}
			}
			else
			{
				r = ' ';
			}
			return r;
		}
	}
}
