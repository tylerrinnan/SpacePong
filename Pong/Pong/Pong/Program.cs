using System;

namespace Pong
{
#if WINDOWS || XBOX
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
			using (Pong pong = new Pong())
			{
				pong.Run();
			}
		}
	}
#endif
}

