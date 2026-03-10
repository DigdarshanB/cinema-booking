using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data;

namespace KumariCinemas
{
    public partial class ShowtimesDetails : System.Web.UI.Page
    {
        protected global::System.Web.UI.WebControls.Label lblMsg;
        protected global::System.Web.UI.WebControls.TextBox txtShowId;
        protected global::System.Web.UI.WebControls.TextBox txtCityhallId;
        protected global::System.Web.UI.WebControls.TextBox txtShowDate;
        protected global::System.Web.UI.WebControls.TextBox txtShowTime;
        protected global::System.Web.UI.WebControls.TextBox txtDayType;
        protected global::System.Web.UI.WebControls.Button btnAdd;
        protected global::System.Web.UI.WebControls.GridView gvShows;

        private string Cs => ConfigurationManager.ConnectionStrings["OracleDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        private void BindGrid()
        {
            lblMsg.Text = "";

            string sql = "SELECT SHOW_ID, CITYHALL_ID, SHOW_DATE, SHOW_TIME, " +
                "\"day_type \" AS DAY_TYPE FROM \"SHOW\" ORDER BY SHOW_ID";

            using (var con = new OracleConnection(Cs))
            using (var cmd = new OracleCommand(sql, con))
            using (var da = new OracleDataAdapter(cmd))
            {
                var dt = new DataTable();
                da.Fill(dt);

                gvShows.DataSource = dt;
                gvShows.DataBind();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = "INSERT INTO \"SHOW\" " +
                    "(SHOW_ID, CITYHALL_ID, SHOW_DATE, SHOW_TIME, \"day_type \") " +
                    "VALUES (:pId, :pCityhallId, :pShowDate, :pShowTime, :pDayType)";

                using (var con = new OracleConnection(Cs))
                using (var cmd = new OracleCommand(sql, con))
                {
                    cmd.Parameters.Add(":pId",         OracleDbType.Varchar2).Value = txtShowId.Text.Trim();
                    cmd.Parameters.Add(":pCityhallId", OracleDbType.Varchar2).Value = txtCityhallId.Text.Trim();
                    cmd.Parameters.Add(":pShowDate",   OracleDbType.Date).Value = DateTime.Parse(txtShowDate.Text.Trim());
                    cmd.Parameters.Add(":pShowTime",   OracleDbType.Date).Value = DateTime.Parse(txtShowTime.Text.Trim());
                    cmd.Parameters.Add(":pDayType",    OracleDbType.Varchar2).Value = txtDayType.Text.Trim();

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                txtShowId.Text     = "";
                txtCityhallId.Text = "";
                txtShowDate.Text   = "";
                txtShowTime.Text   = "";
                txtDayType.Text    = "";
                lblMsg.Text = "Showtime added successfully.";
                BindGrid();
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        protected void gvShows_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            gvShows.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void gvShows_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            gvShows.EditIndex = -1;
            BindGrid();
        }

        protected void gvShows_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            try
            {
                string id         = gvShows.DataKeys[e.RowIndex].Value.ToString();
                string cityhallId = e.NewValues["CITYHALL_ID"]?.ToString() ?? "";
                string showDate   = e.NewValues["SHOW_DATE"]?.ToString()   ?? "";
                string showTime   = e.NewValues["SHOW_TIME"]?.ToString()   ?? "";
                string dayType    = e.NewValues["DAY_TYPE"]?.ToString()    ?? "";

                string sql = "UPDATE \"SHOW\" SET CITYHALL_ID = :pCityhallId, " +
                    "SHOW_DATE = :pShowDate, " +
                    "SHOW_TIME = :pShowTime, " +
                    "\"day_type \" = :pDayType WHERE SHOW_ID = :pId";

                using (var con = new OracleConnection(Cs))
                using (var cmd = new OracleCommand(sql, con))
                {
                    cmd.Parameters.Add(":pCityhallId", OracleDbType.Varchar2).Value = cityhallId.Trim();
                    cmd.Parameters.Add(":pShowDate",   OracleDbType.Date).Value = DateTime.Parse(showDate.Trim());
                    cmd.Parameters.Add(":pShowTime",   OracleDbType.Date).Value = DateTime.Parse(showTime.Trim());
                    cmd.Parameters.Add(":pDayType",    OracleDbType.Varchar2).Value = dayType.Trim();
                    cmd.Parameters.Add(":pId",         OracleDbType.Varchar2).Value = id;

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                gvShows.EditIndex = -1;
                BindGrid();
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        protected void gvShows_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            try
            {
                string id = gvShows.DataKeys[e.RowIndex].Value.ToString();

                string sql = "DELETE FROM \"SHOW\" WHERE SHOW_ID = :pId";

                using (var con = new OracleConnection(Cs))
                using (var cmd = new OracleCommand(sql, con))
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
