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

        private void BtnAprobar_Click(object sender, RoutedEventArgs e)
        {
            if (dgDatosAnticipoSalarial.SelectedItem != null)
            {
                Anticipo a = (Anticipo)dgDatosAnticipoSalarial.SelectedItem;

                a.Estado = "Aprobado";


                datos.Entry(a).State = System.Data.Entity.EntityState.Modified;
                datos.SaveChanges();



                CargarDatosGrilla();
            }
            else
                MessageBox.Show("Debe seleccionar un registro de la grilla para Aprobar");
        }

        private void btnRechazar_Click(object sender, RoutedEventArgs e)
        {
            if (dgDatosAnticipoSalarial.SelectedItem != null)
            {
                Anticipo a = (Anticipo)dgDatosAnticipoSalarial.SelectedItem;

                a.Estado = "Rechazado";


                datos.Entry(a).State = System.Data.Entity.EntityState.Modified;
                datos.SaveChanges();



                CargarDatosGrilla();
            }
            else
                MessageBox.Show("Debe seleccionar un registro de la grilla para Rechazar");
        }
        private void CargarDatosGrilla()
        {
            try
            {

                dgDatosAnticipoSalarial.ItemsSource = datos.Vacaciones.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }




    }
}
