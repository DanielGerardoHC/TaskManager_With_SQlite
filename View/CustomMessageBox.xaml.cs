using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AdministradorDeTareas.View
{
    /// <summary>
    /// Lógica de interacción para CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        public string? description { get; set; }

        public CustomMessageBox(string description)
        {
            InitializeComponent();
            this.description = description;
            Loaded += CustomMessageBox_Loaded; // manejador del evento Loaded
        }

        // manejador del evento Loaded para establecer el DataContext
        private void CustomMessageBox_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = this;
        }

        private void Close_Window(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public static void ShowCustomMessageBox(string description)
        {
              CustomMessageBox customMessageBox = new CustomMessageBox(description);
              customMessageBox.ShowDialog(); // muestra la ventana de manera modal

        }
    }
}
