<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="KumariCinemas._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <div class="kc-hero">
            <h1><i class="bi bi-film"></i> Kumari <span>Cinemas</span></h1>
            <p>Cinema Management System</p>
        </div>

        <h4 class="kc-section-title">Basic Forms</h4>
        <div class="row">
            <div class="col-md-4 mb-4">
                <div class="card kc-dash-card">
                    <div class="card-body">
                        <i class="bi bi-people-fill kc-icon"></i>
                        <h5 class="card-title">User Details</h5>
                        <p class="card-text">Manage registered cinema customers.</p>
                        <a href="UserDetails.aspx" class="btn btn-kc-open">Open</a>
                    </div>
                </div>
            </div>
            <div class="col-md-4 mb-4">
                <div class="card kc-dash-card">
                    <div class="card-body">
                        <i class="bi bi-building kc-icon"></i>
                        <h5 class="card-title">Theater / City Hall Details</h5>
                        <p class="card-text">Manage theater and city hall venues.</p>
                        <a href="TheaterCityHallDetails.aspx" class="btn btn-kc-open">Open</a>
                    </div>
                </div>
            </div>
            <div class="col-md-4 mb-4">
                <div class="card kc-dash-card">
                    <div class="card-body">
                        <i class="bi bi-camera-reels-fill kc-icon"></i>
                        <h5 class="card-title">Movie Details</h5>
                        <p class="card-text">Manage movies in the system.</p>
                        <a href="MovieDetails.aspx" class="btn btn-kc-open">Open</a>
                    </div>
                </div>
            </div>
            <div class="col-md-4 mb-4">
                <div class="card kc-dash-card">
                    <div class="card-body">
                        <i class="bi bi-clock-fill kc-icon"></i>
                        <h5 class="card-title">Showtimes Details</h5>
                        <p class="card-text">Manage movie showtimes and schedules.</p>
                        <a href="ShowtimesDetails.aspx" class="btn btn-kc-open">Open</a>
                    </div>
                </div>
            </div>
            <div class="col-md-4 mb-4">
                <div class="card kc-dash-card">
                    <div class="card-body">
                        <i class="bi bi-ticket-perforated-fill kc-icon"></i>
                        <h5 class="card-title">Ticket Details</h5>
                        <p class="card-text">Manage ticket records.</p>
                        <a href="TicketDetails.aspx" class="btn btn-kc-open">Open</a>
                    </div>
                </div>
            </div>
        </div>

        <h4 class="kc-section-title kc-section-title-gold mt-2">Complex Forms</h4>
        <div class="row">
            <div class="col-md-4 mb-4">
                <div class="card kc-dash-card kc-dash-complex">
                    <div class="card-body">
                        <i class="bi bi-person-badge-fill kc-icon"></i>
                        <h5 class="card-title">User Ticket</h5>
                        <p class="card-text">View tickets by user with 6-month filter.</p>
                        <a href="UserTicket.aspx" class="btn btn-kc-open">Open</a>
                    </div>
                </div>
            </div>
            <div class="col-md-4 mb-4">
                <div class="card kc-dash-card kc-dash-complex">
                    <div class="card-body">
                        <i class="bi bi-collection-play-fill kc-icon"></i>
                        <h5 class="card-title">Theater / City Hall Movie</h5>
                        <p class="card-text">View movies and showtimes by venue.</p>
                        <a href="TheaterCityHallMovie.aspx" class="btn btn-kc-open">Open</a>
                    </div>
                </div>
            </div>
            <div class="col-md-4 mb-4">
                <div class="card kc-dash-card kc-dash-complex">
                    <div class="card-body">
                        <i class="bi bi-graph-up-arrow kc-icon"></i>
                        <h5 class="card-title">Occupancy Report</h5>
                        <p class="card-text">Top 3 city halls by paid-ticket occupancy.</p>
                        <a href="MovieTheatherCityHallOccupancyPerformer.aspx" class="btn btn-kc-open">Open</a>
                    </div>
                </div>
            </div>
        </div>
    </main>

</asp:Content>
