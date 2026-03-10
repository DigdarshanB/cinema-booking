using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data;

namespace KumariCinemas
{
    public partial class MovieDetails : System.Web.UI.Page
    {
        protected global::System.Web.UI.WebControls.Label lblMsg;
        protected global::System.Web.UI.WebControls.TextBox txtMovieId;
        protected global::System.Web.UI.WebControls.TextBox txtMovieTitle;
        protected global::System.Web.UI.WebControls.TextBox txtDuration;
        protected global::System.Web.UI.WebControls.TextBox txtLanguage;
        protected global::System.Web.UI.WebControls.TextBox txtGenre;
        protected global::System.Web.UI.WebControls.TextBox txtReleaseDate;
        protected global::System.Web.UI.WebControls.Button btnAdd;
        protected global::System.Web.UI.WebControls.GridView gvMovies;

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
                "SELECT MOVIE_ID, MOVIE_TITLE, DURATION, MOVIE_LANGUAGE, MOVIE_GENRE, " +
                "\"release_date \" AS RELEASE_DATE FROM MOVIE ORDER BY MOVIE_ID", con))
            using (var da = new OracleDataAdapter(cmd))
            {
                var dt = new DataTable();
                da.Fill(dt);

                gvMovies.DataSource = dt;
                gvMovies.DataBind();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (var con = new OracleConnection(Cs))
                using (var cmd = new OracleCommand(
                    "INSERT INTO MOVIE (MOVIE_ID, MOVIE_TITLE, DURATION, MOVIE_LANGUAGE, MOVIE_GENRE, \"release_date \") " +
                    "VALUES (:pId, :pTitle, TO_DATE(:pDuration, 'MM/DD/YYYY HH:MI:SS AM'), :pLanguage, :pGenre, " +
                    "TO_DATE(:pReleaseDate, 'MM/DD/YYYY HH:MI:SS AM'))", con))
                {
                    cmd.Parameters.Add(":pId",          OracleDbType.Varchar2).Value = txtMovieId.Text.Trim();
                    cmd.Parameters.Add(":pTitle",       OracleDbType.Varchar2).Value = txtMovieTitle.Text.Trim();
                    cmd.Parameters.Add(":pDuration",    OracleDbType.Varchar2).Value = txtDuration.Text.Trim();
                    cmd.Parameters.Add(":pLanguage",    OracleDbType.Varchar2).Value = txtLanguage.Text.Trim();
                    cmd.Parameters.Add(":pGenre",       OracleDbType.Varchar2).Value = txtGenre.Text.Trim();
                    cmd.Parameters.Add(":pReleaseDate", OracleDbType.Varchar2).Value = txtReleaseDate.Text.Trim();

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                txtMovieId.Text     = "";
                txtMovieTitle.Text  = "";
                txtDuration.Text    = "";
                txtLanguage.Text    = "";
                txtGenre.Text       = "";
                txtReleaseDate.Text = "";
                lblMsg.Text = "Movie added successfully.";
                BindGrid();
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        protected void gvMovies_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            gvMovies.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void gvMovies_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            gvMovies.EditIndex = -1;
            BindGrid();
        }

        protected void gvMovies_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            try
            {
                string id          = gvMovies.DataKeys[e.RowIndex].Value.ToString();
                string title       = e.NewValues["MOVIE_TITLE"]?.ToString()    ?? "";
                string duration    = e.NewValues["DURATION"]?.ToString()       ?? "";
                string language    = e.NewValues["MOVIE_LANGUAGE"]?.ToString() ?? "";
                string genre       = e.NewValues["MOVIE_GENRE"]?.ToString()    ?? "";
                string releaseDate = e.NewValues["RELEASE_DATE"]?.ToString()   ?? "";

                using (var con = new OracleConnection(Cs))
                using (var cmd = new OracleCommand(
                    "UPDATE MOVIE SET MOVIE_TITLE = :pTitle, " +
                    "DURATION = TO_DATE(:pDuration, 'MM/DD/YYYY HH:MI:SS AM'), " +
                    "MOVIE_LANGUAGE = :pLanguage, MOVIE_GENRE = :pGenre, " +
                    "\"release_date \" = TO_DATE(:pReleaseDate, 'MM/DD/YYYY HH:MI:SS AM') WHERE MOVIE_ID = :pId", con))
                {
                    cmd.Parameters.Add(":pTitle",       OracleDbType.Varchar2).Value = title.Trim();
                    cmd.Parameters.Add(":pDuration",    OracleDbType.Varchar2).Value = duration.Trim();
                    cmd.Parameters.Add(":pLanguage",    OracleDbType.Varchar2).Value = language.Trim();
                    cmd.Parameters.Add(":pGenre",       OracleDbType.Varchar2).Value = genre.Trim();
                    cmd.Parameters.Add(":pReleaseDate", OracleDbType.Varchar2).Value = releaseDate.Trim();
                    cmd.Parameters.Add(":pId",          OracleDbType.Varchar2).Value = id;

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                gvMovies.EditIndex = -1;
                BindGrid();
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        protected void gvMovies_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            try
            {
                string id = gvMovies.DataKeys[e.RowIndex].Value.ToString();

                using (var con = new OracleConnection(Cs))
                using (var cmd = new OracleCommand(
                    "DELETE FROM MOVIE WHERE MOVIE_ID = :pId", con))
                {
                    cmd.Parameters.Add(":pId", OracleDbType.Varchar2).Value = id;

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
