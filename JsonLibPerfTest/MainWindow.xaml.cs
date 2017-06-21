using JsonLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UwpLib.Services.Serialization;

namespace JsonLibPerfTest
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        //int testCount = 400;

        private void TestJsonObjectParser()
        {
            var watcher = new Stopwatch();
            var stringifyWatcher = new Stopwatch();
            var parseWatcher = new Stopwatch();

            var items = Service.GetItems(Convert.ToInt32(TestCountTextBox.Text));

            watcher.Start();

            stringifyWatcher.Start();
           // var json = JsonConvert.SerializeObject(items);
            var json = JsonObjectSerializer.Stringify(items);
            stringifyWatcher.Stop();

            parseWatcher.Start();
            var result = JsonObjectSerializer.Parse<List<Item>>(json);
            parseWatcher.Stop();

            watcher.Stop();
            ListView.Items.Add($"[JsonObject] [Total:{watcher.Elapsed.Milliseconds.ToString()}ms] [Stringify:{stringifyWatcher.Elapsed.Milliseconds.ToString()}ms] [Parse:{parseWatcher.Elapsed.Milliseconds.ToString()}ms]");
            if (CheckBox.IsChecked.HasValue && CheckBox.IsChecked == true) DataListView.ItemsSource = result;
            else DataListView.ItemsSource = null;
        }

        private void TestDataContractJsonSerializerParser()
        {
            var watcher = new Stopwatch();
            var stringifyWatcher = new Stopwatch();
            var parseWatcher = new Stopwatch();

            var items = Service.GetItems(Convert.ToInt32(TestCountTextBox.Text));
            var knownTypes = new List<Type> { typeof(Item), typeof(OtherItem), typeof(SubItem), typeof(SubSubItem), typeof(List<Item>), typeof(List<OtherItem>) };

            watcher.Start();

            stringifyWatcher.Start();
            var json = DataJsonSerializer.Stringify(items, knownTypes);
            stringifyWatcher.Stop();

            parseWatcher.Start();
            var result = DataJsonSerializer.Parse<List<Item>>(json, knownTypes);
            parseWatcher.Stop();

            watcher.Stop();
            ListView.Items.Add($"[DataContract] [Total:{watcher.Elapsed.Milliseconds.ToString()}ms] [Stringify:{stringifyWatcher.Elapsed.Milliseconds.ToString()}ms] [Parse:{parseWatcher.Elapsed.Milliseconds.ToString()}ms]");
            if (CheckBox.IsChecked.HasValue && CheckBox.IsChecked == true) DataListView.ItemsSource = result;
            else DataListView.ItemsSource = null;
        }

        private void TestJsonNetParser()
        {
            var watcher = new Stopwatch();
            var stringifyWatcher = new Stopwatch();
            var parseWatcher = new Stopwatch();

            var items = Service.GetItems(Convert.ToInt32(TestCountTextBox.Text));

            watcher.Start();

            stringifyWatcher.Start();
            var json = JsonConvert.SerializeObject(items);
            stringifyWatcher.Stop();

            parseWatcher.Start();
            var result = JsonConvert.DeserializeObject<List<Item>>(json);
            parseWatcher.Stop();

            watcher.Stop();
            ListView.Items.Add($"[Json.Net] [Total:{watcher.Elapsed.Milliseconds.ToString()}ms] [Stringify:{stringifyWatcher.Elapsed.Milliseconds.ToString()}ms] [Parse:{parseWatcher.Elapsed.Milliseconds.ToString()}ms]");
            if (CheckBox.IsChecked.HasValue && CheckBox.IsChecked == true) DataListView.ItemsSource = result;
            else DataListView.ItemsSource = null;
        }

        private void OnTestJsonObject(object sender, RoutedEventArgs e)
        {
            TestJsonObjectParser();
        }

        private void OnTestDataContractJsonSerializer(object sender, RoutedEventArgs e)
        {
            TestDataContractJsonSerializerParser();
        }

        private void OnTestJsonNet(object sender, RoutedEventArgs e)
        {
            TestJsonNetParser();
        }

        private void OnStringifyAndBeautify(object sender, RoutedEventArgs e)
        {
            var items = Service.GetItems(Convert.ToInt32(TestCountTextBox.Text));
            var json = JsonObjectSerializer.StringifyAndBeautify(items);

            var w = new BeautifierWindow
            {
                Width = 800,
                Height = 600,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            w.SetContent(json);
            w.Show();
        }
    }

    public enum MyEnum
    {
        Default,
        Other
    }

    public class Service
    {
        public static List<Item> GetItems(int count = 100)
        {

            var result = new List<Item>();
            for (int i = 1; i < count + 1; i++)
            {
                var item = new Item
                {
                    MyGuid = Guid.NewGuid(),
                    MyInt = i,
                    MyDouble = 3.14 * i,
                    MyString = "my \"escape value\" string " + i,
                    MyBool = true,
                    MyEnum = i % 2 == 0 ? MyEnum.Default : MyEnum.Other,
                    Date = DateTime.Now.AddDays(i),
                    MyText = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit.Nulla tempor est quam, ac blandit augue malesuada quis. Etiam cursus, lorem ac pulvinar elementum, elit lorem vehicula est, quis eleifend tellus ipsum in enim. Mauris semper ac odio quis consectetur.

                    Aliquam tempus metus et massa commodo, non porta purus. Mauris non finibus justo. Aenean ullamcorper orci in gravida placerat. Proin at vehicula ipsum, at laoreet ante. Pellentesque vestibulum quam in nibh auctor, ut pharetra massa feugiat. Phasellus et nulla et ligula volutpat fringilla sed eu enim. Praesent in odio vitae lectus ultricies ultrices. Pellentesque bibendum, ex quis fermentum volutpat, magna ipsum cursus nibh, sed bibendum felis risus vel ante. Praesent elementum nec sem in viverra.",
                    Sub = new SubItem
                    {
                        SubItemInt = 100 + i,
                        SubItemString = "sub item value " + i,
                        SubSub = new SubSubItem
                        {
                            SubSubString = "sub sub value " + i
                        }
                    },
                    MyStrings = new List<string> { "a" + i, "b" + i, "c" + i },
                    Others = new List<OtherItem>
                    {
                        new OtherItem {OtherInt=1000 + i,OtherName="other 1" +i },
                        new OtherItem {OtherInt=2000 + i,OtherName="other 2" + i }
                    }
                };
                if (i % 2 == 0)
                {
                    item.MyNullable = i * 10;
                }
                result.Add(item);
            }
            return result;
        }

        //public static async Task<List<Item>> GetItemsAsync(int count = 100)
        //{
        //    var t = new Task<List<Item>>(() =>
        //    {
        //        var result = new List<Item>();
        //        for (int i = 1; i < count + 1; i++)
        //        {
        //            var item = new Item
        //            {
        //                MyGuid = Guid.NewGuid(),
        //                MyInt = i,
        //                MyDouble = i / 10,
        //                MyString = "my \"value\" string " + i,
        //                MyBool = true,
        //                MyEnum = i % 2 == 0 ? MyEnum.Default : MyEnum.Other,
        //                Date = DateTime.Now.AddDays(i),
        //                Sub = new SubItem
        //                {
        //                    SubItemInt = 100 + i,
        //                    SubItemString = "sub item value " + i,
        //                    SubSub = new SubSubItem
        //                    {
        //                        SubSubString = "sub sub value " + i
        //                    }
        //                },
        //                MyStrings = new List<string> { "a" + i, "b" + i, "c" + i },
        //                Others = new List<OtherItem>
        //            {
        //                new OtherItem {OtherInt=1000 + i,OtherName="other 1" +i },
        //                new OtherItem {OtherInt=2000 + i,OtherName="other 2" + i }
        //            }
        //            };
        //            result.Add(item);
        //        }
        //        return result;
        //    });
        //    t.Start();
        //    return await t;
        //}
    }

    public class Item
    {
        public Guid MyGuid { get; set; }
        public int MyInt { get; set; }
        public double MyDouble { get; set; }
        public string MyString { get; set; }
        public string MyText { get; set; }
        public int? MyNullable { get; set; }
        public bool MyBool { get; set; }
        public DateTime Date { get; set; }
        public MyEnum MyEnum { get; set; }
        public List<string> MyStrings { get; set; }
        public List<OtherItem> Others { get; set; }
        public SubItem Sub { get; set; }
    }


    public class OtherItem
    {
        public int OtherInt { get; set; }
        public string OtherName { get; set; }

        public override string ToString()
        {
            return "OtherInt:" + OtherInt + ",OtherName:" + OtherName;
        }
    }

    public class SubItem
    {
        public int SubItemInt { get; set; }
        public string SubItemString { get; set; }
        public SubSubItem SubSub { get; set; }

        public override string ToString()
        {
            return "SubItemInt:" + SubItemInt + ",SubItemString:" + SubItemString;
        }
    }

    public class SubSubItem
    {
        public string SubSubString { get; set; }

        public override string ToString()
        {
            return SubSubString;
        }
    }

}

