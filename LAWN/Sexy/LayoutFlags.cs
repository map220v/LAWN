namespace Sexy
{
	public enum LayoutFlags
	{
		LAY_SameWidth = 1,
		LAY_SameHeight = 2,
		LAY_SetLeft = 0x10,
		LAY_SetTop = 0x20,
		LAY_SetWidth = 0x40,
		LAY_SetHeight = 0x80,
		LAY_Above = 0x100,
		LAY_Below = 0x200,
		LAY_Right = 0x400,
		LAY_Left = 0x800,
		LAY_SameLeft = 0x1000,
		LAY_SameRight = 0x2000,
		LAY_SameTop = 0x4000,
		LAY_SameBottom = 0x8000,
		LAY_GrowToRight = 0x10000,
		LAY_GrowToLeft = 0x20000,
		LAY_GrowToTop = 0x40000,
		LAY_GrowToBottom = 0x80000,
		LAY_HCenter = 0x100000,
		LAY_VCenter = 0x200000,
		LAY_Max = 0x400000,
		LAY_SameSize = 3,
		LAY_SameCorner = 20480,
		LAY_SetPos = 48,
		LAY_SetSize = 192
	}
}
