using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data.OleDb;

namespace MtgLabBrokerPortal
{
    public class FileData
    {
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }
        public string BorrowerNames { get; set; }
        public string FileName { get; set; }
    }
}

namespace MtgLabBrokerPortal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileDataController : ControllerBase {        
        private readonly ILogger<FileDataController> _logger;

        public FileDataController(ILogger<FileDataController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<FileData> Get()
        {
            List<FileData> cList = AccessDatabase();
            return cList;
        }

        private List<FileData> AccessDatabase()
        {
            DataLayer dl = new DataLayer();
            string strSQL = "select * from FILEDATA";
            OleDbDataReader dr = dl.GetDataReader(strSQL);
            List<FileData> fdList = new List<FileData>();

            while (dr.Read())
            {
                FileData fd = new FileData();
                if(!String.IsNullOrEmpty(dr["DateCreated"].ToString()))
                    fd.DateCreated = DateTime.Parse(dr["DateCreated"].ToString());
                if (!String.IsNullOrEmpty(dr["DateModified"].ToString()))
                    fd.DateModified = DateTime.Parse(dr["DateModified"].ToString());
                fd.BorrowerNames = dr["_AllBorrowerNames"].ToString();
                fd.FileName = dr["FileName"].ToString();

                fdList.Add(fd);
            }
            dl.Cleanup();

            return fdList;
        }
    }

}


