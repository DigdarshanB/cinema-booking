<%@ Page Title="Theater CityHall Movie" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TheaterCityHallMovie.aspx.cs" Inherits="KumariCinemas.TheaterCityHallMovie" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="kc-page-header">
        <h2><i class="bi bi-collection-play-fill"></i> Theater CityHall Movie</h2>
        <p>Select a theater city hall to view its movies and showtimes.</p>
    </div>

    <asp:Label ID="lblMsg" runat="server" CssClass="kc-msg"></asp:Label>

    <div class="kc-card">
        <h4><i class="bi bi-funnel-fill"></i> Search Filter</h4>
        <div class="row">
            <div class="col-md-4 mb-2">
                <label class="form-label fw-bold">Select City Hall</label>
                <asp:DropDownList ID="ddlCityHall" runat="server" CssClass="form-select"></asp:DropDownList>
            </div>
            <div class="col-md-2 mb-2 d-flex align-items-end">
                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-kc-primary w-100" OnClick="btnSearch_Click" />
            </div>
        </div>
    </div>

    <asp:Panel ID="pnlResults" runat="server" Visible="false">

        <div class="card kc-detail-card">
            <div class="card-header"><i class="bi bi-building"></i> City Hall Details</div>
            <div class="card-body">
                <div class="row">
                    <div class="col-md-3"><strong>City Hall ID:</strong>&nbsp;<asp:Label ID="lblCityhallId" runat="server"></asp:Label></div>
                    <div class="col-md-3"><strong>Theatre ID:</strong>&nbsp;<asp:Label ID="lblTheatreId" runat="server"></asp:Label></div>
                    <div class="col-md-3"><strong>Name:</strong>&nbsp;<asp:Label ID="lblCityhallName" runat="server"></asp:Label></div>
                    <div class="col-md-3"><strong>Location:</strong>&nbsp;<asp:Label ID="lblCityhallLocation" runat="server"></asp:Label></div>
                </div>
            </div>
        </div>

        <div class="kc-card">
            <h4><i class="bi bi-camera-reels"></i> Movies</h4>
            <div class="table-responsive">
            <asp:GridView ID="gvMovies" runat="server"
                AutoGenerateColumns="False"
                CssClass="table table-bordered table-hover"
                EmptyDataText="No movies found for this city hall.">
                <Columns>
                    <asp:BoundField DataField="MOVIE_ID"       HeaderText="Movie ID" />
                    <asp:BoundField DataField="MOVIE_TITLE"    HeaderText="Title" />
                    <asp:BoundField DataField="DURATION"       HeaderText="Duration" />
                    <asp:BoundField DataField="MOVIE_LANGUAGE" HeaderText="Language" />
                    <asp:BoundField DataField="MOVIE_GENRE"    HeaderText="Genre" />
                    <asp:BoundField DataField="RELEASE_DATE"   HeaderText="Release Date" />
                </Columns>
            </asp:GridView>
            </div>
        </div>

        <div class="kc-card">
            <h4><i class="bi bi-clock"></i> Showtimes</h4>
            <div class="table-responsive">
            <asp:GridView ID="gvShowtimes" runat="server"
                AutoGenerateColumns="False"
                CssClass="table table-bordered table-hover"
                EmptyDataText="No showtimes found for this city hall.">
                <Columns>
                    <asp:BoundField DataField="SHOW_ID"   HeaderText="Show ID" />
                    <asp:BoundField DataField="SHOW_DATE" HeaderText="Show Date" />
                    <asp:BoundField DataField="SHOW_TIME" HeaderText="Show Time" />
                    <asp:BoundField DataField="DAY_TYPE"  HeaderText="Day Type" />
                </Columns>
            </asp:GridView>
            </div>
        </div>

    </asp:Panel>

</asp:Content>
