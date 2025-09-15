namespace WpfDynamicGUI
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;

    using WpfDynamicGUI.Controls;

    using WPFDynamicGUI;

    /// <summary>
    /// Interaktionslogik für DynamicFieldDemo.xaml
    /// </summary>
    public partial class DynamicFieldDemo : Window, INotifyPropertyChanged
    {
        private List<DynamicLabelField> labelContent = null;

        public DynamicFieldDemo()
        {
            InitializeComponent();
            this.DataContext = this;

            List<DynamicLabelField>  labelField = new List<DynamicLabelField>();
            labelField.Add(new DynamicLabelField("Land", typeof(string)));
            labelField.Add(new DynamicLabelField("Postleitzahl", typeof(string)));
            labelField.Add(new DynamicLabelField("Ort", typeof(string)));
            labelField.Add(new DynamicLabelField("Telefon (Private)", typeof(string)));
            labelField.Add(new DynamicLabelField("Telefon (Geschäftlich)", typeof(string)));
            labelField.Add(new DynamicLabelField("eMail (Private)", typeof(string)));
            labelField.Add(new DynamicLabelField("eMail (Geschäftlich)", typeof(string)));
            this.LabelContent = labelField;
        }

        public List<DynamicLabelField> LabelContent
        {
            get { return labelContent; }
            set
            {
                labelContent = value;
                this.OnPropertyChanged();
            }
        }

        #region INotifyPropertyChanged Implementierung
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler == null)
            {
                return;
            }

            var e = new PropertyChangedEventArgs(propertyName);
            handler(this, e);
        }
        #endregion INotifyPropertyChanged Implementierung
    }
}
