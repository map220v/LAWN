using System;

namespace Sexy
{
	[Flags]
	internal enum PrimitiveBatchEffects
	{
		None = 0x0,
		MirrorVertically = 0x1,
		MirrorHorizontally = 0x2
	}
}
