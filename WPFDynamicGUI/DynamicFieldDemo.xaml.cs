namespace WpfDynamicGUI
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Windows;

    using WpfDynamicGUI.Controls;

    using WPFDynamicGUI;
    using WPFDynamicGUI.Core;

    /// <summary>
    /// Interaktionslogik für DynamicFieldDemo.xaml
    /// </summary>
    public partial class DynamicFieldDemo : Window, INotifyPropertyChanged
    {
        private List<DynamicLabelField> labelContent = null;
        private ISubscription<AddNameArgs> subAddName;

        public ObservableCollection<DynamicField> FieldFunctions { get; set; }

        public DynamicFieldDemo()
        {
            InitializeComponent();
            this.subAddName = App.EventAgg.Subscribe<AddNameArgs>(AddNameArgsEventHandler);

            this.DataContext = this;

            List<DynamicLabelField> labelField = new List<DynamicLabelField>();
            labelField.Add(new DynamicLabelField("Land", typeof(string)));
            labelField.Add(new DynamicLabelField("Postleitzahl", typeof(string)));
            labelField.Add(new DynamicLabelField("Strasse", typeof(string)));
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

        private void OnRemoveFieldFunction(object sender, RoutedEventArgs e)
        {
            this.FieldFunctions.Remove((sender as FrameworkElement).DataContext as DynamicField);
        }

        private void OnAddFieldFunction(object sender, RoutedEventArgs e)
        {
            DynamicField df = new DynamicField();
            df.ItemSource = this.LabelContent;
            df.RemoveFieldEvent += new Action<RemoveEventArgs>(this.OnRemoveFieldFunction);
            this.FieldFunctions.Add(df);
        }

        private void OnRemoveFieldFunction(RemoveEventArgs obj)
        {
            DynamicField df = obj.Field;
            this.FieldFunctions.Remove(df);
        }

        private void OnDataLoad(object sender, RoutedEventArgs e)
        {
            DynamicLabelField dlf0 = this.LabelContent[0];
            dlf0.Value = "DE";
            DynamicField df = new DynamicField(dlf0);
            df.ItemSource = this.LabelContent;
            df.RemoveFieldEvent += new Action<RemoveEventArgs>(this.OnRemoveFieldFunction);
            this.FieldFunctions.Add(df);

            DynamicLabelField dlf9 = this.LabelContent[8];
            dlf9.Value = true;
            df = new DynamicField(dlf9);
            df.ItemSource = this.LabelContent;
            df.RemoveFieldEvent += new Action<RemoveEventArgs>(this.OnRemoveFieldFunction);
            this.FieldFunctions.Add(df);

            DynamicLabelField dlf10 = this.LabelContent[9];
            dlf10.Value = DateTime.Now.Date;
            df = new DynamicField(dlf10);
            df.ItemSource = this.LabelContent;
            df.RemoveFieldEvent += new Action<RemoveEventArgs>(this.OnRemoveFieldFunction);
            this.FieldFunctions.Add(df);
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DynamicField item in this.FieldFunctions)
            {
                sb.AppendLine($"{item.DynamicLabelField.FieldName} = {item.Value}");
            }

            if (sb.Length > 0)
            {
                MessageBox.Show(sb.ToString(), "Speichern");
            }
        }

        private void AddNameArgsEventHandler(AddNameArgs eventArgs)
        {
            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrEmpty(eventArgs.Title) == false)
            {
                sb.Append($"{eventArgs.Title}");
            }

            if (string.IsNullOrEmpty(eventArgs.Salutation) == false)
            {
                sb.Append(" ").Append($"{eventArgs.Salutation}");
            }

            if (string.IsNullOrEmpty(eventArgs.Firstname) == false)
            {
                sb.Append(" ").Append($"{eventArgs.Firstname}");
            }

            if (string.IsNullOrEmpty(eventArgs.Lastname) == false)
            {
                sb.Append(" ").Append($"{eventArgs.Lastname}");
            }

            this.txtFullName.Text = sb.ToString().Trim();
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
