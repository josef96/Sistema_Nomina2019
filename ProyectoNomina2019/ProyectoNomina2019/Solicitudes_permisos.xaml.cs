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

            try
            {
                if (dgPermisos.SelectedItem != null)
                {
                    Permisos p = (Permisos)dgPermisos.SelectedItem;

                    if (p.Estado == "Pendiente")
                    {
                        p.Estado = "Aprobado";

                        datos.Entry(p).State = System.Data.Entity.EntityState.Modified;
                        datos.SaveChanges();
                        MessageBox.Show("El permiso fue aprobado con exito!");
                        actualizarGrilla();
                    }
                    else if (p.Estado == "Aprobado")
                        MessageBox.Show("El permiso ya fue aprobado.");
                    else
                        MessageBox.Show("El permiso ya fue rechazado.");

                }
                else
                    MessageBox.Show("Debe seleccionar algun registro de la grilla");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnRechazar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgPermisos.SelectedItem != null)
                {
                    Permisos p = (Permisos)dgPermisos.SelectedItem;

                    if (p.Estado == "Pendiente")
                    {
                        p.Estado = "Rechazado";

                        datos.Entry(p).State = System.Data.Entity.EntityState.Modified;
                        datos.SaveChanges();
                        MessageBox.Show("El permiso fue rechazado con exito!");
                        actualizarGrilla();
                    }
                    else if (p.Estado == "Aprobado")
                        MessageBox.Show("El permiso ya fue aprobado.");
                    else
                        MessageBox.Show("El permiso ya fue rechazado.");

                }
                else
                    MessageBox.Show("Debe seleccionar algun registro de la grilla");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
