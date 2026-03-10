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
        protected global::System.Web.UI.WebControls.Panel pnlResults;
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
            }
        }

        private void BindCustomerDropDown()
        {
            using (var con = new OracleConnection(Cs))
            using (var cmd = new OracleCommand(
                "SELECT CUSTOMER_ID, CUSTOMER_NAME FROM CUSTOMER ORDER BY CUSTOMER_NAME", con))
            using (var da = new OracleDataAdapter(cmd))
            {
                var dt = new DataTable();
                da.Fill(dt);

                ddlCustomer.DataSource     = dt;
                ddlCustomer.DataTextField  = "CUSTOMER_NAME";
                ddlCustomer.DataValueField = "CUSTOMER_ID";
                ddlCustomer.DataBind();
                ddlCustomer.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Customer --", ""));
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";

            if (string.IsNullOrEmpty(ddlCustomer.SelectedValue))
            {
                lblMsg.Text = "Please select a customer.";
                pnlResults.Visible = false;
                return;
            }

            string customerId = ddlCustomer.SelectedValue;
            LoadCustomerDetails(customerId);
            LoadTickets(customerId);
            pnlResults.Visible = true;
        }

        private void LoadCustomerDetails(string customerId)
        {
            string sql = "SELECT CUSTOMER_ID, CUSTOMER_NAME, CUSTOMER_CONTACT, USERNAME, " +
                "\"address \" AS ADDRESS FROM CUSTOMER WHERE CUSTOMER_ID = :pId";

            using (var con = new OracleConnection(Cs))
            using (var cmd = new OracleCommand(sql, con))
            using (var da = new OracleDataAdapter(cmd))
            {
                cmd.Parameters.Add(":pId", OracleDbType.Varchar2).Value = customerId;

                var dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow r        = dt.Rows[0];
                    lblCustomerId.Text   = r["CUSTOMER_ID"].ToString();
                    lblCustomerName.Text = r["CUSTOMER_NAME"].ToString();
                    lblContact.Text      = r["CUSTOMER_CONTACT"].ToString();
                    lblUsername.Text     = r["USERNAME"].ToString();
                    lblAddress.Text      = r["ADDRESS"].ToString();
                }
            }
        }

        private void LoadTickets(string customerId)
        {
            string sql =
                "SELECT t.TICKET_ID, b.BOOKING_ID, b.BOOKING_STATUS, " +
                "t.SEAT_ID, t.TICKET_PRICE, s.SHOW_DATE, s.SHOW_TIME, " +
                "s.\"day_type \" AS DAY_TYPE " +
                "FROM CUSTOMER c " +
                "JOIN \"Customer-Booking\" cb ON c.CUSTOMER_ID = cb.CUSTOMER_ID " +
                "JOIN BOOKING b ON cb.BOOKING_ID = b.BOOKING_ID " +
                "JOIN \"Booking-Ticket\" bt ON b.BOOKING_ID = bt.BOOKING_ID " +
                "JOIN TICKET t ON bt.TICKET_ID = t.TICKET_ID " +
                "JOIN \"SHOW\" s ON b.SHOW_ID = s.SHOW_ID " +
                "WHERE c.CUSTOMER_ID = :pId " +
                "AND s.SHOW_DATE >= ADD_MONTHS(SYSDATE, -6) " +
                "ORDER BY s.SHOW_DATE DESC";

            using (var con = new OracleConnection(Cs))
            using (var cmd = new OracleCommand(sql, con))
            using (var da = new OracleDataAdapter(cmd))
            {
                cmd.Parameters.Add(":pId", OracleDbType.Varchar2).Value = customerId;

                var dt = new DataTable();
                da.Fill(dt);

                gvTickets.DataSource = dt;
                gvTickets.DataBind();

                if (dt.Rows.Count == 0)
                    lblMsg.Text = "No tickets found for this customer in the last 6 months.";
            }
        }
    }
}
