using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KumariCinemas
{
    public partial class ShowtimesDetails : System.Web.UI.Page
    {
        protected global::System.Web.UI.WebControls.Label lblMsg;
        protected global::System.Web.UI.WebControls.TextBox txtShowId;
        protected global::System.Web.UI.WebControls.DropDownList ddlCityhallId;
        protected global::System.Web.UI.WebControls.DropDownList ddlMovieId;
        protected global::System.Web.UI.WebControls.TextBox txtShowDate;
        protected global::System.Web.UI.WebControls.TextBox txtShowTime;
        protected global::System.Web.UI.WebControls.Button btnAdd;
        protected global::System.Web.UI.WebControls.Button btnClear;
        protected global::System.Web.UI.WebControls.GridView gvShows;

        private string Cs => ConfigurationManager.ConnectionStrings["OracleDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCityHallDropDown();
                BindMovieDropDown(ddlCityhallId.SelectedValue);
                BindGrid();
            }
        }

        private void BindCityHallDropDown()
        {
            string sql = "SELECT CITYHALL_ID FROM THEATRE_CITY_HALL ORDER BY CITYHALL_ID";

            using (var con = new OracleConnection(Cs))
            using (var cmd = new OracleCommand(sql, con))
            using (var da = new OracleDataAdapter(cmd))
            {
                var dt = new DataTable();
                da.Fill(dt);

                ddlCityhallId.DataSource = dt;
                ddlCityhallId.DataTextField = "CITYHALL_ID";
                ddlCityhallId.DataValueField = "CITYHALL_ID";
                ddlCityhallId.DataBind();
                ddlCityhallId.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select City Hall --", ""));
            }
        }

        private void BindMovieDropDown(string cityhallId)
        {
            string sql = "SELECT m.MOVIE_ID, m.MOVIE_TITLE " +
                "FROM CITYHALL_MOVIE cm " +
                "JOIN MOVIE m ON cm.MOVIE_ID = m.MOVIE_ID " +
                "WHERE cm.CITYHALL_ID = :pCityhallId " +
                "ORDER BY m.MOVIE_TITLE";

            using (var con = new OracleConnection(Cs))
            using (var cmd = new OracleCommand(sql, con))
            using (var da = new OracleDataAdapter(cmd))
            {
                cmd.Parameters.Add(":pCityhallId", OracleDbType.Varchar2).Value = cityhallId;

                var dt = new DataTable();
                da.Fill(dt);

                ddlMovieId.DataSource = dt;
                ddlMovieId.DataTextField = "MOVIE_TITLE";
                ddlMovieId.DataValueField = "MOVIE_ID";
                ddlMovieId.DataBind();

                string moviePlaceholder = "-- Select Movie --";
                if (string.IsNullOrWhiteSpace(cityhallId))
                    moviePlaceholder = "-- Select City Hall First --";
                else if (dt.Rows.Count == 0)
                    moviePlaceholder = "-- No Mapped Movies --";

                ddlMovieId.Items.Insert(0, new System.Web.UI.WebControls.ListItem(moviePlaceholder, ""));
            }
        }

        protected void ddlCityhallId_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMovieDropDown(ddlCityhallId.SelectedValue);

            if (string.IsNullOrWhiteSpace(ddlCityhallId.SelectedValue))
            {
                SetMessage("Select a city hall first to load movies.", false);
                return;
            }

            if (ddlMovieId.Items.Count <= 1)
                SetMessage("No movies are mapped to this city hall yet.", false);
            else
            {
                lblMsg.Text = "";
                lblMsg.CssClass = "kc-msg";
            }
        }

        private void BindEditCityHallDropDown(DropDownList ddl, string selectedCityhallId)
        {
            string sql = "SELECT CITYHALL_ID FROM THEATRE_CITY_HALL ORDER BY CITYHALL_ID";

            using (var con = new OracleConnection(Cs))
            using (var cmd = new OracleCommand(sql, con))
            using (var da = new OracleDataAdapter(cmd))
            {
                var dt = new DataTable();
                da.Fill(dt);

                ddl.DataSource = dt;
                ddl.DataTextField = "CITYHALL_ID";
                ddl.DataValueField = "CITYHALL_ID";
                ddl.DataBind();
                ddl.Items.Insert(0, new ListItem("-- Select City Hall --", ""));
            }

            if (!string.IsNullOrWhiteSpace(selectedCityhallId) && ddl.Items.FindByValue(selectedCityhallId) != null)
                ddl.SelectedValue = selectedCityhallId;
        }

        private void BindEditMovieDropDown(DropDownList ddl, string cityhallId, string selectedMovieId)
        {
            string sql = "SELECT m.MOVIE_ID, m.MOVIE_TITLE " +
                "FROM CITYHALL_MOVIE cm " +
                "JOIN MOVIE m ON cm.MOVIE_ID = m.MOVIE_ID " +
                "WHERE cm.CITYHALL_ID = :pCityhallId " +
                "ORDER BY m.MOVIE_TITLE";

            using (var con = new OracleConnection(Cs))
            using (var cmd = new OracleCommand(sql, con))
            using (var da = new OracleDataAdapter(cmd))
            {
                cmd.Parameters.Add(":pCityhallId", OracleDbType.Varchar2).Value = cityhallId;

                var dt = new DataTable();
                da.Fill(dt);

                ddl.DataSource = dt;
                ddl.DataTextField = "MOVIE_TITLE";
                ddl.DataValueField = "MOVIE_ID";
                ddl.DataBind();
                ddl.Items.Insert(0, new ListItem("-- Select Movie --", ""));
            }

            if (!string.IsNullOrWhiteSpace(selectedMovieId) && ddl.Items.FindByValue(selectedMovieId) != null)
                ddl.SelectedValue = selectedMovieId;
        }

        protected void gvShows_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow || (e.Row.RowState & DataControlRowState.Edit) == 0)
                return;

            string cityhallId = DataBinder.Eval(e.Row.DataItem, "CITYHALL_ID") == null ? "" : DataBinder.Eval(e.Row.DataItem, "CITYHALL_ID").ToString();
            string movieId = DataBinder.Eval(e.Row.DataItem, "MOVIE_ID") == null ? "" : DataBinder.Eval(e.Row.DataItem, "MOVIE_ID").ToString();

            var ddlEditCityhallId = e.Row.FindControl("ddlEditCityhallId") as DropDownList;
            var ddlEditMovieId = e.Row.FindControl("ddlEditMovieId") as DropDownList;
            if (ddlEditCityhallId == null || ddlEditMovieId == null)
                return;

            BindEditCityHallDropDown(ddlEditCityhallId, cityhallId);
            BindEditMovieDropDown(ddlEditMovieId, ddlEditCityhallId.SelectedValue, movieId);

            var txtEditShowDate = e.Row.FindControl("txtEditShowDate") as TextBox;
            var txtEditShowTime = e.Row.FindControl("txtEditShowTime") as TextBox;

            // Normalize edit values so date/time parsing is deterministic during update.
            object showDateObj = DataBinder.Eval(e.Row.DataItem, "SHOW_DATE");
            if (txtEditShowDate != null && showDateObj != null && showDateObj != DBNull.Value)
            {
                DateTime showDate = Convert.ToDateTime(showDateObj, CultureInfo.InvariantCulture);
                txtEditShowDate.Text = showDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            }

            object showTimeObj = DataBinder.Eval(e.Row.DataItem, "SHOW_TIME");
            if (txtEditShowTime != null && showTimeObj != null)
            {
                string normalizedShowTime;
                if (TryParseShowTime(showTimeObj.ToString(), out normalizedShowTime))
                    txtEditShowTime.Text = normalizedShowTime;
            }
        }

        protected void ddlEditCityhallId_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddlEditCityhallId = sender as DropDownList;
            if (ddlEditCityhallId == null)
                return;

            GridViewRow row = ddlEditCityhallId.NamingContainer as GridViewRow;
            if (row == null)
                return;

            var ddlEditMovieId = row.FindControl("ddlEditMovieId") as DropDownList;
            if (ddlEditMovieId == null)
                return;

            BindEditMovieDropDown(ddlEditMovieId, ddlEditCityhallId.SelectedValue, "");

            if (string.IsNullOrWhiteSpace(ddlEditCityhallId.SelectedValue))
                SetMessage("Select a city hall in edit mode to load movies.", false);
            else if (ddlEditMovieId.Items.Count <= 1)
                SetMessage("No movies are mapped to this city hall yet.", false);
            else
            {
                lblMsg.Text = "";
                lblMsg.CssClass = "kc-msg";
            }
        }

        private bool IsCityHallMovieMapped(string cityhallId, string movieId)
        {
            using (var con = new OracleConnection(Cs))
            using (var cmd = new OracleCommand(
                "SELECT COUNT(*) FROM CITYHALL_MOVIE WHERE CITYHALL_ID = :pCityhallId AND MOVIE_ID = :pMovieId", con))
            {
                cmd.Parameters.Add(":pCityhallId", OracleDbType.Varchar2).Value = cityhallId;
                cmd.Parameters.Add(":pMovieId", OracleDbType.Varchar2).Value = movieId;

                con.Open();
                object result = cmd.ExecuteScalar();
                return result != null && result != DBNull.Value && Convert.ToInt32(result) > 0;
            }
        }

        private bool HasDuplicateShow(string showId, string cityhallId, string movieId, DateTime showDate, string showTime)
        {
            using (var con = new OracleConnection(Cs))
            using (var cmd = new OracleCommand(
                "SELECT COUNT(*) FROM \"SHOW\" " +
                "WHERE CITYHALL_ID = :pCityhallId AND MOVIE_ID = :pMovieId AND SHOW_DATE = :pShowDate AND SHOW_TIME = :pShowTime " +
                "AND SHOW_ID <> :pShowId", con))
            {
                cmd.Parameters.Add(":pCityhallId", OracleDbType.Varchar2).Value = cityhallId;
                cmd.Parameters.Add(":pMovieId", OracleDbType.Varchar2).Value = movieId;
                cmd.Parameters.Add(":pShowDate", OracleDbType.Date).Value = showDate.Date;
                cmd.Parameters.Add(":pShowTime", OracleDbType.Varchar2).Value = showTime;
                cmd.Parameters.Add(":pShowId", OracleDbType.Varchar2).Value = showId;

                con.Open();
                object result = cmd.ExecuteScalar();
                return result != null && result != DBNull.Value && Convert.ToInt32(result) > 0;
            }
        }

        private bool TryParseShowTime(string value, out string normalized)
        {
            normalized = "";
            if (string.IsNullOrWhiteSpace(value))
                return false;

            TimeSpan parsed;
            if (!TimeSpan.TryParseExact(value.Trim(), "hh\\:mm", CultureInfo.InvariantCulture, out parsed))
                return false;

            normalized = parsed.ToString(@"hh\:mm", CultureInfo.InvariantCulture);
            return true;
        }

        private void BindGrid()
        {
            string sql = "SELECT SHOW_ID, CITYHALL_ID, MOVIE_ID, SHOW_DATE, SHOW_TIME " +
                "FROM \"SHOW\" ORDER BY SHOW_ID";

            using (var con = new OracleConnection(Cs))
            using (var cmd = new OracleCommand(sql, con))
            using (var da = new OracleDataAdapter(cmd))
            {
                var dt = new DataTable();
                da.Fill(dt);

                gvShows.DataSource = dt;
                gvShows.DataBind();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ddlCityhallId.SelectedValue))
                {
                    SetMessage("Please select a city hall.", false);
                    return;
                }

                if (string.IsNullOrWhiteSpace(ddlMovieId.SelectedValue))
                {
                    if (ddlMovieId.Items.Count <= 1)
                        SetMessage("No mapped movies are available for the selected city hall.", false);
                    else
                    SetMessage("Please select a movie.", false);
                    return;
                }

                DateTime showDate;
                if (!DateTime.TryParse(txtShowDate.Text.Trim(), out showDate))
                {
                    SetMessage("Please provide a valid show date.", false);
                    return;
                }

                string normalizedShowTime;
                if (!TryParseShowTime(txtShowTime.Text, out normalizedShowTime))
                {
                    SetMessage("Please provide a valid show time in HH:mm format.", false);
                    return;
                }

                if (!IsCityHallMovieMapped(ddlCityhallId.SelectedValue, ddlMovieId.SelectedValue))
                {
                    SetMessage("Selected movie is not mapped to the selected city hall.", false);
                    return;
                }

                if (HasDuplicateShow(txtShowId.Text.Trim(), ddlCityhallId.SelectedValue, ddlMovieId.SelectedValue, showDate, normalizedShowTime))
                {
                    SetMessage("A show with the same hall, movie, date, and time already exists.", false);
                    return;
                }

                string sql = "INSERT INTO \"SHOW\" " +
                    "(SHOW_ID, CITYHALL_ID, MOVIE_ID, SHOW_DATE, SHOW_TIME) " +
                    "VALUES (:pId, :pCityhallId, :pMovieId, :pShowDate, :pShowTime)";

                using (var con = new OracleConnection(Cs))
                using (var cmd = new OracleCommand(sql, con))
                {
                    cmd.Parameters.Add(":pId",         OracleDbType.Varchar2).Value = txtShowId.Text.Trim();
                    cmd.Parameters.Add(":pCityhallId", OracleDbType.Varchar2).Value = ddlCityhallId.SelectedValue;
                    cmd.Parameters.Add(":pMovieId",    OracleDbType.Varchar2).Value = ddlMovieId.SelectedValue;
                    cmd.Parameters.Add(":pShowDate",   OracleDbType.Date).Value = showDate.Date;
                    cmd.Parameters.Add(":pShowTime",   OracleDbType.Varchar2).Value = normalizedShowTime;

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                txtShowId.Text     = "";
                txtShowDate.Text   = "";
                txtShowTime.Text   = "";
                BindGrid();
                SetMessage("Showtime added successfully.", true);
            }
            catch (OracleException ex) when (ex.Number == 1)
            {
                SetMessage("Show ID already exists. Please use a unique Show ID.", false);
            }
            catch (Exception ex)
            {
                SetMessage("Unable to save showtime. Please check the provided values.", false);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ResetForm();
            lblMsg.Text = "";
            lblMsg.CssClass = "kc-msg";
        }

        protected void gvShows_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            gvShows.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void gvShows_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            gvShows.EditIndex = -1;
            BindGrid();
        }

        protected void gvShows_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            try
            {
                string id         = gvShows.DataKeys[e.RowIndex].Value.ToString();
                GridViewRow row = gvShows.Rows[e.RowIndex];
                var ddlEditCityhallId = row.FindControl("ddlEditCityhallId") as DropDownList;
                var ddlEditMovieId = row.FindControl("ddlEditMovieId") as DropDownList;
                var txtEditShowDate = row.FindControl("txtEditShowDate") as TextBox;
                var txtEditShowTime = row.FindControl("txtEditShowTime") as TextBox;
                string cityhallId = ddlEditCityhallId == null ? "" : ddlEditCityhallId.SelectedValue;
                string movieId    = ddlEditMovieId == null ? "" : ddlEditMovieId.SelectedValue;
                string showDate = txtEditShowDate == null ? "" : txtEditShowDate.Text;
                string showTime = txtEditShowTime == null ? "" : txtEditShowTime.Text;

                if (string.IsNullOrWhiteSpace(cityhallId) || string.IsNullOrWhiteSpace(movieId))
                {
                    SetMessage("City hall and movie are required.", false);
                    return;
                }

                if (!IsCityHallMovieMapped(cityhallId.Trim(), movieId.Trim()))
                {
                    SetMessage("Selected movie is not mapped to the selected city hall.", false);
                    return;
                }

                DateTime parsedShowDate;
                if (!DateTime.TryParseExact(showDate.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedShowDate))
                {
                    SetMessage("Please provide a valid show date in YYYY-MM-DD format.", false);
                    return;
                }

                string normalizedShowTime;
                if (!TryParseShowTime(showTime, out normalizedShowTime))
                {
                    SetMessage("Please provide a valid show time in HH:mm format.", false);
                    return;
                }

                if (HasDuplicateShow(id, cityhallId.Trim(), movieId.Trim(), parsedShowDate, normalizedShowTime))
                {
                    SetMessage("A show with the same hall, movie, date, and time already exists.", false);
                    return;
                }

                string sql = "UPDATE \"SHOW\" SET CITYHALL_ID = :pCityhallId, " +
                    "MOVIE_ID = :pMovieId, " +
                    "SHOW_DATE = :pShowDate, " +
                    "SHOW_TIME = :pShowTime WHERE SHOW_ID = :pId";

                using (var con = new OracleConnection(Cs))
                using (var cmd = new OracleCommand(sql, con))
                {
                    cmd.Parameters.Add(":pCityhallId", OracleDbType.Varchar2).Value = cityhallId.Trim();
                    cmd.Parameters.Add(":pMovieId",    OracleDbType.Varchar2).Value = movieId.Trim();
                    cmd.Parameters.Add(":pShowDate",   OracleDbType.Date).Value = parsedShowDate.Date;
                    cmd.Parameters.Add(":pShowTime",   OracleDbType.Varchar2).Value = normalizedShowTime;
                    cmd.Parameters.Add(":pId",         OracleDbType.Varchar2).Value = id;

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                gvShows.EditIndex = -1;
                BindGrid();
                SetMessage("Showtime updated successfully.", true);
            }
            catch (Exception ex)
            {
                SetMessage("Unable to update showtime. Please check the provided values.", false);
            }
        }

        protected void gvShows_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            try
            {
                string id = gvShows.DataKeys[e.RowIndex].Value.ToString();

                string sql = "DELETE FROM \"SHOW\" WHERE SHOW_ID = :pId";

                using (var con = new OracleConnection(Cs))
                using (var cmd = new OracleCommand(sql, con))
                {
                    cmd.Parameters.Add(":pId", OracleDbType.Varchar2).Value = id;

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                BindGrid();
                SetMessage("Showtime deleted successfully.", true);
            }
            catch (OracleException ex) when (ex.Number == 2292)
            {
                SetMessage("Cannot delete this showtime because related records exist.", false);
            }
            catch (Exception ex)
            {
                SetMessage("Unable to delete showtime.", false);
            }
        }

        private void ResetForm()
        {
            txtShowId.Text = "";
            txtShowDate.Text = "";
            txtShowTime.Text = "";
            if (ddlCityhallId.Items.Count > 0)
                ddlCityhallId.SelectedIndex = 0;
            if (ddlMovieId.Items.Count > 0)
                ddlMovieId.SelectedIndex = 0;
        }

        private void SetMessage(string message, bool success)
        {
            lblMsg.Text = message;
            lblMsg.CssClass = success ? "kc-msg kc-msg-success" : "kc-msg kc-msg-error";
        }
    }
}
