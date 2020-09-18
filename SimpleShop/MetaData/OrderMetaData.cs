using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MD.PersianDateTime.Standard;

namespace SimpleShop.Models
{
    [ModelMetadataType(typeof(OrderMetaData))]
    public partial class Order
    {
        public string IsPayedString
        {
            get
            {
                return IsPayed ? "بله" : "خیر";
            }
        }

        public string IsSentString
        {
            get
            {
                return IsSent ? "بله" : "خیر";
            }
        }

        public string PersianOrderDate
        {
            get
            {
                return new PersianDateTime(OrderDate).ToLongDateTimeString();
            }
        }

        public string SentStatus
        {
            get
            {
                return IsSent ? "تغییر به ارسال نشده" : "تغییر به ارسال شده";
            }
        }

        public string PayedStatus
        {
            get
            {
                return IsPayed ? "تغییر به پرداخت نشده" : "تغییر به پرداخت شده";
            }
        }
    }

    public class OrderMetaData
    {

    }
}
