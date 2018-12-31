using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TourismCMS.Services
{
    public class CollectStatusManager
    {
        private static Dictionary<int?, string> _CollectStatus = null;

        public static Dictionary<int?, string> CollectStatus
        {
            get
            {
                _CollectStatus = new Dictionary<int?, string>();
                _CollectStatus.Add(0, "未收");
                _CollectStatus.Add(1, "已收");
                return CollectStatusManager._CollectStatus;
            }
            set { CollectStatusManager._CollectStatus = value; }
        }
    }
}