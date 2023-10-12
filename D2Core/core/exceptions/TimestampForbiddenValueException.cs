using System;
namespace D2Core.core.exceptions
{
	public class TimestampForbiddenValueException : Exception
	{
		public TimestampForbiddenValueException() : base("Forbidden value for timestamp field")
		{
		}
	}
}

