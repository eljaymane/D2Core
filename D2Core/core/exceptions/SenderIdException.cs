using System;
namespace D2Core.core.exceptions
{
	public class SenderIdException : Exception
	{
		public SenderIdException() : base("Forbidden value for senderId")
		{
		}
	}
}

