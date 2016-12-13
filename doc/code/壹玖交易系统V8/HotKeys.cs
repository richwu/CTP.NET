using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class HotKeys
{
	public delegate void HotKeyCallBackHanlder(int id);

	public enum HotkeyModifiers
	{
		Alt = 1,
		Control,
		Shift = 4,
		Win = 8
	}

	public System.Collections.Generic.Dictionary<int, HotKeys.HotKeyCallBackHanlder> keymap = new System.Collections.Generic.Dictionary<int, HotKeys.HotKeyCallBackHanlder>();

	[System.Runtime.InteropServices.DllImport("user32.dll")]
	private static extern bool RegisterHotKey(System.IntPtr hWnd, int id, int modifiers, System.Windows.Forms.Keys vk);

	[System.Runtime.InteropServices.DllImport("user32.dll")]
	private static extern bool UnregisterHotKey(System.IntPtr hWnd, int id);

	public void Regist(System.IntPtr hWnd, int keyid, int modifiers, System.Windows.Forms.Keys vk, HotKeys.HotKeyCallBackHanlder callBack)
	{
		if (!HotKeys.RegisterHotKey(hWnd, keyid, modifiers, vk))
		{
			throw new System.Exception(keyid + "快捷键注册失败！检查是否重复或者被其它软件拦截");
		}
		this.keymap[keyid] = callBack;
	}

	public void UnRegist(System.IntPtr hWnd, HotKeys.HotKeyCallBackHanlder callBack)
	{
		foreach (System.Collections.Generic.KeyValuePair<int, HotKeys.HotKeyCallBackHanlder> current in this.keymap)
		{
			if (current.Value == callBack)
			{
				HotKeys.UnregisterHotKey(hWnd, current.Key);
			}
		}
	}

	public void UnRegist(System.IntPtr hWnd, int key)
	{
		HotKeys.UnregisterHotKey(hWnd, key);
	}

	public void ProcessHotKey(System.Windows.Forms.Message m)
	{
		if (m.Msg == 786)
		{
			int num = m.WParam.ToInt32();
			HotKeys.HotKeyCallBackHanlder hotKeyCallBackHanlder;
			if (this.keymap.TryGetValue(num, out hotKeyCallBackHanlder))
			{
				hotKeyCallBackHanlder(num);
			}
		}
	}
}
