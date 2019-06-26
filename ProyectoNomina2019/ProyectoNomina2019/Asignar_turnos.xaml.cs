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
    /// Lógica de interacción para Asignar_turnos.xaml
    /// </summary>
    public partial class Asignar_turnos : Window
    {
        NominaEntities datos;
        public Asignar_turnos()
        {
            InitializeComponent();
            datos = new NominaEntities();
        }
        private void CargarDatosGrilla()
        {
            try
            {
                dgTurnos.ItemsSource = datos.Turno.ToList();
                dgEmpleados.ItemsSource = datos.Empleado.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CargarDatosGrilla();
        }

        private void BtnAsignarTurno_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (dgEmpleados.SelectedItem != null)
                {
                    if (dgTurnos.SelectedItem != null)
                    {
                        Empleado emp = (Empleado)dgEmpleados.SelectedItem;
                        Turno t = (Turno)dgTurnos.SelectedItem;
                        emp.Turno_Id = t.Id_Turno;

                        datos.Entry(emp).State = System.Data.Entity.EntityState.Modified;
                        datos.SaveChanges();

                        MessageBox.Show("Se ha asignado el turno al empleado exitosamente!");
                        CargarDatosGrilla();

                    }
                    else
                        MessageBox.Show("Debe seleccionar algun turno desde la grilla");
                }
                else
                    MessageBox.Show("Debe seleccionar algun empleado desde la grilla");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
