using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using System.Xml.Linq;


namespace ProyectoNomina2019
{
    /// <summary>
    /// Lógica de interacción para Detalle_Empleado_Liquidacion.xaml
    /// </summary>
    public partial class Detalle_Empleado_Liquidacion : Window
    {
        NominaEntities datos;
        Liquidacion_Mensual lmSelected;
        Empleado empSelected;

        public Detalle_Empleado_Liquidacion(Liquidacion_Mensual lm, Empleado emp)
        {
            InitializeComponent();
            datos = new NominaEntities();
            lmSelected = lm;
            empSelected = emp;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                lblMes.Content = lmSelected.Mes;
                lblAnho.Content = lmSelected.Anho;
                lblEmpleado.Content = empSelected.Nombres + " " + empSelected.Apellidos;

                var ingresos = (from d in datos.Liquidacion_Mensual_Detalle
                                where d.Liquidacion_Id == lmSelected.Id_Liquidacion && d.Empleado_Id == empSelected.Id_Empleado
                                && d.Monto >= 0
                                select d).ToList();

                var egresos = (from d in datos.Liquidacion_Mensual_Detalle
                               where d.Liquidacion_Id == lmSelected.Id_Liquidacion && d.Empleado_Id == empSelected.Id_Empleado
                               && d.Monto < 0
                               select d).ToList();

                int totalIngresos = 0;
                int totalEgresos = 0;

                double margen = 213;
                //double margenEgresos = 245;

                foreach (Liquidacion_Mensual_Detalle lmd in ingresos)
                {
                    Concepto c = datos.Concepto.Find(lmd.Concepto_Id);

                    var bc = new BrushConverter();

                    Label lblConceptoIng = new Label();
                    lblConceptoIng.Foreground = (Brush)bc.ConvertFrom("#FFEAE6E6");
                    lblConceptoIng.FontSize = 14;
                    cvsResumen.Children.Add(lblConceptoIng);
                    Canvas.SetLeft(lblConceptoIng, 28);
                    Canvas.SetTop(lblConceptoIng, margen);
                    lblConceptoIng.Content = c.Descripcion;

                    Label lblMontoIng = new Label();
                    lblMontoIng.Foreground = (Brush)bc.ConvertFrom("#FF047004");
                    lblMontoIng.FontSize = 14;
                    cvsResumen.Children.Add(lblMontoIng);
                    Canvas.SetLeft(lblMontoIng, 343);
                    Canvas.SetTop(lblMontoIng, margen);
                    lblMontoIng.Content = lmd.Monto;

                    margen += 20;
                    totalIngresos += lmd.Monto;
                }

                foreach (Liquidacion_Mensual_Detalle lmd in egresos)
                {
                    Concepto c = datos.Concepto.Find(lmd.Concepto_Id);

                    var bc = new BrushConverter();

                    Label lblConceptoEg = new Label();
                    lblConceptoEg.Foreground = (Brush)bc.ConvertFrom("#FFEAE6E6");
                    lblConceptoEg.FontSize = 14;
                    cvsResumen.Children.Add(lblConceptoEg);
                    Canvas.SetLeft(lblConceptoEg, 28);
                    Canvas.SetTop(lblConceptoEg, margen);
                    lblConceptoEg.Content = c.Descripcion;

                    Label lblMontoEg = new Label();
                    lblMontoEg.Foreground = Brushes.Red;
                    lblMontoEg.FontSize = 14;
                    cvsResumen.Children.Add(lblMontoEg);
                    Canvas.SetLeft(lblMontoEg, 503);
                    Canvas.SetTop(lblMontoEg, margen);
                    lblMontoEg.Content = lmd.Monto;

                    margen += 20;
                    totalEgresos += lmd.Monto;
                }

                totalIngresos += empSelected.Salario_Basico;
                lblTotalIngresos.Content = totalIngresos;
                lblTotalEgresos.Content = totalEgresos;

                Liquidacion_Empleados_Salarios_Totales salTotal =
                    datos.Liquidacion_Empleados_Salarios_Totales.Find(lmSelected.Id_Liquidacion, empSelected.Id_Empleado);

                lblSalarioBasico.Content = empSelected.Salario_Basico;
                lblSalarioTotal.Content = salTotal.SalarioTotal;

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

        void ExportDataTableToPdf(DataTable dtblTable, String strPdfPath, string strHeader)
        {
            System.IO.FileStream fs = new FileStream(strPdfPath, FileMode.Create, FileAccess.Write, FileShare.None);
            Document document = new Document();
            document.SetPageSize(iTextSharp.text.PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();

            //Report Header
            BaseFont bfntHead = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntHead = new Font(bfntHead, 16, 1);
            iTextSharp.text.Paragraph prgHeading = new iTextSharp.text.Paragraph();
            prgHeading.Alignment = Element.ALIGN_CENTER;
            prgHeading.Add(new Chunk(strHeader.ToUpper(), fntHead));
            document.Add(prgHeading);

            //Author
            iTextSharp.text.Paragraph prgAuthor = new iTextSharp.text.Paragraph();
            BaseFont btnAuthor = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntAuthor = new Font(btnAuthor, 8, 2);
            prgAuthor.Alignment = Element.ALIGN_LEFT;
            //prgAuthor.Add(new Chunk("Emitido por : Dotnet Mob", fntAuthor));
            prgAuthor.Add(new Chunk("\nFecha de Emisión : " + DateTime.Now.ToShortDateString(), fntAuthor));
            document.Add(prgAuthor);


            //Empleado
            iTextSharp.text.Paragraph prgEmp = new iTextSharp.text.Paragraph();
            BaseFont btnEmp = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntEmp = new Font(btnEmp, 8, 2);
            prgEmp.Alignment = Element.ALIGN_LEFT;
            prgEmp.Add(new Chunk("Empleado: " + empSelected.Nombres + " " + empSelected.Apellidos + "\n", fntAuthor));
            prgEmp.Add(new Chunk("Fecha de liquidación: " + lmSelected.Mes + "/" + lmSelected.Anho, fntAuthor));
            document.Add(prgEmp);

            //Add a line seperation
            iTextSharp.text.Paragraph p = new iTextSharp.text.Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(p);

            //Add line break
            document.Add(new Chunk("\n", fntHead));

            //Write the table
            PdfPTable table = new PdfPTable(dtblTable.Columns.Count);
            //Table header
            BaseFont btnColumnHeader = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntColumnHeader = new Font(btnColumnHeader, 10, 1/*, BaseColor.WHITE*/);
            for (int i = 0; i < dtblTable.Columns.Count; i++)
            {
                PdfPCell cell = new PdfPCell();
                cell.BackgroundColor = BaseColor.GRAY;
                cell.AddElement(new Chunk(dtblTable.Columns[i].ColumnName.ToUpper(), fntColumnHeader));
                table.AddCell(cell);
            }
            //table Data
            for (int i = 0; i < dtblTable.Rows.Count; i++)
            {
                for (int j = 0; j < dtblTable.Columns.Count; j++)
                {
                    table.AddCell(dtblTable.Rows[i][j].ToString());
                }
            }

            document.Add(table);
            document.Close();
            writer.Close();
            fs.Close();
        }


        DataTable MakeDataTable()
        {
            DataTable friend = new DataTable();
            friend.Columns.Add("Concepto");
            friend.Columns.Add("Ingresos");
            friend.Columns.Add("Egresos");

            var ingresos = (from d in datos.Liquidacion_Mensual_Detalle
                            where d.Liquidacion_Id == lmSelected.Id_Liquidacion && d.Empleado_Id == empSelected.Id_Empleado
                            && d.Monto >= 0
                            select d).ToList();

            var egresos = (from d in datos.Liquidacion_Mensual_Detalle
                           where d.Liquidacion_Id == lmSelected.Id_Liquidacion && d.Empleado_Id == empSelected.Id_Empleado
                           && d.Monto < 0
                           select d).ToList();

            int totalIngresos = 0;
            int totalEgresos = 0;

            foreach (Liquidacion_Mensual_Detalle lmd1 in ingresos)
            {
                DataRow row = friend.NewRow();
                Concepto c = datos.Concepto.Find(lmd1.Concepto_Id);

                row["Concepto"] = c.Descripcion;
                row["Ingresos"] = lmd1.Monto;
                totalIngresos += lmd1.Monto;

                friend.Rows.Add(row);
            }
            foreach (Liquidacion_Mensual_Detalle lmd1 in egresos)
            {
                DataRow row = friend.NewRow();
                Concepto c = datos.Concepto.Find(lmd1.Concepto_Id);

                row["Concepto"] = c.Descripcion;
                row["Egresos"] = lmd1.Monto;

                totalEgresos += lmd1.Monto;
                friend.Rows.Add(row);
            }

            totalIngresos += empSelected.Salario_Basico;

            Liquidacion_Empleados_Salarios_Totales salTotal =
               datos.Liquidacion_Empleados_Salarios_Totales.Find(lmSelected.Id_Liquidacion, empSelected.Id_Empleado);

            try
            {
                DataRow row = friend.NewRow();
                row["Concepto"] = "Salario Basico";
                row["Ingresos"] = empSelected.Salario_Basico;
                friend.Rows.Add(row);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message");
            }

            try
            {
                DataRow row = friend.NewRow();
                row["Concepto"] = " ";
                friend.Rows.Add(row);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message");
            }

            try
            {
                DataRow row = friend.NewRow();
                row["Concepto"] = "Totales";
                row["Ingresos"] = totalIngresos;
                row["Egresos"] = totalEgresos;
                friend.Rows.Add(row);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message");
            }

            try
            {

                DataRow row = friend.NewRow();
                row["Concepto"] = "Salario total a Cobrar";
                row["Ingresos"] = salTotal.SalarioTotal;
                friend.Rows.Add(row);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message");
            }

            return friend;
        }

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string mensaje = "Esta seguro que desea guardar este resumen en formato PDF?";
                string titulo = "Imprimir resumen";
                MessageBoxButton botones = MessageBoxButton.YesNo;

                if (MessageBox.Show(mensaje, titulo, botones) == MessageBoxResult.Yes)
                {
                    DataTable dtbl = MakeDataTable();
                    ExportDataTableToPdf(dtbl, System.IO.Path.GetFullPath(@"..\..\..\..\") + "Resumen_Liquidación_Mensual_" + empSelected.Nombres + "_" + empSelected.Apellidos + ".pdf", "Resumen de Liquidación Mensual");

                    System.Diagnostics.Process.Start(System.IO.Path.GetFullPath(@"..\..\..\..\") + "Resumen_Liquidación_Mensual_" + empSelected.Nombres + "_" + empSelected.Apellidos + ".pdf");

                }
                else
                {
                    // solo se cierra el messageBox
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message");
            }
        }
    }
}
