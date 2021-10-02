using Microsoft.Xna.Framework.Content;
using System;
using System.Text;

namespace Sexy.TodLib
{
	internal class CustomContentReader
	{
		private byte[] buffer;

		private int index;

		private static StringBuilder readStringBuilder = new StringBuilder(30);

		public CustomContentReader(ContentReader reader)
		{
			buffer = reader.ReadBytes((int)reader.BaseStream.Length);
		}

		public int ReadInt32()
		{
			int result = BitConverter.ToInt32(buffer, index);
			index += 4;
			return result;
		}

		public byte ReadByte()
		{
			byte result = buffer[index];
			index++;
			return result;
		}

		public float ReadSingle()
		{
			float result = BitConverter.ToSingle(buffer, index);
			index += 4;
			return result;
		}

		public bool ReadBoolean()
		{
			bool result = BitConverter.ToBoolean(buffer, index);
			index++;
			return result;
		}

		public string ReadString()
		{
			int num = BitConverter.ToInt32(buffer, index);
			index += 4;
			readStringBuilder.Remove(0, readStringBuilder.Length);
			for (int i = 0; i < num; i++)
			{
				readStringBuilder.Append(BitConverter.ToChar(buffer, index));
				index += 2;
			}
			if (num == 0)
			{
				return string.Empty;
			}
			return readStringBuilder.ToString();
		}
	}
}
