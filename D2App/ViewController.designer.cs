// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace D2App
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSScrollView ChatBox { get; set; }

		[Outlet]
		AppKit.NSButton ChatSendBtn { get; set; }

		[Outlet]
		AppKit.NSTextView ChatText { get; set; }

		[Outlet]
		AppKit.NSComboBox DeviceCombo { get; set; }

		[Outlet]
		AppKit.NSTextField MsgText { get; set; }

		[Outlet]
		AppKit.NSButton StartBtn { get; set; }

		[Action ("DeviceSelected:")]
		partial void DeviceSelected (AppKit.NSComboBox sender);

		[Action ("MsgTxt:")]
		partial void MsgTxt (AppKit.NSTextField sender);

		[Action ("SendChat:")]
		partial void SendChat (AppKit.NSButton sender);

		[Action ("StartCapture:")]
		partial void StartCapture (AppKit.NSButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ChatText != null) {
				ChatText.Dispose ();
				ChatText = null;
			}

			if (ChatBox != null) {
				ChatBox.Dispose ();
				ChatBox = null;
			}

			if (ChatSendBtn != null) {
				ChatSendBtn.Dispose ();
				ChatSendBtn = null;
			}

			if (DeviceCombo != null) {
				DeviceCombo.Dispose ();
				DeviceCombo = null;
			}

			if (MsgText != null) {
				MsgText.Dispose ();
				MsgText = null;
			}

			if (StartBtn != null) {
				StartBtn.Dispose ();
				StartBtn = null;
			}
		}
	}
}
