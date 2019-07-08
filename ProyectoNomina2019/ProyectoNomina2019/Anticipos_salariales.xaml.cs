using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            try
            {

                dgDatosAnticipoSalarial.ItemsSource = datos.Anticipo.ToList();
                dgDatosAnticipoSalarial.Columns[8].Visibility = Visibility.Hidden;

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

        private void BtnAprobar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgDatosAnticipoSalarial.SelectedItem != null)
                {
                    Anticipo a = (Anticipo)dgDatosAnticipoSalarial.SelectedItem;

                    if (a.Estado == "Pendiente")
                    {
                        a.Estado = "Aprobado";

                        datos.Entry(a).State = System.Data.Entity.EntityState.Modified;
                        datos.SaveChanges();
                        MessageBox.Show("El anticipo fue aprobado con exito!");
                        actualizarGrilla();
                    }
                    else if (a.Estado == "Aprobado")
                        MessageBox.Show("El anticipo ya fue aprobado.");
                    else
                        MessageBox.Show("El anticipo ya fue rechazado.");

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
                if (dgDatosAnticipoSalarial.SelectedItem != null)
                {
                    Anticipo a = (Anticipo)dgDatosAnticipoSalarial.SelectedItem;

                    if (a.Estado == "Pendiente")
                    {
                        a.Estado = "Rechazado";

                        datos.Entry(a).State = System.Data.Entity.EntityState.Modified;
                        datos.SaveChanges();
                        MessageBox.Show("El anticipo fue rechazado con exito!");
                        actualizarGrilla();
                    }
                    else if (a.Estado == "Aprobado")
                        MessageBox.Show("El anticipo ya fue aprobado.");
                    else
                        MessageBox.Show("El anticipo ya fue rechazado.");

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
