using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;

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
        protected global::System.Web.UI.WebControls.Button btnClear;
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
            using (var con = new OracleConnection(Cs))
            using (var cmd = new OracleCommand(
                "SELECT CUSTOMER_ID, CUSTOMER_NAME, CUSTOMER_CONTACT, USERNAME, ADDRESS " +
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
                string validationMessage;
                if (!ValidateCustomerInput(
                    txtCustomerId.Text,
                    txtCustomerName.Text,
                    txtContact.Text,
                    txtUsername.Text,
                    txtAddress.Text,
                    out validationMessage))
                {
                    SetMessage(validationMessage, false);
                    return;
                }

                using (var con = new OracleConnection(Cs))
                using (var cmd = new OracleCommand(
                    "INSERT INTO CUSTOMER (CUSTOMER_ID, CUSTOMER_NAME, CUSTOMER_CONTACT, USERNAME, ADDRESS) " +
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
                BindGrid();
                SetMessage("Customer added successfully.", true);
            }
            catch (OracleException ex) when (ex.Number == 1)
            {
                SetMessage("Customer ID or username already exists.", false);
            }
            catch (Exception ex)
            {
                SetMessage("Unable to add customer. Please verify the provided values.", false);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ResetForm();
            lblMsg.Text = "";
            lblMsg.CssClass = "kc-msg";
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

                string validationMessage;
                if (!ValidateCustomerInput(id, name, contact, user, address, out validationMessage))
                {
                    SetMessage(validationMessage, false);
                    return;
                }

                using (var con = new OracleConnection(Cs))
                using (var cmd = new OracleCommand(
                    "UPDATE CUSTOMER SET CUSTOMER_NAME = :pName, CUSTOMER_CONTACT = :pContact, " +
                    "USERNAME = :pUsername, ADDRESS = :pAddress WHERE CUSTOMER_ID = :pId", con))
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
                SetMessage("Customer updated successfully.", true);
            }
            catch (OracleException ex) when (ex.Number == 1)
            {
                SetMessage("Customer information conflicts with existing records.", false);
            }
            catch (Exception ex)
            {
                SetMessage("Unable to update customer. Please verify the provided values.", false);
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
                SetMessage("Customer deleted successfully.", true);
            }
            catch (OracleException ex) when (ex.Number == 2292)
            {
                SetMessage("Cannot delete this customer because related booking records exist.", false);
            }
            catch (Exception ex)
            {
                SetMessage("Unable to delete customer.", false);
            }
        }

        private void ResetForm()
        {
            txtCustomerId.Text   = "";
            txtCustomerName.Text = "";
            txtContact.Text      = "";
            txtUsername.Text     = "";
            txtAddress.Text      = "";
        }

        private void SetMessage(string message, bool success)
        {
            lblMsg.Text = message;
            lblMsg.CssClass = success ? "kc-msg kc-msg-success" : "kc-msg kc-msg-error";
        }

        private bool ValidateCustomerInput(string id, string name, string contact, string username, string address, out string message)
        {
            message = "";

            if (string.IsNullOrWhiteSpace(id) ||
                string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(contact) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(address))
            {
                message = "Please fill all required fields.";
                return false;
            }

            string trimmedContact = contact.Trim();
            if (!Regex.IsMatch(trimmedContact, "^[0-9+\\-() ]{7,20}$"))
            {
                message = "Please provide a valid contact number.";
                return false;
            }

            return true;
        }
    }
}
