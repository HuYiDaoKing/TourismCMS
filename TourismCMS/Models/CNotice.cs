using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TourismCMS.Models
{
    public class CNotice
    {
        private bool m_Bresult;

        public bool Bresult
        {
            get { return m_Bresult; }
            set { m_Bresult = value; }
        }

        private string m_Notice;

        public string Notice
        {
            get { return m_Notice; }
            set { m_Notice = value; }
        }

        private string m_Param1;

        public string Param1
        {
            get { return m_Param1; }
            set { m_Param1 = value; }
        }

        private string m_Param2;

        public string Param2
        {
            get { return m_Param2; }
            set { m_Param2 = value; }
        }

        private string m_Param3;

        public string Param3
        {
            get { return m_Param3; }
            set { m_Param3 = value; }
        }
    }
}