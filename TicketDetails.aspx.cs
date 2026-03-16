using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KumariCinemas
{
    public partial class TicketDetails : System.Web.UI.Page
    {
        protected global::System.Web.UI.WebControls.Label lblMsg;
        protected global::System.Web.UI.WebControls.DropDownList ddlCustomerId;
        protected global::System.Web.UI.WebControls.TextBox txtTicketId;
        protected global::System.Web.UI.WebControls.DropDownList ddlBookingId;
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
                BindCustomerDropDown();
                BindBookingDropDown(ddlCustomerId.SelectedValue);
                BindSeatDropDown(ddlBookingId.SelectedValue);
                BindGrid();
            }
        }

        private void BindEditBookingDropDown(DropDownList ddl, string customerId, string selectedBookingId)
        {
            using (var con = new OracleConnection(Cs))
            using (var cmd = new OracleCommand())
            using (var da = new OracleDataAdapter(cmd))
            {
                cmd.Connection = con;
                if (string.IsNullOrWhiteSpace(customerId))
                {
                    cmd.CommandText = "SELECT CAST(NULL AS VARCHAR2(50)) AS BOOKING_ID FROM DUAL WHERE 1 = 0";
                }
                else
                {
                    cmd.CommandText = "SELECT BOOKING_ID FROM BOOKING WHERE CUSTOMER_ID = :pCustomerId ORDER BY BOOKING_ID";
                    cmd.Parameters.Add(":pCustomerId", OracleDbType.Varchar2).Value = customerId;
                }

                var dt = new DataTable();
                da.Fill(dt);

                ddl.DataSource = dt;
                ddl.DataTextField = "BOOKING_ID";
                ddl.DataValueField = "BOOKING_ID";
                ddl.DataBind();
                ddl.Items.Insert(0, new ListItem("-- Select Booking --", ""));

                if (!string.IsNullOrWhiteSpace(selectedBookingId) && ddl.Items.FindByValue(selectedBookingId) != null)
                    ddl.SelectedValue = selectedBookingId;
            }
        }

        private void BindEditSeatDropDown(DropDownList ddl, string bookingId, string selectedSeatId, string ticketId)
        {
            using (var con = new OracleConnection(Cs))
            using (var cmd = new OracleCommand())
            using (var da = new OracleDataAdapter(cmd))
            {
                cmd.Connection = con;
                if (string.IsNullOrWhiteSpace(bookingId))
                {
                    cmd.CommandText = "SELECT CAST(NULL AS VARCHAR2(50)) AS SEAT_ID, CAST(NULL AS VARCHAR2(50)) AS SEAT_NO FROM DUAL WHERE 1 = 0";
                }
                else
                {
                    cmd.CommandText =
                        "SELECT ss.SEAT_ID, s.SEAT_NO " +
                        "FROM BOOKING b " +
                        "JOIN SHOW_SEAT ss ON b.SHOW_ID = ss.SHOW_ID " +
                        "JOIN SEAT s ON ss.SEAT_ID = s.SEAT_ID " +
                        "WHERE b.BOOKING_ID = :pBookingId " +
                        // Keep the current seat selectable while blocking seats used by other tickets in the same show.
                        "AND (ss.SEAT_ID = :pCurrentSeatId OR ss.SEAT_ID NOT IN ( " +
                        "    SELECT t.SEAT_ID " +
                        "    FROM TICKET t " +
                        "    JOIN BOOKING b2 ON t.BOOKING_ID = b2.BOOKING_ID " +
                        "    WHERE b2.SHOW_ID = b.SHOW_ID AND t.TICKET_ID <> :pTicketId " +
                        ")) " +
                        "ORDER BY s.SEAT_NO";
                    cmd.Parameters.Add(":pBookingId", OracleDbType.Varchar2).Value = bookingId;
                    cmd.Parameters.Add(":pCurrentSeatId", OracleDbType.Varchar2).Value = string.IsNullOrWhiteSpace(selectedSeatId) ? (object)DBNull.Value : selectedSeatId;
                    cmd.Parameters.Add(":pTicketId", OracleDbType.Varchar2).Value = ticketId;
                }

                var dt = new DataTable();
                da.Fill(dt);

                ddl.DataSource = dt;
                ddl.DataTextField = "SEAT_NO";
                ddl.DataValueField = "SEAT_ID";
                ddl.DataBind();

                string placeholderText = string.IsNullOrWhiteSpace(bookingId) ? "-- Select Booking First --" : "-- Select Seat --";
                if (!string.IsNullOrWhiteSpace(bookingId) && dt.Rows.Count == 0)
                    placeholderText = HasSeatsMappedForBookingShow(bookingId) ? "-- No Seats Available --" : "-- No Seats Mapped --";
                ddl.Items.Insert(0, new ListItem(placeholderText, ""));

                if (!string.IsNullOrWhiteSpace(selectedSeatId) && ddl.Items.FindByValue(selectedSeatId) != null)
                    ddl.SelectedValue = selectedSeatId;
            }
        }

        protected void gvTickets_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow || (e.Row.RowState & DataControlRowState.Edit) == 0)
                return;

            string customerId = DataBinder.Eval(e.Row.DataItem, "CUSTOMER_ID") == null ? "" : DataBinder.Eval(e.Row.DataItem, "CUSTOMER_ID").ToString();
            string bookingId = DataBinder.Eval(e.Row.DataItem, "BOOKING_ID") == null ? "" : DataBinder.Eval(e.Row.DataItem, "BOOKING_ID").ToString();
            string seatId = DataBinder.Eval(e.Row.DataItem, "SEAT_ID") == null ? "" : DataBinder.Eval(e.Row.DataItem, "SEAT_ID").ToString();
            string ticketId = DataBinder.Eval(e.Row.DataItem, "TICKET_ID") == null ? "" : DataBinder.Eval(e.Row.DataItem, "TICKET_ID").ToString();

            var ddlEditBookingId = e.Row.FindControl("ddlEditBookingId") as DropDownList;
            var ddlEditSeatId = e.Row.FindControl("ddlEditSeatId") as DropDownList;
            if (ddlEditBookingId == null || ddlEditSeatId == null)
                return;

            // Rebind edit controls from the current row context to keep values consistent.
            BindEditBookingDropDown(ddlEditBookingId, customerId, bookingId);
            BindEditSeatDropDown(ddlEditSeatId, ddlEditBookingId.SelectedValue, seatId, ticketId);
        }

        protected void ddlEditBookingId_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddlEditBookingId = sender as DropDownList;
            if (ddlEditBookingId == null)
                return;

            var row = ddlEditBookingId.NamingContainer as GridViewRow;
            if (row == null)
                return;

            string ticketId = gvTickets.DataKeys[row.RowIndex].Value.ToString();
            var ddlEditSeatId = row.FindControl("ddlEditSeatId") as DropDownList;
            if (ddlEditSeatId == null)
                return;

            BindEditSeatDropDown(ddlEditSeatId, ddlEditBookingId.SelectedValue, "", ticketId);

            if (string.IsNullOrWhiteSpace(ddlEditBookingId.SelectedValue))
                SetMessage("Select a booking in edit mode to load seats.", false);
            else if (ddlEditSeatId.Items.Count <= 1)
            {
                if (!HasSeatsMappedForBookingShow(ddlEditBookingId.SelectedValue))
                    SetMessage("No seats are mapped to this booking's show.", false);
                else
                    SetMessage("No seats are currently available for this booking.", false);
            }
            else
            {
                lblMsg.Text = "";
                lblMsg.CssClass = "kc-msg";
            }
        }

        private void BindCustomerDropDown()
        {
            using (var con = new OracleConnection(Cs))
            using (var cmd = new OracleCommand(
                "SELECT CUSTOMER_ID FROM CUSTOMER ORDER BY CUSTOMER_ID", con))
            using (var da = new OracleDataAdapter(cmd))
            {
                var dt = new DataTable();
                da.Fill(dt);

                ddlCustomerId.DataSource = dt;
                ddlCustomerId.DataTextField = "CUSTOMER_ID";
                ddlCustomerId.DataValueField = "CUSTOMER_ID";
                ddlCustomerId.DataBind();

                string customerPlaceholder = dt.Rows.Count == 0 ? "-- No Customers Found --" : "-- Select Customer --";
                ddlCustomerId.Items.Insert(0, new System.Web.UI.WebControls.ListItem(customerPlaceholder, ""));
            }
        }

        private void BindBookingDropDown(string customerId)
        {
            using (var con = new OracleConnection(Cs))
            using (var cmd = new OracleCommand())
            using (var da = new OracleDataAdapter(cmd))
            {
                cmd.Connection = con;
                if (string.IsNullOrWhiteSpace(customerId))
                {
                    cmd.CommandText = "SELECT CAST(NULL AS VARCHAR2(50)) AS BOOKING_ID FROM DUAL WHERE 1 = 0";
                }
                else
                {
                    cmd.CommandText = "SELECT BOOKING_ID FROM BOOKING WHERE CUSTOMER_ID = :pCustomerId ORDER BY BOOKING_ID";
                    cmd.Parameters.Add(":pCustomerId", OracleDbType.Varchar2).Value = customerId;
                }

                var dt = new DataTable();
                da.Fill(dt);

                ddlBookingId.DataSource = dt;
                ddlBookingId.DataTextField = "BOOKING_ID";
                ddlBookingId.DataValueField = "BOOKING_ID";
                ddlBookingId.DataBind();

                string placeholderText = "-- Select Booking --";
                if (string.IsNullOrWhiteSpace(customerId))
                    placeholderText = "-- Select Customer First --";
                else if (dt.Rows.Count == 0)
                    placeholderText = "-- No Bookings Found --";

                ddlBookingId.Items.Insert(0, new System.Web.UI.WebControls.ListItem(placeholderText, ""));
            }
        }

        protected void ddlCustomerId_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindBookingDropDown(ddlCustomerId.SelectedValue);
            BindSeatDropDown(ddlBookingId.SelectedValue);

            if (string.IsNullOrWhiteSpace(ddlCustomerId.SelectedValue))
            {
                SetMessage("Select a customer first to load bookings.", false);
                return;
            }

            if (ddlBookingId.Items.Count <= 1)
            {
                SetMessage("No bookings found for this customer.", false);
                return;
            }

            lblMsg.Text = "";
            lblMsg.CssClass = "kc-msg";
        }

        protected void ddlBookingId_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSeatDropDown(ddlBookingId.SelectedValue);

            if (string.IsNullOrWhiteSpace(ddlBookingId.SelectedValue))
            {
                SetMessage("Select a booking first to load seats.", false);
                return;
            }

            if (ddlSeatId.Items.Count > 1)
            {
                lblMsg.Text = "";
                lblMsg.CssClass = "kc-msg";
                return;
            }

            if (!HasSeatsMappedForBookingShow(ddlBookingId.SelectedValue))
                SetMessage("No seats are mapped to this booking's show.", false);
            else
                SetMessage("No seats are currently available for this booking.", false);
        }

        private bool IsBookingForCustomer(string bookingId, string customerId)
        {
            using (var con = new OracleConnection(Cs))
            using (var cmd = new OracleCommand(
                "SELECT COUNT(*) FROM BOOKING WHERE BOOKING_ID = :pBookingId AND CUSTOMER_ID = :pCustomerId", con))
            {
                cmd.Parameters.Add(":pBookingId", OracleDbType.Varchar2).Value = bookingId;
                cmd.Parameters.Add(":pCustomerId", OracleDbType.Varchar2).Value = customerId;

                con.Open();
                object result = cmd.ExecuteScalar();
                return result != null && result != DBNull.Value && Convert.ToInt32(result) > 0;
            }
        }

        private void BindSeatDropDown(string bookingId)
        {
            using (var con = new OracleConnection(Cs))
            using (var cmd = new OracleCommand())
            using (var da = new OracleDataAdapter(cmd))
            {
                cmd.Connection = con;
                if (string.IsNullOrWhiteSpace(bookingId))
                {
                    cmd.CommandText = "SELECT CAST(NULL AS VARCHAR2(50)) AS SEAT_ID, CAST(NULL AS VARCHAR2(50)) AS SEAT_NO FROM DUAL WHERE 1 = 0";
                }
                else
                {
                    cmd.CommandText =
                        "SELECT ss.SEAT_ID, s.SEAT_NO " +
                        "FROM BOOKING b " +
                        "JOIN SHOW_SEAT ss ON b.SHOW_ID = ss.SHOW_ID " +
                        "JOIN SEAT s ON ss.SEAT_ID = s.SEAT_ID " +
                        "WHERE b.BOOKING_ID = :pBookingId " +
                        "AND ss.SEAT_ID NOT IN ( " +
                        "    SELECT t.SEAT_ID " +
                        "    FROM TICKET t " +
                        "    JOIN BOOKING b2 ON t.BOOKING_ID = b2.BOOKING_ID " +
                        "    WHERE b2.SHOW_ID = b.SHOW_ID " +
                        ") " +
                        "ORDER BY s.SEAT_NO";
                    cmd.Parameters.Add(":pBookingId", OracleDbType.Varchar2).Value = bookingId;
                }

                var dt = new DataTable();
                da.Fill(dt);

                ddlSeatId.DataSource     = dt;
                ddlSeatId.DataTextField  = "SEAT_NO";
                ddlSeatId.DataValueField = "SEAT_ID";
                ddlSeatId.DataBind();

                string placeholderText = "-- Select Seat --";
                if (string.IsNullOrWhiteSpace(bookingId))
                    placeholderText = "-- Select Booking First --";
                else if (dt.Rows.Count == 0)
                    placeholderText = HasSeatsMappedForBookingShow(bookingId) ? "-- No Seats Available --" : "-- No Seats Mapped --";

                ddlSeatId.Items.Insert(0, new System.Web.UI.WebControls.ListItem(placeholderText, ""));
            }
        }

        private bool HasSeatsMappedForBookingShow(string bookingId)
        {
            if (string.IsNullOrWhiteSpace(bookingId))
                return false;

            using (var con = new OracleConnection(Cs))
            using (var cmd = new OracleCommand(
                "SELECT COUNT(*) " +
                "FROM BOOKING b " +
                "JOIN SHOW_SEAT ss ON b.SHOW_ID = ss.SHOW_ID " +
                "WHERE b.BOOKING_ID = :pBookingId", con))
            {
                cmd.Parameters.Add(":pBookingId", OracleDbType.Varchar2).Value = bookingId;
                con.Open();
                object result = cmd.ExecuteScalar();
                return result != null && result != DBNull.Value && Convert.ToInt32(result, CultureInfo.InvariantCulture) > 0;
            }
        }

        private string GetCustomerIdByBooking(string bookingId)
        {
            using (var con = new OracleConnection(Cs))
            using (var cmd = new OracleCommand(
                "SELECT CUSTOMER_ID FROM BOOKING WHERE BOOKING_ID = :pBookingId", con))
            {
                cmd.Parameters.Add(":pBookingId", OracleDbType.Varchar2).Value = bookingId;
                con.Open();
                object result = cmd.ExecuteScalar();
                return result == null || result == DBNull.Value ? "" : result.ToString();
            }
        }

        private bool IsSeatInShow(string bookingId, string seatId)
        {
            using (var con = new OracleConnection(Cs))
            using (var cmd = new OracleCommand(
                "SELECT COUNT(*) " +
                "FROM BOOKING b " +
                "JOIN SHOW_SEAT ss ON b.SHOW_ID = ss.SHOW_ID " +
                "WHERE b.BOOKING_ID = :pBookingId AND ss.SEAT_ID = :pSeatId", con))
            {
                cmd.Parameters.Add(":pBookingId", OracleDbType.Varchar2).Value = bookingId;
                cmd.Parameters.Add(":pSeatId", OracleDbType.Varchar2).Value = seatId;

                con.Open();
                object result = cmd.ExecuteScalar();
                return result != null && result != DBNull.Value && Convert.ToInt32(result, CultureInfo.InvariantCulture) > 0;
            }
        }

        private bool IsSeatAlreadyTicketedForShow(string bookingId, string seatId, string excludeTicketId = null)
        {
            using (var con = new OracleConnection(Cs))
            using (var cmd = new OracleCommand(
                "SELECT COUNT(*) " +
                "FROM TICKET t " +
                "JOIN BOOKING bExisting ON t.BOOKING_ID = bExisting.BOOKING_ID " +
                "WHERE bExisting.SHOW_ID = (SELECT SHOW_ID FROM BOOKING WHERE BOOKING_ID = :pBookingId) " +
                "AND t.SEAT_ID = :pSeatId " +
                "AND (:pExcludeTicketId IS NULL OR t.TICKET_ID <> :pExcludeTicketId)", con))
            {
                cmd.Parameters.Add(":pBookingId", OracleDbType.Varchar2).Value = bookingId;
                cmd.Parameters.Add(":pSeatId", OracleDbType.Varchar2).Value = seatId;
                cmd.Parameters.Add(":pExcludeTicketId", OracleDbType.Varchar2).Value = string.IsNullOrWhiteSpace(excludeTicketId) ? (object)DBNull.Value : excludeTicketId;

                con.Open();
                object result = cmd.ExecuteScalar();
                return result != null && result != DBNull.Value && Convert.ToInt32(result, CultureInfo.InvariantCulture) > 0;
            }
        }

        private void BindGrid()
        {
            using (var con = new OracleConnection(Cs))
            using (var cmd = new OracleCommand(
                "SELECT b.CUSTOMER_ID, t.TICKET_ID, t.BOOKING_ID, t.SEAT_ID, t.TICKET_PRICE " +
                "FROM TICKET t JOIN BOOKING b ON t.BOOKING_ID = b.BOOKING_ID " +
                "ORDER BY t.TICKET_ID", con))
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
                if (string.IsNullOrWhiteSpace(ddlCustomerId.SelectedValue))
                {
                    SetMessage("Please select a customer.", false);
                    return;
                }

                if (string.IsNullOrWhiteSpace(ddlBookingId.SelectedValue))
                {
                    if (ddlBookingId.Items.Count <= 1)
                        SetMessage("No bookings are available for the selected customer.", false);
                    else
                    SetMessage("Please select a booking.", false);
                    return;
                }

                if (string.IsNullOrWhiteSpace(ddlSeatId.SelectedValue))
                {
                    if (ddlSeatId.Items.Count <= 1)
                    {
                        if (!HasSeatsMappedForBookingShow(ddlBookingId.SelectedValue))
                            SetMessage("No seats are mapped to this booking's show yet.", false);
                        else
                            SetMessage("No seats are currently available for this booking.", false);
                    }
                    else
                    SetMessage("Please select a seat.", false);
                    return;
                }

                if (!IsBookingForCustomer(ddlBookingId.SelectedValue, ddlCustomerId.SelectedValue))
                {
                    SetMessage("Selected booking does not belong to the selected customer.", false);
                    return;
                }

                if (!IsSeatInShow(ddlBookingId.SelectedValue, ddlSeatId.SelectedValue))
                {
                    SetMessage("Selected seat does not belong to the booking's show.", false);
                    return;
                }

                if (IsSeatAlreadyTicketedForShow(ddlBookingId.SelectedValue, ddlSeatId.SelectedValue))
                {
                    SetMessage("Selected seat is already ticketed for this show.", false);
                    return;
                }

                decimal price;
                if (!decimal.TryParse(txtTicketPrice.Text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out price) &&
                    !decimal.TryParse(txtTicketPrice.Text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out price))
                {
                    SetMessage("Please provide a valid numeric ticket price.", false);
                    return;
                }

                using (var con = new OracleConnection(Cs))
                using (var cmd = new OracleCommand(
                    "INSERT INTO TICKET (TICKET_ID, BOOKING_ID, SEAT_ID, TICKET_PRICE) " +
                    "VALUES (:pTicketId, :pBookingId, :pSeatId, :pPrice)", con))
                {
                    cmd.Parameters.Add(":pTicketId", OracleDbType.Varchar2).Value = txtTicketId.Text.Trim();
                    cmd.Parameters.Add(":pBookingId", OracleDbType.Varchar2).Value = ddlBookingId.SelectedValue;
                    cmd.Parameters.Add(":pSeatId",   OracleDbType.Varchar2).Value = ddlSeatId.SelectedValue;
                    cmd.Parameters.Add(":pPrice",    OracleDbType.Decimal).Value = price;

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                txtTicketId.Text    = "";
                txtTicketPrice.Text = "";
                BindSeatDropDown(ddlBookingId.SelectedValue);
                BindGrid();
                SetMessage("Ticket added successfully.", true);
            }
            catch (OracleException ex) when (ex.Number == 1)
            {
                SetMessage("Ticket ID already exists. Please use a unique Ticket ID.", false);
            }
            catch (Exception ex)
            {
                SetMessage("Unable to save ticket. Please verify customer, booking, seat, and price.", false);
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
                GridViewRow row = gvTickets.Rows[e.RowIndex];
                var ddlEditBookingId = row.FindControl("ddlEditBookingId") as DropDownList;
                var ddlEditSeatId = row.FindControl("ddlEditSeatId") as DropDownList;
                string bookingId = ddlEditBookingId == null ? "" : ddlEditBookingId.SelectedValue;
                string seatId = ddlEditSeatId == null ? "" : ddlEditSeatId.SelectedValue;
                string price  = e.NewValues["TICKET_PRICE"]?.ToString() ?? "";

                if (string.IsNullOrWhiteSpace(bookingId) || string.IsNullOrWhiteSpace(seatId))
                {
                    SetMessage("Booking and seat are required.", false);
                    return;
                }

                string customerId = GetCustomerIdByBooking(bookingId.Trim());
                if (string.IsNullOrWhiteSpace(customerId))
                {
                    SetMessage("Selected booking is not valid.", false);
                    return;
                }

                if (!IsBookingForCustomer(bookingId.Trim(), customerId.Trim()))
                {
                    SetMessage("Selected booking does not belong to the selected customer.", false);
                    return;
                }

                if (!IsSeatInShow(bookingId.Trim(), seatId.Trim()))
                {
                    SetMessage("Selected seat does not belong to the booking's show.", false);
                    return;
                }

                if (IsSeatAlreadyTicketedForShow(bookingId.Trim(), seatId.Trim(), id))
                {
                    SetMessage("Selected seat is already ticketed for this show.", false);
                    return;
                }

                decimal parsedPrice;
                if (!decimal.TryParse(price.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out parsedPrice) &&
                    !decimal.TryParse(price.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out parsedPrice))
                {
                    SetMessage("Please provide a valid numeric ticket price.", false);
                    return;
                }

                using (var con = new OracleConnection(Cs))
                using (var cmd = new OracleCommand(
                    "UPDATE TICKET SET BOOKING_ID = :pBookingId, SEAT_ID = :pSeatId, TICKET_PRICE = :pPrice " +
                    "WHERE TICKET_ID = :pId", con))
                {
                    cmd.Parameters.Add(":pBookingId", OracleDbType.Varchar2).Value = bookingId.Trim();
                    cmd.Parameters.Add(":pSeatId", OracleDbType.Varchar2).Value = seatId.Trim();
                    cmd.Parameters.Add(":pPrice",  OracleDbType.Decimal).Value = parsedPrice;
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
                SetMessage("Unable to update ticket. Please verify customer, booking, seat, and price.", false);
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
            catch (OracleException ex) when (ex.Number == 2292)
            {
                SetMessage("Cannot delete this ticket because related records exist.", false);
            }
            catch (Exception ex)
            {
                SetMessage("Unable to delete ticket.", false);
            }
        }

        private void ResetForm()
        {
            txtTicketId.Text = "";
            txtTicketPrice.Text = "";
            if (ddlCustomerId.Items.Count > 0)
                ddlCustomerId.SelectedIndex = 0;
            BindBookingDropDown(ddlCustomerId.SelectedValue);
            BindSeatDropDown(ddlBookingId.SelectedValue);
            if (ddlBookingId.Items.Count > 0)
                ddlBookingId.SelectedIndex = 0;
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
