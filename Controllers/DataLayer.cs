using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Data.OleDb;
using System.Security.Principal;

namespace MtgLabBrokerPortal
{
	/// <summary>
	/// Summary description for MyDataLayer.
	/// </summary>
	public class DataLayer
	{
        // Edit These Strings To Correspond to Database Setup
        public static string DBProvider = "SQLOLEDB";
        public static string DBServer = "DESKTOP-DQBU44U\\BYTESOFTWARE";
        public static string DBName = "BytePro";
        public static string DBUserID = "sa";
        public static string DBPassword = "bytepro";

        // Connection String For Accessing Database
        private string DBConnectionStr = "Provider=" + DBProvider + "; Data Source=" + DBServer + 
            "; Initial Catalog=" + DBName + "; User ID=" + DBUserID + "; Password=" + DBPassword + ";";
            
        private OleDbConnection myDataConnection = null;
        private OleDbDataReader myDataReader = null;

        // When passed in a sqlStr, will automatically connect to database
        // retreive data in a OleDbDataReader and return.
        // 
        // Note: after data is finished being used, both the DataReader and 
        //       DataConnection should be closed with the Cleanup()
        //       method I created below.
        public OleDbDataReader GetDataReader(string sqlStr)
        {
            try
            {
                if(myDataConnection == null)
                    myDataConnection = new OleDbConnection(DBConnectionStr);
                myDataConnection.Open();
                OleDbCommand cmdDatabase = new OleDbCommand(sqlStr, myDataConnection);
                myDataReader = cmdDatabase.ExecuteReader();
                return myDataReader;
            }
            catch
            {
                // cleanup any connections on error
                if(myDataReader != null)
                    myDataReader.Close();
                if(myDataConnection != null)
                    myDataConnection.Close();
                return null;
            }
        }

        public void Cleanup()
        {
            if(myDataReader != null)
                myDataReader.Close();
            if(myDataConnection != null)
                myDataConnection.Close();
        }


        /*
        /  The functions below are utility functions that may be helpful
        /  in processing or parsing data retrieved from the database. 
        /  We're not using any of this now, but I copied these in from
        /  old code written from previous projects, as it might be useful
        /  later on.
        */


		// checks to see if the string can be parsed as a valid integer with
		// the only characters allowed = {0-9}
		public static bool isValidInt(string str)
		{
			char[] chArr = str.ToCharArray();
			for(int i=0; i<chArr.Length;i++)
			{
				if(chArr[i] < 48 || chArr[i] > 57)
				{
					return false;
				}
			}
			return true;
		}

		// just formats the date to match standard format
		public static string parseDate(string str)
		{
			string temp = str.Replace("-", "/");
			temp = temp.Replace("\\", "/");
			return temp;
		}

		// checks to see if a valid date by parsing it
		public static bool checkDate(string str)
		{
			string temp = str;
			int month = -1;
			int day = -1;
			int year = -1;

			// if blank, we can just return true
			if(str.Equals(""))
				return true;
			try
			{
				month = int.Parse(temp.Substring(0, temp.IndexOf("/")));
				temp = temp.Substring(temp.IndexOf("/")+1, temp.Length-temp.IndexOf("/")-1);
				day = int.Parse(temp.Substring(0, temp.IndexOf("/")));
				temp = temp.Substring(temp.IndexOf("/")+1, temp.Length-temp.IndexOf("/")-1);
				year = int.Parse(temp);
			}
			catch
			{
				return false;
			}
			if(day<1 || day>31 || month<1 || month>12)
				return false;
			if(year>100)
			{
				if(year<1990)
					return false;
			}
			return true;
		}

		// formats a double into currency format with commas after every third number
		public static string CurrencyFormat(double num)
		{
			string str = num.ToString();
			int decIndex = str.IndexOf(".");
			if(decIndex == -1)
			{
				decIndex = str.Length;
			}

			while(decIndex > 3)
			{
				decIndex -= 3;
				string temp = str.Substring(0,decIndex) + "," + 
					str.Substring(decIndex, str.Length-decIndex);
				str = temp;
			}
			return "$" + str;
		}

