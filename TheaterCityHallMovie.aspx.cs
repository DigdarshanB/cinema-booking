using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data;

namespace KumariCinemas
{
    public partial class TheaterCityHallMovie : System.Web.UI.Page
    {
        protected global::System.Web.UI.WebControls.Label lblMsg;
        protected global::System.Web.UI.WebControls.DropDownList ddlCityHall;
        protected global::System.Web.UI.WebControls.Button btnSearch;
        protected global::System.Web.UI.WebControls.Button btnReset;
        protected global::System.Web.UI.WebControls.Panel pnlResults;
        protected global::System.Web.UI.WebControls.Label lblSelectedCityHall;
        protected global::System.Web.UI.WebControls.Label lblMovieCount;
        protected global::System.Web.UI.WebControls.Label lblShowtimeCount;
        protected global::System.Web.UI.WebControls.Label lblCityhallId;
        protected global::System.Web.UI.WebControls.Label lblTheatreId;
        protected global::System.Web.UI.WebControls.Label lblCityhallName;
        protected global::System.Web.UI.WebControls.Label lblCityhallLocation;
        protected global::System.Web.UI.WebControls.GridView gvMovies;
        protected global::System.Web.UI.WebControls.GridView gvShowtimes;

        private string Cs => ConfigurationManager.ConnectionStrings["OracleDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCityHallDropDown();
                lblSelectedCityHall.Text = "-";
                lblMovieCount.Text = "0";
                lblShowtimeCount.Text = "0";
            }
        }

        private void BindCityHallDropDown()
        {
            try
            {
                using (var con = new OracleConnection(Cs))
                using (var cmd = new OracleCommand(
                    "SELECT CITYHALL_ID, CITYHALL_NAME FROM THEATRE_CITY_HALL ORDER BY CITYHALL_NAME", con))
                using (var da = new OracleDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);

                    ddlCityHall.DataSource = dt;
                    ddlCityHall.DataTextField = "CITYHALL_NAME";
                    ddlCityHall.DataValueField = "CITYHALL_ID";
                    ddlCityHall.DataBind();
                    ddlCityHall.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select City Hall --", ""));
                }
            }
            catch
            {
                ddlCityHall.Items.Clear();
                ddlCityHall.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select City Hall --", ""));
                SetMessage("Unable to load city hall list right now.", false);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            lblMsg.CssClass = "kc-msg";

            if (string.IsNullOrEmpty(ddlCityHall.SelectedValue))
            {
                SetMessage("Please select a city hall.", false);
                pnlResults.Visible = false;
                return;
            }

            string cityhallId = ddlCityHall.SelectedValue;
            try
            {
                LoadCityHallDetails(cityhallId);
                LoadMovies(cityhallId);
                LoadShowtimes(cityhallId);
                pnlResults.Visible = true;

                int movieCount;
                int showtimeCount;
                int.TryParse(lblMovieCount.Text, out movieCount);
                int.TryParse(lblShowtimeCount.Text, out showtimeCount);

                if (movieCount == 0 && showtimeCount == 0)
                    SetMessage("No related movies or showtimes found for the selected city hall.", false);
                else
                    SetMessage("Relationship records loaded successfully.", true);
            }
            catch (ApplicationException ex)
            {
                pnlResults.Visible = false;
                SetMessage(ex.Message, false);
            }
            catch
            {
                pnlResults.Visible = false;
                SetMessage("Unable to load relationship details right now.", false);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            if (ddlCityHall.Items.Count > 0)
                ddlCityHall.SelectedIndex = 0;

            pnlResults.Visible = false;
            lblMsg.Text = "";
            lblMsg.CssClass = "kc-msg";
            lblSelectedCityHall.Text = "-";
            lblMovieCount.Text = "0";
            lblShowtimeCount.Text = "0";
        }

        private void LoadCityHallDetails(string cityhallId)
        {
            try
            {
                string sql = "SELECT CITYHALL_ID, THEATRE_ID, CITYHALL_NAME, CITYHALL_LOCATION " +
                    "FROM THEATRE_CITY_HALL WHERE CITYHALL_ID = :pId";

                using (var con = new OracleConnection(Cs))
                using (var cmd = new OracleCommand(sql, con))
                using (var da = new OracleDataAdapter(cmd))
                {
                    cmd.Parameters.Add(":pId", OracleDbType.Varchar2).Value = cityhallId;

                    var dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        DataRow r = dt.Rows[0];
                        lblCityhallId.Text = r["CITYHALL_ID"].ToString();
                        lblTheatreId.Text = r["THEATRE_ID"].ToString();
                        lblCityhallName.Text = r["CITYHALL_NAME"].ToString();
                        lblCityhallLocation.Text = r["CITYHALL_LOCATION"].ToString();
                        lblSelectedCityHall.Text = lblCityhallName.Text;
                    }
                }
            }
            catch
            {
                throw new ApplicationException("Unable to load hall details right now.");
            }
        }

        private void LoadMovies(string cityhallId)
        {
            try
            {
                string sql =
                    "SELECT m.MOVIE_ID, m.MOVIE_TITLE, m.DURATION, " +
                    "m.MOVIE_LANGUAGE, m.MOVIE_GENRE, " +
                    "m.RELEASE_DATE " +
                    "FROM CITYHALL_MOVIE cm " +
                    "JOIN MOVIE m ON cm.MOVIE_ID = m.MOVIE_ID " +
                    "WHERE cm.CITYHALL_ID = :pId " +
                    "ORDER BY m.MOVIE_TITLE";

                using (var con = new OracleConnection(Cs))
                using (var cmd = new OracleCommand(sql, con))
                using (var da = new OracleDataAdapter(cmd))
                {
                    cmd.Parameters.Add(":pId", OracleDbType.Varchar2).Value = cityhallId;

                    var dt = new DataTable();
                    da.Fill(dt);

                    gvMovies.DataSource = dt;
                    gvMovies.DataBind();
                    lblMovieCount.Text = dt.Rows.Count.ToString();
                }
            }
            catch
            {
                throw new ApplicationException("Unable to load movie list right now.");
            }
        }

        private void LoadShowtimes(string cityhallId)
        {
            try
            {
                string sql =
                    "SELECT s.SHOW_ID, s.MOVIE_ID, m.MOVIE_TITLE, s.SHOW_DATE, s.SHOW_TIME " +
                    "FROM \"SHOW\" s " +
                    "LEFT JOIN MOVIE m ON s.MOVIE_ID = m.MOVIE_ID " +
                    "WHERE s.CITYHALL_ID = :pId " +
                    "ORDER BY s.SHOW_DATE DESC";

                using (var con = new OracleConnection(Cs))
                using (var cmd = new OracleCommand(sql, con))
                using (var da = new OracleDataAdapter(cmd))
                {
                    cmd.Parameters.Add(":pId", OracleDbType.Varchar2).Value = cityhallId;

                    var dt = new DataTable();
                    da.Fill(dt);

                    gvShowtimes.DataSource = dt;
                    gvShowtimes.DataBind();
                    lblShowtimeCount.Text = dt.Rows.Count.ToString();
                }
            }
            catch
            {
                throw new ApplicationException("Unable to load showtime details right now.");
            }
        }

        private void SetMessage(string message, bool success)
        {
            lblMsg.Text = message;
            lblMsg.CssClass = success ? "kc-msg kc-msg-success" : "kc-msg kc-msg-error";
        }
    }
}
