using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Sexy.TodLib
{
    internal class Reanimator
    {
		private static ReanimatorTransform previous;

		public ReanimatorDefinition ParseReanimationFile(string theFilename)
		{
			ReanimatorDefinition reanimatorDefinition = new ReanimatorDefinition();

			XmlDocument reanim = new XmlDocument();
			string readText = File.ReadAllText(theFilename);
			//HACK: little hack to make XmlDocument work without editing reanim files
			reanim.LoadXml("<reanim>"+readText+"</reanim>");
			XmlNode node = reanim.DocumentElement.SelectSingleNode("/reanim/fps");
			XmlNodeList tracks = reanim.GetElementsByTagName("track");
			reanimatorDefinition.mFPS = float.Parse(node.InnerText);
			reanimatorDefinition.mTrackCount = (short)tracks.Count;
			reanimatorDefinition.mTracks = new ReanimatorTrack[reanimatorDefinition.mTrackCount];
			for (int i = 0; i < reanimatorDefinition.mTrackCount; i++)
			{
				ReanimatorTrack track;
				ReadReanimTrack(tracks[i], out track);
				reanimatorDefinition.mTracks[i] = track;
			}
			return reanimatorDefinition;
		}

		private void ReadReanimTrack(XmlNode input, out ReanimatorTrack track)
		{
			string name = input.SelectSingleNode("name").InnerText;
			XmlNodeList transforms = input.SelectNodes("t");
			int transformCount = transforms.Count;
			track = new ReanimatorTrack(name, transformCount);
			for (int i = 0; i < track.mTransformCount; i++)
			{
				ReanimatorTransform transform;
				ReadReanimTransform(transforms[i], i, out transform);
				track.mTransforms[i] = transform;
			}
		}

		private static ReanimatorTransform GetDefault()
		{
			ReanimatorTransform newReanimatorTransform = ReanimatorTransform.GetNewReanimatorTransform();
			newReanimatorTransform.mFontName = string.Empty;
			newReanimatorTransform.mImageName = string.Empty;
			newReanimatorTransform.mText = string.Empty;
			newReanimatorTransform.mAlpha = ReanimatorXnaHelpers.DEFAULT_FIELD_PLACEHOLDER;
			newReanimatorTransform.mFrame = ReanimatorXnaHelpers.DEFAULT_FIELD_PLACEHOLDER;
			newReanimatorTransform.mScaleX = ReanimatorXnaHelpers.DEFAULT_FIELD_PLACEHOLDER;
			newReanimatorTransform.mScaleY = ReanimatorXnaHelpers.DEFAULT_FIELD_PLACEHOLDER;
			newReanimatorTransform.mSkewX = ReanimatorXnaHelpers.DEFAULT_FIELD_PLACEHOLDER;
			newReanimatorTransform.mSkewY = ReanimatorXnaHelpers.DEFAULT_FIELD_PLACEHOLDER;
			newReanimatorTransform.mSkewXCos = ReanimatorXnaHelpers.DEFAULT_FIELD_PLACEHOLDER;
			newReanimatorTransform.mSkewXSin = ReanimatorXnaHelpers.DEFAULT_FIELD_PLACEHOLDER;
			newReanimatorTransform.mSkewYCos = ReanimatorXnaHelpers.DEFAULT_FIELD_PLACEHOLDER;
			newReanimatorTransform.mSkewYSin = ReanimatorXnaHelpers.DEFAULT_FIELD_PLACEHOLDER;
			newReanimatorTransform.mTransX = ReanimatorXnaHelpers.DEFAULT_FIELD_PLACEHOLDER;
			newReanimatorTransform.mTransY = ReanimatorXnaHelpers.DEFAULT_FIELD_PLACEHOLDER;
			return newReanimatorTransform;
		}

		private void ReadReanimTransform(XmlNode input, int transformNum, out ReanimatorTransform transform)
		{
			transform = GetDefault();
			if (previous == null || transformNum == 0)
			{
				previous = GetDefault();
			}
			//Perfect code XD
			if (input.SelectSingleNode("x") == null)
			{
				transform.mTransX = previous.mTransX;
			}
			if (input.SelectSingleNode("y") == null)
			{
				transform.mTransY = previous.mTransY;
			}
			if (input.SelectSingleNode("kx") == null)
			{
				transform.mSkewX = previous.mSkewX;
			}
			if (input.SelectSingleNode("ky") == null)
			{
				transform.mSkewY = previous.mSkewY;
			}
			if (input.SelectSingleNode("sx") == null)
			{
				transform.mScaleX = previous.mScaleX;
			}
			if (input.SelectSingleNode("sy") == null)
			{
				transform.mScaleY = previous.mScaleY;
			}
			if (input.SelectSingleNode("f") == null)
			{
				transform.mFrame = previous.mFrame;
			}
			if (input.SelectSingleNode("a") == null)
			{
				transform.mAlpha = previous.mAlpha;
			}
			if (input.SelectSingleNode("i") == null)
			{
				transform.mImageName = previous.mImageName;
			}
			foreach (XmlNode node in input)
			{
				if (node.Name == "x")
				{
					transform.mTransX = float.Parse(node.InnerText);
				}
				if (node.Name == "y")
				{
					transform.mTransY = float.Parse(node.InnerText);
				}
				if (node.Name == "kx")
				{
					transform.mSkewX = float.Parse(node.InnerText);
				}
				if (node.Name == "ky")
				{
					transform.mSkewY = float.Parse(node.InnerText);
				}
				if (node.Name == "sx")
				{
					transform.mScaleX = float.Parse(node.InnerText);
				}
				if (node.Name == "sy")
				{
					transform.mScaleY = float.Parse(node.InnerText);
				}
				if (node.Name == "f")
				{
					transform.mFrame = int.Parse(node.InnerText);
				}
				if (node.Name == "a")
				{
					transform.mAlpha = float.Parse(node.InnerText);
				}
				if (node.Name == "i")
				{
					transform.mImageName = node.InnerText;
				}
			}
			previous = transform;
		}
	}
}
