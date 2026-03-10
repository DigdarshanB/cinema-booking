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
        protected global::System.Web.UI.WebControls.Panel pnlResults;
        protected global::System.Web.UI.WebControls.GridView gvOccupancy;

        private string Cs => ConfigurationManager.ConnectionStrings["OracleDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMovieDropDown();
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

            if (string.IsNullOrEmpty(ddlMovie.SelectedValue))
            {
                lblMsg.Text = "Please select a movie.";
                pnlResults.Visible = false;
                return;
            }

            LoadOccupancy(ddlMovie.SelectedValue);
            pnlResults.Visible = true;
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

                if (dt.Rows.Count == 0)
                    lblMsg.Text = "No occupancy data found for this movie.";
            }
        }
    }
}
