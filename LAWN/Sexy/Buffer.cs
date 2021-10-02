using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sexy
{
	public class Buffer
	{
		internal static int[] gWebDecodeMap = new int[256]
		{
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			0,
			-1,
			1,
			0,
			-1,
			2,
			3,
			4,
			5,
			6,
			7,
			8,
			9,
			10,
			11,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			12,
			13,
			14,
			15,
			16,
			17,
			18,
			19,
			20,
			21,
			22,
			23,
			24,
			25,
			26,
			27,
			28,
			29,
			30,
			31,
			32,
			33,
			34,
			35,
			36,
			37,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			38,
			39,
			40,
			41,
			42,
			43,
			44,
			45,
			46,
			47,
			48,
			49,
			50,
			51,
			52,
			53,
			54,
			55,
			56,
			57,
			58,
			59,
			60,
			61,
			62,
			63,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1
		};

		internal static string gWebEncodeMap = ".-0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

		public MemoryStream mData;

		public int mDataBitSize;

		public int mReadBitPos;

		public int mWriteBitPos;

		private UTF8Encoding encoding = new UTF8Encoding();

		public byte[] Data
		{
			get
			{
				return mData.GetBuffer();
			}
			set
			{
				mData.Close();
				mData = new MemoryStream(value);
			}
		}

		public Buffer()
		{
			mDataBitSize = 0;
			mReadBitPos = 0;
			mWriteBitPos = 0;
			mData = new MemoryStream();
		}

		public virtual void Dispose()
		{
		}

		public void Clear()
		{
			mReadBitPos = 0;
			mWriteBitPos = 0;
			mDataBitSize = 0;
			mData.Close();
			mData = new MemoryStream();
		}

		public void SeekFront()
		{
			mReadBitPos = 0;
			mData.Seek(0L, SeekOrigin.Begin);
		}

		public int GetDataLen()
		{
			return (mDataBitSize + 7) / 8;
		}

		public void FromWebString(string theString)
		{
			Clear();
			if (theString.Length < 4)
			{
				return;
			}
			int num = 0;
			for (int i = 0; i < 8; i++)
			{
				char c = theString[i];
				int num2 = 0;
				if (c >= '0' && c <= '9')
				{
					num2 = c - 48;
				}
				else if (c >= 'A' && c <= 'F')
				{
					num2 = c - 65 + 10;
				}
				else if (c >= 'a' && c <= 'f')
				{
					num2 = c - 102 + 10;
				}
				num += num2 << (7 - i) * 4;
			}
			int num3 = 8;
			int num4 = num;
			while (num4 > 0)
			{
				char c2 = theString[num3++];
				int theNum = gWebDecodeMap[c2];
				int num5 = Math.Min(num4, 6);
				WriteNumBits(theNum, num5);
				num4 -= num5;
			}
			SeekFront();
		}

		public void WriteByte(byte theByte)
		{
			mData.WriteByte(theByte);
		}

		public void WriteNumBits(int theNum, int theBits)
		{
		}

		public static int GetBitsRequired(int theNum, bool isSigned)
		{
			if (theNum < 0)
			{
				theNum = -theNum - 1;
			}
			int i;
			for (i = 0; theNum >= 1 << i; i++)
			{
			}
			if (isSigned)
			{
				i++;
			}
			return i;
		}

		public void WriteBoolean(bool theBool)
		{
			byte[] bytes = BitConverter.GetBytes(theBool);
			mData.Write(bytes, 0, bytes.Length);
		}

		public void WriteBooleanArray(bool[] theBool)
		{
			WriteLong(theBool.Length);
			for (int i = 0; i < theBool.Length; i++)
			{
				WriteBoolean(theBool[i]);
			}
		}

		public void WriteBoolean2DArray(bool[,] theBool)
		{
			int length = theBool.GetLength(0);
			int length2 = theBool.GetLength(1);
			WriteLong(length);
			WriteLong(length2);
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length2; j++)
				{
					WriteBoolean(theBool[i, j]);
				}
			}
		}

		public void WriteShort(short theShort)
		{
			byte[] bytes = BitConverter.GetBytes(theShort);
			mData.Write(bytes, 0, bytes.Length);
		}

		public void WriteLong(int theLong)
		{
			byte[] bytes = BitConverter.GetBytes(theLong);
			mData.Write(bytes, 0, bytes.Length);
		}

		public void WriteLongArray(int[] theLong)
		{
			WriteLong(theLong.Length);
			for (int i = 0; i < theLong.Length; i++)
			{
				WriteLong(theLong[i]);
			}
		}

		public void WriteLong2DArray(int[,] theLong)
		{
			int length = theLong.GetLength(0);
			int length2 = theLong.GetLength(1);
			WriteLong(length);
			WriteLong(length2);
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length2; j++)
				{
					WriteLong(theLong[i, j]);
				}
			}
		}

		public void WriteLong3DArray(int[,,] theLong)
		{
			int length = theLong.GetLength(0);
			int length2 = theLong.GetLength(1);
			int length3 = theLong.GetLength(2);
			WriteLong(length);
			WriteLong(length2);
			WriteLong(length3);
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length2; j++)
				{
					for (int k = 0; k < length3; k++)
					{
						WriteLong(theLong[i, j, k]);
					}
				}
			}
		}

		public void WriteFloat(float theFloat)
		{
			byte[] bytes = BitConverter.GetBytes(theFloat);
			mData.Write(bytes, 0, bytes.Length);
		}

		public void WriteDouble(double theDouble)
		{
			byte[] bytes = BitConverter.GetBytes(theDouble);
			mData.Write(bytes, 0, bytes.Length);
		}

		public void WriteDateTime(DateTime theTime)
		{
			WriteLong(theTime.Millisecond);
			WriteLong(theTime.Second);
			WriteLong(theTime.Minute);
			WriteLong(theTime.Hour);
			WriteLong(theTime.Day);
			WriteLong(theTime.Month);
			WriteLong(theTime.Year);
		}

		public DateTime ReadDateTime()
		{
			int millisecond = ReadLong();
			int second = ReadLong();
			int minute = ReadLong();
			int hour = ReadLong();
			int day = ReadLong();
			int month = ReadLong();
			int year = ReadLong();
			return new DateTime(year, month, day, hour, minute, second, millisecond, DateTimeKind.Unspecified);
		}

		public void WriteString(string theString)
		{
			WriteLong(theString.Length);
			byte[] bytes = encoding.GetBytes(theString);
			for (int i = 0; i < bytes.Length; i++)
			{
				WriteByte(bytes[i]);
			}
		}

		public void WriteStringArray(string[] theString)
		{
			WriteLong(theString.Length);
			for (int i = 0; i < theString.Length; i++)
			{
				WriteString(theString[i]);
			}
		}

		public void WriteRect(TRect theRect)
		{
			WriteLong(theRect.mX);
			WriteLong(theRect.mY);
			WriteLong(theRect.mWidth);
			WriteLong(theRect.mHeight);
		}

		public void WriteUTF8String(string theString)
		{
			WriteString(theString);
		}

		public void WriteLine(string theString)
		{
			WriteString(theString + "\r\n");
		}

		public void WriteBuffer(List<byte> theBuffer)
		{
		}

		public void WriteBytes(string theByte, int theCount)
		{
			for (int i = 0; i < theCount; i++)
			{
				WriteByte((byte)theByte[i]);
			}
		}

		public void SetData(List<byte> theBuffer)
		{
			mData.Seek(0L, SeekOrigin.Begin);
			for (int i = 0; i < theBuffer.Count; i++)
			{
				mData.WriteByte(theBuffer[i]);
			}
			mDataBitSize = (int)mData.Length * 8;
		}

		public void SetData(List<byte> theOtherList, int theCount)
		{
			mData = new MemoryStream();
			mData.Write(theOtherList.ToArray(), 0, theCount);
			mDataBitSize = (int)mData.Length * 8;
		}

		public string ToWebString()
		{
			string str = "";
			int num = mWriteBitPos;
			int num2 = mReadBitPos;
			mReadBitPos = 0;
			string text = new string(new char[256]);
			text = string.Format("{0:X8}", num);
			str += text;
			int num3 = (num + 5) / 6;
			for (int i = 0; i < num3; i++)
			{
				str += gWebEncodeMap[ReadNumBits(6, false)];
			}
			mReadBitPos = num2;
			return str;
		}

		public byte ReadByte()
		{
			return (byte)mData.ReadByte();
		}

		public int ReadNumBits(int theBits, bool isSigned)
		{
			return 0;
		}

		public bool ReadBoolean()
		{
			byte[] array = new byte[1];
			mData.Read(array, 0, array.Length);
			return BitConverter.ToBoolean(array, 0);
		}

		public bool[] ReadBooleanArray()
		{
			int num = ReadLong();
			bool[] array = new bool[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = ReadBoolean();
			}
			return array;
		}

		public bool[,] ReadBoolean2DArray()
		{
			int num = ReadLong();
			int num2 = ReadLong();
			bool[,] array = new bool[num, num2];
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					array[i, j] = ReadBoolean();
				}
			}
			return array;
		}

		public short ReadShort()
		{
			byte[] array = new byte[2];
			mData.Read(array, 0, array.Length);
			return BitConverter.ToInt16(array, 0);
		}

		public int ReadLong()
		{
			byte[] array = new byte[4];
			mData.Read(array, 0, array.Length);
			return BitConverter.ToInt32(array, 0);
		}

		public int[] ReadLongArray()
		{
			int num = ReadLong();
			int[] array = new int[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = ReadLong();
			}
			return array;
		}

		public int[,] ReadLong2DArray()
		{
			int num = ReadLong();
			int num2 = ReadLong();
			int[,] array = new int[num, num2];
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					array[i, j] = ReadLong();
				}
			}
			return array;
		}

		public int[,,] ReadLong3DArray()
		{
			int num = ReadLong();
			int num2 = ReadLong();
			int num3 = ReadLong();
			int[,,] array = new int[num, num2, num3];
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					for (int k = 0; k < num3; k++)
					{
						array[i, j, k] = ReadLong();
					}
				}
			}
			return array;
		}

		public float ReadFloat()
		{
			byte[] array = new byte[4];
			mData.Read(array, 0, 4);
			return BitConverter.ToSingle(array, 0);
		}

		public double ReadDouble()
		{
			byte[] array = new byte[8];
			mData.Read(array, 0, 8);
			return BitConverter.ToDouble(array, 0);
		}

		public string ReadString()
		{
			string text = "";
			int num = ReadLong();
			byte[] array = new byte[num];
			mData.Read(array, 0, num);
			return encoding.GetString(array, 0, num);
		}

		public string[] ReadStringArray()
		{
			int num = ReadLong();
			string[] array = new string[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = ReadString();
			}
			return array;
		}

		public TRect ReadRect()
		{
			int theX = ReadLong();
			int theY = ReadLong();
			int theWidth = ReadLong();
			int theHeight = ReadLong();
			return new TRect(theX, theY, theWidth, theHeight);
		}

		public string ReadUTF8String()
		{
			return ReadString();
		}

		public string ReadLine()
		{
			string text = "";
			while (true)
			{
				byte b = ReadByte();
				switch (b)
				{
				case 13:
					break;
				default:
					goto IL_001a;
				case 0:
				case 10:
					return text;
				}
				continue;
				IL_001a:
				text += b;
			}
		}

		public void ReadBytes(byte[] theData, int theLen)
		{
			for (int i = 0; i < theLen; i++)
			{
				theData[i] = ReadByte();
			}
		}

		public void ReadBuffer(ref List<byte> theByteVector)
		{
		}

		public string GetDataPtr()
		{
			return "";
		}

		public int GetDataLenBits()
		{
			return mDataBitSize;
		}

		public uint GetCRC32()
		{
			return GetCRC32(0u);
		}

		public uint GetCRC32(uint theSeed)
		{
			return theSeed;
		}

		public bool AtEnd()
		{
			return mData.Position == mData.Length;
		}

		public bool PastEnd()
		{
			return mData.Position > mData.Length;
		}
	}
}
