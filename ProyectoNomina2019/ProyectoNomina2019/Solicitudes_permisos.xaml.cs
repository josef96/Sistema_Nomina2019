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
    /// Lógica de interacción para Solicitudes_permisos.xaml
    /// </summary>
    public partial class Solicitudes_permisos : Window
    {
        NominaEntities datos;
        public Solicitudes_permisos()
        {
            InitializeComponent();
            datos = new NominaEntities();
        }
        public void actualizarGrilla()
        {

            try
            {
                dgPermisos.ItemsSource = datos.Permisos.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            //dgPermisos.ItemsSource = null;
            //var vPermiso = datos.Vacaciones.ToList();
            //dgPermisos.ItemsSource = vPermiso;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            actualizarGrilla();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();    
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {

            if (dgPermisos != null)
            {
                Permisos p = (Permisos)dgPermisos.SelectedItem;

                if (p.Estado == "Pendiente")
                {
                    p.Estado = "Aceptado";
                }

                else if (p.Estado == "Aceptado")
                {
                    MessageBox.Show("El pedido ya fue aceptado.");
                }
                else
                {
                    MessageBox.Show("El pedido ya fue rechazado.");
                }

                datos.Entry(p).State = System.Data.Entity.EntityState.Modified;
                datos.SaveChanges();
                actualizarGrilla();

            }

        }

        private void btnRechazar_Click(object sender, RoutedEventArgs e)
        {

            Permisos p = (Permisos)dgPermisos.SelectedItem;

            if (p.Estado == "Pendiente")
            {
                p.Estado = "Rechazado";
            }

            else if (p.Estado == "Rechazado")
            {
                MessageBox.Show("El pedido ya fue Rechazado.");
            }
            else
            {
                MessageBox.Show("El pedido ya fue aceptado.");
            }



        }
    }
}
