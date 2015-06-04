using System;
using System.Threading;
using GameStateManagement;

namespace Pong.PowerUps
{
	public interface IPowerUp
	{
		InputAction ActivationKey { get; }

		Timer Duration { get; set; }

		Boolean IsActive { get; set; }
	}
}
