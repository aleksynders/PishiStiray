//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PishiStiray
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.OrderProduct = new HashSet<OrderProduct>();
        }
    
        public int OrderID { get; set; }
        public int OrderStatusID { get; set; }
        public System.DateTime OrderDeliveryDate { get; set; }
        public int OrderPickupPointID { get; set; }
        public System.DateTime OrderDate { get; set; }
        public Nullable<int> UserID { get; set; }
        public int OrderCode { get; set; }
    
        public virtual OrderStatus OrderStatus { get; set; }
        public virtual PickupPoint PickupPoint { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderProduct> OrderProduct { get; set; }
    }
}
