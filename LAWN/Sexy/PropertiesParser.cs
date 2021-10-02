using System.Collections.Generic;

namespace Sexy
{
	internal class PropertiesParser
	{
		public SexyAppBase mApp;

		public XMLParser mXMLParser;

		public string mError;

		public bool mHasFailed;

		protected void Fail(string theErrorText)
		{
			if (!mHasFailed)
			{
				mHasFailed = true;
				int currentLineNum = mXMLParser.GetCurrentLineNum();
				mError = theErrorText;
				if (currentLineNum > 0)
				{
					mError += Common.StrFormat_(" on Line {0}", currentLineNum);
				}
				if (mXMLParser.GetFileName().Length != 0)
				{
					mError += Common.StrFormat_(" in File '{0}'", mXMLParser.GetFileName());
				}
			}
		}

		protected bool ParseSingleElement(ref string aString)
		{
			aString = "";
			while (true)
			{
				XMLElement theElement = new XMLElement();
				if (!mXMLParser.NextElement(ref theElement))
				{
					return false;
				}
				if (theElement.mType == XMLElementType.TYPE_START)
				{
					Fail("Unexpected Section: '" + theElement.mValue + "'");
					return false;
				}
				if (theElement.mType == XMLElementType.TYPE_ELEMENT)
				{
					aString = theElement.mValue;
				}
				else if (theElement.mType == XMLElementType.TYPE_END)
				{
					break;
				}
			}
			return true;
		}

		protected bool ParseStringArray(ref List<string> theStringVector)
		{
			theStringVector.Clear();
			while (true)
			{
				XMLElement theElement = new XMLElement();
				if (!mXMLParser.NextElement(ref theElement))
				{
					return false;
				}
				if (theElement.mType == XMLElementType.TYPE_START)
				{
					if (!(theElement.mValue == "String"))
					{
						Fail("Invalid Section '" + theElement.mValue + "'");
						return false;
					}
					string aString = string.Empty;
					if (!ParseSingleElement(ref aString))
					{
						return false;
					}
					theStringVector.Add(aString);
				}
				else
				{
					if (theElement.mType == XMLElementType.TYPE_ELEMENT)
					{
						Fail("Element Not Expected '" + theElement.mValue + "'");
						return false;
					}
					if (theElement.mType == XMLElementType.TYPE_END)
					{
						break;
					}
				}
			}
			return true;
		}

		protected bool ParseProperties()
		{
			while (true)
			{
				XMLElement theElement = new XMLElement();
				if (!mXMLParser.NextElement(ref theElement))
				{
					return false;
				}
				if (theElement.mType == XMLElementType.TYPE_START)
				{
					if (theElement.mValue == "String")
					{
						string aString = string.Empty;
						if (!ParseSingleElement(ref aString))
						{
							return false;
						}
						string theId = theElement.mAttributes["id"];
						mApp.SetString(theId, aString);
					}
					else if (theElement.mValue == "StringArray")
					{
						List<string> theStringVector = new List<string>();
						if (!ParseStringArray(ref theStringVector))
						{
							return false;
						}
						string key = theElement.mAttributes["id"];
						mApp.mStringVectorProperties.Add(key, theStringVector);
					}
					else if (theElement.mValue == "Boolean")
					{
						string aString2 = string.Empty;
						if (!ParseSingleElement(ref aString2))
						{
							return false;
						}
						aString2 = Common.Upper(aString2);
						bool theValue;
						switch (aString2)
						{
						case "1":
						case "YES":
						case "ON":
						case "TRUE":
							theValue = true;
							break;
						case "0":
						case "NO":
						case "OFF":
						case "FALSE":
							theValue = false;
							break;
						default:
							Fail("Invalid Boolean Value: '" + aString2 + "'");
							return false;
						}
						string theId2 = theElement.mAttributes["id"];
						mApp.SetBoolean(theId2, theValue);
					}
					else if (theElement.mValue == "Integer")
					{
						string aString3 = string.Empty;
						if (!ParseSingleElement(ref aString3))
						{
							return false;
						}
						int theIntVal = 0;
						if (!Common.StringToInt(aString3, ref theIntVal))
						{
							Fail("Invalid Integer Value: '" + aString3 + "'");
							return false;
						}
						string theId3 = theElement.mAttributes["id"];
						mApp.SetInteger(theId3, theIntVal);
					}
					else
					{
						if (!(theElement.mValue == "Double"))
						{
							Fail("Invalid Section '" + theElement.mValue + "'");
							return false;
						}
						string aString4 = string.Empty;
						if (!ParseSingleElement(ref aString4))
						{
							return false;
						}
						double theDoubleVal = 0.0;
						if (!Common.StringToDouble(aString4, ref theDoubleVal))
						{
							Fail("Invalid Double Value: '" + aString4 + "'");
							return false;
						}
						string theId4 = theElement.mAttributes["id"];
						mApp.SetDouble(theId4, theDoubleVal);
					}
				}
				else
				{
					if (theElement.mType == XMLElementType.TYPE_ELEMENT)
					{
						Fail("Element Not Expected '" + theElement.mValue + "'");
						return false;
					}
					if (theElement.mType == XMLElementType.TYPE_END)
					{
						break;
					}
				}
			}
			return true;
		}

		protected bool DoParseProperties()
		{
			if (!mXMLParser.HasFailed())
			{
				while (true)
				{
					XMLElement theElement = new XMLElement();
					if (!mXMLParser.NextElement(ref theElement))
					{
						break;
					}
					if (theElement.mType == XMLElementType.TYPE_START)
					{
						if (!(theElement.mValue == "Properties"))
						{
							Fail("Invalid Section '" + theElement.mValue + "'");
							break;
						}
						if (!ParseProperties())
						{
							break;
						}
					}
					else if (theElement.mType == XMLElementType.TYPE_ELEMENT)
					{
						Fail("Element Not Expected '" + theElement.mValue + "'");
						break;
					}
				}
			}
			if (mXMLParser.HasFailed())
			{
				Fail(mXMLParser.GetErrorText());
			}
			mXMLParser.Dispose();
			mXMLParser = null;
			return !mHasFailed;
		}

		public PropertiesParser(SexyAppBase theApp)
		{
			mApp = theApp;
			mHasFailed = false;
			mXMLParser = null;
		}

		public virtual void Dispose()
		{
		}

		public bool ParsePropertiesFile(string theFilename)
		{
			mXMLParser = new XMLParser();
			mXMLParser.OpenFile(theFilename);
			return DoParseProperties();
		}

		public bool ParsePropertiesBuffer(Buffer theBuffer)
		{
			mXMLParser = new XMLParser();
			mXMLParser.SetStringSource("");
			return DoParseProperties();
		}

		public string GetErrorText()
		{
			return mError;
		}
	}
}
