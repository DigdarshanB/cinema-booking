<%@ Page Title="Movie Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MovieDetails.aspx.cs" Inherits="KumariCinemas.MovieDetails" MaintainScrollPositionOnPostBack="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="kc-page-header">
        <h2><i class="bi bi-camera-reels-fill"></i> Movie Management</h2>
        <p>Curate the cinema catalog, maintain release details, and keep show-ready records.</p>
    </div>

    <div class="kc-card">
        <h4><i class="bi bi-film"></i> Movie Entry Form</h4>
        <div class="row g-3 align-items-end">
            <div class="col-md-6 col-xl-2">
                <label class="form-label">Movie ID</label>
                <asp:TextBox ID="txtMovieId" runat="server" CssClass="form-control" placeholder="e.g. M001"></asp:TextBox>
            </div>
            <div class="col-md-6 col-xl-3">
                <label class="form-label">Title</label>
                <asp:TextBox ID="txtMovieTitle" runat="server" CssClass="form-control" placeholder="Movie Title"></asp:TextBox>
            </div>
            <div class="col-md-6 col-xl-2">
                <label class="form-label">Duration</label>
                <asp:TextBox ID="txtDuration" runat="server" CssClass="form-control" placeholder="mins"></asp:TextBox>
            </div>
            <div class="col-md-6 col-xl-2">
                <label class="form-label">Language</label>
                <asp:TextBox ID="txtLanguage" runat="server" CssClass="form-control" placeholder="Language"></asp:TextBox>
            </div>
            <div class="col-md-6 col-xl-2">
                <label class="form-label">Genre</label>
                <asp:TextBox ID="txtGenre" runat="server" CssClass="form-control" placeholder="Genre"></asp:TextBox>
            </div>
            <div class="col-md-6 col-xl-1">
                <label class="form-label">Release Date</label>
                <asp:TextBox ID="txtReleaseDate" runat="server" CssClass="form-control" TextMode="Date" placeholder="DD/MM/YYYY"></asp:TextBox>
            </div>
            <div class="col-12">
                <div class="kc-form-actions">
                    <asp:Button ID="btnAdd" runat="server" Text="Add Movie" CssClass="btn btn-kc-primary" OnClick="btnAdd_Click" />
                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-kc-secondary" OnClick="btnClear_Click" CausesValidation="false" />
                </div>
            </div>
        </div>
    </div>

    <asp:Label ID="lblMsg" runat="server" CssClass="kc-msg" />

    <div class="kc-card">
        <h4>Movie Records</h4>
        <div class="table-responsive">
            <asp:GridView ID="gvMovies" runat="server"
                AutoGenerateColumns="False"
                DataKeyNames="MOVIE_ID"
                CssClass="table table-bordered table-hover kc-movie-grid"
                EmptyDataText="No movies found."
                OnRowEditing="gvMovies_RowEditing"
                OnRowCancelingEdit="gvMovies_RowCancelingEdit"
                OnRowUpdating="gvMovies_RowUpdating"
                OnRowDeleting="gvMovies_RowDeleting">
                <Columns>
                    <asp:TemplateField HeaderText="Poster" ItemStyle-Width="84px" ItemStyle-CssClass="text-center">
                        <ItemTemplate>
                            <%# GetMoviePoster(Eval("MOVIE_TITLE")) %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="MOVIE_ID"       HeaderText="Movie ID"    ReadOnly="True" />
                    <asp:BoundField DataField="MOVIE_TITLE"    HeaderText="Title" />
                    <asp:BoundField DataField="DURATION"       HeaderText="Duration" />
                    <asp:BoundField DataField="MOVIE_LANGUAGE" HeaderText="Language" />
                    <asp:BoundField DataField="MOVIE_GENRE"    HeaderText="Genre" />
                    <asp:BoundField DataField="RELEASE_DATE"   HeaderText="Release Date" />
                    <asp:CommandField ShowEditButton="True" ShowDeleteButton="True"
                        HeaderText="Actions" ButtonType="Link" ControlStyle-CssClass="kc-grid-action" />
                </Columns>
            </asp:GridView>
        </div>
    </div>

</asp:Content>
