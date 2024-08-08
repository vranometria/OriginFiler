using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Input;

namespace OriginFiler.Models
{
    public class HotkeyHelper : IDisposable
    {
        /// <summary>Hotkey IDの最大採番</summary>
        private const int MAX_HOTKEY_ID = 0xC000;

        private const int WM_HOTKEY = 0x0312;

        private IntPtr WindowHandle { set; get; }

        private int HotkeyId { get; set; }

        private EventHandler? EventHandler { get; set; }


        /// <summary>ホットキーが登録されているか否か</summary>
        public bool Registerd { get; private set; }


        [DllImport("user32.dll")]
        private static extern int RegisterHotKey(IntPtr hWnd, int id, int modKey, int vKey);


        [DllImport("user32.dll")]
        private static extern int UnregisterHotKey(IntPtr hWnd, int id);


        private HotkeyHelper(Window window)
        {
            var host = new WindowInteropHelper(window);
            WindowHandle = host.Handle;

            ComponentDispatcher.ThreadPreprocessMessage += ComponentDispatcher_ThreadPreprocessMessage;
        }

        public HotkeyHelper(Window window, EventHandler eventHandler) : this(window)
        {
            EventHandler = eventHandler;
        }


        private void ComponentDispatcher_ThreadPreprocessMessage(ref MSG msg, ref bool handled)
        {
            if (msg.message != WM_HOTKEY) { return; }

            var id = msg.wParam.ToInt32();

            if (HotkeyId == id)
            {
                EventHandler?.Invoke(this, EventArgs.Empty);
            }

        }


        public void Dispose()
        {
            if (Registerd)
            {
                Unregister();
            }
        }

        /// <summary>
        /// ホットキーを登録する
        /// </summary>
        /// <param name="modifierKeys"></param>
        /// <param name="key"></param>
        /// <param name="handler"></param>
        /// <returns>登録できなかった場合はfalseを返す</returns>
        public bool Register(ModifierKeys modifierKeys, Key key)
        {
            int hotkeyId = 0x0000;

            while (hotkeyId < MAX_HOTKEY_ID)
            {
                // HotKey登録
                var registerReturn = RegisterHotKey(
                    WindowHandle,
                    hotkeyId,
                    (int)modifierKeys,
                    KeyInterop.VirtualKeyFromKey(key));

                bool registerSucceeded = registerReturn != 0;

                if (registerSucceeded)
                {
                    HotkeyId = hotkeyId;
                    Registerd = true;
                    return true;
                }

                hotkeyId++;
            }

            return false;
        }


        /// <summary>
        /// 登録済みのホットキー解除
        /// </summary>
        /// <returns>解除に失敗した場合はtrue</returns>
        public bool Unregister()
        {
            bool failed = UnregisterHotKey(WindowHandle, HotkeyId) == 0;

            if (!failed)
            {
                Registerd = false;
            }

            return failed;
        }

    }
}
