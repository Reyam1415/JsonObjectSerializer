using Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private void TestJsonObjectParser()
        {
            var watcher = new Stopwatch();
            watcher.Start();

            var items = Service.GetItems(200);

            var json = JsonObjectSerializer.Stringify(items);

            var result = JsonObjectSerializer.Parse<List<Item>>(json);

            watcher.Stop();
            ListView.Items.Add("JsonObject : " + watcher.Elapsed.Milliseconds.ToString() + "ms");
            DataListView.ItemsSource = result;
        }

        private void TestDataContractJsonSerializerParser()
        {
            var watcher = new Stopwatch();
            watcher.Start();

            var items = Service.GetItems(200);
            var knownTypes = new List<Type> { typeof(Item), typeof(OtherItem), typeof(SubItem),typeof(SubSubItem), typeof(List<Item>), typeof(List<OtherItem>) };

            var json = DataJsonSerializer.Stringify(items,knownTypes);

            var result = DataJsonSerializer.Parse<List<Item>>(json,knownTypes);

            watcher.Stop();
            ListView.Items.Add("DataContractJsonSerializer : " + watcher.Elapsed.Milliseconds.ToString() + "ms");
            DataListView.ItemsSource = result;
        }

        private void TestJsonNetParser()
        {
            var watcher = new Stopwatch();
            watcher.Start();

            var items = Service.GetItems(200);

            var json = JsonConvert.SerializeObject(items);

            var result = JsonConvert.DeserializeObject<List<Item>>(json);

            watcher.Stop();
            ListView.Items.Add("Json.Net : " + watcher.Elapsed.Milliseconds.ToString() + "ms");
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
    }

    public class Service
    {
        public static List<Item> GetItems(int count=100)
        {
            var result = new List<Item>();
            for (int i = 0; i < count ; i++)
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