		// overloaded function for currency formatting as above,
		// but for a string input.
		public static string CurrencyFormat(string num)
		{
			string str = num;
			int decIndex = str.IndexOf(".");
			if(decIndex == -1)
			{
				decIndex = str.Length;
			}

			while(decIndex > 3)
			{
				decIndex -= 3;
				string temp = str.Substring(0,decIndex) + "," + 
					str.Substring(decIndex, str.Length-decIndex);
				str = temp;
			}
			return "$" + str;
		}

		// this function will standardize the date to mm/dd/yyyy format
		public static string standardizeDate(DateTime dt)
		{
			int month = dt.Month;
			int day = dt.Day;
			int year = dt.Year;

			string returnStr = "";
			if(month < 10)
				returnStr += "0" + month + "/";
			else
				returnStr += month + "/";
			if(day < 10)
				returnStr += "0" + day + "/";
			else
				returnStr += day + "/";
			returnStr += year;

			return returnStr;	
		}

		// this function will return a formatted version of today's date
		public static string todayDate()
		{
			DateTime dt = DateTime.Now;
			return standardizeDate(dt);
		}

		// this function will round the decimal number to a whole number
		public static double roundCurrency(double amount)
		{
			return Math.Round(amount);
		}

		// this function will return the file name only, without the directory
		public static string fileNameOnly(string fileName)
		{
			int loc = fileName.LastIndexOf("\\");
			string temp = fileName.Substring(loc+1, fileName.Length - loc - 1);

			return temp;
		}

		// removes all spaces from the string
		public static string removeSpaces(string str)
		{
			return str.Replace(" ", "_");
		}

		// get next comma separated item
		public static string nextItem(string list)
		{
			string tempStr = list.Substring(1, list.Length-1);
			int nextLoc = tempStr.IndexOf(",");
			if(nextLoc == -1)
			{
				return tempStr;
			}
			return tempStr.Substring(0, nextLoc);
		}

		// get string without next item
		public static string nextStr(string list)
		{
			string tempStr = list.Substring(1, list.Length-1);
			int nextLoc = tempStr.IndexOf(",");
			if(nextLoc == -1)
			{
				return "";
			}
			return tempStr.Substring(nextLoc, tempStr.Length-nextLoc);
		}

		// this function is used to determine the type of file to open
		private string parseExtension(string fileName)
		{
			// gets the last 3 chars from fileName
			string str = fileName.Substring(fileName.Length-3, 3).ToLower();
			
			// then compares them to different file extensions 
			// to return the appropriate string which will be 
			// assigned to Response.ContentType
			if(str.Equals("doc"))
				return "Application/msword";
			else if(str.Equals("xls") || str.Equals("dif") || str.Equals("xla") || 
				str.Equals("xlt") || str.Equals("xlc") || str.Equals("xlm"))
				return "Application/vnd.ms-excel";
			else if(str.Equals("ppt"))
				return "Application/vnd.ms-powerpoint";
			else if(str.Equals("pdf"))
				return "Application/pdf";
			else if(str.Equals("peg"))
				return "image/JPEG";
			else if(str.Equals("gif"))
				return "image/GIF";
			else
				return "text/HTML";
		}

		// this function will extract a file name from a span element string
		// and return just the file name
		public static string extractFN(string fname)
		{
			if(fname.LastIndexOf("\\") == -1)
			{
				return fname.Substring(fname.LastIndexOf("(")+1, fname.Length - fname.LastIndexOf("(") - 9);
			}
			return fname.Substring(fname.LastIndexOf("\\")+1, fname.Length - fname.LastIndexOf("\\") - 9); 
		}

		// this function will extract the phone extension from the given string
		public static int extractPN(string phoneTxt)
		{
			if(phoneTxt.Length < 5)
				return 0;
			int temp = 0;
			while(true)
			{
				if(phoneTxt[temp]>=48 && phoneTxt[temp]<=57)
				{
					if(phoneTxt.Substring(temp).Length == 5)
					{
						try
						{
							int retVal = int.Parse(phoneTxt.Substring(temp));
							return retVal;
						}
						catch
						{
							return 0;
						}
					}
				}
				temp++;
				if(temp > 3)
					return 0;
			}
		}
	}
}
