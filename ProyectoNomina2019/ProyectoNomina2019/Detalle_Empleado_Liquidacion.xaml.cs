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
    /// Lógica de interacción para Detalle_Empleado_Liquidacion.xaml
    /// </summary>
    public partial class Detalle_Empleado_Liquidacion : Window
    {
        NominaEntities datos;
        Liquidacion_Mensual lmSelected;
        Empleado empSelected;

        public Detalle_Empleado_Liquidacion(Liquidacion_Mensual lm, Empleado emp)
        {
            InitializeComponent();
            datos = new NominaEntities();
            lmSelected = lm;
            empSelected = emp;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                lblMes.Content = lmSelected.Mes;
                lblAnho.Content = lmSelected.Anho;
                lblEmpleado.Content = empSelected.Nombres + " " + empSelected.Apellidos;

                var ingresos = (from d in datos.Liquidacion_Mensual_Detalle
                                where d.Liquidacion_Id == lmSelected.Id_Liquidacion && d.Empleado_Id == empSelected.Id_Empleado
                                && d.Monto >= 0
                                select d).ToList();

                var egresos = (from d in datos.Liquidacion_Mensual_Detalle
                               where d.Liquidacion_Id == lmSelected.Id_Liquidacion && d.Empleado_Id == empSelected.Id_Empleado
                               && d.Monto < 0
                               select d).ToList();

                int totalIngresos = 0;
                int totalEgresos = 0;

                double margen = 213;
                //double margenEgresos = 245;

                foreach (Liquidacion_Mensual_Detalle lmd in ingresos)
                {
                    Concepto c = datos.Concepto.Find(lmd.Concepto_Id);

                    var bc = new BrushConverter();

                    Label lblConceptoIng = new Label();
                    lblConceptoIng.Foreground = (Brush)bc.ConvertFrom("#FFEAE6E6");
                    lblConceptoIng.FontSize = 14;
                    cvsResumen.Children.Add(lblConceptoIng);
                    Canvas.SetLeft(lblConceptoIng, 28);
                    Canvas.SetTop(lblConceptoIng, margen);
                    lblConceptoIng.Content = c.Descripcion;

                    Label lblMontoIng = new Label();
                    lblMontoIng.Foreground = (Brush)bc.ConvertFrom("#FF047004");
                    lblMontoIng.FontSize = 14;
                    cvsResumen.Children.Add(lblMontoIng);
                    Canvas.SetLeft(lblMontoIng, 343);
                    Canvas.SetTop(lblMontoIng, margen);
                    lblMontoIng.Content = lmd.Monto;

                    margen += 20;
                    totalIngresos += lmd.Monto;
                }

                foreach (Liquidacion_Mensual_Detalle lmd in egresos)
                {
                    Concepto c = datos.Concepto.Find(lmd.Concepto_Id);

                    var bc = new BrushConverter();

                    Label lblConceptoEg = new Label();
                    lblConceptoEg.Foreground = (Brush)bc.ConvertFrom("#FFEAE6E6");
                    lblConceptoEg.FontSize = 14;
                    cvsResumen.Children.Add(lblConceptoEg);
                    Canvas.SetLeft(lblConceptoEg, 28);
                    Canvas.SetTop(lblConceptoEg, margen);
                    lblConceptoEg.Content = c.Descripcion;

                    Label lblMontoEg = new Label();
                    lblMontoEg.Foreground = Brushes.Red;
                    lblMontoEg.FontSize = 14;
                    cvsResumen.Children.Add(lblMontoEg);
                    Canvas.SetLeft(lblMontoEg, 503);
                    Canvas.SetTop(lblMontoEg, margen);
                    lblMontoEg.Content = lmd.Monto;

                    margen += 20;
                    totalEgresos += lmd.Monto;
                }

                totalIngresos += empSelected.Salario_Basico;
                lblTotalIngresos.Content = totalIngresos;
                lblTotalEgresos.Content = totalEgresos;

                Liquidacion_Empleados_Salarios_Totales salTotal =
                    datos.Liquidacion_Empleados_Salarios_Totales.Find(lmSelected.Id_Liquidacion, empSelected.Id_Empleado);

                lblSalarioBasico.Content = empSelected.Salario_Basico;
                lblSalarioTotal.Content = salTotal.SalarioTotal;

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
    }
}
