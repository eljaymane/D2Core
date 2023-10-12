using System;
namespace D2Core.core.exceptions
{
	public class XmlStoreFileNotFoundException : Exception
	{
		public XmlStoreFileNotFoundException() : base ("The xml message store file was not found, are you sure of the path ? ")
		{
		}
	}
}

