using Microsoft.Xna.Framework.Content;
using System;

namespace Sexy.TodLib
{
	internal class ReanimReader : ContentTypeReader<ReanimatorDefinition>
	{
		internal enum ReanimOptimisationType
		{
			New,
			CopyPrevious,
			Placeholder
		}

		private static ReanimatorTransform previous;

		public static float DEG_TO_RAD = (float)Math.PI / 180f;

		protected override ReanimatorDefinition Read(ContentReader input, ReanimatorDefinition existingInstance)
		{
			ReanimatorDefinition reanimatorDefinition = new ReanimatorDefinition();
			CustomContentReader customContentReader = new CustomContentReader(input);
			bool doScale = customContentReader.ReadBoolean();
			reanimatorDefinition.mFPS = customContentReader.ReadSingle();
			reanimatorDefinition.mTrackCount = (short)customContentReader.ReadInt32();
			reanimatorDefinition.mTracks = new ReanimatorTrack[reanimatorDefinition.mTrackCount];
			for (int i = 0; i < reanimatorDefinition.mTrackCount; i++)
			{
				ReanimatorTrack track;
				ReadReanimTrack(customContentReader, doScale, out track);
				reanimatorDefinition.mTracks[i] = track;
			}
			return reanimatorDefinition;
		}

		private void ReadReanimTrack(CustomContentReader input, bool doScale, out ReanimatorTrack track)
		{
			string name = input.ReadString();
			int transformCount = input.ReadInt32();
			track = new ReanimatorTrack(name, transformCount);
			for (int i = 0; i < track.mTransformCount; i++)
			{
				ReanimatorTransform transform;
				ReadReanimTransform(input, doScale, out transform);
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

		private void ReadReanimTransform(CustomContentReader input, bool doScale, out ReanimatorTransform transform)
		{
			switch (input.ReadByte())
			{
			case 2:
				transform = GetDefault();
				break;
			case 1:
				Debug.ASSERT(previous != null);
				transform = previous;
				break;
			default:
			{
				transform = ReanimatorTransform.GetReanimatorTransformForLoadingThread();
				transform.mFontName = input.ReadString();
				transform.mImageName = input.ReadString();
				transform.mText = input.ReadString();
				transform.mAlpha = input.ReadSingle();
				transform.mFrame = input.ReadSingle();
				transform.mScaleX = input.ReadSingle();
				transform.mScaleY = input.ReadSingle();
				transform.mSkewX = input.ReadSingle();
				transform.mSkewY = input.ReadSingle();
				float num = input.ReadSingle();
				float num2 = input.ReadSingle();
				if (doScale)
				{
					transform.mTransX = ((num == ReanimatorXnaHelpers.DEFAULT_FIELD_PLACEHOLDER) ? num : Constants.InvertAndScale(num));
					transform.mTransY = ((num2 == ReanimatorXnaHelpers.DEFAULT_FIELD_PLACEHOLDER) ? num2 : Constants.InvertAndScale(num2));
				}
				else
				{
					transform.mTransX = num;
					transform.mTransY = num2;
				}
				break;
			}
			}
			previous = transform;
		}
	}
}
