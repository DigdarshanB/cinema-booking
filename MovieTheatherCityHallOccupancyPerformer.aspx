<%@ Page Title="Movie Theater CityHall Occupancy" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MovieTheatherCityHallOccupancyPerformer.aspx.cs" Inherits="KumariCinemas.MovieTheatherCityHallOccupancyPerformer" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="kc-page-header">
        <h2><i class="bi bi-graph-up-arrow"></i> Occupancy Analytics Dashboard</h2>
        <p>Analyze paid-ticket occupancy performance by movie and compare top city hall performers.</p>
    </div>

    <asp:Label ID="lblMsg" runat="server" CssClass="kc-msg" />

    <div class="kc-card">
        <h4><i class="bi bi-funnel-fill"></i> Analytics Filter</h4>
        <div class="row g-3 align-items-end">
            <div class="col-md-8 col-lg-7">
                <label class="form-label">Select Movie</label>
                <asp:DropDownList ID="ddlMovie" runat="server" CssClass="form-select"></asp:DropDownList>
                <small class="text-muted d-block mt-1">Load occupancy insights for a single movie across city halls.</small>
            </div>
            <div class="col-md-4 col-lg-5">
                <div class="kc-form-actions">
                    <asp:Button ID="btnSearch" runat="server" Text="Load Analytics" CssClass="btn btn-kc-primary" OnClick="btnSearch_Click" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-kc-secondary" OnClick="btnReset_Click" CausesValidation="false" />
                </div>
            </div>
        </div>
    </div>

    <asp:Panel ID="pnlResults" runat="server" Visible="false">

        <div class="row g-3 mb-3">
            <div class="col-md-4">
                <div class="kc-kpi-card">
                    <p>Selected Movie</p>
                    <h3 class="kc-kpi-title"><asp:Label ID="lblSelectedMovie" runat="server" /></h3>
                </div>
            </div>
            <div class="col-md-4">
                <div class="kc-kpi-card">
                    <p>Top Occupancy</p>
                    <h3><asp:Label ID="lblTopOccupancy" runat="server" /></h3>
                </div>
            </div>
            <div class="col-md-4">
                <div class="kc-kpi-card">
                    <p>Average Occupancy</p>
                    <h3><asp:Label ID="lblAvgOccupancy" runat="server" /></h3>
                </div>
            </div>
        </div>

        <div class="kc-chart-card mb-3">
            <div class="kc-chart-card__head">
                <h5>Occupancy Trend Placeholder</h5>
            </div>
            <div class="kc-analytics-placeholder">
                <i class="bi bi-bar-chart-line"></i>
                <span>Chart area ready for future occupancy visualization.</span>
            </div>
        </div>

        <div class="kc-card">
            <h4><i class="bi bi-bar-chart-fill"></i> Top 3 City Halls by Paid-Ticket Occupancy</h4>
            <div class="table-responsive">
                <asp:GridView ID="gvOccupancy" runat="server"
                    AutoGenerateColumns="False"
                    CssClass="table table-bordered table-hover"
                    EmptyDataText="No occupancy data found for this movie.">
                    <Columns>
                        <asp:BoundField DataField="CITYHALL_ID"       HeaderText="City Hall ID" />
                        <asp:BoundField DataField="CITYHALL_NAME"     HeaderText="Name" />
                        <asp:BoundField DataField="CITYHALL_LOCATION" HeaderText="Location" />
                        <asp:BoundField DataField="TOTAL_SEATS"       HeaderText="Total Seats" />
                        <asp:BoundField DataField="PAID_TICKETS"      HeaderText="Paid Tickets" />
                        <asp:TemplateField HeaderText="Occupancy %">
                            <ItemTemplate>
                                <span class='<%# GetOccupancyClass(Eval("OCCUPANCY_PCT")) %>'><%# Eval("OCCUPANCY_PCT") %>%</span>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>

    </asp:Panel>

</asp:Content>
