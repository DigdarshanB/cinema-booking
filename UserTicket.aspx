<%@ Page Title="User Ticket" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserTicket.aspx.cs" Inherits="KumariCinemas.UserTicket" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="kc-page-header">
        <h2><i class="bi bi-person-badge-fill"></i> User Ticket Explorer</h2>
        <p>Track customer-to-ticket relationships and review booking activity from the last 6 months.</p>
    </div>

    <asp:Label ID="lblMsg" runat="server" CssClass="kc-msg" />

    <div class="kc-card">
        <h4><i class="bi bi-funnel-fill"></i> Filter by Customer</h4>
        <div class="row g-3 align-items-end kc-filter-row">
            <div class="col-md-7 col-lg-6">
                <label class="form-label">Select Customer</label>
                <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-select"></asp:DropDownList>
            </div>
            <div class="col-md-5 col-lg-6">
                <div class="kc-form-actions kc-filter-actions">
                    <asp:Button ID="btnSearch" runat="server" Text="Load Tickets" CssClass="btn btn-kc-primary" OnClick="btnSearch_Click" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-kc-secondary" OnClick="btnReset_Click" CausesValidation="false" />
                </div>
            </div>
            <div class="col-md-7 col-lg-6">
                <small class="text-muted d-block kc-filter-help">Choose a customer to load profile details and recent tickets.</small>
            </div>
        </div>
    </div>

    <asp:Panel ID="pnlResults" runat="server" Visible="false">

        <div class="kc-card mb-3">
            <div class="kc-hero-badges">
                <span class="kc-hero-badge"><i class="bi bi-person"></i> Customer: <asp:Label ID="lblSelectedCustomer" runat="server" /></span>
                <span class="kc-hero-badge"><i class="bi bi-ticket-perforated"></i> Tickets Found: <asp:Label ID="lblTicketCount" runat="server" /></span>
            </div>
        </div>

        <div class="card kc-detail-card">
            <div class="card-header"><i class="bi bi-person-fill"></i> Customer Profile</div>
            <div class="card-body">
                <div class="row g-3">
                    <div class="col-md-3"><strong>Customer ID:</strong><br /><asp:Label ID="lblCustomerId" runat="server" /></div>
                    <div class="col-md-3"><strong>Name:</strong><br /><asp:Label ID="lblCustomerName" runat="server" /></div>
                    <div class="col-md-3"><strong>Contact:</strong><br /><asp:Label ID="lblContact" runat="server" /></div>
                    <div class="col-md-3"><strong>Username:</strong><br /><asp:Label ID="lblUsername" runat="server" /></div>
                </div>
                <div class="row g-3 mt-1">
                    <div class="col-md-8"><strong>Address:</strong><br /><asp:Label ID="lblAddress" runat="server" /></div>
                </div>
            </div>
        </div>

        <div class="kc-card">
            <h4><i class="bi bi-ticket-perforated"></i> Ticket History (Last 6 Months)</h4>
            <div class="table-responsive">
                <asp:GridView ID="gvTickets" runat="server"
                    AutoGenerateColumns="False"
                    CssClass="table table-bordered table-hover"
                    EmptyDataText="No tickets found for this customer in the last 6 months.">
                    <Columns>
                        <asp:BoundField DataField="TICKET_ID"      HeaderText="Ticket ID" />
                        <asp:BoundField DataField="BOOKING_ID"     HeaderText="Booking ID" />
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <span class='<%# GetBookingStatusClass(Eval("BOOKING_STATUS")) %>'><%# Eval("BOOKING_STATUS") %></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="SEAT_ID"        HeaderText="Seat ID" />
                        <asp:BoundField DataField="TICKET_PRICE"   HeaderText="Price" />
                        <asp:BoundField DataField="SHOW_DATE"      HeaderText="Show Date" />
                        <asp:BoundField DataField="SHOW_TIME"      HeaderText="Show Time" />
                        <asp:BoundField DataField="DAY_TYPE"       HeaderText="Day Type" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>

    </asp:Panel>

</asp:Content>
