namespace WpfDynamicGUI.Controls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;
    using System.Windows.Media;

    /// <summary>
    /// Interaktionslogik für DynamicField.xaml
    /// </summary>
    public partial class DynamicField : UserControl
    {
        public event Action<RemoveEventArgs> RemoveFieldEvent;

        public static readonly DependencyProperty ItemSourceProperty = DependencyProperty.Register("ItemSource", typeof(IEnumerable), typeof(DynamicField));
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(DynamicField));
        public static readonly DependencyProperty FieldNameProperty = DependencyProperty.Register("FieldName", typeof(string), typeof(DynamicField));

        public DynamicField(DynamicLabelField dlf = null)
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "LostFocus", this.OnLostFocus);
            WeakEventManager<ComboBox, SelectionChangedEventArgs>.AddHandler(this.cbLabel, "SelectionChanged", this.OnSelectionChanged);
            WeakEventManager<ContentPresenter, RoutedEventArgs>.AddHandler(this.DataTypeContent, "Loaded", this.OnLoadedPresenter);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.btnRemove, "Click", this.OnRemoveClick);

            if (dlf != null)
            {
                this.DataTypeContent.Content = dlf.FieldType;
                this.FieldName = dlf.FieldName;
                this.Value = dlf.Value;
            }
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

        public object Value
        {
            get
            {
                return (object)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        public string FieldName
        {
            get
            {
                return (string)GetValue(FieldNameProperty);
            }
            set
            {
                SetValue(FieldNameProperty, value);
            }
        }

        public DynamicLabelField DynamicLabelField { get; private set; }

        private DynamicLabelField CurrentField { get; set; }

        private void OnRemoveClick(object sender, RoutedEventArgs e)
        {
            if (this.RemoveFieldEvent != null)
            {
                RemoveEventArgs args = new RemoveEventArgs();
                args.Field = this;
                this.RemoveFieldEvent(args);
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.cbLabel.DisplayMemberPath = "FieldName";
            this.cbLabel.SelectedValuePath = "FieldName";
            this.cbLabel.ItemsSource = this.ItemSource;

            if (string.IsNullOrEmpty(this.FieldName) == false)
            {
                this.cbLabel.SelectedValue = FieldName;
                this.CurrentField = this.cbLabel.SelectedItem as DynamicLabelField;
            }
            else
            {
                this.CurrentField = this.cbLabel.SelectedItem as DynamicLabelField;
            }
        }

        private void OnLoadedPresenter(object sender, RoutedEventArgs e)
        {
            if (this.CurrentField != null)
            {
                if (this.CurrentField.FieldType == typeof(string))
                {
                    TextBox txtString = FindVisualChildren<TextBox>(this).First(w => w.Name.ToLower() == "txtstring");
                    txtString.Text = this.Value.ToString();
                }
                else if (this.CurrentField.FieldType == typeof(bool))
                {
                    CheckBox chkBool = FindVisualChildren<CheckBox>(this).First();
                    chkBool.IsChecked = Convert.ToBoolean(this.Value);
                }
                else if (this.CurrentField.FieldType == typeof(DateTime))
                {
                    DatePicker dtPicker = FindVisualChildren<DatePicker>(this).First();
                    dtPicker.SelectedDate = Convert.ToDateTime(this.Value);
                }
            }
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (this.DynamicLabelField != null)
            {
                Type value = ((Type)this.DynamicLabelField.FieldType);
                if (value.FullName == typeof(string).FullName)
                {
                    TextBox txtString = this.GetVisualChild<TextBox>(this.DataTypeContent);
                    if (txtString != null)
                    {
                        this.Value = txtString.Text;
                    }
                }
                else if (value.FullName == typeof(bool).FullName)
                {
                    CheckBox chkBool = this.GetVisualChild<CheckBox>(this.DataTypeContent);
                    if (chkBool != null)
                    {
                        this.Value = chkBool.IsChecked;
                    }
                }
                else if (value.FullName == typeof(DateTime).FullName)
                {
                    DatePicker dtDateTime = this.GetVisualChild<DatePicker>(this.DataTypeContent);
                    if (dtDateTime != null)
                    {
                        this.Value = dtDateTime.SelectedDate;
                    }
                }
            }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DynamicLabelField lf = (DynamicLabelField)e.AddedItems[0];
            if (lf != null)
            {
                this.DynamicLabelField = lf;
                this.DataTypeContent.Content = lf.FieldType;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        private T GetVisualChild<T>(DependencyObject parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = this.GetVisualChild<T>(v);
                }

                if (child != null)
                {
                    break;
                }
            }

            return child;
        }

        public IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);

                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }


    [DebuggerDisplay("FieldName={FieldName}, Typ={FieldType}")]
    public class DynamicLabelField
    {
        public Guid Id { get; private set; }
        public string FieldName { get; set; }
        public int FieldSize { get; set; }
        public Type FieldType { get; set; }
        public object Value { get; set; }

        public DynamicLabelField(string labelName, Type fieldType, int fieldSize = 50)
        {
            this.Id = Guid.NewGuid();
            this.FieldName = labelName;
            this.FieldSize = fieldSize;
            this.FieldType = fieldType;
            this.Value = null;
        }
    }

    public class DataTypeTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TypeNull { get; set; }

        public DataTemplate TypeString { get; set; }

        public DataTemplate TypeBool { get; set; }

        public DataTemplate TypeDateTime { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
            {
                return this.TypeNull;
            }

            if ((item is DynamicLabelField) == true)
            {
                return this.TypeNull;
            }

            Type value = ((Type)item);

            if (value.FullName == typeof(string).FullName)
            {
                return this.TypeString;
            }
            else if (value.FullName == typeof(bool).FullName)
            {
                return this.TypeBool;
            }
            else if (value.FullName == typeof(DateTime).FullName)
            {
                return this.TypeDateTime;
            }
            else
            {
                return this.TypeNull;
            }
        }
    }

    public class RemoveEventArgs : EventArgs
    {
        public DynamicField Field { get; set; }
    }
}
