using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;
using System.Data.SqlClient;

namespace InteticsTestApp.Controllers
{



    public class InteticsTestAppAPIController : ApiController
    {
        // GET api/<controller>
        public Dictionary<int,string> Get()
        {

            var request = this.Url;
            //    var request = HttpContext.Current.Request;
           var IP = ((System.Web.HttpContextBase)request.Request.Properties["MS_HttpContext"]).Request.UserHostAddress;


            string querystr = "select  Tag from (select top 4  ViewCount,Tag from Tags order by ViewCount desc ,Tag) as list";
            SqlConnection sc = new SqlConnection();
            sc.ConnectionString =    WebConfigurationManager.ConnectionStrings["DB_A2C173_InteticsTestDb"].ConnectionString;
            SqlCommand scom = new SqlCommand();
            scom.CommandText=querystr;
            scom.Connection = sc;
            sc.Open();
            SqlDataReader reader = scom.ExecuteReader();
            Dictionary<int,string> ditionar =new Dictionary<int,string>();
            try
            {
                
                while (reader.Read())
                {

                    ditionar.Add(ditionar.Count+1, (string)reader["Tag"]);
                }
            }
            finally
            {
                reader.Close();
                sc.Close();
            }


            return ditionar;

        }
       
        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";

        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }





       

    }
}