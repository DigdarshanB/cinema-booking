<%@ Page Title="Movie Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MovieDetails.aspx.cs" Inherits="KumariCinemas.MovieDetails" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="kc-page-header">
        <h2><i class="bi bi-camera-reels-fill"></i> Movie Details</h2>
        <p>Manage movie records.</p>
    </div>

    <asp:Label ID="lblMsg" runat="server" CssClass="kc-msg"></asp:Label>

    <div class="kc-card">
        <h4><i class="bi bi-plus-circle"></i> Add New Movie</h4>
        <div class="row mb-2">
            <div class="col-md-2 mb-2">
                <asp:TextBox ID="txtMovieId" runat="server" CssClass="form-control" placeholder="Movie ID"></asp:TextBox>
            </div>
            <div class="col-md-2 mb-2">
                <asp:TextBox ID="txtMovieTitle" runat="server" CssClass="form-control" placeholder="Title"></asp:TextBox>
            </div>
            <div class="col-md-2 mb-2">
                <asp:TextBox ID="txtDuration" runat="server" CssClass="form-control" placeholder="Duration"></asp:TextBox>
            </div>
            <div class="col-md-2 mb-2">
                <asp:TextBox ID="txtLanguage" runat="server" CssClass="form-control" placeholder="Language"></asp:TextBox>
            </div>
            <div class="col-md-2 mb-2">
                <asp:TextBox ID="txtGenre" runat="server" CssClass="form-control" placeholder="Genre"></asp:TextBox>
            </div>
            <div class="col-md-2 mb-2">
                <asp:TextBox ID="txtReleaseDate" runat="server" CssClass="form-control" placeholder="Release Date"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="col-md-2 mb-2">
                <asp:Button ID="btnAdd" runat="server" Text="Add Movie" CssClass="btn btn-kc-primary w-100" OnClick="btnAdd_Click" />
            </div>
        </div>
    </div>

    <div class="kc-card">
        <h4><i class="bi bi-table"></i> Movie List</h4>
        <div class="table-responsive">
        <asp:GridView ID="gvMovies" runat="server"
            AutoGenerateColumns="False"
            DataKeyNames="MOVIE_ID"
            CssClass="table table-bordered table-hover"
            OnRowEditing="gvMovies_RowEditing"
            OnRowCancelingEdit="gvMovies_RowCancelingEdit"
            OnRowUpdating="gvMovies_RowUpdating"
            OnRowDeleting="gvMovies_RowDeleting">
            <Columns>
                <asp:BoundField DataField="MOVIE_ID"       HeaderText="Movie ID" ReadOnly="True" />
                <asp:BoundField DataField="MOVIE_TITLE"    HeaderText="Title" />
                <asp:BoundField DataField="DURATION"       HeaderText="Duration" />
                <asp:BoundField DataField="MOVIE_LANGUAGE" HeaderText="Language" />
                <asp:BoundField DataField="MOVIE_GENRE"    HeaderText="Genre" />
                <asp:BoundField DataField="RELEASE_DATE"   HeaderText="Release Date" />
                <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>
        </div>
    </div>

</asp:Content>
