using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Lawn
{
	[DebuggerNonUserCode]
	[CompilerGenerated]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	internal class Strings
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(resourceMan, null))
				{
					ResourceManager resourceManager = resourceMan = new ResourceManager("Lawn.Strings", typeof(Strings).Assembly);
				}
				return resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return resourceCulture;
			}
			set
			{
				resourceCulture = value;
			}
		}

		internal static string CANCEL
		{
			get
			{
				return ResourceManager.GetString("CANCEL", resourceCulture);
			}
		}

		internal static string NO
		{
			get
			{
				return ResourceManager.GetString("NO", resourceCulture);
			}
		}

		internal static string OK
		{
			get
			{
				return ResourceManager.GetString("OK", resourceCulture);
			}
		}

		internal static string Update_Required
		{
			get
			{
				return ResourceManager.GetString("Update_Required", resourceCulture);
			}
		}

		internal static string YES
		{
			get
			{
				return ResourceManager.GetString("YES", resourceCulture);
			}
		}

		internal Strings()
		{
		}
	}
}
