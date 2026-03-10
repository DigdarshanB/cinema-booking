using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data;

namespace KumariCinemas
{
    public partial class CustomerTest : System.Web.UI.Page
    {
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
                "SELECT CUSTOMER_ID, FULL_NAME, PHONE FROM TEST_CUSTOMER ORDER BY CUSTOMER_ID", con))
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
                    "INSERT INTO TEST_CUSTOMER (CUSTOMER_ID, FULL_NAME, PHONE) " +
                    "VALUES (TEST_CUSTOMER_SEQ.NEXTVAL, :pName, :pPhone)", con))
                {
                    cmd.Parameters.Add(":pName", OracleDbType.Varchar2).Value = txtName.Text.Trim();
                    cmd.Parameters.Add(":pPhone", OracleDbType.Varchar2).Value = txtPhone.Text.Trim();

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                txtName.Text = "";
                txtPhone.Text = "";
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
                int id = Convert.ToInt32(gvCustomers.DataKeys[e.RowIndex].Value);
                string name = e.NewValues["FULL_NAME"]?.ToString() ?? "";
                string phone = e.NewValues["PHONE"]?.ToString() ?? "";

                using (var con = new OracleConnection(Cs))
                using (var cmd = new OracleCommand(
                    "UPDATE TEST_CUSTOMER SET FULL_NAME = :pName, PHONE = :pPhone WHERE CUSTOMER_ID = :pId", con))
                {
                    cmd.Parameters.Add(":pName", OracleDbType.Varchar2).Value = name.Trim();
                    cmd.Parameters.Add(":pPhone", OracleDbType.Varchar2).Value = phone.Trim();
                    cmd.Parameters.Add(":pId", OracleDbType.Int32).Value = id;

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
                int id = Convert.ToInt32(gvCustomers.DataKeys[e.RowIndex].Value);

                using (var con = new OracleConnection(Cs))
                using (var cmd = new OracleCommand(
                    "DELETE FROM TEST_CUSTOMER WHERE CUSTOMER_ID = :pId", con))
                {
                    cmd.Parameters.Add(":pId", OracleDbType.Int32).Value = id;

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