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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace U2_Proyecto_Unidad
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            recetario.CambiarVistaCommand.Execute(Vistas.Agregar);
        }
        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lista.SelectedItem != null)
            {
                recetario.CambiarVistaCommand.Execute(Vistas.Agregar);

            }
        }

        private void listbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (lista.SelectedItem != null && e.Key == Key.Delete)
            {
                recetario.CambiarVistaCommand.Execute(Vistas.Eliminar);
            }
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ListBox_MouseDoubleClick(null, null);
        }

    }
}
