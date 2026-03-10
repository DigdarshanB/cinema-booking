using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data;

namespace KumariCinemas
{
    public partial class TheaterCityHallDetails : System.Web.UI.Page
    {
        protected global::System.Web.UI.WebControls.Label lblMsg;
        protected global::System.Web.UI.WebControls.TextBox txtCityhallId;
        protected global::System.Web.UI.WebControls.DropDownList ddlTheatreId;
        protected global::System.Web.UI.WebControls.TextBox txtCityhallName;
        protected global::System.Web.UI.WebControls.TextBox txtCityhallLocation;
        protected global::System.Web.UI.WebControls.Button btnAdd;
        protected global::System.Web.UI.WebControls.GridView gvTheaters;

        private string Cs => ConfigurationManager.ConnectionStrings["OracleDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindTheatreDropDown();
                BindGrid();
            }
        }

        private void BindTheatreDropDown()
        {
            string sql = "SELECT THEATRE_ID, \"theatre_name \" AS THEATRE_NAME " +
                "FROM THEATRE ORDER BY THEATRE_NAME";

            using (var con = new OracleConnection(Cs))
            using (var cmd = new OracleCommand(sql, con))
            using (var da = new OracleDataAdapter(cmd))
            {
                var dt = new DataTable();
                da.Fill(dt);

                ddlTheatreId.DataSource     = dt;
                ddlTheatreId.DataTextField  = "THEATRE_NAME";
                ddlTheatreId.DataValueField = "THEATRE_ID";
                ddlTheatreId.DataBind();
            }
        }

        private void BindGrid()
        {
            lblMsg.Text = "";

            using (var con = new OracleConnection(Cs))
            using (var cmd = new OracleCommand(
                "SELECT CITYHALL_ID, THEATRE_ID, CITYHALL_NAME, CITYHALL_LOCATION " +
                "FROM THEATRECITYHALL ORDER BY CITYHALL_ID", con))
            using (var da = new OracleDataAdapter(cmd))
            {
                var dt = new DataTable();
                da.Fill(dt);

                gvTheaters.DataSource = dt;
                gvTheaters.DataBind();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (var con = new OracleConnection(Cs))
                using (var cmd = new OracleCommand(
                    "INSERT INTO THEATRECITYHALL (CITYHALL_ID, THEATRE_ID, CITYHALL_NAME, CITYHALL_LOCATION) " +
                    "VALUES (:pId, :pTheatreId, :pName, :pLocation)", con))
                {
                    cmd.Parameters.Add(":pId",        OracleDbType.Varchar2).Value = txtCityhallId.Text.Trim();
                    cmd.Parameters.Add(":pTheatreId", OracleDbType.Varchar2).Value = ddlTheatreId.SelectedValue;
                    cmd.Parameters.Add(":pName",      OracleDbType.Varchar2).Value = txtCityhallName.Text.Trim();
                    cmd.Parameters.Add(":pLocation",  OracleDbType.Varchar2).Value = txtCityhallLocation.Text.Trim();

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                txtCityhallId.Text       = "";
                txtCityhallName.Text     = "";
                txtCityhallLocation.Text = "";
                lblMsg.Text = "Theater City Hall added successfully.";
                BindGrid();
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        protected void gvTheaters_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            gvTheaters.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void gvTheaters_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            gvTheaters.EditIndex = -1;
            BindGrid();
        }

        protected void gvTheaters_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            try
            {
                string id       = gvTheaters.DataKeys[e.RowIndex].Value.ToString();
                string theatreId = e.NewValues["THEATRE_ID"]?.ToString()       ?? "";
                string name     = e.NewValues["CITYHALL_NAME"]?.ToString()     ?? "";
                string location = e.NewValues["CITYHALL_LOCATION"]?.ToString() ?? "";

                using (var con = new OracleConnection(Cs))
                using (var cmd = new OracleCommand(
                    "UPDATE THEATRECITYHALL SET THEATRE_ID = :pTheatreId, CITYHALL_NAME = :pName, " +
                    "CITYHALL_LOCATION = :pLocation WHERE CITYHALL_ID = :pId", con))
                {
                    cmd.Parameters.Add(":pTheatreId", OracleDbType.Varchar2).Value = theatreId.Trim();
                    cmd.Parameters.Add(":pName",      OracleDbType.Varchar2).Value = name.Trim();
                    cmd.Parameters.Add(":pLocation",  OracleDbType.Varchar2).Value = location.Trim();
                    cmd.Parameters.Add(":pId",        OracleDbType.Varchar2).Value = id;

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                gvTheaters.EditIndex = -1;
                BindGrid();
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        protected void gvTheaters_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            try
            {
                string id = gvTheaters.DataKeys[e.RowIndex].Value.ToString();

                using (var con = new OracleConnection(Cs))
                using (var cmd = new OracleCommand(
                    "DELETE FROM THEATRECITYHALL WHERE CITYHALL_ID = :pId", con))
                {
                    cmd.Parameters.Add(":pId", OracleDbType.Varchar2).Value = id;

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                BindGrid();
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }
    }
}
