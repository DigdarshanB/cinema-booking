using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data;

namespace KumariCinemas
{
    public partial class MovieTheatherCityHallOccupancyPerformer : System.Web.UI.Page
    {
        protected global::System.Web.UI.WebControls.Label lblMsg;
        protected global::System.Web.UI.WebControls.DropDownList ddlMovie;
        protected global::System.Web.UI.WebControls.Button btnSearch;
        protected global::System.Web.UI.WebControls.Button btnReset;
        protected global::System.Web.UI.WebControls.Panel pnlResults;
        protected global::System.Web.UI.WebControls.Label lblSelectedMovie;
        protected global::System.Web.UI.WebControls.Label lblTopOccupancy;
        protected global::System.Web.UI.WebControls.Label lblAvgOccupancy;
        protected global::System.Web.UI.WebControls.GridView gvOccupancy;

        private string Cs => ConfigurationManager.ConnectionStrings["OracleDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMovieDropDown();
                lblSelectedMovie.Text = "-";
                lblTopOccupancy.Text = "0%";
                lblAvgOccupancy.Text = "0%";
            }
        }

        private void BindMovieDropDown()
        {
            using (var con = new OracleConnection(Cs))
            using (var cmd = new OracleCommand(
                "SELECT MOVIE_ID, MOVIE_TITLE FROM MOVIE ORDER BY MOVIE_TITLE", con))
            using (var da = new OracleDataAdapter(cmd))
            {
                var dt = new DataTable();
                da.Fill(dt);

                ddlMovie.DataSource     = dt;
                ddlMovie.DataTextField  = "MOVIE_TITLE";
                ddlMovie.DataValueField = "MOVIE_ID";
                ddlMovie.DataBind();
                ddlMovie.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Movie --", ""));
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            lblMsg.CssClass = "kc-msg";

            if (string.IsNullOrEmpty(ddlMovie.SelectedValue))
            {
                SetMessage("Please select a movie.", false);
                pnlResults.Visible = false;
                return;
            }

            LoadOccupancy(ddlMovie.SelectedValue);
            pnlResults.Visible = true;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            if (ddlMovie.Items.Count > 0)
                ddlMovie.SelectedIndex = 0;

            pnlResults.Visible = false;
            lblMsg.Text = "";
            lblMsg.CssClass = "kc-msg";
            lblSelectedMovie.Text = "-";
            lblTopOccupancy.Text = "0%";
            lblAvgOccupancy.Text = "0%";
        }

        private void LoadOccupancy(string movieId)
        {
            string sql =
                "SELECT * FROM ( " +
                "SELECT tch.CITYHALL_ID, tch.CITYHALL_NAME, tch.CITYHALL_LOCATION, " +
                "COUNT(DISTINCT ss.SEAT_ID) AS TOTAL_SEATS, " +
                "COUNT(DISTINCT CASE WHEN p.PAYMENT_STATUS = 'Paid' THEN bt.TICKET_ID END) AS PAID_TICKETS, " +
                "ROUND(COUNT(DISTINCT CASE WHEN p.PAYMENT_STATUS = 'Paid' THEN bt.TICKET_ID END) " +
                "* 100.0 / NULLIF(COUNT(DISTINCT ss.SEAT_ID), 0), 2) AS OCCUPANCY_PCT " +
                "FROM \"Movie-Show\" ms " +
                "JOIN \"SHOW\" s ON ms.SHOW_ID = s.SHOW_ID " +
                "JOIN THEATRECITYHALL tch ON s.CITYHALL_ID = tch.CITYHALL_ID " +
                "LEFT JOIN \"Show-Seat\" ss ON s.SHOW_ID = ss.SHOW_ID " +
                "LEFT JOIN BOOKING b ON s.SHOW_ID = b.SHOW_ID " +
                "LEFT JOIN \"Booking-Ticket\" bt ON b.BOOKING_ID = bt.BOOKING_ID " +
                "LEFT JOIN \"Booking-Payment\" bp ON b.BOOKING_ID = bp.BOOKING_ID " +
                "LEFT JOIN PAYMENT p ON bp.PAYMENT_ID = p.PAYMENT_ID " +
                "WHERE ms.MOVIE_ID = :pMovieId " +
                "GROUP BY tch.CITYHALL_ID, tch.CITYHALL_NAME, tch.CITYHALL_LOCATION " +
                "ORDER BY OCCUPANCY_PCT DESC " +
                ") WHERE ROWNUM <= 3";

            using (var con = new OracleConnection(Cs))
            using (var cmd = new OracleCommand(sql, con))
            using (var da = new OracleDataAdapter(cmd))
            {
                cmd.Parameters.Add(":pMovieId", OracleDbType.Varchar2).Value = movieId;

                var dt = new DataTable();
                da.Fill(dt);

                gvOccupancy.DataSource = dt;
                gvOccupancy.DataBind();

                lblSelectedMovie.Text = ddlMovie.SelectedItem == null ? "-" : ddlMovie.SelectedItem.Text;

                if (dt.Rows.Count == 0)
                {
                    lblTopOccupancy.Text = "0%";
                    lblAvgOccupancy.Text = "0%";
                    SetMessage("No occupancy data found for this movie.", false);
                    return;
                }

                decimal top = Convert.ToDecimal(dt.Rows[0]["OCCUPANCY_PCT"] == DBNull.Value ? 0 : dt.Rows[0]["OCCUPANCY_PCT"]);
                decimal total = 0m;

                foreach (DataRow row in dt.Rows)
                {
                    total += Convert.ToDecimal(row["OCCUPANCY_PCT"] == DBNull.Value ? 0 : row["OCCUPANCY_PCT"]);
                }

                decimal avg = total / dt.Rows.Count;
                lblTopOccupancy.Text = top.ToString("0.##") + "%";
                lblAvgOccupancy.Text = avg.ToString("0.##") + "%";
                SetMessage("Occupancy analytics loaded successfully.", true);
            }
        }

        protected string GetOccupancyClass(object occupancyValue)
        {
            decimal value;
            if (!decimal.TryParse((occupancyValue ?? "0").ToString(), out value))
                value = 0m;

            if (value >= 75m) return "kc-status-badge kc-status-badge-success";
            if (value >= 40m) return "kc-status-badge kc-status-badge-warning";
            return "kc-status-badge kc-status-badge-danger";
        }

        private void SetMessage(string message, bool success)
        {
            lblMsg.Text = message;
            lblMsg.CssClass = success ? "kc-msg kc-msg-success" : "kc-msg kc-msg-error";
        }
    }
}
