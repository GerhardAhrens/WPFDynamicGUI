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
    /// <remarks>
    /// https://zamjad.wordpress.com/2011/09/21/using-contenttemplateselector/
    /// </remarks>
    public partial class DynamicField : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty ItemSourceProperty = DependencyProperty.Register("ItemSource", typeof(IEnumerable), typeof(DynamicField));
        public DataTemplate _WorkContent = null;

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

        public DataTemplate WorkContent
        {
            get { return this._WorkContent; }
            set 
            { 
                if (this._WorkContent != value)
                {
                    this._WorkContent = value;
                    this.OnPropertyChanged();
                }
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
            this.WorkContent = new DataTypeTemplateSelector().SelectTemplate(typeof(string), null);
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
    public class DynamicLabelField
    {
        public Guid Id { get; private set; }

        public string FieldName { get; set; }
        public int FieldSize { get; set; }
        public Type FieldType { get; set; }

        public object FieldValue { get; set; }

        public DynamicLabelField(string labelName, Type fieldType, int fieldSize = 50)
        {
            this.Id = Guid.NewGuid();
            this.FieldName = labelName;
            this.FieldSize = fieldSize;
            this.FieldType = fieldType;
        }
    }

    public class DataTypeTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate dataTemplate = null;
            ContentPresenter pres = container as ContentPresenter;
            DynamicLabelField fieldTyp = item as DynamicLabelField;

            ResourceDictionary resourceDictionary = new ResourceDictionary();
            resourceDictionary.Source = new Uri($"{Assembly.GetCallingAssembly()};component/{"/Resources/Style/FieldDataTemplate.xaml"}", UriKind.RelativeOrAbsolute);


            if (fieldTyp.FieldType == typeof(string))
            {
                dataTemplate = resourceDictionary["DataTypeStringTemplate"] as DataTemplate;
            }

            return dataTemplate;
        }
    }
}
