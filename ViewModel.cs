using System;
using System.Collections.Generic;
using System.Text;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Security.Cryptography.X509Certificates;

namespace Herbs
{
    public class ViewModel : IDisposable
    {
        private Model model = new Model();

        private readonly CompositeDisposable disposables = new CompositeDisposable();

        public ReactiveCollection<ApplicationInfo> Apps => model.Apps;

        public ReactiveCommand OnGotFocus { get; set; } = new ReactiveCommand();
        public ReactiveCommand OnTopChecked { get; set; } = new ReactiveCommand();
        public ReactiveCommand OnNormalChecked { get; set; } = new ReactiveCommand();
        public ReactiveCommand OnBehindChecked { get; set; } = new ReactiveCommand();

        public ReactiveProperty<ApplicationInfo> Selected { get; set; } = new ReactiveProperty<ApplicationInfo>();

        public ViewModel()
        {
            OnGotFocus.Subscribe(() =>
            {
                model.UpdateAppList();
            }).AddTo(disposables);

            OnTopChecked.Subscribe(n =>
            {
                if(Selected.Value != null)
                {
                    Selected.Value.WindowState = WindowState.Top;
                }
            }).AddTo(disposables);
            OnNormalChecked.Subscribe(n =>
            {
                if (Selected.Value != null)
                {
                    Selected.Value.WindowState = WindowState.Normal;
                }
            }).AddTo(disposables);
        }

        public void Dispose()
        {
            disposables.Dispose();
        }
    }
}
