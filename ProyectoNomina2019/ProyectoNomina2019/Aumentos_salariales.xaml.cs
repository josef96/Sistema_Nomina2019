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
    /// Lógica de interacción para Aumentos_salariales.xaml
    /// </summary>
    public partial class Aumentos_salariales : Window
    {
        NominaEntities datos;
        public int usuarioID;

        public Aumentos_salariales(int userId)
        {
            InitializeComponent();
            datos = new NominaEntities();
            usuarioID = userId;
        }

        private void CargarDatosGrilla()
        {
            try
            {
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

        private void BtnActualizarSalario_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (dgEmpleados.SelectedItem != null)
                {
                    Empleado emp = (Empleado)dgEmpleados.SelectedItem;
                    int nuevoSalario = int.Parse(txtNuevoSalario.Text);
                    int anteriorSalario = emp.Salario_Basico;


                    if (nuevoSalario > anteriorSalario)
                    {
                        Empleado_Salario_Historico historico = new Empleado_Salario_Historico();
                        historico.Empleado_Id = emp.Id_Empleado;
                        historico.Salario_Basico_Anterior = anteriorSalario;
                        historico.Salario_Basico_Nuevo = nuevoSalario;
                        historico.Fecha_Hora = System.DateTime.Now;

                        //var usuarioId = (from u in datos.Usuario
                        //                where u.Empleado_Id == emp.Id_Empleado
                        //                select u.Id_Usuario).FirstOrDefault(); UsuarioId correspondiente al EmpleadoId

                        historico.Usuario_Id = usuarioID; // UsuarioId correspondiente al usuario logueado
                        emp.Salario_Basico = nuevoSalario;

                        datos.Empleado_Salario_Historico.Add(historico);
                        datos.Entry(emp).State = System.Data.Entity.EntityState.Modified;
                        datos.SaveChanges();
                        MessageBox.Show("La operacion se completado con exito! (:");

                        CargarDatosGrilla();
                    }
                    else
                        MessageBox.Show("El nuevo salario debe ser mayor al anterior.");
                    
                }
                else
                    MessageBox.Show("Debe seleccionar algun empleado.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
