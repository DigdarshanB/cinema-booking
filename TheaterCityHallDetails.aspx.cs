using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

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
        protected global::System.Web.UI.WebControls.Button btnClear;
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
            string sql = "SELECT THEATRE_ID, THEATRE_NAME " +
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
                ddlTheatreId.Items.Insert(0, new ListItem("-- Select Theatre --", ""));
            }
        }

        private void BindGrid()
        {
            using (var con = new OracleConnection(Cs))
            using (var cmd = new OracleCommand(
                "SELECT CITYHALL_ID, THEATRE_ID, CITYHALL_NAME, CITYHALL_LOCATION " +
                "FROM THEATRE_CITY_HALL ORDER BY CITYHALL_ID", con))
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
                if (string.IsNullOrWhiteSpace(txtCityhallId.Text) ||
                    string.IsNullOrWhiteSpace(txtCityhallName.Text) ||
                    string.IsNullOrWhiteSpace(txtCityhallLocation.Text))
                {
                    SetMessage("Please fill all required fields before saving.", false);
                    return;
                }

                if (string.IsNullOrWhiteSpace(ddlTheatreId.SelectedValue))
                {
                    SetMessage("Please choose a theatre.", false);
                    return;
                }

                using (var con = new OracleConnection(Cs))
                using (var cmd = new OracleCommand(
                    "INSERT INTO THEATRE_CITY_HALL (CITYHALL_ID, THEATRE_ID, CITYHALL_NAME, CITYHALL_LOCATION) " +
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
                BindGrid();
                SetMessage("Theater City Hall added successfully.", true);
            }
            catch (OracleException ex) when (ex.Number == 1)
            {
                SetMessage("City Hall ID already exists. Please use a unique City Hall ID.", false);
            }
            catch (Exception ex)
            {
                SetMessage("Unable to add theater city hall. Please verify the provided values.", false);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ResetForm();
            lblMsg.Text = "";
            lblMsg.CssClass = "kc-msg";
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
                GridViewRow row = gvTheaters.Rows[e.RowIndex];
                var ddlEditTheatreId = row.FindControl("ddlEditTheatreId") as DropDownList;
                string theatreId = ddlEditTheatreId == null ? "" : ddlEditTheatreId.SelectedValue;
                string name     = e.NewValues["CITYHALL_NAME"]?.ToString()     ?? "";
                string location = e.NewValues["CITYHALL_LOCATION"]?.ToString() ?? "";

                if (string.IsNullOrWhiteSpace(theatreId) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(location))
                {
                    SetMessage("Please choose a theatre and complete name/location.", false);
                    return;
                }

                using (var con = new OracleConnection(Cs))
                using (var cmd = new OracleCommand(
                    "UPDATE THEATRE_CITY_HALL SET THEATRE_ID = :pTheatreId, CITYHALL_NAME = :pName, " +
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
                SetMessage("Theater City Hall updated successfully.", true);
            }
            catch (Exception ex)
            {
                SetMessage("Unable to update theater city hall. Please verify the provided values.", false);
            }
        }

        protected void gvTheaters_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            try
            {
                string id = gvTheaters.DataKeys[e.RowIndex].Value.ToString();

                using (var con = new OracleConnection(Cs))
                using (var cmd = new OracleCommand(
                    "DELETE FROM THEATRE_CITY_HALL WHERE CITYHALL_ID = :pId", con))
                {
                    cmd.Parameters.Add(":pId", OracleDbType.Varchar2).Value = id;

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                BindGrid();
                SetMessage("Theater City Hall deleted successfully.", true);
            }
            catch (OracleException ex) when (ex.Number == 2292)
            {
                SetMessage("Cannot delete this city hall because it is referenced by other records.", false);
            }
            catch (Exception ex)
            {
                SetMessage("Unable to delete theater city hall.", false);
            }
        }

        protected void gvTheaters_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow || (e.Row.RowState & DataControlRowState.Edit) == 0)
                return;

            var ddlEditTheatreId = e.Row.FindControl("ddlEditTheatreId") as DropDownList;
            if (ddlEditTheatreId == null)
                return;

            using (var con = new OracleConnection(Cs))
            using (var cmd = new OracleCommand("SELECT THEATRE_ID, THEATRE_NAME FROM THEATRE ORDER BY THEATRE_NAME", con))
            using (var da = new OracleDataAdapter(cmd))
            {
                var dt = new DataTable();
                da.Fill(dt);

                ddlEditTheatreId.DataSource = dt;
                ddlEditTheatreId.DataTextField = "THEATRE_NAME";
                ddlEditTheatreId.DataValueField = "THEATRE_ID";
                ddlEditTheatreId.DataBind();

                object theatreObj = DataBinder.Eval(e.Row.DataItem, "THEATRE_ID");
                if (theatreObj != null)
                {
                    var item = ddlEditTheatreId.Items.FindByValue(theatreObj.ToString());
                    if (item != null)
                        ddlEditTheatreId.SelectedValue = theatreObj.ToString();
                }
            }
        }

        private void ResetForm()
        {
            txtCityhallId.Text = "";
            txtCityhallName.Text = "";
            txtCityhallLocation.Text = "";
            if (ddlTheatreId.Items.Count > 0)
                ddlTheatreId.SelectedIndex = 0;
        }

        private void SetMessage(string message, bool success)
        {
            lblMsg.Text = message;
            lblMsg.CssClass = success ? "kc-msg kc-msg-success" : "kc-msg kc-msg-error";
        }
    }
}
