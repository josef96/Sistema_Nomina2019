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
        public string imgPerfil = null;

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
                imgPerfil = imgPhoto.Source.ToString();
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
                if (stringPath != null)
                {
                    Uri imageUri = new Uri(stringPath);
                    BitmapImage imageBitmap = new BitmapImage(imageUri);
                    imgPhoto.Source = imageBitmap;
                }
                else
                    imgPhoto.Source = null;

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
                MessageBox.Show("Se ha eliminado un registro exitosamente!");
                CargarDatosGrilla();
            }
            else
                MessageBox.Show("Debe seleccionar un registro Empleado de la grilla para eliminar!");
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (dgNomina.SelectedItem != null)
            {
                try
                {
                    Empleado registro = new Empleado();

                    string nombres = txtNombres.Text;
                    string apellidos = txtApellidos.Text;
                    string nroDoc = txtCedula.Text;
                    string dir = txtDireccion.Text;
                    string tel = txtTelefono.Text;
                    DateTime fecNac = FechaNacimiento.DisplayDate;
                    DateTime fecInc = FechaIncorporacion.DisplayDate;
                    int salario = int.Parse(txtSueldo.Text);


                    if (nombres.Length <= 255 && nombres != null)
                    {
                        if (apellidos.Length <= 255 && apellidos != null)
                        {
                            if (nroDoc.Length <= 50 && nroDoc != null)
                            {
                                if (dir.Length <= 255)
                                {
                                    if (tel.Length <= 20 && tel != null)
                                    {
                                        if (fecInc <= System.DateTime.Now)
                                        {
                                            if (salario > 0)
                                            {
                                                Empleado a = (Empleado)dgNomina.SelectedItem;

                                                a.Nombres = nombres;
                                                a.Apellidos = apellidos;
                                                a.Nro_Documento = nroDoc;
                                                a.Direccion = dir;
                                                a.Nro_Telefono = tel;
                                                a.Fecha_Nacimiento = fecNac;
                                                a.Fecha_Incorporacion = fecInc;
                                                // a.Salario_Basico = int.Parse(txtSueldo.Text); No se debe modificar el salario basico
                                                a.Imagen_Perfil = imgPerfil;

                                                datos.Entry(a).State = System.Data.Entity.EntityState.Modified;
                                                datos.SaveChanges();
                                                MessageBox.Show("Se ha modificado un registro exitosamente!");
                                                CargarDatosGrilla();

                                            }
                                            else
                                                MessageBox.Show("El salario basico no puede ser igual a cero");
                                        }
                                        else
                                            MessageBox.Show("La fecha de incorporacion no puede ser mayor al fecha actual");
                                    }
                                    else
                                        MessageBox.Show("El campo de telefono no puede tener mas de 20 caracteres ni estar vacio");
                                }
                                else
                                    MessageBox.Show("El campo de direccion no puede tener mas de 255 caracteres");
                            }
                            else
                                MessageBox.Show("El campo de Nro. de documento no puede tener mas de 50 caracteres ni estar vacio");
                        }
                        else
                            MessageBox.Show("El campo de apellidos no puede tener mas de 255 caracteres ni estar vacio");
                    }
                    else
                        MessageBox.Show("El campo de nombres no puede tener mas de 255 caracteres ni estar vacio");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                MessageBox.Show("Debe seleccionar un registro de Empleado de la grilla para modificar!");
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Empleado registro = new Empleado();
                string nombres = txtNombres.Text;
                string apellidos = txtApellidos.Text;
                string nroDoc = txtCedula.Text;
                string dir = txtDireccion.Text;
                string tel = txtTelefono.Text;
                DateTime fecNac = FechaNacimiento.DisplayDate;
                DateTime fecInc = FechaIncorporacion.DisplayDate;
                int salario = int.Parse(txtSueldo.Text);


                if (nombres.Length <= 255 && nombres != null)
                {
                    if (apellidos.Length <= 255 && apellidos != null)
                    {
                        if (nroDoc.Length <= 50 && nroDoc != null)
                        {
                            if (dir.Length <= 255)
                            {
                                if (tel.Length <= 20 && tel != null)
                                {
                                    if (fecInc <= System.DateTime.Now)
                                    {
                                        if (salario > 0)
                                        {
                                            registro.Nombres = nombres;
                                            registro.Apellidos = apellidos;
                                            registro.Nro_Documento = nroDoc;
                                            registro.Direccion = dir;
                                            registro.Nro_Telefono = tel;
                                            registro.Fecha_Nacimiento = fecNac;
                                            registro.Fecha_Incorporacion = fecInc;
                                            registro.Salario_Basico = salario;
                                            registro.Imagen_Perfil = imgPerfil;

                                            datos.Empleado.Add(registro);
                                            datos.SaveChanges();
                                            MessageBox.Show("Se ha agregado un nuevo empleado exitosamente!");
                                            CargarDatosGrilla();

                                        }
                                        else
                                            MessageBox.Show("El salario basico no puede ser igual a cero");
                                    }
                                    else
                                        MessageBox.Show("La fecha de incorporacion no puede ser mayor al fecha actual");
                                }
                                else
                                    MessageBox.Show("El campo de telefono no puede tener mas de 20 caracteres ni estar vacio");
                            }
                            else
                                MessageBox.Show("El campo de direccion no puede tener mas de 255 caracteres");
                        }
                        else
                            MessageBox.Show("El campo de Nro. de documento no puede tener mas de 50 caracteres ni estar vacio");
                    }
                    else
                        MessageBox.Show("El campo de apellidos no puede tener mas de 255 caracteres ni estar vacio");
                }
                else
                    MessageBox.Show("El campo de nombres no puede tener mas de 255 caracteres ni estar vacio");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

    }
}
