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

namespace ProyectoNomina2019
{
    /// <summary>
    /// Lógica de interacción para Anticipos_salariales.xaml
    /// </summary>
    public partial class Anticipos_salariales : Window
    {
        NominaEntities datos;
        public Anticipos_salariales()
        {
            InitializeComponent();
            datos = new NominaEntities();
        }
        public void actualizarGrilla()
        {
            dgDatosAnticipoSalarial.ItemsSource = null;
            var vAnticipoSalarial = datos.Anticipo.ToList();
            dgDatosAnticipoSalarial.ItemsSource = vAnticipoSalarial;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            actualizarGrilla();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
