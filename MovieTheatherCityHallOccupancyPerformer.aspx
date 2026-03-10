<%@ Page Title="Movie Theater CityHall Occupancy" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MovieTheatherCityHallOccupancyPerformer.aspx.cs" Inherits="KumariCinemas.MovieTheatherCityHallOccupancyPerformer" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="kc-page-header">
        <h2><i class="bi bi-graph-up-arrow"></i> Movie TheaterCityHall Occupancy</h2>
        <p>Select a movie to view the top 3 theater city halls by paid-ticket seat occupancy percentage.</p>
    </div>

    <asp:Label ID="lblMsg" runat="server" CssClass="kc-msg"></asp:Label>

    <div class="kc-card">
        <h4><i class="bi bi-funnel-fill"></i> Search Filter</h4>
        <div class="row">
            <div class="col-md-4 mb-2">
                <label class="form-label fw-bold">Select Movie</label>
                <asp:DropDownList ID="ddlMovie" runat="server" CssClass="form-select"></asp:DropDownList>
            </div>
            <div class="col-md-2 mb-2 d-flex align-items-end">
                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-kc-primary w-100" OnClick="btnSearch_Click" />
            </div>
        </div>
    </div>

    <asp:Panel ID="pnlResults" runat="server" Visible="false">

        <div class="kc-card">
            <h4><i class="bi bi-bar-chart-fill"></i> Top 3 City Halls by Occupancy</h4>
            <div class="table-responsive">
            <asp:GridView ID="gvOccupancy" runat="server"
                AutoGenerateColumns="False"
                CssClass="table table-bordered table-hover"
                EmptyDataText="No occupancy data found for this movie.">
                <Columns>
                    <asp:BoundField DataField="CITYHALL_ID"        HeaderText="City Hall ID" />
                    <asp:BoundField DataField="CITYHALL_NAME"      HeaderText="Name" />
                    <asp:BoundField DataField="CITYHALL_LOCATION"  HeaderText="Location" />
                    <asp:BoundField DataField="TOTAL_SEATS"        HeaderText="Total Seats" />
                    <asp:BoundField DataField="PAID_TICKETS"       HeaderText="Paid Tickets" />
                    <asp:BoundField DataField="OCCUPANCY_PCT"      HeaderText="Occupancy %" />
                </Columns>
            </asp:GridView>
            </div>
        </div>

    </asp:Panel>

</asp:Content>
