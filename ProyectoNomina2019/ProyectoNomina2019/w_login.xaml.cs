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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProyectoNomina2019
{
    /// <summary>
    /// Lógica de interacción para w_login.xaml
    /// </summary>
    public partial class w_login : Window
    {
        NominaEntities datos;
        string vu = string.Empty;
        string vp = string.Empty;
        public w_login()
        {
            InitializeComponent();
            datos = new NominaEntities();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string valida = validar();
            if (valida == "OK")
            {
                // Tratamos de hacer el LOGIN
                string iniciaSesion = login();
                if (iniciaSesion == "OK")
                {
                    MenuPrincipal mm = new MenuPrincipal();
                    mm.ShowDialog();
                }
                else
                {
                    MessageBox.Show(iniciaSesion, "ERROR", MessageBoxButton.OK);
                    txtUsuario.Focus();
                }
            }
            else
            {
                MessageBox.Show(valida, "ERROR", MessageBoxButton.OK);
            }
        }

        public string validar()
        {
            string resultado = "OK";
            vu = txtUsuario.Text.Trim(); //.ToUpper()
            vp = txtPass.Password; //.ToString()

            if (vu == "")
            {
                resultado = "El campo usuario esta vacio";
                return resultado;
            }

            if (vp == "")
            {
                resultado = "Ingrese la contraseña.";
                return resultado;
            }

            return resultado;
        }


        //
        public string login()
        {
            string resul = "OK";
            bool existeUsuario = false;

            var Usuarios = datos.Usuario.ToList();

            foreach (Usuario user in Usuarios)
            {
                if (user.Usuario1.ToString() == vu)
                {
                    existeUsuario = true;
                    if (user.Password.ToString() != vp)
                    {
                        resul = "Contraseña Incorrecta";
                        return resul;
                    }
                }
            }

            if (!existeUsuario)
                resul = "Usuario ingresado no existe.";

            return resul;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtUsuario.Focus();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtUser_LostFocus(object sender, RoutedEventArgs e)
        {
            txtPass.Focus();
        }
    }
}
