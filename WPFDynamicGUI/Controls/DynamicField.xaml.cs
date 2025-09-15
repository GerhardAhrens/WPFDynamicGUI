namespace WpfDynamicGUI.Controls
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaktionslogik für DynamicField.xaml
    /// </summary>
    public partial class DynamicField : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty ItemSourceProperty = DependencyProperty.Register("ItemSource", typeof(IEnumerable), typeof(DynamicField));

        public DynamicField()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<ComboBox, SelectionChangedEventArgs>.AddHandler(this.cbLabel, "SelectionChanged", this.OnSelectionChanged);
        }

        public IEnumerable ItemSource
        {
            get
            {
                return (IEnumerable)GetValue(ItemSourceProperty);
            }
            set
            {
                SetValue(ItemSourceProperty, value);
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.cbLabel.DisplayMemberPath = "FieldName";
            this.cbLabel.SelectedValuePath = "FieldName";
            this.cbLabel.ItemsSource = this.ItemSource;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DynamicLabelField lf = (DynamicLabelField)e.AddedItems[0];
            this.DataTypeContent.Content = lf.FieldType;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
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
    public class DynamicLabelField : INotifyPropertyChanged
    {
        public Guid Id { get; private set; }
        public string FieldName { get; set; }
        public int FieldSize { get; set; }
        public Type FieldType { get; set; }

        private object _FieldValue;

        public object FieldValue
        {
            get { return this._FieldValue; }
            set
            {
                this._FieldValue = value;
                this.OnPropertyChanged();
            }
        }

        public DynamicLabelField(string labelName, Type fieldType, int fieldSize = 50)
        {
            this.Id = Guid.NewGuid();
            this.FieldName = labelName;
            this.FieldSize = fieldSize;
            this.FieldType = fieldType;
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

    public class DataTypeTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TypeNull { get; set; }

        public DataTemplate TypeString { get; set; }

        public DataTemplate TypeBool { get; set; }

        public DataTemplate TypeDateTime { get; set; }

        public DataTemplate TypeInt { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
            {
                return (DataTemplate)((FrameworkElement)container).FindResource("dtTypNull");
            }

            Type value = ((Type)item);

            if (value.FullName == typeof(string).FullName)
            {
                return (DataTemplate)((FrameworkElement)container).FindResource("dtTypString");
            }
            else if (value.FullName == typeof(bool).FullName)
            {
                return (DataTemplate)((FrameworkElement)container).FindResource("dtTypBool");
            }
            else if (value.FullName == typeof(DateTime).FullName)
            {
                return (DataTemplate)((FrameworkElement)container).FindResource("dtTypDateTime");
            }
            else
            {
                return (DataTemplate)((FrameworkElement)container).FindResource("dtTypNull");
            }
        }
    }
}
