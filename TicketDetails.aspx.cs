using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data;

namespace KumariCinemas
{
    public partial class TicketDetails : System.Web.UI.Page
    {
        protected global::System.Web.UI.WebControls.Label lblMsg;
        protected global::System.Web.UI.WebControls.TextBox txtTicketId;
        protected global::System.Web.UI.WebControls.DropDownList ddlSeatId;
        protected global::System.Web.UI.WebControls.TextBox txtTicketPrice;
        protected global::System.Web.UI.WebControls.Button btnAdd;
        protected global::System.Web.UI.WebControls.Button btnClear;
        protected global::System.Web.UI.WebControls.GridView gvTickets;

        private string Cs => ConfigurationManager.ConnectionStrings["OracleDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindSeatDropDown();
                BindGrid();
            }
        }

        private void BindSeatDropDown()
        {
            using (var con = new OracleConnection(Cs))
            using (var cmd = new OracleCommand(
                "SELECT SEAT_ID, SEAT_NO FROM SEAT ORDER BY SEAT_NO", con))
            using (var da = new OracleDataAdapter(cmd))
            {
                var dt = new DataTable();
                da.Fill(dt);

                ddlSeatId.DataSource     = dt;
                ddlSeatId.DataTextField  = "SEAT_NO";
                ddlSeatId.DataValueField = "SEAT_ID";
                ddlSeatId.DataBind();
            }
        }

        private void BindGrid()
        {
            using (var con = new OracleConnection(Cs))
            using (var cmd = new OracleCommand(
                "SELECT TICKET_ID, SEAT_ID, TICKET_PRICE FROM TICKET ORDER BY TICKET_ID", con))
            using (var da = new OracleDataAdapter(cmd))
            {
                var dt = new DataTable();
                da.Fill(dt);

                gvTickets.DataSource = dt;
                gvTickets.DataBind();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (var con = new OracleConnection(Cs))
                using (var cmd = new OracleCommand(
                    "INSERT INTO TICKET (TICKET_ID, SEAT_ID, TICKET_PRICE) " +
                    "VALUES (:pTicketId, :pSeatId, :pPrice)", con))
                {
                    cmd.Parameters.Add(":pTicketId", OracleDbType.Varchar2).Value = txtTicketId.Text.Trim();
                    cmd.Parameters.Add(":pSeatId",   OracleDbType.Varchar2).Value = ddlSeatId.SelectedValue;
                    cmd.Parameters.Add(":pPrice",    OracleDbType.Varchar2).Value = txtTicketPrice.Text.Trim();

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                txtTicketId.Text    = "";
                txtTicketPrice.Text = "";
                BindGrid();
                SetMessage("Ticket added successfully.", true);
            }
            catch (Exception ex)
            {
                SetMessage(ex.Message, false);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ResetForm();
            lblMsg.Text = "";
            lblMsg.CssClass = "kc-msg";
        }

        protected void gvTickets_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            gvTickets.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void gvTickets_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            gvTickets.EditIndex = -1;
            BindGrid();
        }

        protected void gvTickets_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            try
            {
                string id     = gvTickets.DataKeys[e.RowIndex].Value.ToString();
                string seatId = e.NewValues["SEAT_ID"]?.ToString()      ?? "";
                string price  = e.NewValues["TICKET_PRICE"]?.ToString() ?? "";

                using (var con = new OracleConnection(Cs))
                using (var cmd = new OracleCommand(
                    "UPDATE TICKET SET SEAT_ID = :pSeatId, TICKET_PRICE = :pPrice " +
                    "WHERE TICKET_ID = :pId", con))
                {
                    cmd.Parameters.Add(":pSeatId", OracleDbType.Varchar2).Value = seatId.Trim();
                    cmd.Parameters.Add(":pPrice",  OracleDbType.Varchar2).Value = price.Trim();
                    cmd.Parameters.Add(":pId",     OracleDbType.Varchar2).Value = id;

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                gvTickets.EditIndex = -1;
                BindGrid();
                SetMessage("Ticket updated successfully.", true);
            }
            catch (Exception ex)
            {
                SetMessage(ex.Message, false);
            }
        }

        protected void gvTickets_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            try
            {
                string id = gvTickets.DataKeys[e.RowIndex].Value.ToString();

                using (var con = new OracleConnection(Cs))
                using (var cmd = new OracleCommand(
                    "DELETE FROM TICKET WHERE TICKET_ID = :pId", con))
                {
                    cmd.Parameters.Add(":pId", OracleDbType.Varchar2).Value = id;

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                BindGrid();
                SetMessage("Ticket deleted successfully.", true);
            }
            catch (Exception ex)
            {
                SetMessage(ex.Message, false);
            }
        }

        private void ResetForm()
        {
            txtTicketId.Text = "";
            txtTicketPrice.Text = "";
            if (ddlSeatId.Items.Count > 0)
                ddlSeatId.SelectedIndex = 0;
        }

        private void SetMessage(string message, bool success)
        {
            lblMsg.Text = message;
            lblMsg.CssClass = success ? "kc-msg kc-msg-success" : "kc-msg kc-msg-error";
        }
    }
}
