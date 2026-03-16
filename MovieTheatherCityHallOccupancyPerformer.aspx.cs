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
            try
            {
                using (var con = new OracleConnection(Cs))
                using (var cmd = new OracleCommand(
                    "SELECT MOVIE_ID, MOVIE_TITLE FROM MOVIE ORDER BY MOVIE_TITLE", con))
                using (var da = new OracleDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);

                    ddlMovie.DataSource = dt;
                    ddlMovie.DataTextField = "MOVIE_TITLE";
                    ddlMovie.DataValueField = "MOVIE_ID";
                    ddlMovie.DataBind();
                    ddlMovie.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Movie --", ""));
                }
            }
            catch
            {
                ddlMovie.Items.Clear();
                ddlMovie.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Movie --", ""));
                SetMessage("Unable to load movie list right now.", false);
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

            try
            {
                LoadOccupancy(ddlMovie.SelectedValue);
                pnlResults.Visible = true;
            }
            catch
            {
                ResetResultState();
                pnlResults.Visible = false;
                SetMessage("Unable to load occupancy analytics right now.", false);
            }
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
                "COUNT(DISTINCT (s.SHOW_ID || '-' || ss.SEAT_ID)) AS TOTAL_SEATS, " +
                "COUNT(DISTINCT CASE " +
                "    WHEN UPPER(p.PAYMENT_STATUS) = 'PAID' AND ss_paid.SEAT_ID IS NOT NULL " +
                "    THEN (s.SHOW_ID || '-' || t.SEAT_ID) " +
                "END) AS PAID_TICKETS, " +
                "ROUND(COUNT(DISTINCT CASE " +
                "    WHEN UPPER(p.PAYMENT_STATUS) = 'PAID' AND ss_paid.SEAT_ID IS NOT NULL " +
                "    THEN (s.SHOW_ID || '-' || t.SEAT_ID) " +
                "END) * 100.0 / NULLIF(COUNT(DISTINCT (s.SHOW_ID || '-' || ss.SEAT_ID)), 0), 2) AS OCCUPANCY_PCT " +
                "FROM \"SHOW\" s " +
                "JOIN THEATRE_CITY_HALL tch ON s.CITYHALL_ID = tch.CITYHALL_ID " +
                "LEFT JOIN SHOW_SEAT ss ON s.SHOW_ID = ss.SHOW_ID " +
                "LEFT JOIN BOOKING b ON s.SHOW_ID = b.SHOW_ID " +
                "LEFT JOIN PAYMENT p ON b.BOOKING_ID = p.BOOKING_ID " +
                "LEFT JOIN TICKET t ON b.BOOKING_ID = t.BOOKING_ID " +
                "LEFT JOIN SHOW_SEAT ss_paid ON ss_paid.SHOW_ID = s.SHOW_ID AND ss_paid.SEAT_ID = t.SEAT_ID " +
                "WHERE s.MOVIE_ID = :pMovieId " +
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

                int mappedShowSeats = GetMovieShowSeatCount(movieId);
                int paidTickets = GetMoviePaidTicketCount(movieId);

                if (mappedShowSeats == 0)
                    SetMessage("No show-seat mappings found for this movie. Add SHOW_SEAT rows to get meaningful occupancy.", false);
                else if (paidTickets == 0)
                    SetMessage("No paid tickets found for this movie yet. Occupancy is based on paid tickets only.", false);
                else
                    SetMessage("Paid seat occupancy analytics loaded successfully.", true);
            }
        }

        private int GetMovieShowSeatCount(string movieId)
        {
            using (var con = new OracleConnection(Cs))
            using (var cmd = new OracleCommand(
                "SELECT COUNT(DISTINCT (s.SHOW_ID || '-' || ss.SEAT_ID)) " +
                "FROM \"SHOW\" s " +
                "JOIN SHOW_SEAT ss ON s.SHOW_ID = ss.SHOW_ID " +
                "WHERE s.MOVIE_ID = :pMovieId", con))
            {
                cmd.Parameters.Add(":pMovieId", OracleDbType.Varchar2).Value = movieId;
                con.Open();
                object result = cmd.ExecuteScalar();
                return result == null || result == DBNull.Value ? 0 : Convert.ToInt32(result);
            }
        }

        private int GetMoviePaidTicketCount(string movieId)
        {
            using (var con = new OracleConnection(Cs))
            using (var cmd = new OracleCommand(
                "SELECT COUNT(DISTINCT (s.SHOW_ID || '-' || t.SEAT_ID)) " +
                "FROM \"SHOW\" s " +
                "JOIN BOOKING b ON s.SHOW_ID = b.SHOW_ID " +
                "JOIN PAYMENT p ON b.BOOKING_ID = p.BOOKING_ID " +
                "JOIN TICKET t ON b.BOOKING_ID = t.BOOKING_ID " +
                "JOIN SHOW_SEAT ss ON s.SHOW_ID = ss.SHOW_ID AND t.SEAT_ID = ss.SEAT_ID " +
                "WHERE s.MOVIE_ID = :pMovieId AND UPPER(p.PAYMENT_STATUS) = 'PAID'", con))
            {
                cmd.Parameters.Add(":pMovieId", OracleDbType.Varchar2).Value = movieId;
                con.Open();
                object result = cmd.ExecuteScalar();
                return result == null || result == DBNull.Value ? 0 : Convert.ToInt32(result);
            }
        }

        private void ResetResultState()
        {
            lblSelectedMovie.Text = "-";
            lblTopOccupancy.Text = "0%";
            lblAvgOccupancy.Text = "0%";
            gvOccupancy.DataSource = null;
            gvOccupancy.DataBind();
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
