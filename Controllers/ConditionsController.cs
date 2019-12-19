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
            DataLayer dl = new DataLayer();
            string strSQL = "select * from CONDITION";
            OleDbDataReader dr = dl.GetDataReader(strSQL);
            List<ConditionData> cList = new List<ConditionData>();

            while (dr.Read())
            {
                ConditionData cd = new ConditionData();
                if(!String.IsNullOrEmpty(dr["RequestedDate"].ToString()))
                    cd.RequestedDate = DateTime.Parse(dr["RequestedDate"].ToString());
                cd.RequestedBy = dr["RequestedBy"].ToString();
                cd.Description = dr["_Description"].ToString();
                cd.ConditionTypeAndNo = dr["_ConditionTypeAndNo"].ToString();

                cList.Add(cd);
            }
            dl.Cleanup();

            return cList;
        }
    }

}


