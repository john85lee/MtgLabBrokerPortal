using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data.OleDb;

namespace MtgLabBrokerPortal
{
    public class ConditionData
    {
        public DateTime RequestedDate { get; set; }
        public string RequestedBy { get; set; }
        public string Description { get; set; }
        public string ConditionTypeAndNo { get; set; }
    }
}

namespace MtgLabBrokerPortal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConditionsController : ControllerBase {        
        private readonly ILogger<ConditionsController> _logger;

        public ConditionsController(ILogger<ConditionsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<ConditionData> Get()
        {
            List<ConditionData> cList = AccessDatabase();
            return cList;
        }

        private List<ConditionData> AccessDatabase()
        {
            List<ConditionData> cList = new List<ConditionData>();
            OleDbConnection conDatabase = null;
            OleDbDataReader Dr = null;
            conDatabase = new OleDbConnection("Provider=SQLOLEDB; Data Source=DESKTOP-DQBU44U\\BYTESOFTWARE; Initial Catalog=BytePro; User ID=sa; Password=bytepro;");
            conDatabase.Open();
            string strSQL = "select * from CONDITION";
            OleDbCommand cmdDatabase = new OleDbCommand(strSQL, conDatabase);
            Dr = cmdDatabase.ExecuteReader();

            while (Dr.Read())
            {
                ConditionData cd = new ConditionData();
                if(!String.IsNullOrEmpty(Dr["RequestedDate"].ToString()))
                    cd.RequestedDate = DateTime.Parse(Dr["RequestedDate"].ToString());
                cd.RequestedBy = Dr["RequestedBy"].ToString();
                cd.Description = Dr["_Description"].ToString();
                cd.ConditionTypeAndNo = Dr["_ConditionTypeAndNo"].ToString();

                cList.Add(cd);
            }

            Dr.Close();
            conDatabase.Close();

            return cList;
        }
    }

}


