using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 使用者控制項項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234236

namespace Xuite_Player {
    public sealed partial class Loading : UserControl {
        public Loading() {
            this.InitializeComponent();
            this.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        public async void Show() {
            this.Opacity = 0;
            this.Visibility = Windows.UI.Xaml.Visibility.Visible;
            ring.IsActive = true;
            for (int i = 0; i <= 1000; i++) {
                await Task.Delay(TimeSpan.FromSeconds(0.0005));
                this.Opacity = i / 1000.0;
            }
        }

        public async void Hide() {
            this.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            for (int i = 0; i <= 1000; i++) {
                await Task.Delay(TimeSpan.FromSeconds(0.0005));
                this.Opacity = (1000-i) / 1000.0;
            }
            ring.IsActive = false;
        }
    }
}
