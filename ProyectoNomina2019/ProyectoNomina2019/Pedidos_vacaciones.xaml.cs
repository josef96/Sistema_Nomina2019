using System;
using System.Collections.Generic;
using System.Data;
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
    /// Lógica de interacción para Pedidos_vacaciones.xaml
    /// </summary>
    public partial class Pedidos_vacaciones : Window
    {
        NominaEntities datos;
        public Pedidos_vacaciones()
        {
            InitializeComponent();
            datos = new NominaEntities();
        }
        public void actualizarGrilla()
        {
            dgDatosVaciones.ItemsSource = null;
            var vVacaciones = datos.Vacaciones.ToList();
            dgDatosVaciones.ItemsSource = vVacaciones;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            actualizarGrilla();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAprobar_Click(object sender, RoutedEventArgs e)
        {
            if (dgDatosVaciones.SelectedItem != null)
            {
                Vacaciones a = (Vacaciones)dgDatosVaciones.SelectedItem;

                a.Estado = "Aprobado";


                datos.Entry(a).State = System.Data.Entity.EntityState.Modified;
                datos.SaveChanges();



                CargarDatosGrilla();
            }
            else
                MessageBox.Show("Debe seleccionar un registro de la grilla para Aprobar");
        }

        private void CargarDatosGrilla()
        {
            try
            {
                dgDatosVaciones.ItemsSource = datos.Vacaciones.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnRechazar_Click(object sender, RoutedEventArgs e)
        {
            if (dgDatosVaciones.SelectedItem != null)
            {
                Anticipo a = (Anticipo)dgDatosVaciones.SelectedItem;

                a.Estado = "Aprobado";


                datos.Entry(a).State = System.Data.Entity.EntityState.Modified;
                datos.SaveChanges();



                CargarDatosGrilla();
            }
            else
                MessageBox.Show("Debe seleccionar un registro de la grilla para Aprobar");
        }
    }
}
