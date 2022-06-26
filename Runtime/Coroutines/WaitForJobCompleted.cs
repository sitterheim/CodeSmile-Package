// Copyright (C) 2021-2022 Steffen Itterheim
// Usage is bound to the Unity Asset Store Terms of Service and EULA: https://unity3d.com/legal/as_terms

using System.Collections;
using Unity.Jobs;
using UnityEngine;

namespace CodeSmile
{
	/// <summary>
	/// Coroutine that waits until the given JobHandle is complete.
	/// Note that Coroutines are checked only once per frame. If there is a good chance the JobHandle completes within the same frame
	/// or where it is important that the JobHandle completes in the same frame you should not use this coroutine.
	/// </summary>
	public class WaitForJobCompleted : IEnumerator
	{
		private readonly bool _isUsingTempJobAllocator;
		private readonly int _forceJobCompletionFrameCount;
		private JobHandle _jobHandle;

		public object Current => null;

		public WaitForJobCompleted(JobHandle jobHandle, bool isUsingTempJobAllocator = true)
		{
			_jobHandle = jobHandle;
			_isUsingTempJobAllocator = isUsingTempJobAllocator;

			// force completion before running into native Allocator.TempJob's lifetime limit of 4 frames
			_forceJobCompletionFrameCount = Time.frameCount + 4;
		}

		public bool MoveNext()
		{
			if (_isUsingTempJobAllocator && Time.frameCount >= _forceJobCompletionFrameCount)
				_jobHandle.Complete();

			return _jobHandle.IsCompleted == false;
		}

		public void Reset() {}
	}
}