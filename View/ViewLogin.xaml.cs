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
    /// Lógica de interacción para ViewLogin.xaml
    /// </summary>
    public partial class ViewLogin : Window
    {
        public ViewLogin()
        {
            InitializeComponent();
        }
        private void Window_MouseDown(Object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {  
            
            ViewMainWindow Main = new ViewMainWindow();
            Main.Show();
            this.Close();
        }
    }
}
