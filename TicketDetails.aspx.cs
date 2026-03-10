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
            lblMsg.Text = "";

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
                lblMsg.Text = "Ticket added successfully.";
                BindGrid();
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
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
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
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
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }
    }
}
