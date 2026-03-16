<%@ Page Title="Theater CityHall Movie" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TheaterCityHallMovie.aspx.cs" Inherits="KumariCinemas.TheaterCityHallMovie" MaintainScrollPositionOnPostBack="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="kc-page-header">
        <h2><i class="bi bi-collection-play-fill"></i> Theater-City Hall Movie Mapper</h2>
        <p>Review venue-wise movie lineups and schedules in one clear relationship view.</p>
    </div>

    <div class="kc-card">
        <h4><i class="bi bi-funnel-fill"></i> City Hall Filter</h4>
        <div class="row g-3 align-items-end kc-filter-row">
            <div class="col-md-8 col-lg-7">
                <label class="form-label">Select City Hall</label>
                <asp:DropDownList ID="ddlCityHall" runat="server" CssClass="form-select"></asp:DropDownList>
            </div>
            <div class="col-md-4 col-lg-5">
                <div class="kc-form-actions kc-filter-actions">
                    <asp:Button ID="btnSearch" runat="server" Text="Load Relationship" CssClass="btn btn-kc-primary" OnClick="btnSearch_Click" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-kc-secondary" OnClick="btnReset_Click" CausesValidation="false" />
                </div>
            </div>
            <div class="col-md-8 col-lg-7">
                <small class="text-muted d-block kc-filter-help">Select a city hall to load linked movies and scheduled shows.</small>
            </div>
        </div>
    </div>

    <asp:Label ID="lblMsg" runat="server" CssClass="kc-msg" />

    <asp:Panel ID="pnlResults" runat="server" Visible="false">

        <div class="kc-card mb-3">
            <div class="kc-hero-badges">
                <span class="kc-hero-badge"><i class="bi bi-building"></i> City Hall: <asp:Label ID="lblSelectedCityHall" runat="server" /></span>
                <span class="kc-hero-badge"><i class="bi bi-camera-reels"></i> Movies: <asp:Label ID="lblMovieCount" runat="server" /></span>
                <span class="kc-hero-badge"><i class="bi bi-clock-history"></i> Showtimes: <asp:Label ID="lblShowtimeCount" runat="server" /></span>
            </div>
        </div>

        <div class="card kc-detail-card">
            <div class="card-header"><i class="bi bi-building"></i> City Hall Profile</div>
            <div class="card-body">
                <div class="row g-3">
                    <div class="col-md-3"><strong>City Hall ID:</strong><br /><asp:Label ID="lblCityhallId" runat="server" /></div>
                    <div class="col-md-3"><strong>Theatre ID:</strong><br /><asp:Label ID="lblTheatreId" runat="server" /></div>
                    <div class="col-md-3"><strong>Name:</strong><br /><asp:Label ID="lblCityhallName" runat="server" /></div>
                    <div class="col-md-3"><strong>Location:</strong><br /><asp:Label ID="lblCityhallLocation" runat="server" /></div>
                </div>
            </div>
        </div>

        <div class="kc-card">
            <h4><i class="bi bi-camera-reels"></i> Movies Linked to This City Hall</h4>
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
            <h4><i class="bi bi-clock"></i> Showtimes in This City Hall</h4>
            <div class="table-responsive">
                <asp:GridView ID="gvShowtimes" runat="server"
                    AutoGenerateColumns="False"
                    CssClass="table table-bordered table-hover"
                    EmptyDataText="No showtimes found for this city hall.">
                    <Columns>
                        <asp:BoundField DataField="SHOW_ID"   HeaderText="Show ID" />
                        <asp:BoundField DataField="MOVIE_ID" HeaderText="Movie ID" />
                        <asp:BoundField DataField="MOVIE_TITLE" HeaderText="Movie Title" />
                        <asp:BoundField DataField="SHOW_DATE" HeaderText="Show Date" />
                        <asp:BoundField DataField="SHOW_TIME" HeaderText="Show Time" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>

    </asp:Panel>

</asp:Content>
