using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TourismCMS.Services
{
    public class BalanceStatusManager
    {
        private static Dictionary<int ?, string> _BalanceStatus = null;

        public static Dictionary<int ?, string> BalanceStatus
        {
            get
            {
                _BalanceStatus = new Dictionary<int ?, string>();
                _BalanceStatus.Add(0, "未结");
                _BalanceStatus.Add(1, "已结");
                return BalanceStatusManager._BalanceStatus;
            }
            set { BalanceStatusManager._BalanceStatus = value; }
        }
    }
}