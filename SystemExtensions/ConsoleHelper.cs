namespace System
{
	public static class ConsoleHelper
	{
		public static void PauseBeforeExit()
		{
			Console.Write("Press any key to exit...");
			Console.ReadKey();
		}
	}
}