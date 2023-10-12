using System;
namespace D2App
{
	public class DeviceComboDataSource : NSComboBoxDataSource
	{
		readonly List<string> source;

		public DeviceComboDataSource(List<string> source)
		{
			this.source = source;
		}

        public override string CompletedString(NSComboBox comboBox, string uncompletedString)
        {
            return source.Find(n => n.StartsWith(uncompletedString, StringComparison.InvariantCultureIgnoreCase));
        }

        public override nint IndexOfItem(NSComboBox comboBox, string value)
        {
            return source.FindIndex(n => n.Equals(value, StringComparison.InvariantCultureIgnoreCase));
        }

        public override nint ItemCount(NSComboBox comboBox)
        {
            return source.Count;
        }

        public override NSObject ObjectValueForItem(NSComboBox comboBox, nint index)
        {
            return NSObject.FromObject(source[(int)index]);
        }
    }
}

