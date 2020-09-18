using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MD.PersianDateTime.Standard;

namespace SimpleShop.Models
{
    [ModelMetadataType(typeof(OrderDetailMetaData))]
    public partial class OrderDetail
    {
        public int TotalPrice
        {
            get
            {
                return Quantity * UnitPrice;
            }
        }
    }

    public class OrderDetailMetaData
    {

    }
}
