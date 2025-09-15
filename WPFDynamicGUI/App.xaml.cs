namespace WPFDynamicGUI
{
    using System.Windows;

    using WPFDynamicGUI.Core;

    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        private static EventAggregator eventAgg;

        public static EventAggregator EventAgg
        {
            get
            {
                if (eventAgg == null)
                {
                    eventAgg = new EventAggregator();
                }

                return eventAgg;
            }
        }


    }
}
