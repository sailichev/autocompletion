namespace Autocompletion.TCP
{
	using System;
	using System.Net.Sockets;
	using System.Collections.Generic;

	using Autocompletion.Interfaces;


	public class Client : IAutocompletionSource, IDisposable
	{
		private readonly TcpClient tcpClient;

		public Client(string host, int port)
		{
			this.tcpClient = new TcpClient(host, port);
		}

		public void Dispose()
		{
			this.tcpClient.Close();
		}

		public IEnumerable<string> GetStrings(string prefix)
		{
			var stream = tcpClient.GetStream();

			var writeBuf = Server.Encoding.GetBytes(Server.Command + prefix);
			stream.Write(writeBuf, 0, writeBuf.Length);

			var readBuf = new byte[8192];
			var readCount = stream.Read(readBuf, 0, readBuf.Length);
			var readString = Server.Encoding.GetString(readBuf, 0, readCount);

			return readString.TrimEnd().Split(new string[1] { Server.NewLine }, StringSplitOptions.None);
		}
	}
}
