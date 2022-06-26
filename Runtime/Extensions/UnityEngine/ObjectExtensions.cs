using UnityEngine;

namespace CodeSmile
{
	public static class ObjectExt
	{
		/// <summary>
		/// Destroys the object safely, regardless of mode.
		/// Depending on the mode (editor vs playmode/player) it calls either DestroyImmediate or Destroy.
		/// </summary>
		/// <param name="self"></param>
		public static void DestroyInAnyMode(this Object self)
		{
			if (Application.isEditor)
				Object.DestroyImmediate(self);
			else
				Object.Destroy(self);
		}
	}
}