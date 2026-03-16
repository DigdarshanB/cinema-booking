using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data;

namespace KumariCinemas
{
    public partial class UserTicket : System.Web.UI.Page
    {
        protected global::System.Web.UI.WebControls.Label lblMsg;
        protected global::System.Web.UI.WebControls.DropDownList ddlCustomer;
        protected global::System.Web.UI.WebControls.Button btnSearch;
        protected global::System.Web.UI.WebControls.Button btnReset;
        protected global::System.Web.UI.WebControls.Panel pnlResults;
        protected global::System.Web.UI.WebControls.Label lblSelectedCustomer;
        protected global::System.Web.UI.WebControls.Label lblTicketCount;
        protected global::System.Web.UI.WebControls.Label lblCustomerId;
        protected global::System.Web.UI.WebControls.Label lblCustomerName;
        protected global::System.Web.UI.WebControls.Label lblContact;
        protected global::System.Web.UI.WebControls.Label lblUsername;
        protected global::System.Web.UI.WebControls.Label lblAddress;
        protected global::System.Web.UI.WebControls.GridView gvTickets;

        private string Cs => ConfigurationManager.ConnectionStrings["OracleDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCustomerDropDown();
                lblSelectedCustomer.Text = "-";
                lblTicketCount.Text = "0";
            }
        }

        private void BindCustomerDropDown()
        {
            try
            {
                using (var con = new OracleConnection(Cs))
                using (var cmd = new OracleCommand(
                    "SELECT CUSTOMER_ID, CUSTOMER_NAME FROM CUSTOMER ORDER BY CUSTOMER_NAME", con))
                using (var da = new OracleDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);

                    ddlCustomer.DataSource = dt;
                    ddlCustomer.DataTextField = "CUSTOMER_NAME";
                    ddlCustomer.DataValueField = "CUSTOMER_ID";
                    ddlCustomer.DataBind();
                    ddlCustomer.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Customer --", ""));
                }
            }
            catch
            {
                ddlCustomer.Items.Clear();
                ddlCustomer.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Customer --", ""));
                SetMessage("Unable to load customer list right now.", false);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            lblMsg.CssClass = "kc-msg";

            if (string.IsNullOrEmpty(ddlCustomer.SelectedValue))
            {
                SetMessage("Please select a customer.", false);
                pnlResults.Visible = false;
                return;
            }

            string customerId = ddlCustomer.SelectedValue;
            try
            {
                bool customerLoaded = LoadCustomerDetails(customerId);
                bool ticketsLoaded = LoadTickets(customerId);
                pnlResults.Visible = customerLoaded || ticketsLoaded;
            }
            catch
            {
                pnlResults.Visible = false;
                SetMessage("Unable to load ticket history right now.", false);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            if (ddlCustomer.Items.Count > 0)
                ddlCustomer.SelectedIndex = 0;
            pnlResults.Visible = false;
            lblMsg.Text = "";
            lblMsg.CssClass = "kc-msg";
            lblSelectedCustomer.Text = "-";
            lblTicketCount.Text = "0";
        }

        private bool LoadCustomerDetails(string customerId)
        {
            string sql = "SELECT CUSTOMER_ID, CUSTOMER_NAME, CUSTOMER_CONTACT, USERNAME, " +
                "ADDRESS FROM CUSTOMER WHERE CUSTOMER_ID = :pId";

            try
            {
                using (var con = new OracleConnection(Cs))
                using (var cmd = new OracleCommand(sql, con))
                using (var da = new OracleDataAdapter(cmd))
                {
                    cmd.Parameters.Add(":pId", OracleDbType.Varchar2).Value = customerId;

                    var dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        DataRow r = dt.Rows[0];
                        lblCustomerId.Text = r["CUSTOMER_ID"].ToString();
                        lblCustomerName.Text = r["CUSTOMER_NAME"].ToString();
                        lblContact.Text = r["CUSTOMER_CONTACT"].ToString();
                        lblUsername.Text = r["USERNAME"].ToString();
                        lblAddress.Text = r["ADDRESS"].ToString();
                        lblSelectedCustomer.Text = lblCustomerName.Text;
                        return true;
                    }
                }

                lblCustomerId.Text = "";
                lblCustomerName.Text = "";
                lblContact.Text = "";
                lblUsername.Text = "";
                lblAddress.Text = "";
                lblSelectedCustomer.Text = "-";
                SetMessage("Customer details are not available.", false);
                return false;
            }
            catch
            {
                throw;
            }
        }

        private bool LoadTickets(string customerId)
        {
            string sql =
                "SELECT t.TICKET_ID, b.BOOKING_ID, b.BOOKING_STATUS, " +
                "t.SEAT_ID, t.TICKET_PRICE, s.SHOW_DATE, s.SHOW_TIME " +
                "FROM CUSTOMER c " +
                "JOIN BOOKING b ON c.CUSTOMER_ID = b.CUSTOMER_ID " +
                "JOIN TICKET t ON b.BOOKING_ID = t.BOOKING_ID " +
                "JOIN \"SHOW\" s ON b.SHOW_ID = s.SHOW_ID " +
                "WHERE c.CUSTOMER_ID = :pId " +
                "AND s.SHOW_DATE >= ADD_MONTHS(SYSDATE, -6) " +
                "ORDER BY s.SHOW_DATE DESC";

            try
            {
                using (var con = new OracleConnection(Cs))
                using (var cmd = new OracleCommand(sql, con))
                using (var da = new OracleDataAdapter(cmd))
                {
                    cmd.Parameters.Add(":pId", OracleDbType.Varchar2).Value = customerId;

                    var dt = new DataTable();
                    da.Fill(dt);

                    gvTickets.DataSource = dt;
                    gvTickets.DataBind();
                    lblTicketCount.Text = dt.Rows.Count.ToString();

                    if (dt.Rows.Count == 0)
                    {
                        SetMessage("No tickets found for this customer for shows in the last 6 months.", false);
                        return false;
                    }

                    SetMessage("Ticket records loaded successfully.", true);
                    return true;
                }
            }
            catch
            {
                throw;
            }
        }

        protected string GetBookingStatusClass(object bookingStatus)
        {
            string value = bookingStatus == null ? "" : bookingStatus.ToString().Trim().ToLowerInvariant();

            if (value == "paid") return "kc-status-badge kc-status-badge-success";
            if (value == "pending") return "kc-status-badge kc-status-badge-warning";
            if (value == "cancelled") return "kc-status-badge kc-status-badge-danger";

            return "kc-status-badge";
        }

        private void SetMessage(string message, bool success)
        {
            lblMsg.Text = message;
            lblMsg.CssClass = success ? "kc-msg kc-msg-success" : "kc-msg kc-msg-error";
        }
    }
}
