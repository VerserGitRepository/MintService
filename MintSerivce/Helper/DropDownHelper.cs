using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MintSerivce.Models;

namespace MintSerivce.Helper
{
    public class DropDownHelper
    {
        public static List<ListItemModel> States()
        {
            var states = new List<ListItemModel>
            {
                new ListItemModel{Id=1,Value="NSW"},
                new ListItemModel{Id=2,Value="VIC"},
                new ListItemModel{Id=3,Value="QLD"},
                new ListItemModel{Id=4,Value="SA"},
                new ListItemModel{Id=5,Value="TAS"},
                new ListItemModel{Id=6,Value="WA"},
                new ListItemModel{Id=7,Value="ACT"},
                new ListItemModel{Id=8,Value="NT"}
            };
            return states;
        }

        public static List<ListItemModel> ReturnTypes()
        {
            var _ReturnTypes = new List<ListItemModel>
            {
                new ListItemModel{Id=1,Value="Return"},
                new ListItemModel{Id=2,Value="Swap"},
                new ListItemModel{Id=3,Value="Warranty"},
                new ListItemModel{Id=4,Value="Shopify Return"},
                new ListItemModel{Id=5,Value="Shopify Swap"},
                new ListItemModel{Id=6,Value="Shopify Warranty"} 
            };
            return _ReturnTypes;
        }

        public static List<ListItemModel> CoolingoffPeriods()
        {
            var _CoolingoffPeriods = new List<ListItemModel>
            {
                new ListItemModel{Id=1,Value="Yes"},
                new ListItemModel{Id=2,Value="No"}               
            };
            return _CoolingoffPeriods;
        }
        public static List<ListItemModel> IsReplacedList()
        {
            var _CoolingoffPeriods = new List<ListItemModel>
            {
                new ListItemModel{Id=1,Value="Yes"},
                new ListItemModel{Id=2,Value="No"}
            };
            return _CoolingoffPeriods;
        }
        public static List<ListItemModel> SMSReminder()
        {
            var _CoolingoffPeriods = new List<ListItemModel>
            {
                new ListItemModel{Id=1,Value="Yes"},
                new ListItemModel{Id=2,Value="No"},
                 new ListItemModel{Id=3,Value="Multiple Contacts"}
            };
            return _CoolingoffPeriods;
        }
    }
}