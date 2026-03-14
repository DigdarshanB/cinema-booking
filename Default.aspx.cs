using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Globalization;
using System.Web.UI;

namespace KumariCinemas
{
    public partial class _Default : Page
    {
        private string Cs => ConfigurationManager.ConnectionStrings["OracleDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDashboardMetrics();
            }
        }

        private void LoadDashboardMetrics()
        {
            try
            {
                using (var con = new OracleConnection(Cs))
                {
                    con.Open();

                    int totalMovies = GetScalarInt(con, "SELECT COUNT(*) FROM MOVIE");
                    int totalTheaters = GetScalarInt(con, "SELECT COUNT(*) FROM THEATRECITYHALL");
                    int ticketsSold = GetScalarInt(con, "SELECT COUNT(*) FROM TICKET");
                    int dailyBookings = GetScalarInt(con,
                        "SELECT COUNT(*) FROM BOOKING b " +
                        "JOIN \"SHOW\" s ON b.SHOW_ID = s.SHOW_ID " +
                        "WHERE TRUNC(s.SHOW_DATE) = TRUNC(SYSDATE)");

                    string popularMovie = GetScalarString(con,
                        "SELECT MOVIE_TITLE FROM (" +
                        "SELECT m.MOVIE_TITLE, COUNT(bt.TICKET_ID) AS SOLD_COUNT " +
                        "FROM MOVIE m " +
                        "LEFT JOIN \"Movie-Show\" ms ON m.MOVIE_ID = ms.MOVIE_ID " +
                        "LEFT JOIN \"SHOW\" s ON ms.SHOW_ID = s.SHOW_ID " +
                        "LEFT JOIN BOOKING b ON s.SHOW_ID = b.SHOW_ID " +
                        "LEFT JOIN \"Booking-Ticket\" bt ON b.BOOKING_ID = bt.BOOKING_ID " +
                        "GROUP BY m.MOVIE_TITLE " +
                        "ORDER BY SOLD_COUNT DESC, m.MOVIE_TITLE" +
                        ") WHERE ROWNUM = 1",
                        "N/A");

                    decimal occupancyPct = GetScalarDecimal(con,
                        "SELECT NVL(ROUND((COUNT(DISTINCT bt.TICKET_ID) * 100) / " +
                        "NULLIF(COUNT(DISTINCT ss.SHOW_ID || '-' || ss.SEAT_ID), 0), 0), 0) " +
                        "FROM \"SHOW\" s " +
                        "LEFT JOIN \"Show-Seat\" ss ON s.SHOW_ID = ss.SHOW_ID " +
                        "LEFT JOIN BOOKING b ON s.SHOW_ID = b.SHOW_ID " +
                        "LEFT JOIN \"Booking-Ticket\" bt ON b.BOOKING_ID = bt.BOOKING_ID");

                    string occupancyText = Math.Round(occupancyPct, 0).ToString("0", CultureInfo.InvariantCulture) + "%";

                    // get total movie count
                    litTotalMovies.Text = totalMovies.ToString("N0", CultureInfo.InvariantCulture);
                    litTotalTheaters.Text = totalTheaters.ToString("N0", CultureInfo.InvariantCulture);
                    litTicketsSold.Text = ticketsSold.ToString("N0", CultureInfo.InvariantCulture);
                    litDailyBookings.Text = dailyBookings.ToString("N0", CultureInfo.InvariantCulture);
                    litPopularMovie.Text = Server.HtmlEncode(popularMovie);
                    litOccupancyRate.Text = occupancyText;

                    // bind hero badge values
                    litHeroMovies.Text = totalMovies.ToString("N0", CultureInfo.InvariantCulture);
                    litHeroTheaters.Text = totalTheaters.ToString("N0", CultureInfo.InvariantCulture);
                    litHeroOccupancy.Text = occupancyText;
                }
            }
            catch
            {
                litTotalMovies.Text = "0";
                litTotalTheaters.Text = "0";
                litTicketsSold.Text = "0";
                litDailyBookings.Text = "0";
                litPopularMovie.Text = "N/A";
                litOccupancyRate.Text = "0%";

                litHeroMovies.Text = "0";
                litHeroTheaters.Text = "0";
                litHeroOccupancy.Text = "0%";
            }
        }

        private int GetScalarInt(OracleConnection con, string sql)
        {
            using (var cmd = new OracleCommand(sql, con))
            {
                object value = cmd.ExecuteScalar();
                return value == null || value == DBNull.Value ? 0 : Convert.ToInt32(value, CultureInfo.InvariantCulture);
            }
        }

        private decimal GetScalarDecimal(OracleConnection con, string sql)
        {
            using (var cmd = new OracleCommand(sql, con))
            {
                object value = cmd.ExecuteScalar();
                return value == null || value == DBNull.Value ? 0m : Convert.ToDecimal(value, CultureInfo.InvariantCulture);
            }
        }

        private string GetScalarString(OracleConnection con, string sql, string fallback)
        {
            using (var cmd = new OracleCommand(sql, con))
            {
                object value = cmd.ExecuteScalar();
                return value == null || value == DBNull.Value ? fallback : value.ToString();
            }
        }
    }
}