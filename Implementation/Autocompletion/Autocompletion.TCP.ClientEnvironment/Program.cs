namespace Autocompletion.TCP.ClientEnvironment
{
	using System;


	public static class Program
	{
		public static void Main(string[] args)
		{
			string host = args[0];
			int port = int.Parse(args[1]);

			using (var client = new Client(host, port))
			{
				do
				{
					var prefix = Console.ReadLine();

					foreach (var a in client.GetStrings(prefix))
						Console.WriteLine(a);
				} while (true);
			}
		}
	}
}
