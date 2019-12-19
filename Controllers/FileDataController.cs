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
            List<FileData> fdList = new List<FileData>();
            OleDbConnection conDatabase = null;
            OleDbDataReader Dr = null;
            conDatabase = new OleDbConnection("Provider=SQLOLEDB; Data Source=DESKTOP-DQBU44U\\BYTESOFTWARE; Initial Catalog=BytePro; User ID=sa; Password=bytepro;");
            conDatabase.Open();
            string strSQL = "select * from FILEDATA";
            OleDbCommand cmdDatabase = new OleDbCommand(strSQL, conDatabase);
            Dr = cmdDatabase.ExecuteReader();

            while (Dr.Read())
            {
                FileData fd = new FileData();
                if(!String.IsNullOrEmpty(Dr["DateCreated"].ToString()))
                    fd.DateCreated = DateTime.Parse(Dr["DateCreated"].ToString());
                if (!String.IsNullOrEmpty(Dr["DateModified"].ToString()))
                    fd.DateModified = DateTime.Parse(Dr["DateModified"].ToString());
                fd.BorrowerNames = Dr["_AllBorrowerNames"].ToString();
                fd.FileName = Dr["FileName"].ToString();

                fdList.Add(fd);
            }

            Dr.Close();
            conDatabase.Close();

            return fdList;
        }
    }

}


