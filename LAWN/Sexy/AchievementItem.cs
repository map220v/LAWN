using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Sexy
{
	internal class AchievementItem
	{
		private int gamerScore;

		private string name;

		private string description;

		private Image achievementImage;

		public AchievementId Id
		{
			get;
			private set;
		}

		public string Key
		{
			get;
			private set;
		}

		public bool IsEarned
		{
			get;
			private set;
		}

		public int GamerScore
		{
			get
			{
				return gamerScore;
			}
			protected set
			{
				gamerScore = value;
			}
		}

		public string Name
		{
			get
			{
				return name;
			}
			protected set
			{
				name = value;
			}
		}

		public string Description
		{
			get
			{
				return description;
			}
			protected set
			{
				description = value;
			}
		}

		public Image AchievementImage
		{
			get
			{
				return achievementImage;
			}
			protected set
			{
				achievementImage = value;
			}
		}

		/*public AchievementItem(Achievement a)
		{
			Name = a.Name;
			Description = a.Description;
			GamerScore = a.GamerScore;
			Key = a.Key;
			IsEarned = a.IsEarned;
			using (Stream stream = a.GetPicture())
			{
				AchievementImage = new Image(Texture2D.FromStream(GlobalStaticVars.g.GraphicsDevice, stream));
			}
		}*/

		public void Dispose()
		{
			AchievementImage.Dispose();
		}

		public static bool operator ==(AchievementItem a, AchievementItem b)
		{
			if ((object)a == null && (object)b == null)
			{
				return true;
			}
			if ((object)a == null || (object)b == null)
			{
				return false;
			}
			return a.Equals(b);
		}

		public static bool operator !=(AchievementItem a, AchievementItem b)
		{
			return !(a == b);
		}

		public override bool Equals(object obj)
		{
			if (!(obj is AchievementItem))
			{
				return false;
			}
			AchievementItem achievementItem = obj as AchievementItem;
			return achievementItem.Name == Name;
		}

		public override int GetHashCode()
		{
			return name.GetHashCode();
		}

		public override string ToString()
		{
			return name;
		}
	}
}
