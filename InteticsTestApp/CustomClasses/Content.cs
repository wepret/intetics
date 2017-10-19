using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace InteticsTestApp.CustomClasses
{

  

    public static class Content
    {
       
        public static bool  insertMetaData(string name, string pathname, string[] tagArr, string description, string username)
        {
            try
            {
                string querystr = @"DECLARE @id int
DECLARE @idtags int
DECLARE @idimage int
set @idimage=(Select max(id) from Images)+1
insert into Images(id,Name,Description,Username,filename)values(@idimage,@name,@description,@username,@filename)";
foreach (var s in tagArr)
{
querystr += @" IF  EXISTS(select   tag from tags where UserName = @username and tag =" + "'" + s + "'" +
@")BEGIN
set @idtags = (Select max(id) from imageTag)+1
insert into imageTag(id, idimage, idtagname)values(@idtags, @idimage, (select  top 1 id from tags where UserName = @username and tag = " + "'" + s + "'" + @"))
END
IF NOT EXISTS(select  tag from tags where UserName = @username and tag = " + "'" + s + "'" + @")
BEGIN
set @id = (Select max(id) from Tags)+1
insert into Tags(id, Tag, ViewCount, UserName)values(@id, " + "'" + s + "'" + @", 1, @username)
set @idtags = (Select max(id) from imageTag)+1
insert into imageTag(id, idimage, idtagname)values(@idtags, @idimage, @id)
END ";
                }
                SqlConnection sc = new SqlConnection();
                sc.ConnectionString = WebConfigurationManager.ConnectionStrings["DB_A2C173_InteticsTestDb"].ConnectionString;
                SqlCommand scom = new SqlCommand();
                scom.Parameters.AddWithValue("@name", name);
                scom.Parameters.AddWithValue("@filename", pathname);
                scom.Parameters.AddWithValue("@description", description);
                scom.Parameters.AddWithValue("@username", username);
                scom.CommandText = querystr;
                scom.Connection = sc;
                sc.Open();
                scom.ExecuteNonQuery();
                sc.Close();
                return true;
            }
            catch (SqlException ex)
            {


                return false;

            }

        }

        public static Stack<string> selectSearchTags(string tagname)
        {
            Stack<string> searchTags = new Stack<string>();
           string querystr = @"select tag from tags where tag like('"+ tagname + "%')";



            SqlConnection sc = new SqlConnection();
            sc.ConnectionString = WebConfigurationManager.ConnectionStrings["DB_A2C173_InteticsTestDb"].ConnectionString;
            SqlCommand scom = new SqlCommand();
            scom.CommandText = querystr;
            scom.Connection = sc;
            sc.Open();
            SqlDataReader reader = scom.ExecuteReader();
            while (reader.Read())
            {
                if (!reader.IsDBNull(0))
                {
                    searchTags.Push(reader.GetString(0));
                }
            }
                    return searchTags;
        }


        public static List<Imagedata> selectMetaData(string username,string tagName,bool checker)
        {
           
            List<Imagedata> listImData = new List<Imagedata>();
            try
            {
                string querystr="";
                switch (checker)
                {
                    case true:
                        //querystr = "select name, description, filename from images where Images.Id = (select idimage from imagetag where idtagname = (select id from Tags where Tag = "+"'"+ tagName+"'"+ "))  and username=" + "'" + username + "'" ;

                        querystr = @"SELECT images.Name,images.Description,images.filename,  Tags.Tag
FROM ((images
INNER JOIN imageTag ON images.Id = imageTag.idimage)
INNER JOIN Tags ON imageTag.idtagname = Tags.Id  )
where images.Username =" + "'" + username + "'" +"and Tags.tag = " + "'" + tagName + "'" +
"order by filename desc";

                        break;
                    case false:
                        querystr =    @"SELECT images.Name,images.Description,images.filename,  Tags.Tag 
FROM ((images 
INNER JOIN imageTag ON images.Id = imageTag.idimage)
INNER JOIN Tags ON imageTag.idtagname = Tags.Id  )
where  images.Username=@username
order by filename desc";
                        //"select name,description,filename from images where username=@username";
                        break;
                }
                //string querystr = select name, description, filename from images where Images.Id = (select idimage from imagetag where idtagname = (select id from Tags where Tag = 'sadssssss'));
                //string querystr = "select name,description,filename from images where username=@username";

                SqlConnection sc = new SqlConnection();
                sc.ConnectionString = WebConfigurationManager.ConnectionStrings["DB_A2C173_InteticsTestDb"].ConnectionString;
                SqlCommand scom = new SqlCommand();
                scom.Parameters.AddWithValue("@username", username);

                scom.CommandText = querystr;
                scom.Connection = sc;
                sc.Open();
                SqlDataReader reader = scom.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.IsDBNull(1))
                    { 
                    listImData.Add(new Imagedata(reader.GetString(0), "", username + "/" + reader.GetString(2), reader.GetString(3)));
                    }
                    if (!reader.IsDBNull(1))
                    {
                        listImData.Add(new Imagedata(reader.GetString(0), reader.GetString(0),  username + "/" + reader.GetString(2), reader.GetString(3)));
                    }

                        //topid = adap.GetInt32(0);

                    }
                //scom.ExecuteNonQuery();
                sc.Close();
                return listImData;
            }
            catch (SqlException ex)
            {


                return null;

            }
        }







    }



   public  class Imagedata
    {
        public string name { get; set; }
        public string description { get; set; }
        public string filename { get; set; }
        public string tagname { get; set; }
        public   Imagedata(string name,string description,string filename, string tagname)
        {
            this.name = name;
            this.description = description;
            this.filename=filename;
            this.tagname = tagname;
        }

        public Imagedata()
        { }

    }
}