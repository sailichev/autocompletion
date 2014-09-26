namespace Autocompletion.TCP
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Net;
	using System.Net.Sockets;
	using System.Text;
	using System.Threading.Tasks;

	using Autocompletion.Interfaces;


	public class Server : IDisposable
	{
		public static readonly Encoding Encoding = Encoding.ASCII;
		public static readonly string Command = "get ";
		public static readonly string NewLine = "\r\n";

		private readonly TcpListener listener;
		private readonly ICollection<TcpClient> clients = new List<TcpClient>();

		public readonly IAutocompletionSource autocompletionSource;

		public Server(int port, IAutocompletionSource autocompletionSource)
		{
			this.autocompletionSource = autocompletionSource;

			this.listener = new TcpListener(IPAddress.Any, port);

			this.listener.Start();

			Task.Factory.StartNew(Listen);
		}

		public void Dispose()
		{
			listener.Stop();

			foreach (var client in clients)
				if (client.Connected)
					client.Close();

			clients.Clear();
		}

		private void Listen()
		{
			do
			{
				try
				{
					var client = listener.AcceptTcpClient();
					
					Task.Factory.
						StartNew(
							() => Loop(client)
						).ContinueWith(
							_ => client.Close()
						);
					
					clients.Add(client);
				}
				catch (SocketException)
				{
					break;
				}
			} while (true);
		}

		private void Loop(TcpClient client)
		{
			do
			{
				var readBuf = new byte[40]; // max <prefix> length is defined to be 15 symbols so 40 bytes is enough anyways, including the command itself

				try
				{
					var stream = client.GetStream();

					var readCount = stream.Read(readBuf, 0, readBuf.Length);

					if (readCount == 0)
						break;

					var readString = Encoding.GetString(readBuf, 0, readCount);

					if (readString.StartsWith(Command))
					{
						var prefix = readString.Substring(Command.Length);
						var result = this.autocompletionSource.GetStrings(prefix);

						var writeBuf = Encoding.GetBytes(string.Join(NewLine, result));
						stream.Write(writeBuf, 0, writeBuf.Length);
					}
				}
				catch (SocketException)
				{
					break;
				}
				catch (IOException)
				{
					break;
				}
				
			} while (true);
		}
	}
}
