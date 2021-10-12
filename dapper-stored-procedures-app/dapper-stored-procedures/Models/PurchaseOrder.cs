using Dapper;
using System;

namespace dapper_stored_procedures.Models
{
    [Table("PurchaseOrders", Schema = "Purchasing")]
    public class PurchaseOrder
    {
        [Key]
        public int PurchaseOrderId { get; set; }
        public int SupplierId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public string SupplierReference { get; set; }
    }
}
