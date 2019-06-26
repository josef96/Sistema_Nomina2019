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
            try
            {
                dgDatosVacaciones.ItemsSource = null;
                var vVacaciones = datos.Vacaciones.ToList();
                dgDatosVacaciones.ItemsSource = vVacaciones;
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

        private void btnAprobar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgDatosVacaciones.SelectedItem != null)
                {
                    Vacaciones v = (Vacaciones)dgDatosVacaciones.SelectedItem;

                    if (v.Estado == "Pendiente")
                    {
                        v.Estado = "Aprobado";

                        datos.Entry(v).State = System.Data.Entity.EntityState.Modified;
                        datos.SaveChanges();
                        MessageBox.Show("El pedido fue aprobado con exito!");
                        actualizarGrilla();
                    }
                    else if (v.Estado == "Aprobado")
                        MessageBox.Show("El pedido ya fue aprobado.");
                    else
                        MessageBox.Show("El pedido ya fue rechazado.");

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
                if (dgDatosVacaciones.SelectedItem != null)
                {
                    Vacaciones v = (Vacaciones)dgDatosVacaciones.SelectedItem;

                    if (v.Estado == "Pendiente")
                    {
                        v.Estado = "Rechazado";

                        datos.Entry(v).State = System.Data.Entity.EntityState.Modified;
                        datos.SaveChanges();
                        MessageBox.Show("El pedido fue rechazado con exito!");
                        actualizarGrilla();
                    }
                    else if (v.Estado == "Aprobado")
                        MessageBox.Show("El pedido ya fue aprobado.");
                    else
                        MessageBox.Show("El pedido ya fue rechazado.");

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
