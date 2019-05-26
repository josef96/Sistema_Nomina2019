﻿using System;
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            string desc = txtDescripcion.Text;
            string tipo = txtTipo.Text;

            if (desc != null && tipo != null )
            {
                if (desc.Length <= 100)
                {
                    if (tipo.Length == 1)
                    {
                        Concepto c = new Concepto();
                        c.Descripcion = desc;
                        c.Tipo = tipo;

                        datos.Concepto.Add(c);
                        datos.SaveChanges();
                        CargarDatos();
                    }
                    else
                        MessageBox.Show("El campo de tipo debe tener solo una letra");
                }
                else
                    MessageBox.Show("El campo descripcion no puede exceder los 100 caracteres");
            }
            else
                MessageBox.Show("Hay campos vacios");
           

        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            txtIdConcepto.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            txtTipo.Text = string.Empty;

        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (dgConceptos.SelectedItem != null)
            {
                Concepto c = (Concepto)dgConceptos.SelectedItem;


                datos.Concepto.Remove(c);
                datos.SaveChanges();
                CargarDatos();
            }
            else
                MessageBox.Show("Debe seleccionar un turno");
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (dgConceptos.SelectedItem != null)
            {
                Concepto c = (Concepto)dgConceptos.SelectedItem;

                c.Descripcion = txtDescripcion.Text;
                c.Tipo = txtTipo.Text;

                datos.Entry(c).State = System.Data.Entity.EntityState.Modified;
                
                datos.SaveChanges();
                CargarDatos();
            }
            else
                MessageBox.Show("Debe seleccionar un Concepto");
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
        }
    }
}
