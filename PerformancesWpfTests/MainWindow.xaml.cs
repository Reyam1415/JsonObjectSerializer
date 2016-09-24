using Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfLib.Services;

namespace PerformancesWpfTests
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

            watcher.Start();

            var items = Service.GetItems(5);

            stringifyWatcher.Start();
            var json = JsonObjectSerializer.Stringify(items);
            stringifyWatcher.Stop();

            //var json = await ReadFileFromApplicationAsync("items.json");

            parseWatcher.Start();
            var result = JsonObjectSerializer.Parse<List<Item>>(json);
            parseWatcher.Stop();

            watcher.Stop();
            ListView.Items.Add($"[JsonObject] [Total:{watcher.Elapsed.Milliseconds.ToString()}ms] [Stringify:{stringifyWatcher.Elapsed.Milliseconds.ToString()}ms] [Parse:{parseWatcher.Elapsed.Milliseconds.ToString()}ms]");
            DataListView.ItemsSource = result;
        }

        private void TestDataContractJsonSerializerParser()
        {
            var watcher = new Stopwatch();
            var stringifyWatcher = new Stopwatch();
            var parseWatcher = new Stopwatch();

            watcher.Start();

            var items =  Service.GetItems(Convert.ToInt32(TestCountTextBox.Text));
            var knownTypes = new List<Type> { typeof(Item), typeof(OtherItem), typeof(SubItem), typeof(SubSubItem), typeof(List<Item>), typeof(List<OtherItem>) };

            stringifyWatcher.Start();
            var json = DataJsonSerializer.Stringify(items, knownTypes);
            stringifyWatcher.Stop();

            parseWatcher.Start();
            var result = DataJsonSerializer.Parse<List<Item>>(json, knownTypes);
            parseWatcher.Stop();

            watcher.Stop();
            ListView.Items.Add($"[DataContract] [Total:{watcher.Elapsed.Milliseconds.ToString()}ms] [Stringify:{stringifyWatcher.Elapsed.Milliseconds.ToString()}ms] [Parse:{parseWatcher.Elapsed.Milliseconds.ToString()}ms]");
            DataListView.ItemsSource = result;
        }

        private async void TestJsonNetParser()
        {
            var watcher = new Stopwatch();
            var stringifyWatcher = new Stopwatch();
            var parseWatcher = new Stopwatch();

            watcher.Start();

           // var items = await Service.GetItemsAsync(Convert.ToInt32(TestCountTextBox.Text));

            stringifyWatcher.Start();
            //var json = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(items));
            stringifyWatcher.Stop();

            var json = await ReadFileFromApplicationAsync("items.json");

            parseWatcher.Start();
            var result = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<List<Item>>(json));
            parseWatcher.Stop();

            watcher.Stop();
            ListView.Items.Add($"[Json.Net] [Total:{watcher.Elapsed.Milliseconds.ToString()}ms] [Stringify:{stringifyWatcher.Elapsed.Milliseconds.ToString()}ms] [Parse:{parseWatcher.Elapsed.Milliseconds.ToString()}ms]");
            DataListView.ItemsSource = result;
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

        protected async Task<string> ReadFileFromApplicationAsync(string configFile)
        {
            try
            {
                using (var reader = new StreamReader(configFile))
                {
                    return await reader.ReadToEndAsync();
                }
            }
            catch (FileNotFoundException)
            {
                throw new ArgumentException($"Config file '{configFile}' does not found");
            }
        }
    }

    public class Service
    {
        public static List<Item> GetItems(int count = 100)
        {
            var result = new List<Item>();
            for (int i = 0; i < count; i++)
            {
                var item = new Item
                {
                    MyInt = i,
                    MyDouble = 3.14 * i,
                    MyString = "my value string " + i,
                    MyBool = true,
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
                result.Add(item);
            }
            return result;
        }

        public static async Task<List<Item>> GetItemsAsync(int count = 100)
        {
            var t = new Task<List<Item>>(() =>
            {
                var result = new List<Item>();
                for (int i = 0; i < count; i++)
                {
                    var item = new Item
                    {
                        MyInt = i,
                        MyDouble = i / 10,
                        MyString = "my value string " + i,
                        MyBool = true,
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
                    result.Add(item);
                }
                return result;
            });
            t.Start();
            return await t;
        }
    }

    public class Item
    {
        public int MyInt { get; set; }
        public double MyDouble { get; set; }
        public string MyString { get; set; }
        public bool MyBool { get; set; }
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
