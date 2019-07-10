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
    /// Lógica de interacción para Habilitar_Planilla_De_Liquidaciones.xaml
    /// </summary>
    public partial class Habilitar_Planilla_De_Liquidaciones : Window
    {
        NominaEntities datos;
        public int usuarioID;

        public Habilitar_Planilla_De_Liquidaciones(int userId)
        {
            InitializeComponent();
            datos = new NominaEntities();
            usuarioID = userId;
        }
        public void CargarGrilla()
        {
            try
            {
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CargarGrilla();
            txtMes.Text = System.DateTime.Now.Month.ToString();
            txtAnho.Text = System.DateTime.Now.Year.ToString();
        }

        private void BtnHabilitar_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (txtMes.Text != null && txtAnho.Text != null)
                {
                    if (int.Parse(txtMes.Text) > 0 && int.Parse(txtMes.Text) < 13 && txtAnho.Text.Length == 4)
                    {
                        Liquidacion_Mensual lmNew = new Liquidacion_Mensual();
                        short vMes = short.Parse(txtMes.Text);
                        short vAnho = short.Parse(txtAnho.Text);

                        lmNew.Mes = vMes;
                        lmNew.Anho = vAnho;
                        lmNew.Fecha_Generacion = System.DateTime.Now;
                        lmNew.Usuario_Id = usuarioID;
                        lmNew.Estado = "A";

                        var lmdb = (from l in datos.Liquidacion_Mensual
                                    where l.Anho == vAnho && l.Mes == vMes
                                    select l).FirstOrDefault();

                        if (lmdb != null)
                        {
                            MessageBox.Show("Ya existe una liquidacion con el mes y año ingresados");
                            return;
                        }
                        else
                        {
                            datos.Liquidacion_Mensual.Add(lmNew);
                            datos.SaveChanges();
                            MessageBox.Show("Se ha habilitado una liquidacion exitosamente!");
                            CargarGrilla();
                        }

                    }
                    else
                        MessageBox.Show("Uno de los campos tiene formato incorrecto");
                }
                else
                    MessageBox.Show("Uno de los campos tiene formato incorrecto");

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
