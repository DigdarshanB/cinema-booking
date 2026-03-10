using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data;

namespace KumariCinemas
{
    public partial class UserDetails : System.Web.UI.Page
    {
        protected global::System.Web.UI.WebControls.Label lblMsg;
        protected global::System.Web.UI.WebControls.TextBox txtCustomerId;
        protected global::System.Web.UI.WebControls.TextBox txtCustomerName;
        protected global::System.Web.UI.WebControls.TextBox txtContact;
        protected global::System.Web.UI.WebControls.TextBox txtUsername;
        protected global::System.Web.UI.WebControls.TextBox txtAddress;
        protected global::System.Web.UI.WebControls.Button btnAdd;
        protected global::System.Web.UI.WebControls.GridView gvCustomers;

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

            using (var con = new OracleConnection(Cs))
            using (var cmd = new OracleCommand(
                "SELECT CUSTOMER_ID, CUSTOMER_NAME, CUSTOMER_CONTACT, USERNAME, \"address \" AS ADDRESS " +
                "FROM CUSTOMER ORDER BY CUSTOMER_ID", con))
            using (var da = new OracleDataAdapter(cmd))
            {
                var dt = new DataTable();
                da.Fill(dt);

                gvCustomers.DataSource = dt;
                gvCustomers.DataBind();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (var con = new OracleConnection(Cs))
                using (var cmd = new OracleCommand(
                    "INSERT INTO CUSTOMER (CUSTOMER_ID, CUSTOMER_NAME, CUSTOMER_CONTACT, USERNAME, \"address \") " +
                    "VALUES (:pId, :pName, :pContact, :pUsername, :pAddress)", con))
                {
                    cmd.Parameters.Add(":pId",       OracleDbType.Varchar2).Value = txtCustomerId.Text.Trim();
                    cmd.Parameters.Add(":pName",     OracleDbType.Varchar2).Value = txtCustomerName.Text.Trim();
                    cmd.Parameters.Add(":pContact",  OracleDbType.Varchar2).Value = txtContact.Text.Trim();
                    cmd.Parameters.Add(":pUsername", OracleDbType.Varchar2).Value = txtUsername.Text.Trim();
                    cmd.Parameters.Add(":pAddress",  OracleDbType.Varchar2).Value = txtAddress.Text.Trim();

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                txtCustomerId.Text   = "";
                txtCustomerName.Text = "";
                txtContact.Text      = "";
                txtUsername.Text     = "";
                txtAddress.Text      = "";
                lblMsg.Text = "Customer added successfully.";
                BindGrid();
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        protected void gvCustomers_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            gvCustomers.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void gvCustomers_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            gvCustomers.EditIndex = -1;
            BindGrid();
        }

        protected void gvCustomers_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            try
            {
                string id      = gvCustomers.DataKeys[e.RowIndex].Value.ToString();
                string name    = e.NewValues["CUSTOMER_NAME"]?.ToString()    ?? "";
                string contact = e.NewValues["CUSTOMER_CONTACT"]?.ToString() ?? "";
                string user    = e.NewValues["USERNAME"]?.ToString()         ?? "";
                string address = e.NewValues["ADDRESS"]?.ToString()          ?? "";

                using (var con = new OracleConnection(Cs))
                using (var cmd = new OracleCommand(
                    "UPDATE CUSTOMER SET CUSTOMER_NAME = :pName, CUSTOMER_CONTACT = :pContact, " +
                    "USERNAME = :pUsername, \"address \" = :pAddress WHERE CUSTOMER_ID = :pId", con))
                {
                    cmd.Parameters.Add(":pName",     OracleDbType.Varchar2).Value = name.Trim();
                    cmd.Parameters.Add(":pContact",  OracleDbType.Varchar2).Value = contact.Trim();
                    cmd.Parameters.Add(":pUsername", OracleDbType.Varchar2).Value = user.Trim();
                    cmd.Parameters.Add(":pAddress",  OracleDbType.Varchar2).Value = address.Trim();
                    cmd.Parameters.Add(":pId",       OracleDbType.Varchar2).Value = id;

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                gvCustomers.EditIndex = -1;
                BindGrid();
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        protected void gvCustomers_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            try
            {
                string id = gvCustomers.DataKeys[e.RowIndex].Value.ToString();

                using (var con = new OracleConnection(Cs))
                using (var cmd = new OracleCommand(
                    "DELETE FROM CUSTOMER WHERE CUSTOMER_ID = :pId", con))
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
