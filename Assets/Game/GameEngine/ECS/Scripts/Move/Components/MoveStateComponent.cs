using System;
using UnityEngine;

namespace Game.GameEngine.Ecs
{
	[Serializable]
	public struct MoveStateComponent
	{
		public bool moveRequired;
		public Vector3 direction;
	}
}