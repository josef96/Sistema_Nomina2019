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
    /// Lógica de interacción para Generar_Liquidaciones_Mensuales.xaml
    /// </summary>
    public partial class Generar_Liquidaciones_Mensuales : Window
    {
        NominaEntities datos;
        public Generar_Liquidaciones_Mensuales()
        {
            InitializeComponent();
            datos = new NominaEntities();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CargarDatosGrilla();
           
        }

        public void CargarDatosGrilla()
        {
            try
            {
                var liquidacionesAbiertas = (from l in datos.Liquidacion_Mensual
                                             where l.Estado == "A"
                                             select l).ToList();

                dgLiquidaciones.ItemsSource = liquidacionesAbiertas;
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

        private void BtnGenerarSalarios_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgLiquidaciones.SelectedItem != null)
                {
                    Liquidacion_Mensual lm = (Liquidacion_Mensual)dgLiquidaciones.SelectedItem;


                    var existe = (from s in datos.Liquidacion_Empleados_Salarios_Totales
                                  where s.Liquidacion_Id == lm.Id_Liquidacion
                                  select s).ToList();

                    if (existe.Count > 0)
                    {
                        MessageBox.Show("Las liquidaciones de los empleados para la planilla de liquidacion seleccionada ya han sido creadas");
                        return;
                    }
                    Concepto c = new Concepto();
                    Empleado emp = new Empleado();
                    Anticipo a = new Anticipo();



                    var detalleLiquidacion = (from d in datos.Liquidacion_Mensual_Detalle
                                              where d.Liquidacion_Id == lm.Id_Liquidacion
                                              select d).ToList();

                    List<Liquidacion_Mensual_Detalle> detAux1 = new List<Liquidacion_Mensual_Detalle>(); //Copia del original
                    List<Liquidacion_Mensual_Detalle> detAux2 = new List<Liquidacion_Mensual_Detalle>(); //Lista vacia
                    detAux1.AddRange(detalleLiquidacion);
                    //detAux2.AddRange(detalleLiquidacion);

                    var anticipos = (from ant in datos.Anticipo
                                     select ant).ToList();


                    foreach (Liquidacion_Mensual_Detalle lmd1 in detalleLiquidacion)
                    {
                        // buscamos el empleado actual y la liquidacion actual en la tabla de salarios totales
                        Liquidacion_Empleados_Salarios_Totales detalleAux =
                            datos.Liquidacion_Empleados_Salarios_Totales.Find(lmd1.Liquidacion_Id, lmd1.Empleado_Id);

                        var salariosTotales = datos.Liquidacion_Empleados_Salarios_Totales.ToList();

                        // Si ya existe un registro de salariototal para el empleadoId entonces simplemente iterar
                        // al siguiente objeto detalle hasta que sea un empleadoId que aun no se le haya generado 
                        // el salario total a cobrar
                        if (detalleAux != null)
                            continue;

                        // La idea es reiniciar las variables por cada empleado
                        int ingresos = 0;
                        int egresos = 0;
                        double ips = 0;
                        int anticipo = 0;
                        int salarioBasico = 0;
                        int salarioTotal = 0;

                        foreach (Liquidacion_Mensual_Detalle lmd2 in detAux1)
                        {

                            if (lmd1.Empleado_Id == lmd2.Empleado_Id)
                            {
                                // Se acumula los ingresos, egresos
                                c = datos.Concepto.Find(lmd2.Concepto_Id);

                                if (c.Tipo == "-")
                                {
                                    egresos += lmd2.Monto;
                                }
                                else
                                {
                                    ingresos += lmd2.Monto;
                                }

                                detAux2.Add(lmd2);

                            }

                        }
                        /* Una vez todos los montos acumulados en las respectivas variables
                                 * ya se puede borrar el registro en la lista de detalles auxiliar()
                                 * La idea es que no se vuelvan a recorrer varias veces los mismos registros cuyos
                                 * valores ya han sido usados para calcular el salariototal de un empleadoId
                                 * Ocasionando asi que se encimen los calculos acumulados
                                 */


                        foreach (Liquidacion_Mensual_Detalle index in detAux2)
                            detAux1.Remove(index);
                        // Se vuelve a vaciar la lista de detalles auxiliar detAux2
                        detAux2.Clear();

                        // Calculo del IPS
                        emp = datos.Empleado.Find(lmd1.Empleado_Id);
                        salarioBasico = emp.Salario_Basico;

                        ips = (salarioBasico + ingresos) * 0.09;

                        // Primero hay que buscar si existe un anticipo en el mes correspondiente a la liquidacion y empleado.
                        // Si existe, entonces se carga el valor del campo Monto_Aprobado a la variable anticipo.
                        // En caso contrario, la variable anticipo permanece en cero como se definio al principio.
                        foreach (Anticipo ant in anticipos)
                        {
                            if (ant.Empleado_Id == lmd1.Empleado_Id)
                            {
                                if (ant.Fecha_Solicitud.Month == lm.Mes && ant.Estado == "Aprobado")
                                {
                                    anticipo += ant.Monto_Aprobado; // anticipo total por empleado
                                }
                            }
                        }

                        // Ahora que se tiene todas las variables cargadas se puede proceder al calculo del salario total
                        // Los egresos siempre seran valores negativos. Por eso suma en lugar de restar 
                        salarioTotal = (int)((salarioBasico + ingresos + egresos) - ips - anticipo);

                        Liquidacion_Empleados_Salarios_Totales lest = new Liquidacion_Empleados_Salarios_Totales();
                        lest.Liquidacion_Id = lmd1.Liquidacion_Id;
                        lest.Empleado_Id = lmd1.Empleado_Id;
                        lest.SalarioTotal = salarioTotal;

                        datos.Liquidacion_Empleados_Salarios_Totales.Add(lest);

                        // Ademas hay que crear dos registros de detalle_liquidacion mas para almacenar el monto de IPS y anticipo  
                        Liquidacion_Mensual_Detalle detIps = new Liquidacion_Mensual_Detalle();
                        var idIps = (from con in datos.Concepto
                                     where con.Descripcion == "IPS"
                                     select con.Id_Concepto).FirstOrDefault();

                        detIps.Liquidacion_Id = lm.Id_Liquidacion;
                        detIps.Empleado_Id = lmd1.Empleado_Id;
                        detIps.Concepto_Id = idIps;
                        detIps.Monto = (int)ips * (-1);

                        datos.Liquidacion_Mensual_Detalle.Add(detIps);

                        Liquidacion_Mensual_Detalle detAnticipo = new Liquidacion_Mensual_Detalle();
                        var idAnticipo = (from con in datos.Concepto
                                          where con.Descripcion == "Anticipo"
                                          select con.Id_Concepto).FirstOrDefault();

                        detAnticipo.Liquidacion_Id = lm.Id_Liquidacion;
                        detAnticipo.Empleado_Id = lmd1.Empleado_Id;
                        detAnticipo.Concepto_Id = idAnticipo;
                        detAnticipo.Monto = anticipo * (-1);

                        datos.Liquidacion_Mensual_Detalle.Add(detAnticipo);

                    }
                    datos.SaveChanges();
                    MessageBox.Show("Se han generado correctamente las liquidaciones mensuales para cada empleado!");
                }
                else
                    MessageBox.Show("Debe seleccionar algun registro de la grilla");

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
