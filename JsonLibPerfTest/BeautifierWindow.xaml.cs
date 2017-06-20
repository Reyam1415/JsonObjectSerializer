using System.Windows;
using System.Windows.Documents;

namespace JsonLibPerfTest
{
    public partial class BeautifierWindow : Window
    {
        public BeautifierWindow()
        {
            InitializeComponent();
        }

        public void SetContent(string json)
        {
            this.BeautifyTextBox.Document.Blocks.Clear();
            this.BeautifyTextBox.Document.Blocks.Add(new Paragraph(new Run(json)));
        }
    }
}
