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
using System.Collections;

public partial class CoreContent : System.Web.UI.Page
{
    private SqlConnection con;
    private SqlCommand cmd;
    private SqlDataReader dr;
    private String userName;
    private String userId;
    private DataTable dtImage;
    private DataTable dtThought;
    private static int imageButtonId = 1;
    private Comparison<string> a, b;

    protected void Page_Load(object sender, EventArgs e)
    {
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
        loadTimeLine();
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

    private void loadTimeLine()
    {
        GetConnection();
        cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.Add(new SqlParameter("@user_Id", userId));
        
        cmd.CommandText = "select substring(Image_Date,1,10) as Dates from UploadImage where User_Id=@user_Id UNION select substring(Thought_Date, 1, 10) as Dates from UploadThoughts where User_Id=@user_Id order by Dates desc;";
        dr = cmd.ExecuteReader();
        ArrayList dates = new ArrayList();

        while (dr.Read())
            dates.Add(dr["Dates"].ToString());
        dr.Close();
        
        dates.Sort(Comparer<String>.Create((a, b) => -1 * a.Substring(3, 2).CompareTo(b.Substring(3, 2))));

        int dateCount = dates.Count;
        for(int i=0;i<dateCount;i++)
        {
            PlaceHolder1.Controls.Add(new LiteralControl("<div class=form-horizontal><p><font size=6><b><u>" + getMonths(dates[i].ToString()) + "</u></b></font></p><br>"));
            loadImages(dates[i].ToString());
            loadThoughts(dates[i].ToString());
            PlaceHolder1.Controls.Add(new LiteralControl("</div><br><br><hr><br><br>"));
        }
            
        con.Close();
    }

    private String getMonths(String date)
    {
        switch(date.Substring(3,2))
        {
            case "01": return (date.Substring(0, 2) + " January, " + date.Substring(6, 4));
            case "02": return (date.Substring(0, 2) + " February, " + date.Substring(6, 4));
            case "03": return (date.Substring(0, 2) + " March, " + date.Substring(6, 4));
            case "04": return (date.Substring(0, 2) + " April, " + date.Substring(6, 4));
            case "05": return (date.Substring(0, 2) + " May, " + date.Substring(6, 4));
            case "06": return (date.Substring(0, 2) + " June, " + date.Substring(6, 4));
            case "07": return (date.Substring(0, 2) + " July, " + date.Substring(6, 4));
            case "08": return (date.Substring(0, 2) + " August, " + date.Substring(6, 4));
            case "09": return (date.Substring(0, 2) + " September, " + date.Substring(6, 4));
            case "10": return (date.Substring(0, 2) + " October, " + date.Substring(6, 4));
            case "11": return (date.Substring(0, 2) + " November, " + date.Substring(6, 4));
            case "12": return (date.Substring(0, 2) + " December, " + date.Substring(6, 4));
            default: return date;
        }
    }

    private void loadThoughts(String date)
    {
        cmd.CommandText = "select count(Thought_Id) from UploadThoughts where '"+date+"'=substring(Thought_Date,1,10) and User_Id=@user_Id";
        int thoughtCount = (int)cmd.ExecuteScalar();

        cmd.CommandText = "select Thought_Text, substring(Thought_Date, 1, 10) as Thought_Date from UploadThoughts where '"+date+"'=substring(Thought_Date,1,10) and User_Id = @user_Id order by Thought_Date desc";
        dr = cmd.ExecuteReader();
        dtThought = new DataTable();
        dtThought.Load(dr);

        for (int i=0;i<thoughtCount;i++)
            PlaceHolder1.Controls.Add(new LiteralControl("<p>" + dtThought.Rows[i]["Thought_Text"].ToString() + "<p>"));

    }
    
    private void loadImages(String date)
    {
        ImageButton img;
        String path;
        cmd.CommandText = "select count(Image_Id) from UploadImage where '"+date+"'=substring(Image_Date,1,10) and User_Id=@user_Id";
        int imageCount = (int)cmd.ExecuteScalar();

        cmd.CommandText = "select Image, substring(Image_Date, 1, 10) as Image_Date from UploadImage where '" + date + "'=substring(Image_Date,1,10) and User_Id = @user_Id order by Image_Date desc";
        dr = cmd.ExecuteReader();
        dtImage = new DataTable();
        dtImage.Load(dr);
        
        for (int i = 0; i < imageCount; i++, imageButtonId++)
        {
            path = Server.MapPath("~") + "Images/temp" + (i + 1) + ".jpg";
            byte[] b = (byte[])dtImage.Rows[i]["Image"];

            if (File.Exists(path))
                File.Delete(path);
            FileStream fs = File.Create(path);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(b);

            img = new ImageButton();
            img.ID = "ImageButton" + imageButtonId;
            img.ImageUrl = "~/Images/temp" + (i + 1) + ".jpg";
            img.Height = 300;
            img.Width = 600;

            PlaceHolder1.Controls.Add(img);

            bw.Close();
            fs.Close();
        }
    }

    protected void ButtonUpload_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Upload.aspx");
    }
    
}
 