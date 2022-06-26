// Copyright (C) 2021-2022 Steffen Itterheim
// Usage is bound to the Unity Asset Store Terms of Service and EULA: https://unity3d.com/legal/as_terms

using System.Linq;
using System.Text;
using UnityEngine.LowLevel;

namespace CodeSmile
{
	public static class PlayerLoopExt
	{
		/// <summary>
		/// Inserts a PlayerLoopSystem before the given other system.
		/// </summary>
		/// <param name="afterSystem"></param>
		/// <param name="insertSystem"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns>False if the afterSystem could not be found.</returns>
		public static bool InsertSystemAfter<T>(ref PlayerLoopSystem afterSystem, ref PlayerLoopSystem insertSystem)
		{
			if (afterSystem.type == typeof(T))
				return true;

			if (afterSystem.subSystemList != null)
			{
				for (var i = 0; i < afterSystem.subSystemList.Length; i++)
				{
					if (InsertSystemAfter<T>(ref afterSystem.subSystemList[i], ref insertSystem))
					{
						var systems = afterSystem.subSystemList.ToList();
						systems.Insert(i, systems[i]); // duplicate existing
						systems[i + 1] = insertSystem; // replace previous system
						afterSystem.subSystemList = systems.ToArray();
						return false;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Inserts a PlayerLoopSystem before the given other system.
		/// </summary>
		/// <param name="beforeSystem"></param>
		/// <param name="insertSystem"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns>False if the beforeSystem could not be found.</returns>
		public static bool InsertSystemBefore<T>(ref PlayerLoopSystem beforeSystem, ref PlayerLoopSystem insertSystem)
		{
			if (beforeSystem.type == typeof(T))
				return true;

			if (beforeSystem.subSystemList != null)
			{
				for (var i = 0; i < beforeSystem.subSystemList.Length; i++)
				{
					if (InsertSystemBefore<T>(ref beforeSystem.subSystemList[i], ref insertSystem))
					{
						var systems = beforeSystem.subSystemList.ToList();
						systems.Insert(i, insertSystem);
						beforeSystem.subSystemList = systems.ToArray();
						return false;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Replaces a PlayerLoopSystem with another.
		/// </summary>
		/// <param name="systemToReplace"></param>
		/// <param name="newSystem"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns>False if the systemToReplace was not found.</returns>
		public static bool ReplaceSystem<T>(ref PlayerLoopSystem systemToReplace, ref PlayerLoopSystem newSystem)
		{
			if (systemToReplace.type == typeof(T))
			{
				systemToReplace = newSystem;
				return true;
			}

			if (systemToReplace.subSystemList != null)
			{
				for (var i = 0; i < systemToReplace.subSystemList.Length; i++)
				{
					if (ReplaceSystem<T>(ref systemToReplace.subSystemList[i], ref newSystem))
						return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Returns a PlayerLoopSystem string for logging.
		/// </summary>
		/// <param name="system"></param>
		/// <returns></returns>
		public static string LogSubSystems(PlayerLoopSystem system)
		{
			var sb = new StringBuilder();
			return LogSubSystems(system.subSystemList, sb).ToString();
		}

		private static StringBuilder LogSubSystems(PlayerLoopSystem[] systems, StringBuilder sb, string prefix = "")
		{
			if (systems == null || systems.Length == 0)
				return sb;

			var count = 0;
			foreach (var playerLoopSystem in systems)
			{
				var delegateLog = playerLoopSystem.updateDelegate != null ? $" => delegate: {playerLoopSystem.updateDelegate}" : "";
				sb.AppendLine($"{prefix}[{count++}] = {playerLoopSystem.type}{delegateLog}");

				LogSubSystems(playerLoopSystem.subSystemList, sb, prefix + "   ");
			}

			return sb;
		}
	}
}