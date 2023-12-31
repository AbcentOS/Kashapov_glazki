//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Kashapov_glazki
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Timers;
    using System.Windows;
    using System.Windows.Media;

    public partial class Agent
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Agent()
        {
            this.AgentPriorityHistory = new HashSet<AgentPriorityHistory>();
            this.ProductSale = new HashSet<ProductSale>();
            this.Shop = new HashSet<Shop>();
        }
        public int res;
        public int ID { get; set; }
        public string Title { get; set; }
        public int AgentTypeID { get; set; }
        public string Address { get; set; }
        public string INN { get; set; }
        public string KPP { get; set; }
        public string DirectorName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Logo { get; set; }
        public int Priority { get; set; }
    
        public virtual AgentType AgentType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AgentPriorityHistory> AgentPriorityHistory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductSale> ProductSale { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Shop> Shop { get; set; }

        public string AgentTypeString
        {
            get
            {
                return AgentType.Title;
            }
        }

        public decimal NumberOfSale
        {
            get
            {

                decimal count = 0;
                var currentService_ProductSale = Kashapov_glazkiEntities.GetContext().Product.ToList();
                foreach (var item in ProductSale)
                {
                    if (item.AgentID == ID)
                    {
                        count += item.ProductCount;
                    }
                }
               
                return count;
            }
        }
        public decimal NumberSale
        {
            get
            {
                
                decimal count = 0;
                var currentService_ProductSale = Kashapov_glazkiEntities.GetContext().Product.ToList();
                foreach (var item in ProductSale)
                {
                    if (item.AgentID == ID)
                    {
                        foreach (var item1 in currentService_ProductSale)
                        {
                            if (item1.ID == ID)
                            {
                                count += item1.MinCostForAgent*item.ProductCount;
                            }
                        }
                        //currentService_Product = currentService_ProductSale.Where(p => p.ID == currentService_ProductSale).ToList();
                    }
                }

                if (count > 0 && count <= 10000)
                {
                    res = 0;
                }
                if (count > 10000 && count <= 50000)
                {
                    res = 5;
                }
                if (count > 50000 && count <= 150000)
                {
                    res = 10;
                }
                if (count > 150000 && count <= 500000)
                {
                    res = 20;
                }
                if (count > 500000)
                {
                    res = 25;
                }
                return res;
            }
        }
     public SolidColorBrush FonStyle
        {
            get
            {
                if (res >=25)
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("LightGreen");
                }
                else
                {
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("White");

                }
            }
        }
        public string Product_sale
        {
            get
            {
                var currentService_ProductSale = Kashapov_glazkiEntities.GetContext().ProductSale.ToList();
                var prod = "";
                foreach (var item in currentService_ProductSale)
                {
                    if (item.AgentID == ID)
                    {
                        prod = item.ProductID.ToString();
                    }
                }
                return prod;
            }
        }
    }

}
