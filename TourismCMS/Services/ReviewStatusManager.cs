using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TourismCMS.Services
{
    public class ReviewStatusManager
    {
        private static Dictionary<int ?, string> _ReviewStatus = null;

        public static Dictionary<int ?, string> ReviewStatus
        {
            get
            {
                _ReviewStatus = new Dictionary<int ?, string>();
                _ReviewStatus.Add(0, "未审");
                _ReviewStatus.Add(1, "已审");
                return ReviewStatusManager._ReviewStatus;
            }
            set { ReviewStatusManager._ReviewStatus = value; }
        }
    }
}