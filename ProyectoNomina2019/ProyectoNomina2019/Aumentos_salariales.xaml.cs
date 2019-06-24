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
    /// Lógica de interacción para Aumentos_salariales.xaml
    /// </summary>
    public partial class Aumentos_salariales : Window
    {
        NominaEntities datos;
        public int usuarioID;

        public Aumentos_salariales(int userId)
        {
            InitializeComponent();
            datos = new NominaEntities();
            usuarioID = userId;
        }


    }
}
