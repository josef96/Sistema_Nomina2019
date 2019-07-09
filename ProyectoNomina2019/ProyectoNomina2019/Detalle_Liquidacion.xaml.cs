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
    /// Lógica de interacción para Detalle_Liquidacion.xaml
    /// </summary>
    public partial class Detalle_Liquidacion : Window
    {
        NominaEntities datos;


        public Detalle_Liquidacion()
        {
            InitializeComponent();
            datos = new NominaEntities();
        }

        public void cargarDatosGrilla()
        {

            try
            {
                dgEmpleados.ItemsSource = datos.Empleado.ToList();
                dgEmpleados.Columns[0].Visibility = Visibility.Hidden;
                for (int i = 4; i < 19; i++)
                {
                    dgEmpleados.Columns[i].Visibility = Visibility.Hidden;
                }

                dgLiquidaciones.ItemsSource = datos.Liquidacion_Mensual.ToList();
                dgLiquidaciones.Columns[0].Visibility = Visibility.Hidden;
                dgLiquidaciones.Columns[4].Visibility = Visibility.Hidden;
                dgLiquidaciones.Columns[6].Visibility = Visibility.Hidden;
                dgLiquidaciones.Columns[7].Visibility = Visibility.Hidden;
                dgLiquidaciones.Columns[8].Visibility = Visibility.Hidden;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void BtnVerDetalle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgLiquidaciones.SelectedItem != null)
                {
                    if (dgEmpleados.SelectedItem != null)
                    {
                        Empleado emp = (Empleado)dgEmpleados.SelectedItem;
                        Liquidacion_Mensual lm = (Liquidacion_Mensual)dgLiquidaciones.SelectedItem;

                        Liquidacion_Empleados_Salarios_Totales lest =
                            datos.Liquidacion_Empleados_Salarios_Totales.Find(lm.Id_Liquidacion, emp.Id_Empleado);

                        // Valida que solo muestre detalles de empleados a los cuales se les haya calculado el salario Total a cobrar
                        if(lest == null)
                        {
                            MessageBox.Show("El empleado no figura en la planilla");
                            return;
                        }

                        Detalle_Empleado_Liquidacion del = new Detalle_Empleado_Liquidacion(lm, emp);
                        del.ShowDialog();

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cargarDatosGrilla();
        }
    }
}
