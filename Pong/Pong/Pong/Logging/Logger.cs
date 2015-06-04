namespace Pong.Logging
{
	public static class Logger
	{
		public static void Log(string lines)
		{
			{
				using (System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\test.txt", true))
				{
					file.WriteLine(lines);
					file.Close();
				}
			}
		}
	}
}
