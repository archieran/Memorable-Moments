<%@ WebHandler Language="C#" Class="ShowImage" %>

using System;
using System.Web;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

public class ShowImage : IHttpHandler
{
    private SqlConnection con;
    private SqlCommand cmd;
    private SqlDataReader dr;

    public void ProcessRequest (HttpContext context)
    {
        int imgId;
        if (context.Request.QueryString["id"] != null)
            imgId = Convert.ToInt32(context.Request.QueryString["id"]);
        else
            throw new ArgumentException("No parameter specified");

        context.Response.ContentType = "image/jpeg";
        Stream strm = getImage(imgId);
        byte[] buffer = new byte[4096];
        int byteSeq = strm.Read(buffer, 0, 4096);

        while (byteSeq > 0)
        {
            context.Response.OutputStream.Write(buffer, 0, byteSeq);
            byteSeq = strm.Read(buffer, 0, 4096);
        }
    }

    public Stream getImage(int imgId)
    {
        GetConnection();
        cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "select Image from UploadImage where UserId = @imgId";
        cmd.Parameters.Add(new SqlParameter("@imgId", imgId));
        cmd.CommandType = CommandType.Text;
        object img = cmd.ExecuteScalar();
        
        try
        {
            return new MemoryStream((byte[])img);
        }
        catch
        {
            return null;
        }
        finally
        {
            con.Close();
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

    private void GetConnection()
    {
        con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        con.Open();
    }

}