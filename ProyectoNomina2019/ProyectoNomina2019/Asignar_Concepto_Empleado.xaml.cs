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
    /// Lógica de interacción para Asignar_Concepto_Empleado.xaml
    /// </summary>
    public partial class Asignar_Concepto_Empleado : Window
    {
        NominaEntities datos;
        public DataTable detalleGlobal;

        public Asignar_Concepto_Empleado()
        {
            InitializeComponent();
            datos = new NominaEntities();
        }

        private void CargarDatosVentana()
        {
            try
            {
                dgEmpleados.ItemsSource = datos.Empleado.ToList();
                for (int i = 4; i < 19; i++)
                {
                    dgEmpleados.Columns[i].Visibility = Visibility.Hidden;
                }
                dgEmpleados.Columns[0].Visibility = Visibility.Hidden;

                //dgLiquidaciones.ItemsSource = datos.Liquidacion_Mensual.ToList();

                var liquidacionesAbiertas = (from l in datos.Liquidacion_Mensual
                                             where l.Estado == "A"
                                             select l).ToList();

                var conceptos = (from c in datos.Concepto
                                 where c.Descripcion != "IPS" && c.Descripcion != "Anticipo"
                                 select c.Descripcion).ToList();

                dgLiquidaciones.ItemsSource = liquidacionesAbiertas;
                dgLiquidaciones.Columns[0].Visibility = Visibility.Hidden;
                dgLiquidaciones.Columns[4].Visibility = Visibility.Hidden;
                dgLiquidaciones.Columns[6].Visibility = Visibility.Hidden;
                dgLiquidaciones.Columns[7].Visibility = Visibility.Hidden;
                dgLiquidaciones.Columns[8].Visibility = Visibility.Hidden;

                cboConceptos.ItemsSource = conceptos;
                //cboConceptos.DisplayMemberPath = "Descripcion";
                //cboConceptos.SelectedValuePath = "Id_Concepto";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            CargarDatosVentana();

        }


        public void LimpiarCampos()
        {
            txtMonto.Clear();
            cboConceptos.SelectedIndex = -1;
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgEmpleados.SelectedItem != null)
                {
                    if (dgLiquidaciones.SelectedItem != null)
                    {
                        if (cboConceptos.SelectedItem != null)
                        {
                            if (txtMonto.Text != null && int.Parse(txtMonto.Text) >= 0)
                            {
                                Liquidacion_Mensual_Detalle detalle = new Liquidacion_Mensual_Detalle();
                                Empleado emp = (Empleado)dgEmpleados.SelectedItem;
                                Liquidacion_Mensual lm = (Liquidacion_Mensual)dgLiquidaciones.SelectedItem;
                                int vMonto = int.Parse(txtMonto.Text);
                                string desc = cboConceptos.SelectedItem.ToString();

                                var conceptoObjeto = (from c in datos.Concepto
                                                      where c.Descripcion == desc
                                                      select c).FirstOrDefault();

                                // Carga de columnas del objeto Liquidacion_Mensual_Detalle
                                detalle.Liquidacion_Id = lm.Id_Liquidacion;
                                detalle.Empleado_Id = emp.Id_Empleado;
                                detalle.Concepto_Id = conceptoObjeto.Id_Concepto;


                                if (conceptoObjeto.Tipo == "-")
                                    detalle.Monto = vMonto * (-1);
                                else
                                    detalle.Monto = vMonto;


                                int idLiquidacion = lm.Id_Liquidacion;
                                int idEmpleado = emp.Id_Empleado;
                                int idConcepto = conceptoObjeto.Id_Concepto;

                                Liquidacion_Empleados_Salarios_Totales lest =
                                    datos.Liquidacion_Empleados_Salarios_Totales.Find(idLiquidacion, idEmpleado);

                                //Validar que no se cargue un concepto para una liquidacion ya generada
                                if (lest != null)
                                {
                                    MessageBox.Show("No se puede cargar el concepto " + conceptoObjeto.Descripcion +
                                        " en la liquidacion del anho " + lm.Anho + "y mes " + lm.Mes + ".");
                                    return;
                                }

                                // Validar si ya existe un detalle con la liquidacion, empleado y concepto seleccionados
                                foreach (Liquidacion_Mensual_Detalle det in datos.Liquidacion_Mensual_Detalle.ToList())
                                {
                                    if (det.Liquidacion_Id == idLiquidacion && det.Empleado_Id == idEmpleado && det.Concepto_Id == idConcepto)
                                    {
                                        MessageBox.Show("Ya existe un registro para la liquidacion, empleado y concepto seleccionados");
                                        return;
                                    }

                                }

                                datos.Liquidacion_Mensual_Detalle.Add(detalle);
                                datos.SaveChanges();
                                MessageBox.Show("Se ha creado un detalle para la liquidacion seleccionada exitosamente!");
                                LimpiarCampos();

                            }
                            else
                                MessageBox.Show("El campo monto no puede estar vacio");
                        }
                        else
                            MessageBox.Show("Debe seleccionar algun concepto del combo");
                    }
                    else
                        MessageBox.Show("Debe seleccionar alguna liquidacion de la grilla");
                }
                else
                    MessageBox.Show("Debe seleccionar algun empleado de la grilla");

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

        private void DgLiquidaciones_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                grbDetalles.Header = "Detalles de la liquidacion seleccionada";
                Liquidacion_Mensual lm = (Liquidacion_Mensual)dgLiquidaciones.SelectedItem;
                DataTable dt = new DataTable("Detalles_Liquidacion");
                dt.Columns.Add("Anho");
                dt.Columns.Add("Mes");
                dt.Columns.Add("Empleado");
                dt.Columns.Add("NroDocumento");
                dt.Columns.Add("Concepto");
                dt.Columns.Add("Monto");

                var detallesLiquidacion = (from d in datos.Liquidacion_Mensual_Detalle
                                           where d.Liquidacion_Id == lm.Id_Liquidacion
                                           select d).ToList();

                foreach (Liquidacion_Mensual_Detalle lmd in detallesLiquidacion)
                {
                    Empleado emp = datos.Empleado.Find(lmd.Empleado_Id);
                    Concepto c = datos.Concepto.Find(lmd.Concepto_Id);
                    string nombreCompleto = emp.Nombres + " " + emp.Apellidos;
                    string doc = emp.Nro_Documento;

                    dt.Rows.Add(lm.Anho, lm.Mes, nombreCompleto, doc, c.Descripcion, lmd.Monto);

                }

                dgDetalle_Liquidacion.ItemsSource = dt.DefaultView;
                //dgDetalle_Liquidacion.Columns[5].Visibility = Visibility.Hidden;
                //dgDetalle_Liquidacion.Columns[6].Visibility = Visibility.Hidden;
                //dgDetalle_Liquidacion.Columns[7].Visibility = Visibility.Hidden;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnEliminarDetalle_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (dgDetalle_Liquidacion.SelectedItem != null)
                {
                    //Liquidacion_Mensual_Detalle lmd = (Liquidacion_Mensual_Detalle)dgDetalle_Liquidacion.SelectedItem;
                    DataRowView filaSelected = dgDetalle_Liquidacion.SelectedItem as DataRowView;
                    short anho = short.Parse(filaSelected["Anho"].ToString());
                    short mes = short.Parse(filaSelected["Mes"].ToString());
                    string nroDoc = filaSelected["NroDocumento"].ToString();
                    string conc = filaSelected["Concepto"].ToString();


                    var liquidacion = (from l in datos.Liquidacion_Mensual
                                       where l.Mes == mes && l.Anho == anho
                                       select l).FirstOrDefault();

                    var empleado = (from emp in datos.Empleado
                                    where emp.Nro_Documento == nroDoc
                                    select emp).FirstOrDefault();

                    Liquidacion_Empleados_Salarios_Totales lest =
                        datos.Liquidacion_Empleados_Salarios_Totales.Find(liquidacion.Id_Liquidacion, empleado.Id_Empleado);

                    if (lest != null)
                    {
                        MessageBox.Show("No se puede eliminar este detalle," +
                            " porque forma parte del calculo de liquidacion del anho " + anho.ToString() + " y mes " + mes.ToString() + ".");
                        return;
                    }

                    var concepto = (from c in datos.Concepto
                                    where c.Descripcion == conc
                                    select c).FirstOrDefault();



                    var detalleEliminar = (from d in datos.Liquidacion_Mensual_Detalle
                                           where d.Liquidacion_Id == liquidacion.Id_Liquidacion && d.Empleado_Id == empleado.Id_Empleado && d.Concepto_Id == concepto.Id_Concepto
                                           select d).FirstOrDefault();

                    datos.Liquidacion_Mensual_Detalle.Remove(detalleEliminar);
                    datos.SaveChanges();
                    MessageBox.Show("Se ha eliminado un registro de detalle exitosamente!");
                    LimpiarCampos();
                }
                else
                    MessageBox.Show("Debe seleccionar un registro de la grilla de detalles!");


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void DgEmpleados_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (dgLiquidaciones.SelectedItem != null)
                {
                    Empleado empSelected = (Empleado)dgEmpleados.SelectedItem;
                    Liquidacion_Mensual lmSelected = (Liquidacion_Mensual)dgLiquidaciones.SelectedItem;

                    var detallePorEmpleado = (from d in datos.Liquidacion_Mensual_Detalle
                                              where d.Liquidacion_Id == lmSelected.Id_Liquidacion && d.Empleado_Id == empSelected.Id_Empleado
                                              select d).ToList();

                    DataTable dt = new DataTable("Detalles_Liquidacion");
                    dt.Columns.Add("Anho");
                    dt.Columns.Add("Mes");
                    dt.Columns.Add("Empleado");
                    dt.Columns.Add("NroDocumento");
                    dt.Columns.Add("Concepto");
                    dt.Columns.Add("Monto");

                    foreach (Liquidacion_Mensual_Detalle lmd in detallePorEmpleado)
                    {
                        Empleado emp = datos.Empleado.Find(lmd.Empleado_Id);
                        Concepto c = datos.Concepto.Find(lmd.Concepto_Id);
                        string nombreCompleto = emp.Nombres + " " + emp.Apellidos;
                        string doc = emp.Nro_Documento;

                        dt.Rows.Add(lmSelected.Anho, lmSelected.Mes, nombreCompleto, doc, c.Descripcion, lmd.Monto);

                    }

                    dgDetalle_Liquidacion.ItemsSource = dt.DefaultView;
                    grbDetalles.Header = "Detalles por empleado y liquidacion";

                }
                else
                    MessageBox.Show("Debe seleccionar un registro de la grilla de liquidaciones para filtrar");


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
