using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TourismCMS.Services
{
    public class CollectMoneyStatusManager
    {
        private static Dictionary<int ?, string> _CollectMoneyStatus = null;

        public static Dictionary<int ?, string> CollectMoneyStatus
        {
            get
            {
                _CollectMoneyStatus = new Dictionary<int ?, string>();
                _CollectMoneyStatus.Add(0, "未收");
                _CollectMoneyStatus.Add(1, "已收");
                return CollectMoneyStatusManager._CollectMoneyStatus;
            }
            set { CollectMoneyStatusManager._CollectMoneyStatus = value; }
        }
    }
}