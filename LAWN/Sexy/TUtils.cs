using Microsoft.Xna.Framework.Input;
using System;

namespace Sexy
{
    class TUtils
    {
		/// <summary>
		/// Converts a key to a char.
		/// </summary>
		/// <param name="Key">They key to convert.</param>
		/// <param name="Shift">Whether or not shift is pressed.</param>
		/// <returns>The key in a char.</returns>
		public static Char KeyToChar(Keys Key, bool Shift = false)
		{
			/* It's the space key. */
			if (Key == Keys.Space)
			{
				return ' ';
			}
			else
			{
				string String = Key.ToString();

				/* It's a letter. */
				if (String.Length == 1)
				{
					Char Character = Char.Parse(String);
					byte Byte = Convert.ToByte(Character);

					if (
						(Byte >= 65 && Byte <= 90) ||
						(Byte >= 97 && Byte <= 122)
						)
					{
						return (!Shift ? Character.ToString().ToLower() : Character.ToString())[0];
					}
				}

				/* 
				 * 
				 * The only issue is, if it's a symbol, how do I know which one to take if the user isn't using United States international?
				 * Anyways, thank you, for saving my time
				 * down here:
				 */

				#region Credits :  http://roy-t.nl/2010/02/11/code-snippet-converting-keyboard-input-to-text-in-xna.html for saving my time.
				switch (Key)
				{
					case Keys.D0:
						if (Shift) { return ')'; } else { return '0'; }
					case Keys.D1:
						if (Shift) { return '!'; } else { return '1'; }
					case Keys.D2:
						if (Shift) { return '@'; } else { return '2'; }
					case Keys.D3:
						if (Shift) { return '#'; } else { return '3'; }
					case Keys.D4:
						if (Shift) { return '$'; } else { return '4'; }
					case Keys.D5:
						if (Shift) { return '%'; } else { return '5'; }
					case Keys.D6:
						if (Shift) { return '^'; } else { return '6'; }
					case Keys.D7:
						if (Shift) { return '&'; } else { return '7'; }
					case Keys.D8:
						if (Shift) { return '*'; } else { return '8'; }
					case Keys.D9:
						if (Shift) { return '('; } else { return '9'; }

					case Keys.NumPad0: return '0';
					case Keys.NumPad1: return '1';
					case Keys.NumPad2: return '2';
					case Keys.NumPad3: return '3';
					case Keys.NumPad4: return '4';
					case Keys.NumPad5: return '5';
					case Keys.NumPad6: return '6';
					case Keys.NumPad7: return '7';
					case Keys.NumPad8: return '8';
					case Keys.NumPad9: return '9';

					case Keys.OemTilde:
						if (Shift) { return '~'; } else { return '`'; }
					case Keys.OemSemicolon:
						if (Shift) { return ':'; } else { return ';'; }
					case Keys.OemQuotes:
						if (Shift) { return '"'; } else { return '\''; }
					case Keys.OemQuestion:
						if (Shift) { return '?'; } else { return '/'; }
					case Keys.OemPlus:
						if (Shift) { return '+'; } else { return '='; }
					case Keys.OemPipe:
						if (Shift) { return '|'; } else { return '\\'; }
					case Keys.OemPeriod:
						if (Shift) { return '>'; } else { return '.'; }
					case Keys.OemOpenBrackets:
						if (Shift) { return '{'; } else { return '['; }
					case Keys.OemCloseBrackets:
						if (Shift) { return '}'; } else { return ']'; }
					case Keys.OemMinus:
						if (Shift) { return '_'; } else { return '-'; }
					case Keys.OemComma:
						if (Shift) { return '<'; } else { return ','; }
				}
				#endregion

				return (Char)0;

			}
		}
	}
}
