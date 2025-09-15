namespace WPFDynamicGUI.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    using WPFDynamicGUI.Core;

    /// <summary>
    /// Interaktionslogik für AddName.xaml
    /// </summary>
    public partial class AddName : UserControl
    {
        public AddName()
        {
            this.InitializeComponent();
            this.LostFocus += AddName_LostFocus;
        }

        private void AddName_LostFocus(object sender, RoutedEventArgs e)
        {
            AddNameArgs args = new AddNameArgs();
            args.Sender = this;
            args.Title = this.txtTitle.Text;
            args.Salutation = this.txtSalutation.Text;
            args.Firstname = this.txtFirstname.Text;
            args.Lastname = this.txtLastname.Text;
            App.EventAgg.Publish<AddNameArgs>(args);
        }
    }
}