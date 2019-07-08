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
    /// Lógica de interacción para Conceptos_parametrizables.xaml
    /// </summary>
    public partial class Conceptos_parametrizables : Window
    {
        NominaEntities datos;

        public Conceptos_parametrizables()
        {
            InitializeComponent();

            datos = new NominaEntities();
        }


        private void CargarDatos()
        {
            try
            {
                dgConceptos.ItemsSource = datos.Concepto.ToList();
                dgConceptos.Columns[0].Visibility = Visibility.Hidden;
                dgConceptos.Columns[3].Visibility = Visibility.Hidden;



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
                string desc = txtDescripcion.Text;
                string tipo = txtTipo.Text;

                if (desc != null || tipo != null)
                {
                    if (desc.Length <= 100)
                    {
                        if ((tipo.Length == 1 && tipo == "+") || (tipo.Length == 1 && tipo == "-"))
                        {
                            Concepto c = new Concepto();
                            c.Descripcion = desc;
                            c.Tipo = tipo;

                            datos.Concepto.Add(c);
                            datos.SaveChanges();
                            MessageBox.Show("Se ha agregado un registro exitosamente!");
                            CargarDatos();
                            LimpiarCampos();
                            txtDescripcion.Focus();
                        }
                        else
                            MessageBox.Show("En este campo debe cargarse el caracter '+' o '-'");
                    }
                    else
                        MessageBox.Show("El campo descripcion no puede exceder los 100 caracteres");
                }
                else
                    MessageBox.Show("Hay campos vacios");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }

        public void LimpiarCampos()
        {
            txtIdConcepto.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            txtTipo.Text = string.Empty;
            txtDescripcion.Focus();
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCampos();

        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgConceptos.SelectedItem != null)
                {
                    Concepto c = (Concepto)dgConceptos.SelectedItem;
                    var detalle = (from d in datos.Liquidacion_Mensual_Detalle
                                   where d.Concepto_Id == c.Id_Concepto
                                   select d).FirstOrDefault();

                    if (detalle != null)
                    {
                        MessageBox.Show("No se puede eliminar un concepto asociado a un empleado");
                        return;
                    }

                    datos.Concepto.Remove(c);
                    datos.SaveChanges();
                    MessageBox.Show("Se ha eliminado un registro exitosamente!");
                    CargarDatos();
                    LimpiarCampos();
                    txtDescripcion.Focus();
                }
                else
                    MessageBox.Show("Debe seleccionar un concepto");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (dgConceptos.SelectedItem != null)
            {
                Concepto c = (Concepto)dgConceptos.SelectedItem;

                string desc = txtDescripcion.Text;
                string tipo = txtTipo.Text;

                if (desc != null || tipo != null)
                {
                    if (desc.Length <= 100)
                    {
                        if ((tipo.Length == 1 && tipo == "+") || (tipo.Length == 1 && tipo == "-"))
                        {
                            c.Descripcion = desc;
                            c.Tipo = tipo;

                            datos.Entry(c).State = System.Data.Entity.EntityState.Modified;
                            datos.SaveChanges();
                            MessageBox.Show("Se ha modificado un registro exitosamente!");
                            CargarDatos();
                            LimpiarCampos();
                            txtDescripcion.Focus();
                        }
                        else
                        {
                            MessageBox.Show("En el campo 'Tipo' debe cargarse el caracter '+' o '-'");
                            txtTipo.Focus();
                        }

                    }
                    else
                    {
                        MessageBox.Show("El campo descripcion no puede exceder los 100 caracteres");
                        txtDescripcion.Focus();
                    }

                }
                else
                    MessageBox.Show("Hay campos vacios");

            }
            else
                MessageBox.Show("Debe seleccionar un concepto");
        }

        private void dgConceptos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgConceptos.SelectedItem != null)
            {
                Concepto c = (Concepto)dgConceptos.SelectedItem;

                txtIdConcepto.Text = c.Id_Concepto.ToString();
                txtDescripcion.Text = c.Descripcion;
                txtTipo.Text = c.Tipo;

            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CargarDatos();
            txtDescripcion.Focus();
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
