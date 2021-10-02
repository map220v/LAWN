using System.Collections.Generic;
using System.Xml;

namespace Sexy
{
	internal class XMLParser : EncodingParser
	{
		protected string mFileName;

		protected string mErrorText;

		protected int mLineNum;

		protected bool mHasFailed;

		protected bool mAllowComments;

		protected string mSection;

		protected void Fail(string theErrorText)
		{
			mHasFailed = true;
			mErrorText = theErrorText;
		}

		protected void Init()
		{
			mSection = "";
			mLineNum = 1;
			mHasFailed = false;
			mErrorText = "";
		}

		protected bool AddAttribute(XMLElement theElement, string theAttributeKey, string theAttributeValue)
		{
			bool result = true;
			if (theElement.mAttributes.ContainsKey(theAttributeKey))
			{
				result = false;
			}
			theElement.mAttributes[theAttributeKey] = theAttributeValue;
			Dictionary<string, string>.Enumerator enumerator = theElement.mAttributes.GetEnumerator();
			if (enumerator.MoveNext() && enumerator.Current.Key == theAttributeKey && theAttributeKey != "/")
			{
				theElement.mAttributeIteratorList.Add(enumerator);
			}
			return result;
		}

		protected bool AddAttributeEncoded(XMLElement theElement, string theAttributeKey, string theAttributeValue)
		{
			bool result = true;
			if (theElement.mAttributesEncoded.ContainsKey(theAttributeKey))
			{
				result = false;
			}
			theElement.mAttributesEncoded[theAttributeKey] = theAttributeValue;
			Dictionary<string, string>.Enumerator enumerator = theElement.mAttributesEncoded.GetEnumerator();
			if (enumerator.MoveNext() && enumerator.Current.Key == theAttributeKey && theAttributeKey != "/")
			{
				theElement.mAttributeEncodedIteratorList.Add(enumerator);
			}
			return result;
		}

		public XMLParser()
		{
			mLineNum = 0;
			mAllowComments = false;
			mFileName = string.Empty;
		}

		public override void Dispose()
		{
			base.Dispose();
		}

		public override bool OpenFile(string theFileName)
		{
			if (!base.OpenFile(theFileName))
			{
				mLineNum = 0;
				Fail("Unable to open file " + theFileName);
				return false;
			}
			mFileName = theFileName;
			Init();
			return true;
		}

