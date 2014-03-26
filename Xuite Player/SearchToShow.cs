using Exa.Format.JSON;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Xuite_Player {
    public class ItemData {
        public string vlog_id{get;set;}
        public string title { get; set; }
        public string pub_time { get; set; }
        public string duration { get; set; }
        public string thumb { get; set; }
    }
    class SearchToShow : ObservableCollection<ItemData>, ISupportIncrementalLoading {
        public string keyword;
        public int flag = 0;
        public int count = 0;
        public Json.Array Now;
        public ItemData Item;

        public SearchToShow(string keyword) {
            this.keyword = keyword;
            flag = 0;
        }

        delegate string md5(string value);
        private string CreateAPIUrl() {
            md5 MD5 = value => Exa.Text.Convert.GetMD5(value);
            string url = "http://api.xuite.net/api.php?" +
                         "api_key={0}&" +
                         "api_sig={1}&" +
                         "method={2}&" +
                         "stype={3}&" +
                         "q={4}&" +
                         "s={5}&" +
                         "start={6}&" +
                         "limit={7}";
            string api_key = "3a96cc87f505f4f323a53a8a21599fdd";
            string sec_key = "9950964948";
            string method = "xuite.vlog.public.searchVlogs";
            string stype = "title";
            string q = Exa.Text.Convert.StringToUnicode(keyword);
            string s = "time";
            string start = flag.ToString();
            string limit = "10";
            //A B C D E F G H I J K L M N O P Q R S T U V W X Y Z
            string TEMP = sec_key + api_key + limit + method + q + s + start + stype;
            string sig = MD5(TEMP);
            sig = sig.ToLower();
            return string.Format(url, api_key, sig, method, stype, Uri.EscapeUriString(q), s, start, limit);
        }
        public static Task<string> GetHTML(string url) {
            Task<string> HTML=null;
            HttpClient client = new HttpClient();
            int errorcount = 0;
            do {
                try {
                    HTML = client.GetStringAsync(url);
                } catch {}
            } while (++errorcount < 10 && string.IsNullOrEmpty(HTML.Result));
            return HTML;
        }

        public static string temp00;
        public async Task<bool> Read() {
            READED = true;
            if (flag % 10 == 0) {
                string url = CreateAPIUrl();
                string Data = await GetHTML(url);//等候
                temp00 = Data;
                Json.Object temp=null;
                    temp = new Json.Object(Data);
               
                if ((bool)temp["ok"]) {
                    count = int.Parse(temp["rsp", "total"].ToString());
                    Now = (Json.Array)temp["rsp", "vlogs"];
                } else {
                    return false;
                }
            }
            if (flag == count || count == 0) return false;
            this.Item = new ItemData();
            this.Item.FromJson((Json.Object)Now[flag % 10], true);

            this.Item.thumb = Regex.Unescape(this.Item.thumb);
            this.Item.title = Regex.Unescape(this.Item.title);
            flag++;
            return true;
        }

        bool READED=false;

        public bool HasMoreItems {//類似HAS NEXT
            get {
                if (!READED) return true;
                if (flag >= count) return false;
                return true;
            }
        }
        public Windows.Foundation.IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count) {
            CoreDispatcher coreDispatcher = Window.Current.Dispatcher;
            return Task.Run<LoadMoreItemsResult>(async () => {
                List<ItemData> items = new List<ItemData>();
                int NowCount = 0;
                while (await Read()) {
                    items.Add(this.Item);
                    if (++NowCount == count) break;
                }
                
                await coreDispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () => {
                        foreach (ItemData item in items) {
                            this.Add(item);
                        }
                    });

                return new LoadMoreItemsResult() { Count = count };
            }).AsAsyncOperation<LoadMoreItemsResult>();
        }
    }
}
