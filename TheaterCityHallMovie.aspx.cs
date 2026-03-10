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
        protected global::System.Web.UI.WebControls.Panel pnlResults;
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
            }
        }

        private void BindCityHallDropDown()
        {
            using (var con = new OracleConnection(Cs))
            using (var cmd = new OracleCommand(
                "SELECT CITYHALL_ID, CITYHALL_NAME FROM THEATRECITYHALL ORDER BY CITYHALL_NAME", con))
            using (var da = new OracleDataAdapter(cmd))
            {
                var dt = new DataTable();
                da.Fill(dt);

                ddlCityHall.DataSource     = dt;
                ddlCityHall.DataTextField  = "CITYHALL_NAME";
                ddlCityHall.DataValueField = "CITYHALL_ID";
                ddlCityHall.DataBind();
                ddlCityHall.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select City Hall --", ""));
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";

            if (string.IsNullOrEmpty(ddlCityHall.SelectedValue))
            {
                lblMsg.Text = "Please select a city hall.";
                pnlResults.Visible = false;
                return;
            }

            string cityhallId = ddlCityHall.SelectedValue;
            LoadCityHallDetails(cityhallId);
            LoadMovies(cityhallId);
            LoadShowtimes(cityhallId);
            pnlResults.Visible = true;
        }

        private void LoadCityHallDetails(string cityhallId)
        {
            string sql = "SELECT CITYHALL_ID, THEATRE_ID, CITYHALL_NAME, CITYHALL_LOCATION " +
                "FROM THEATRECITYHALL WHERE CITYHALL_ID = :pId";

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
                    lblCityhallId.Text       = r["CITYHALL_ID"].ToString();
                    lblTheatreId.Text        = r["THEATRE_ID"].ToString();
                    lblCityhallName.Text     = r["CITYHALL_NAME"].ToString();
                    lblCityhallLocation.Text = r["CITYHALL_LOCATION"].ToString();
                }
            }
        }

        private void LoadMovies(string cityhallId)
        {
            string sql =
                "SELECT m.MOVIE_ID, m.MOVIE_TITLE, m.DURATION, " +
                "m.MOVIE_LANGUAGE, m.MOVIE_GENRE, " +
                "m.\"release_date \" AS RELEASE_DATE " +
                "FROM \"CityHall-Movie\" cm " +
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
            }
        }

        private void LoadShowtimes(string cityhallId)
        {
            string sql =
                "SELECT s.SHOW_ID, s.SHOW_DATE, s.SHOW_TIME, " +
                "s.\"day_type \" AS DAY_TYPE " +
                "FROM \"SHOW\" s " +
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
            }
        }
    }
}
