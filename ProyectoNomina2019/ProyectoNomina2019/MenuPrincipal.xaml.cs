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
    /// Lógica de interacción para MenuPrincipal.xaml
    /// </summary>
    public partial class MenuPrincipal : Window
    {
        public int userIdLogin;

        public MenuPrincipal(int userId)
        {
            InitializeComponent();
            userIdLogin = userId;
        }

        private void w_Reg_Empleado_item_Click(object sender, RoutedEventArgs e)
        {
            Registro_Empleados re = new Registro_Empleados();
            re.ShowDialog();
        }

        private void w_Turnos_Horarios_item_Click(object sender, RoutedEventArgs e)
        {
            Turnos_Horarios th = new Turnos_Horarios();
            th.ShowDialog();
        }

        private void w_Conceptos_param_item_Click(object sender, RoutedEventArgs e)
        {
            Conceptos_parametrizables cp = new Conceptos_parametrizables();
            cp.ShowDialog();
        }

        private void w_Anticipos_salariales_item_Click(object sender, RoutedEventArgs e)
        { 
        
            Anticipos_salariales ant = new Anticipos_salariales();
            ant.ShowDialog();

        }

        private void w_Pedidos_vacaciones_item_Click(object sender, RoutedEventArgs e)
        {
            Pedidos_vacaciones pv = new Pedidos_vacaciones();
            pv.ShowDialog();
        }

        private void w_Solicitudes_permisos_item_Click(object sender, RoutedEventArgs e)
        {
            Solicitudes_permisos sp = new Solicitudes_permisos();
            sp.ShowDialog();
        }

        private void W_Aumentos_Salariales_Click(object sender, RoutedEventArgs e)
        {
            Aumentos_salariales au = new Aumentos_salariales(userIdLogin);
            au.ShowDialog();
        }

        private void W_Asignar_Turnos_Click(object sender, RoutedEventArgs e)
        {
            Asignar_turnos at = new Asignar_turnos();
            at.ShowDialog();
        }
    }
}