		public virtual bool NextElement(ref XMLElement theElement)
		{
			do
			{
				theElement.mType = XMLElementType.TYPE_NONE;
				theElement.mSection = mSection;
				theElement.mValue = "";
				theElement.mValueEncoded = "";
				theElement.mAttributes.Clear();
				theElement.mAttributesEncoded.Clear();
				theElement.mInstruction = "";
				theElement.mAttributeIteratorList.Clear();
				theElement.mAttributeEncodedIteratorList.Clear();
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = false;
				bool flag5 = false;
				string text = "";
				string text2 = "";
				string text3 = "";
				string text4 = "";
				while (true)
				{
					char theChar = '\0';
					int num;
					switch (GetChar(ref theChar))
					{
					case GetCharReturnType.SUCCESSFUL:
						num = 1;
						break;
					case GetCharReturnType.INVALID_CHARACTER:
						Fail("Illegal Character");
						return false;
					case GetCharReturnType.FAILURE:
						Fail("Internal Error");
						return false;
					default:
						num = 0;
						break;
					}
					if (num == 1)
					{
						bool flag6 = false;
						if (theChar == '\n')
						{
							mLineNum++;
						}
						if (theElement.mType == XMLElementType.TYPE_COMMENT)
						{
							theElement.mInstruction += theChar;
							int length = theElement.mInstruction.Length;
							if (theChar == '>' && length >= 3 && theElement.mInstruction[length - 2] == '-' && theElement.mInstruction[length - 3] == '-')
							{
								theElement.mInstruction = theElement.mInstruction.Substring(0, length - 3);
								break;
							}
							continue;
						}
						if (theElement.mType == XMLElementType.TYPE_INSTRUCTION)
						{
							int length2;
							string text5;
							if (theElement.mInstruction.Length != 0 || char.IsWhiteSpace(theChar))
							{
								theElement.mInstruction += theChar;
								length2 = theElement.mInstruction.Length;
								text5 = theElement.mInstruction;
							}
							else
							{
								theElement.mValue += theChar;
								length2 = theElement.mValue.Length;
								text5 = theElement.mValue;
							}
							if (theChar == '>' && length2 >= 2 && text5[length2 - 2] == '?')
							{
								text5 = text5.Substring(0, length2 - 2);
								break;
							}
							continue;
						}
						if (theChar == '"')
						{
							flag2 = !flag2;
							if (theElement.mType == XMLElementType.TYPE_NONE || theElement.mType == XMLElementType.TYPE_ELEMENT)
							{
								flag6 = true;
							}
							if (!flag2)
							{
								flag3 = true;
							}
						}
						else if (!flag2)
						{
							if (theChar == '<')
							{
								if (theElement.mType == XMLElementType.TYPE_ELEMENT)
								{
									PutChar(theChar);
									break;
								}
								if (theElement.mType != 0)
								{
									Fail("Unexpected '<'");
									return false;
								}
								theElement.mType = XMLElementType.TYPE_START;
							}
							else
							{
								if (theChar == '>')
								{
									if (theElement.mType == XMLElementType.TYPE_START)
									{
										bool flag7 = false;
										if (text == "/")
										{
											flag7 = true;
										}
										else
										{
											if (text.Length > 0)
											{
												text3 = XMLDecodeString(text);
												text4 = text;
												AddAttribute(theElement, XMLDecodeString(text), XMLDecodeString(text2));
												AddAttributeEncoded(theElement, text, text2);
												text = "";
												text2 = "";
											}
											if (text3.Length > 0)
											{
												string text6 = theElement.mAttributes[text3];
												int length3 = text6.Length;
												if (length3 > 0 && text6[length3 - 1] == '/')
												{
													AddAttribute(theElement, text3, XMLDecodeString(text6.Substring(0, length3 - 1)));
													flag7 = true;
												}
												text6 = theElement.mAttributesEncoded[text4];
												length3 = text6.Length;
												if (length3 > 0 && text6[length3 - 1] == '/')
												{
													AddAttributeEncoded(theElement, text4, text6.Substring(0, length3 - 1));
													flag7 = true;
												}
											}
											else
											{
												int length4 = theElement.mValue.Length;
												if (length4 > 0 && theElement.mValue[length4 - 1] == '/')
												{
													theElement.mValue = theElement.mValue.Substring(0, length4 - 1);
													flag7 = true;
												}
											}
										}
										if (flag7)
										{
											string theString = "</" + theElement.mValue + ">";
											PutString(theString);
											text = "";
										}
										if (mSection.Length != 0)
										{
											mSection += "/";
										}
										mSection += theElement.mValue;
										break;
									}
									if (theElement.mType == XMLElementType.TYPE_END)
									{
										int num2 = mSection.LastIndexOf('/');
										if (num2 == -1 && mSection.Length == 0)
										{
											Fail("Unexpected End");
											return false;
										}
										string text7 = mSection.Substring(num2 + 1);
										if (text7 != theElement.mValue)
										{
											Fail("End '" + theElement.mValue + "' Doesn't Match Start '" + text7 + "'");
											return false;
										}
										if (num2 == -1)
										{
											mSection = mSection.Remove(0, mSection.Length);
										}
										else
										{
											mSection = mSection.Remove(num2, mSection.Length - num2);
										}
										break;
									}
									Fail("Unexpected '>'");
									return false;
								}
								if (theChar == '/' && theElement.mType == XMLElementType.TYPE_START && theElement.mValue == "")
								{
									theElement.mType = XMLElementType.TYPE_END;
								}
								else if (theChar == '?' && theElement.mType == XMLElementType.TYPE_START && theElement.mValue == "")
								{
									theElement.mType = XMLElementType.TYPE_INSTRUCTION;
								}
								else if (char.IsWhiteSpace(theChar))
								{
									if (theElement.mValue != "")
									{
										flag = true;
									}
								}
								else
								{
									if (theChar <= ' ')
									{
										Fail("Illegal Character");
										return false;
									}
									flag6 = true;
								}
							}
						}
						else
						{
							flag6 = true;
						}
						if (!flag6)
						{
							continue;
						}
						if (theElement.mType == XMLElementType.TYPE_NONE)
						{
							theElement.mType = XMLElementType.TYPE_ELEMENT;
						}
						if (theElement.mType == XMLElementType.TYPE_START)
						{
							if (flag)
							{
								if (!flag4)
								{
									flag4 = true;
									flag5 = false;
								}
								else if (!flag2)
								{
									if ((!flag5 && theChar != '=') || (flag5 && (!string.IsNullOrEmpty(text2) || flag3)))
									{
										AddAttribute(theElement, XMLDecodeString(text), XMLDecodeString(text2));
										AddAttributeEncoded(theElement, text, text2);
										text = "";
										text2 = "";
										text3 = "";
										text4 = "";
									}
									else
									{
										flag4 = true;
									}
									flag5 = false;
								}
								flag = false;
							}
							if (!flag4)
							{
								theElement.mValue += theChar;
								if (theElement.mValue == "!--")
								{
									theElement.mType = XMLElementType.TYPE_COMMENT;
								}
							}
							else if (!flag2 && theChar == '=')
							{
								flag5 = true;
								flag3 = false;
							}
							else if (!flag5)
							{
								text += theChar;
							}
							else
							{
								text2 += theChar;
							}
						}
						else
						{
							if (flag)
							{
								theElement.mValue += " ";
								flag = false;
							}
							theElement.mValue += theChar;
						}
						continue;
					}
					if (theElement.mType != 0)
					{
						Fail("Unexpected End of File");
					}
					return false;
				}
				if (text.Length > 0)
				{
					AddAttribute(theElement, XMLDecodeString(text), XMLDecodeString(text2));
					AddAttribute(theElement, text, text2);
				}
				theElement.mValueEncoded = theElement.mValue;
				theElement.mValue = XMLDecodeString(theElement.mValue);
			}
			while (theElement.mType == XMLElementType.TYPE_COMMENT && !mAllowComments);
			return true;
		}

		private string XMLDecodeString(string s)
		{
			return XmlConvert.DecodeName(s);
		}

		public string GetErrorText()
		{
			return mErrorText;
		}

		public int GetCurrentLineNum()
		{
			return mLineNum;
		}

		public string GetFileName()
		{
			return mFileName;
		}

		public override void SetStringSource(string theString)
		{
			Init();
			base.SetStringSource(theString);
		}

		public void AllowComments(bool doAllow)
		{
			mAllowComments = doAllow;
		}

		public bool HasFailed()
		{
			return mHasFailed;
		}
	}
}
