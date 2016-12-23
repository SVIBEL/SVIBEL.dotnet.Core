using System;
using SVIBEL.Core.Common.Components;

namespace SVIBEL.Core.Common
{
	public class ConsoleLogger : ILogger
	{
		public event EventHandler Started;

		public ConsoleLogger()
		{
		}

		public bool IsStarted
		{
			get;
			private set;
		}

		public void Build(BuildParams buildParams)
		{
		}

		public void Log(string log)
		{
			Console.WriteLine(log);
		}

		public void Log(string format, params string[] items)
		{
			Console.WriteLine(string.Format(format, items));
		}

		public void RaiseStarted()
		{
			Started?.Invoke(this, EventArgs.Empty);
		}

		public void Start(object args)
		{
			IsStarted = true;
			RaiseStarted();
		}

		public void Stop()
		{
			IsStarted = false;
		}
	}
}
