using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Diagnostics;
using System.Collections.Specialized;
using System.ComponentModel;
using Prism.Mvvm;
using Reactive.Bindings;

namespace Herbs
{
    public class Model : BindableBase, IDisposable
    {
        private ReactiveCollection<ApplicationInfo> apps = new ReactiveCollection<ApplicationInfo>();

        public ReactiveCollection<ApplicationInfo> Apps => apps;

        public Model()
        {
            UpdateAppList();
        }

        public void UpdateAppList()
        {
            apps.Clear();
            apps.AddRangeOnScheduler(Process.GetProcesses()
                                                    .Where(n => !string.IsNullOrEmpty(n.MainWindowTitle))
                                                    .Where(n => !InValildApplicationMap.IsInVaild(n.MainWindowTitle))
                                                    .Where(n => n.MainWindowHandle != IntPtr.Zero)
                                                    .Where(n => n.MainModule.ModuleName != Process.GetCurrentProcess().MainModule.ModuleName)
                                                    .Select(n => new ApplicationInfo(n)));
        }

        public void Dispose()
        {
            foreach (var item in apps)
            {
                if (item.WindowState != WindowState.Normal) item.WindowState = WindowState.Normal;
            }
        }
    }
}
