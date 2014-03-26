using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// 使用者控制項項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234236

namespace Xuite_Player {
    public sealed partial class SearchItem : UserControl {
        public string VID{get;set;}
        public string title {
            set {
                try {
                    this.Title.Text = value;
                } catch { }
            }
            get {
                return this.Title.Text;
            }
        }

        public string time {
            set {
                try {
                    this.TimeB.Text = value;
                    this.TimeT.Text = this.TimeB.Text;
                } catch { }
            }
            get {
                return this.TimeB.Text;
            }
        }

        string URL;
        public string url {
            set {
                URL = value;
                ImageBrush bg = new ImageBrush();
                bg.ImageSource = new BitmapImage(new Uri(value));
                this.BaseGrid.Background = bg;
            }
            get {
                return URL;
            }
        }

        public SearchItem() {
            title = "";
            time = "00:00";
        }

        public void SetBackground(string Url) {
            ImageBrush bg = new ImageBrush();
            bg.ImageSource = new BitmapImage(new Uri(Url));
            this.BaseGrid.Background = bg;
        }

        public void SetBackground(Brush value) {
            this.BaseGrid.Background = value;
        }
    }
}
