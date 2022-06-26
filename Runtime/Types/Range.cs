// Copyright (C) 2021-2022 Steffen Itterheim
// Usage is bound to the Unity Asset Store Terms of Service and EULA: https://unity3d.com/legal/as_terms

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace CodeSmile
{
	/// <summary>
	/// Provides operations on a Min/Max range of values.
	/// Min and Max are inclusive, ie a range of Min=0 and Max=1 allows 0 and 1.
	/// Based on: https://stackoverflow.com/a/5343033/18162198 (Licensed under CC-BY-SA 3.0)
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[Serializable]
	public struct Range<T> where T : IComparable<T>
	{
		[SerializeField] private T _min;
		[SerializeField] private T _max;

		public Range(T min, T max)
		{
			_min = min;
			_max = max;

			if (LessThanMin(_max))
				throw new ArgumentException($"Range Min value ({_min}) must be less or equal than Max value ({_max})");
		}

		public T Min => _min;
		public T Max => _max;

		public bool LessThanMin(T value) => _min.CompareTo(value) > 0;
		public bool GreaterThanMax(T value) => _max.CompareTo(value) > 0;
		public bool Contains(T value) => _min.CompareTo(value) <= 0 && value.CompareTo(_max) <= 0;
		public bool Contains(Range<T> range) => Contains(range.Min) && Contains(range.Max);
		public T Clamp(T value) => Contains(value) ? value : LessThanMin(value) ? _min : _max;

		public override string ToString() => $"Range from {_min} to {_max}";
	}
}