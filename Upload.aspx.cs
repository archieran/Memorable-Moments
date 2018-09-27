using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;

public partial class CoreContent : System.Web.UI.Page
{
    private SqlConnection con;
    private SqlCommand cmd;
    private SqlDataReader dr;
    private String userName;
    private String userId;
    private String thought;
    private String today;
    private String fileName;
    private String filePath;
    private int fileSize;
    private byte[] binImage;

    protected void Page_Load(object sender, EventArgs e)
    {
        today = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt");
        LabelDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

        try
        {
            userName = Session["UserName"].ToString();
            userId = getUserId(userName);
        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "key", "alert('Session Expired. Please Login Again!", true);
            Context.GetOwinContext().Authentication.SignOut();
            Response.Redirect("~/Account/Login.aspx");
        }
        
    }

    protected void ButtonUploadThought_Click(object sender, EventArgs e)
    {
        thought = TextBoxThought.Text;

        int status = UploadThought();
        if(status == 1)
            ClientScript.RegisterStartupScript(this.GetType(), "key", "alert('Success')", true);
        else
            ClientScript.RegisterStartupScript(this.GetType(), "key", "alert('Check It Again!')", true);
    }

    private String getUserId(String userName)
    {
        GetConnection();
        cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "select Id from AspNetUsers where UserName = @userName";
        cmd.Parameters.Add(new SqlParameter("@userName", userName));
        cmd.CommandType = CommandType.Text;
        String userId = cmd.ExecuteScalar().ToString();
        con.Close();
        return userId;
    }

    private void GetConnection()
    {
        con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        con.Open();
    }

    private int UploadThought()
    {
        GetConnection();
        cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "insert into UploadThoughts(User_Id, Thought_Text, Thought_Date) values(@id, @thought, @date)";
        cmd.Parameters.Add(new SqlParameter("@id", userId));
        cmd.Parameters.Add(new SqlParameter("@thought", thought));
        cmd.Parameters.Add(new SqlParameter("@date", today));
        cmd.CommandType = CommandType.Text;
        int status = cmd.ExecuteNonQuery();
        con.Close();
        return status;
    }
    
    protected void ButtonUpload_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile && FileUpload1.PostedFile.ContentType.Equals("image/jpg") || FileUpload1.PostedFile.ContentType.Equals("image/jpeg"))
        {
            fileName = FileUpload1.FileName;
            filePath = Server.MapPath(fileName);
            fileSize = FileUpload1.PostedFile.ContentLength;
            Stream s = FileUpload1.PostedFile.InputStream;
            BinaryReader br = new BinaryReader(s);
            binImage = br.ReadBytes(fileSize);
            int status = UploadImage();
            if (status == 1)
                ClientScript.RegisterStartupScript(this.GetType(), "key", "alert('Image Success')", true);
            else
                ClientScript.RegisterStartupScript(this.GetType(), "key", "alert('Check Image Again!')", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "key", "alert('Select One File!')", true);
        }

    }

    private int UploadImage()
    {
        GetConnection();   
        cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "insert into UploadImage(User_Id, Image, Image_Size, Image_Date) values(@id, @img, @imgSize, @date)";
        cmd.Parameters.Add(new SqlParameter("@id", userId));
        cmd.Parameters.Add(new SqlParameter("@img", binImage));
        cmd.Parameters.Add(new SqlParameter("@imgSize", fileSize));
        cmd.Parameters.Add(new SqlParameter("@date", today));
        cmd.CommandType = CommandType.Text;
        int status = cmd.ExecuteNonQuery();
        con.Close();
        return status;
    }
    
}