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
                dgTurnos.Columns[0].Visibility = Visibility.Hidden;
                dgTurnos.Columns[4].Visibility = Visibility.Hidden;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TimeSpan ts;
                var hora_entrada = txtHoraEntrada.Text;
                var hora_salida = txtHoraSalida.Text;
                if (TimeSpan.TryParse(hora_entrada, out ts) && TimeSpan.TryParse(hora_salida, out ts))
                {
                    Turno turno = new Turno();
                    turno.Hora_Entrada = hora_entrada;
                    turno.Hora_Salida = hora_salida;
                    turno.Observaciones = txtObservaciones.Text;

                    datos.Turno.Add(turno);
                    datos.SaveChanges();
                    MessageBox.Show("Se guardo un horario de turno exitosamente!");
                    CargarDatos();
                }
                else
                {
                    MessageBox.Show("Debe ingresar en formato 24 horas (hh:mm)");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            txtIdTurno.Text = string.Empty;
            txtHoraEntrada.Text = string.Empty;
            txtHoraSalida.Text = string.Empty;
            txtObservaciones.Text = string.Empty;
            txtHoraEntrada.Focus();

        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {

            if (dgTurnos.SelectedItem != null)
            {
                Turno tur = (Turno)dgTurnos.SelectedItem;

                var buscar = (from emp in datos.Empleado
                              where emp.Turno_Id == tur.Id_Turno
                              select emp).FirstOrDefault();

                if (buscar != null)
                {
                    MessageBox.Show("No se puede eliminar un turno ya asociado a un empleado");
                    return;
                }

                datos.Turno.Remove(tur);
                datos.SaveChanges();
                MessageBox.Show("Se ha eliminado un registro exitosamente!");
                CargarDatos();
            }
            else
                MessageBox.Show("Debe seleccionar un turno");

        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgTurnos.SelectedItem != null)
                {
                    Turno tur = (Turno)dgTurnos.SelectedItem;

                    TimeSpan ts;
                    var hora_entrada = txtHoraEntrada.Text;
                    var hora_salida = txtHoraSalida.Text;
                    if (TimeSpan.TryParse(hora_entrada, out ts) && TimeSpan.TryParse(hora_salida, out ts))
                    {
                        tur.Hora_Entrada = txtHoraEntrada.Text;
                        tur.Hora_Salida = txtHoraSalida.Text;
                        tur.Observaciones = txtObservaciones.Text;

                        datos.Entry(tur).State = System.Data.Entity.EntityState.Modified;
                        datos.SaveChanges();
                        MessageBox.Show("Se ha modificado un registro exitosamente!");
                        CargarDatos();
                    }
                    else
                    {
                        MessageBox.Show("Debe ingresar en formato 24 horas (hh:mm)");
                    }

                }
                else
                    MessageBox.Show("Debe seleccionar un turno");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void dgTurnos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Turno t = (Turno)dgTurnos.SelectedItem;

                txtIdTurno.Text = t.Id_Turno.ToString();
                txtHoraEntrada.Text = t.Hora_Entrada;
                txtHoraSalida.Text = t.Hora_Salida;
                txtObservaciones.Text = t.Observaciones;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CargarDatos();
        }
    }
}
