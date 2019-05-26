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
    /// Lógica de interacción para Turnos_Horarios.xaml
    /// </summary>
    public partial class Turnos_Horarios : Window
    {
        NominaEntities datos;

        public Turnos_Horarios()
        {
            InitializeComponent();

            datos = new NominaEntities();
        }



        private void CargarDatos()
        {
            try
            {
                dgTurnos.ItemsSource = datos.Turno.ToList();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {


            // string asd = Convert.ToString(txtHoraEntrada.Text);


            int ejem = 0;



            if (txtHoraEntrada.Text.Length == 5)
            {

                if (int.TryParse(txtHoraEntrada.Text, out ejem))
                {
                    Turno turno = new Turno();
                    turno.Hora_Entrada = txtHoraEntrada.Text;
                    turno.Hora_Salida = txtHoraSalida.Text;
                    turno.Observaciones = txtObservaciones.Text;

                    datos.Turno.Add(turno);
                    datos.SaveChanges();
                    CargarDatos();
                }
                else
                {
                    MessageBox.Show("Debe ingresar en formato fecha (hh:mm)");
                }

            }
            else
            {
                MessageBox.Show("Debe ingresar en formato fecha (hh:mm)");
            }

        }

        //public void PreviewTextInputOnlyNumbers(object sender, TextCompositionEventArgs e)
        //{
        //    int character = Convert.ToInt32(Convert.ToChar(e.Text));
        //    if (character >= 48 && character <= 57)
        //        e.Handled = false;
        //    else
        //        e.Handled = true;
        //}

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {

            txtHoraEntrada.Text = string.Empty;
            txtHoraSalida.Text = string.Empty;
            txtObservaciones.Text = string.Empty;

        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {

            if (dgTurnos.SelectedItem != null)
            {
                Turno tur = (Turno)dgTurnos.SelectedItem;

                datos.Turno.Remove(tur);
                datos.SaveChanges();
                CargarDatos();
            }
            else
                MessageBox.Show("Debe seleccionar un turno");

        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (dgTurnos.SelectedItem != null)
            {
                Turno tur = (Turno)dgTurnos.SelectedItem;

                tur.Hora_Entrada = txtHoraEntrada.Text;
                tur.Hora_Salida = txtHoraSalida.Text;
                tur.Observaciones = txtObservaciones.Text;

                datos.Entry(tur).State = System.Data.Entity.EntityState.Modified;
                datos.SaveChanges();

                CargarDatos();
            }
            else
                MessageBox.Show("Debe seleccionar un turno");
        }

        private void dgTurnos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {


            if (dgTurnos.SelectedItem != null)
            {
                Turno t = (Turno)dgTurnos.SelectedItem;

                txtIdTurno.Text = t.Id_Turno.ToString();
                txtHoraEntrada.Text = t.Hora_Entrada;
                txtHoraSalida.Text = t.Hora_Salida;
                txtObservaciones.Text = t.Observaciones;

            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CargarDatos();
        }
    }
}
