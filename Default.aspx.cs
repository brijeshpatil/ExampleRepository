using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
public partial class _Default : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
    SqlDataAdapter da;
    DataTable dt;
    SqlCommand cmd;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillGrid();
        }
    }

    private void FillGrid()
    {
        da = new SqlDataAdapter("select * from country",con);
        dt = new DataTable();
        da.Fill(dt);
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Add")
        {
            cmd = new SqlCommand("insert into country values(@CName)",con);
            cmd.Parameters.AddWithValue("@CName", ((TextBox)GridView1.HeaderRow.FindControl("TextBox2")).Text);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            FillGrid();
        }
    }
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView1.EditIndex = e.NewEditIndex;
        FillGrid();
    }
    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        cmd = new SqlCommand("update country set countryname=@CName where countryid=@CID",con);
        cmd.Parameters.AddWithValue("@CName", ((TextBox)GridView1.Rows[e.RowIndex].FindControl("TextBox1")).Text);
        cmd.Parameters.AddWithValue("@CID", Convert.ToInt16(GridView1.DataKeys[e.RowIndex].Value));
        con.Open();
        cmd.ExecuteNonQuery();
        con.Close();
        GridView1.EditIndex = -1;
        FillGrid();
    }
    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView1.EditIndex = -1;
        FillGrid();
    }
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        cmd = new SqlCommand("delete from country where countryid=@CID", con);
        cmd.Parameters.AddWithValue("@CID", Convert.ToInt16(GridView1.DataKeys[e.RowIndex].Value));
        con.Open();
        cmd.ExecuteNonQuery();
        con.Close();
        FillGrid();
    }
}