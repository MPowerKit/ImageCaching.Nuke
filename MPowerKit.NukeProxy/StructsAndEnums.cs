using ObjCRuntime;

namespace MPowerKit.ImageCaching.Nuke
{
	[Native]
	public enum Destination : long
	{
		MemoryCache = 0,
		DiskCache = 1
	}
}
