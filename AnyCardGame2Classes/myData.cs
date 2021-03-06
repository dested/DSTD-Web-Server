using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace AnyCardGame2Classes
{
    public class myData
    {
        public myADO.MYADO m_ado;
        public DataSet m_DS;
        public bool m_HasData;

        public bool HasData
        {
            get { return m_HasData; }
        }

        public myData()
        {
            m_ado = new myADO.MYADO();
            InitVars();
        }

        private void InitVars()
        {
            m_HasData = false;
        }

        public DataSet RunProcedure(string ProcName, ArrayList Params)
        {
            m_ado.ProcName = ProcName;
            if (!(Params == null))
            {
                m_ado.AddParam(Params);
            }
            m_DS = m_ado.GetDS();
            m_HasData = m_ado.HasData;
            return m_DS;
        }

        public DataSet RunProcedure(string ProcName, List<SqlParameter> Params)
        {
            m_ado.ProcName = ProcName;
            if (!(Params == null))
            {
                foreach (SqlParameter param in Params)
                {
                    m_ado.AddParam(param);
                }
            }
            m_DS = m_ado.GetDS();
            m_HasData = m_ado.HasData;
            return m_DS;
        }

        public DataSet RunProcedure(string ProcName)
        {
            m_ado.ProcName = ProcName;
            m_DS = m_ado.GetDS();
            return m_DS;
        }

        public DataSet RunProcedure(string ProcName, SqlParameter SQLParam)
        {
            m_ado.ProcName = ProcName;
            if (!(SQLParam == null))
            {
                m_ado.AddParam(SQLParam);
            }
            m_DS = m_ado.GetDS();
            m_HasData = m_ado.HasData;
            return m_DS;
        }
    }
}