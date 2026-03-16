using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web.Script.Serialization;
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
            lblDashboardMsg.Text = "";
            lblDashboardMsg.CssClass = "kc-msg";

            try
            {
                using (var con = new OracleConnection(Cs))
                {
                    con.Open();

                    int totalMovies = GetScalarInt(con, "SELECT COUNT(*) FROM MOVIE");
                    int totalTheaters = GetScalarInt(con, "SELECT COUNT(*) FROM THEATRE");
                    // Count paid occupied show-seat pairs to avoid inflated sold totals.
                    int ticketsSold = GetScalarInt(con,
                        "SELECT COUNT(DISTINCT CASE " +
                        "WHEN UPPER(p.PAYMENT_STATUS) = 'PAID' AND ss.SEAT_ID IS NOT NULL " +
                        "THEN (s.SHOW_ID || '-' || t.SEAT_ID) END) " +
                        "FROM \"SHOW\" s " +
                        "LEFT JOIN BOOKING b ON s.SHOW_ID = b.SHOW_ID " +
                        "LEFT JOIN PAYMENT p ON b.BOOKING_ID = p.BOOKING_ID " +
                        "LEFT JOIN TICKET t ON b.BOOKING_ID = t.BOOKING_ID " +
                        "LEFT JOIN SHOW_SEAT ss ON ss.SHOW_ID = s.SHOW_ID AND ss.SEAT_ID = t.SEAT_ID");
                    int dailyBookings = GetScalarInt(con,
                        "SELECT COUNT(*) FROM BOOKING b " +
                        "JOIN \"SHOW\" s ON b.SHOW_ID = s.SHOW_ID " +
                        "WHERE TRUNC(s.SHOW_DATE) = TRUNC(SYSDATE)");

                    string popularMovie = GetScalarString(con,
                        "SELECT MOVIE_TITLE FROM (" +
                        "SELECT m.MOVIE_TITLE, COUNT(DISTINCT CASE " +
                        "WHEN UPPER(p.PAYMENT_STATUS) = 'PAID' AND ss.SEAT_ID IS NOT NULL " +
                        "THEN (s.SHOW_ID || '-' || t.SEAT_ID) END) AS SOLD_COUNT " +
                        "FROM MOVIE m " +
                        "LEFT JOIN \"SHOW\" s ON m.MOVIE_ID = s.MOVIE_ID " +
                        "LEFT JOIN BOOKING b ON s.SHOW_ID = b.SHOW_ID " +
                        "LEFT JOIN PAYMENT p ON b.BOOKING_ID = p.BOOKING_ID " +
                        "LEFT JOIN TICKET t ON b.BOOKING_ID = t.BOOKING_ID " +
                        "LEFT JOIN SHOW_SEAT ss ON ss.SHOW_ID = s.SHOW_ID AND ss.SEAT_ID = t.SEAT_ID " +
                        "GROUP BY m.MOVIE_TITLE " +
                        "ORDER BY SOLD_COUNT DESC, m.MOVIE_TITLE" +
                        ") WHERE ROWNUM = 1",
                        "N/A");

                    decimal occupancyPct = GetScalarDecimal(con,
                        // Use mapped show seats as denominator and paid occupied seats as numerator.
                        "SELECT NVL(ROUND((COUNT(DISTINCT CASE WHEN UPPER(p.PAYMENT_STATUS) = 'PAID' AND ss_paid.SEAT_ID IS NOT NULL THEN (s.SHOW_ID || '-' || t.SEAT_ID) END) * 100) / " +
                        "NULLIF(COUNT(DISTINCT ss.SHOW_ID || '-' || ss.SEAT_ID), 0), 0), 0) " +
                        "FROM \"SHOW\" s " +
                        "LEFT JOIN SHOW_SEAT ss ON s.SHOW_ID = ss.SHOW_ID " +
                        "LEFT JOIN BOOKING b ON s.SHOW_ID = b.SHOW_ID " +
                        "LEFT JOIN PAYMENT p ON b.BOOKING_ID = p.BOOKING_ID " +
                        "LEFT JOIN TICKET t ON b.BOOKING_ID = t.BOOKING_ID " +
                        "LEFT JOIN SHOW_SEAT ss_paid ON ss_paid.SHOW_ID = s.SHOW_ID AND ss_paid.SEAT_ID = t.SEAT_ID");

                    string occupancyText = Math.Round(occupancyPct, 0).ToString("0", CultureInfo.InvariantCulture) + "%";

                    litTotalMovies.Text = totalMovies.ToString("N0", CultureInfo.InvariantCulture);
                    litTotalTheaters.Text = totalTheaters.ToString("N0", CultureInfo.InvariantCulture);
                    litTicketsSold.Text = ticketsSold.ToString("N0", CultureInfo.InvariantCulture);
                    litDailyBookings.Text = dailyBookings.ToString("N0", CultureInfo.InvariantCulture);
                    litPopularMovie.Text = Server.HtmlEncode(popularMovie);
                    litOccupancyRate.Text = occupancyText;

                    litHeroMovies.Text = totalMovies.ToString("N0", CultureInfo.InvariantCulture);
                    litHeroTheaters.Text = totalTheaters.ToString("N0", CultureInfo.InvariantCulture);
                    litHeroOccupancy.Text = occupancyText;

                    int totalShowSeatMappings = GetScalarInt(con, "SELECT COUNT(*) FROM SHOW_SEAT");
                    int totalBookings = GetScalarInt(con, "SELECT COUNT(*) FROM BOOKING");
                    int totalPayments = GetScalarInt(con, "SELECT COUNT(*) FROM PAYMENT");

                    if (totalShowSeatMappings == 0)
                    {
                        lblDashboardMsg.Text = "Analytics need SHOW_SEAT mappings before occupancy and sold metrics can be meaningful.";
                        lblDashboardMsg.CssClass = "kc-msg kc-msg-error";
                    }
                    else if (totalBookings == 0 || totalPayments == 0)
                    {
                        lblDashboardMsg.Text = "Analytics are limited until booking and payment records are available.";
                        lblDashboardMsg.CssClass = "kc-msg kc-msg-error";
                    }

                    try
                    {
                        PopulateChartData(con);
                    }
                    catch
                    {
                        hfChartBookings.Value = "{\"Labels\":[],\"Values\":[]}";
                        hfChartTickets.Value = "{\"Labels\":[],\"Values\":[]}";
                        hfChartOccupancy.Value = "{\"Labels\":[],\"Values\":[]}";
                        lblDashboardMsg.Text = "Some dashboard charts are temporarily unavailable.";
                        lblDashboardMsg.CssClass = "kc-msg kc-msg-error";
                    }
                }
            }
            catch
            {
                lblDashboardMsg.Text = "Unable to load dashboard metrics right now.";
                lblDashboardMsg.CssClass = "kc-msg kc-msg-error";
                litTotalMovies.Text = "0";
                litTotalTheaters.Text = "0";
                litTicketsSold.Text = "0";
                litDailyBookings.Text = "0";
                litPopularMovie.Text = "N/A";
                litOccupancyRate.Text = "0%";

                litHeroMovies.Text = "0";
                litHeroTheaters.Text = "0";
                litHeroOccupancy.Text = "0%";

                hfChartBookings.Value = "{\"Labels\":[],\"Values\":[]}";
                hfChartTickets.Value = "{\"Labels\":[],\"Values\":[]}";
                hfChartOccupancy.Value = "{\"Labels\":[],\"Values\":[]}";
            }
        }

        private void PopulateChartData(OracleConnection con)
        {
            var serializer = new JavaScriptSerializer();

            var bookingMap = new Dictionary<string, int>(StringComparer.Ordinal);
            using (var cmd = new OracleCommand(
                "SELECT TO_CHAR(TRUNC(s.SHOW_DATE), 'YYYY-MM-DD') AS DAY_LABEL, COUNT(*) AS CNT " +
                "FROM BOOKING b " +
                "JOIN \"SHOW\" s ON b.SHOW_ID = s.SHOW_ID " +
                "WHERE TRUNC(s.SHOW_DATE) BETWEEN TRUNC(SYSDATE) - 6 AND TRUNC(SYSDATE) " +
                "GROUP BY TRUNC(s.SHOW_DATE) " +
                "ORDER BY TRUNC(s.SHOW_DATE)", con))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    bookingMap[reader.GetString(0)] = Convert.ToInt32(reader.GetDecimal(1), CultureInfo.InvariantCulture);
                }
            }

            var bookingLabels = new List<string>();
            var bookingValues = new List<int>();
            for (int i = 6; i >= 0; i--)
            {
                string key = DateTime.Today.AddDays(-i).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                bookingLabels.Add(DateTime.Today.AddDays(-i).ToString("dd MMM", CultureInfo.InvariantCulture));
                int value;
                bookingValues.Add(bookingMap.TryGetValue(key, out value) ? value : 0);
            }

            var ticketsLabels = new List<string>();
            var ticketsValues = new List<decimal>();
            using (var cmd = new OracleCommand(
                "SELECT * FROM (" +
                "SELECT m.MOVIE_TITLE, COUNT(DISTINCT CASE " +
                "WHEN UPPER(p.PAYMENT_STATUS) = 'PAID' AND ss.SEAT_ID IS NOT NULL " +
                "THEN (s.SHOW_ID || '-' || t.SEAT_ID) END) AS SOLD_COUNT " +
                "FROM MOVIE m " +
                "LEFT JOIN \"SHOW\" s ON m.MOVIE_ID = s.MOVIE_ID " +
                "LEFT JOIN BOOKING b ON s.SHOW_ID = b.SHOW_ID " +
                "LEFT JOIN PAYMENT p ON b.BOOKING_ID = p.BOOKING_ID " +
                "LEFT JOIN TICKET t ON b.BOOKING_ID = t.BOOKING_ID " +
                "LEFT JOIN SHOW_SEAT ss ON ss.SHOW_ID = s.SHOW_ID AND ss.SEAT_ID = t.SEAT_ID " +
                "GROUP BY m.MOVIE_TITLE " +
                "ORDER BY SOLD_COUNT DESC, m.MOVIE_TITLE" +
                ") WHERE ROWNUM <= 5", con))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    ticketsLabels.Add(reader.IsDBNull(0) ? "N/A" : reader.GetString(0));
                    ticketsValues.Add(reader.IsDBNull(1) ? 0m : reader.GetDecimal(1));
                }
            }

            var occupancyLabels = new List<string>();
            var occupancyValues = new List<decimal>();
            using (var cmd = new OracleCommand(
                "SELECT * FROM (" +
                "SELECT tch.CITYHALL_NAME, " +
                "NVL(ROUND((COUNT(DISTINCT CASE " +
                "WHEN UPPER(p.PAYMENT_STATUS) = 'PAID' AND ss_paid.SEAT_ID IS NOT NULL " +
                "THEN (s.SHOW_ID || '-' || t.SEAT_ID) END) * 100) / " +
                "NULLIF(COUNT(DISTINCT (s.SHOW_ID || '-' || ss.SEAT_ID)), 0), 0), 0) AS OCCUPANCY_PCT " +
                "FROM THEATRE_CITY_HALL tch " +
                "LEFT JOIN \"SHOW\" s ON tch.CITYHALL_ID = s.CITYHALL_ID " +
                "LEFT JOIN SHOW_SEAT ss ON s.SHOW_ID = ss.SHOW_ID " +
                "LEFT JOIN BOOKING b ON s.SHOW_ID = b.SHOW_ID " +
                "LEFT JOIN PAYMENT p ON b.BOOKING_ID = p.BOOKING_ID " +
                "LEFT JOIN TICKET t ON b.BOOKING_ID = t.BOOKING_ID " +
                "LEFT JOIN SHOW_SEAT ss_paid ON ss_paid.SHOW_ID = s.SHOW_ID AND ss_paid.SEAT_ID = t.SEAT_ID " +
                "GROUP BY tch.CITYHALL_NAME " +
                "ORDER BY OCCUPANCY_PCT DESC, tch.CITYHALL_NAME" +
                ") WHERE ROWNUM <= 7", con))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    occupancyLabels.Add(reader.IsDBNull(0) ? "N/A" : reader.GetString(0));
                    occupancyValues.Add(reader.IsDBNull(1) ? 0m : reader.GetDecimal(1));
                }
            }

            hfChartBookings.Value = serializer.Serialize(new { Labels = bookingLabels, Values = bookingValues });
            hfChartTickets.Value = serializer.Serialize(new { Labels = ticketsLabels, Values = ticketsValues });
            hfChartOccupancy.Value = serializer.Serialize(new { Labels = occupancyLabels, Values = occupancyValues });
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