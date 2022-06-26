// Copyright (C) 2021-2022 Steffen Itterheim
// Usage is bound to the Unity Asset Store Terms of Service and EULA: https://unity3d.com/legal/as_terms

using System.IO;

namespace CodeSmile
{
	public static class DirectoryExt
	{
		/// <summary>
		/// Creates the directory if it does not exist. Does NOT raise exception if directory already exists.
		/// </summary>
		/// <param name="directory"></param>
		/// <returns>Full path to directory.</returns>
		public static string TryCreate(string directory)
		{
			var directoryFullPath = Path.GetFullPath(directory);
#if UNITY_EDITOR
			if (Directory.Exists(directoryFullPath) == false)
				Directory.CreateDirectory(directoryFullPath);
#endif
			return directoryFullPath;
		}
	}
}