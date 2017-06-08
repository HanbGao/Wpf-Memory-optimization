using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Wpf.MemoryOptimization
{
    public class MemoryOptimization
    {
        private const string Data = "SOFTWARE\\YourApp\\Components";

        private const string Keys = "LastAboutShowedTime";

        private const string Formate = "MM/dd/yyyy HH:mm:ss";

        private const string Disable = "DisableSmartTag";

        private const string SmartTagWidth = "SmartTagWidth";

        private void SetDate()
        {
            CreateKey();
            var expr_0B = Registry.CurrentUser;
            var expr17 = expr_0B.OpenSubKey(Data, true);
            if (expr17 != null)
            {
                expr17.GetValue(Keys);
                var value = DateTime.Now.ToString(Formate);
                expr17.SetValue(Keys, value);
            }
            expr_0B.Dispose();
        }

        [DllImport("kernel32.dll")]
        private static extern bool SetProcessWorkingSetSize(IntPtr proc, int min, int max);

        private void FlushMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
            }
        }

        private void CreateKey()
        {
            var currentUser = Registry.CurrentUser;
            if (currentUser.OpenSubKey(Data, true) == null)
            {
                var expr_1F = currentUser.CreateSubKey(Data);
                if (expr_1F != null)
                {
                    var registryKey = expr_1F.CreateSubKey(Keys);
                    if (registryKey != null)
                        registryKey.SetValue(Keys, DateTime.Now.ToString(Formate));
                    var subKey = expr_1F.CreateSubKey(Disable);
                    if (subKey != null)
                        subKey.SetValue(Keys, false);
                    var key = expr_1F.CreateSubKey(SmartTagWidth);
                    if (key != null)
                        key.SetValue(Keys, 350);
                }
            }
            currentUser.Dispose();
        }

        public void Cracker(int sleepSpan = 30)
        {
            Task.Factory.StartNew(delegate
            {
                while (true)
                {
                    try
                    {
                        SetDate();
                        FlushMemory();
                        Thread.Sleep(TimeSpan.FromSeconds((double)sleepSpan));
                    }
                    catch (Exception)
                    {
                    }
                }
            });
        }
    }
}
