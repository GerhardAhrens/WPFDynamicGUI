
namespace WPFDynamicGUI
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Windows;

    using WPFDynamicGUI.Core;

    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ISubscription<AddNameArgs> subAddName;

        private ObservableCollection<LabelFieldName> labelContent = null;

        public ObservableCollection<FieldFunction> FieldFunctions { get; set; }

        public ObservableCollection<LabelFieldName> LabelContent
        {
            get { return labelContent; }
            set
            {
                labelContent = value;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            this.subAddName = App.EventAgg.Subscribe<AddNameArgs>(AddNameArgsEventHandler);

            this.DataContext = this;

            this.lstFunctions.ItemsSource = FieldFunctions = new ObservableCollection<FieldFunction>();

            this.LabelContent = new ObservableCollection<LabelFieldName>();
            this.LabelContent.Add(new LabelFieldName("Land",typeof(string)));
            this.LabelContent.Add(new LabelFieldName("Postleitzahl", typeof(string)));
            this.LabelContent.Add(new LabelFieldName("Ort", typeof(string)));
            this.LabelContent.Add(new LabelFieldName("Telefon (Private)", typeof(string)));
            this.LabelContent.Add(new LabelFieldName("Telefon (Geschäftlich)", typeof(string)));
            this.LabelContent.Add(new LabelFieldName("eMail (Private)", typeof(string)));
            this.LabelContent.Add(new LabelFieldName("eMail (Geschäftlich)", typeof(string)));
        }

        private void OnDeleteFieldFunction(object sender, RoutedEventArgs e)
        {
            this.FieldFunctions.Remove((sender as FrameworkElement).DataContext as FieldFunction);
        }

        private void OnAddFieldFunction(object sender, RoutedEventArgs e)
        {
            this.FieldFunctions.Add(new FieldFunction());
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(new string('*',30));
            foreach (FieldFunction item in this.FieldFunctions)
            {
                if (item != null)
                {
                    if (item.HasValue == true)
                    {
                        string content = $"Label: {item.LabelName.FieldName}, Inhalt: {item.FieldContent}";
                        Console.WriteLine(content);
                    }
                }
            }

            Console.WriteLine(new string('*', 30));
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

    [DebuggerDisplay("FieldName={FieldName}, Typ={FieldType}")]
    public class LabelFieldName
    {
        public Guid Id { get; private set; }

        public string FieldName { get; set; }
        public int FieldSize { get; set; }
        public Type FieldType { get; set; }

        public LabelFieldName(string labelName, Type fieldType, int fieldSize = 50)
        {
            this.Id = Guid.NewGuid();
            this.FieldName = labelName;
            this.FieldSize = fieldSize;
            this.FieldType = fieldType;
        }
    }

    [DebuggerDisplay("LabelName={LabelName.FieldName}, Typ={LabelName.FieldType}")]
    public class FieldFunction
    {
        private LabelFieldName _LabelName = null;

        public LabelFieldName LabelName
        {
            get { return _LabelName; }
            set {
                _LabelName = value;
            }
        }

        public bool HasValue
        {
            get
            {
                bool result = true;
                if (this.LabelName == null)
                {
                    result = false;
                }

                return result;
            }
        }

        public object FieldContent { get; set; }
    }
}