namespace WpfDynamicGUI
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;

    using WpfDynamicGUI.Controls;

    /// <summary>
    /// Interaktionslogik für DynamicFieldDemo.xaml
    /// </summary>
    public partial class DynamicFieldDemo : Window, INotifyPropertyChanged
    {
        private List<DynamicLabelField> labelContent = null;
        public ObservableCollection<DynamicField> FieldFunctions { get; set; }

        public DynamicFieldDemo()
        {
            InitializeComponent();
            this.DataContext = this;

            List<DynamicLabelField> labelField = new List<DynamicLabelField>();
            labelField.Add(new DynamicLabelField("Vorname", typeof(string)));
            labelField.Add(new DynamicLabelField("Nachname", typeof(string)));
            labelField.Add(new DynamicLabelField("Land", typeof(string)));
            labelField.Add(new DynamicLabelField("Postleitzahl", typeof(string)));
            labelField.Add(new DynamicLabelField("Ort", typeof(string)));
            labelField.Add(new DynamicLabelField("Telefon (Private)", typeof(string)));
            labelField.Add(new DynamicLabelField("Telefon (Geschäftlich)", typeof(string)));
            labelField.Add(new DynamicLabelField("eMail (Private)", typeof(string)));
            labelField.Add(new DynamicLabelField("eMail (Geschäftlich)", typeof(string)));
            labelField.Add(new DynamicLabelField("Aktiv", typeof(bool)));
            labelField.Add(new DynamicLabelField("Geburtstag", typeof(DateTime)));
            this.LabelContent = labelField;

            this.FieldFunctions = new ObservableCollection<DynamicField>();
            this.lstFunctions.ItemsSource = this.FieldFunctions;
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

        private void OnDeleteFieldFunction(object sender, RoutedEventArgs e)
        {
            this.FieldFunctions.Remove((sender as FrameworkElement).DataContext as DynamicField);
        }

        private void OnAddFieldFunction(object sender, RoutedEventArgs e)
        {
            DynamicField df = new DynamicField();
            df.ItemSource = this.LabelContent;
            df.FieldName = "Aktiv";
            df.Value = true;
            this.FieldFunctions.Add(df);
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            foreach (DynamicField item in this.FieldFunctions)
            {
                Console.WriteLine($"{item.DynamicLabelField.FieldName}={item.Value}");
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
