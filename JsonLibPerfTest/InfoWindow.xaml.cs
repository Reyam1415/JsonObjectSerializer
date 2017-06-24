using System.Windows;
using System.Windows.Documents;

namespace JsonLibPerfTest
{
    public partial class InfoWindow : Window
    {
        public InfoWindow()
        {
            InitializeComponent();
        }

        public void SetContent(string json)
        {
            this.TextBox.Document.Blocks.Clear();
            this.TextBox.Document.Blocks.Add(new Paragraph(new Run(json)));
        }
    }
}
