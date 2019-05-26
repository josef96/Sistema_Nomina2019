using Microsoft.Win32;
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
    /// Lógica de interacción para Registro_Empleados.xaml
    /// </summary>
    public partial class Registro_Empleados : Window
    {

        //Creamos el objeto 
        NominaEntities datos;

        public Registro_Empleados()
        {
            InitializeComponent();
            datos = new NominaEntities();
        }
        private void CargarDatosGrilla()
        {
            try
            {

                dgNomina.ItemsSource = datos.Empleado.ToList();
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

        private void btnImagen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Elegir una imagen";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                imgPhoto.Source = new BitmapImage(new Uri(op.FileName));
            }
        }

        private void dgNomina_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgNomina.SelectedItem != null)
            {
                Empleado a = (Empleado)dgNomina.SelectedItem;

                txtId.Text = a.Id_Empleado.ToString();
                txtNombres.Text = a.Nombres;
                txtApellidos.Text = a.Apellidos;
                txtCedula.Text = a.Nro_Documento;
                txtDireccion.Text = a.Direccion;
                txtTelefono.Text = a.Nro_Telefono;
                txtSueldo.Text = a.Salario_Basico.ToString();
                FechaIncorporacion.Text = a.Fecha_Incorporacion.ToString();
                FechaNacimiento.Text = a.Fecha_Nacimiento.ToString();
                String stringPath = a.Imagen_Perfil;
                Uri imageUri = new Uri(stringPath);
                BitmapImage imageBitmap = new BitmapImage(imageUri);
                imgPhoto.Source = imageBitmap;
            }
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            txtId.Text = string.Empty;
            txtNombres.Text = string.Empty;
            txtApellidos.Text = string.Empty;
            txtCedula.Text = string.Empty;
            txtDireccion.Text = string.Empty;
            txtTelefono.Text = string.Empty;


            if (FechaNacimiento.GetType() == typeof(DatePicker))
            {
                var datePicker = (DatePicker)FechaNacimiento;
                datePicker.SelectedDate = DateTime.Now.Date;
            }
            if (FechaIncorporacion.GetType() == typeof(DatePicker))
            {
                var datePicker = (DatePicker)FechaIncorporacion;
                datePicker.SelectedDate = DateTime.Now.Date;
            }
            txtSueldo.Text = string.Empty;

            imgPhoto.Source = null;
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (dgNomina.SelectedItem != null)
            {
                Empleado a = (Empleado)dgNomina.SelectedItem;



                datos.Empleado.Remove(a);
                datos.SaveChanges();
                CargarDatosGrilla();
            }
            else
                MessageBox.Show("Debe seleccionar un registro Empleado de la grilla para eliminar!");
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (dgNomina.SelectedItem != null)
            {
                Empleado a = (Empleado)dgNomina.SelectedItem;

                a.Nombres = txtNombres.Text;
                a.Apellidos = txtApellidos.Text;
                a.Nro_Documento = txtCedula.Text;
                a.Direccion = txtDireccion.Text;
                a.Nro_Telefono = txtTelefono.Text;

                a.Fecha_Nacimiento = DateTime.Parse(FechaNacimiento.Text);
                a.Fecha_Incorporacion = DateTime.Parse(FechaIncorporacion.Text);
                a.Salario_Basico = int.Parse(txtSueldo.Text);

                a.Imagen_Perfil = imgPhoto.Source.ToString();


                datos.Entry(a).State = System.Data.Entity.EntityState.Modified;
                datos.SaveChanges();



                CargarDatosGrilla();
            }
            else
                MessageBox.Show("Debe seleccionar un registro de Empleado de la grilla para modificar!");
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {



            Empleado registro = new Empleado();

            registro.Nombres = txtNombres.Text;
            registro.Apellidos = txtApellidos.Text;
            registro.Nro_Documento = txtCedula.Text;
            registro.Direccion = txtDireccion.Text;
            registro.Nro_Telefono = txtTelefono.Text;

            registro.Fecha_Nacimiento = DateTime.Parse(FechaNacimiento.Text);
            registro.Fecha_Incorporacion = DateTime.Parse(FechaIncorporacion.Text);
            registro.Salario_Basico = int.Parse(txtSueldo.Text);

            registro.Imagen_Perfil = imgPhoto.Source.ToString();


            datos.Empleado.Add(registro);
            datos.SaveChanges();

        }


    }
}
