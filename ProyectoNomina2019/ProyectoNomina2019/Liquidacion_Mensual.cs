//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProyectoNomina2019
{
    using System;
    using System.Collections.Generic;
    
    public partial class Liquidacion_Mensual
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Liquidacion_Mensual()
        {
            this.Liquidacion_Mensual_Detalle = new HashSet<Liquidacion_Mensual_Detalle>();
        }
    
        public int Id_Liquidacion { get; set; }
        public short Mes { get; set; }
        public short Anho { get; set; }
        public System.DateTime Fecha_Generacion { get; set; }
        public int Usuario_Id { get; set; }
        public string Estado { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Liquidacion_Mensual_Detalle> Liquidacion_Mensual_Detalle { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}