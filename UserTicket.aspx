<%@ Page Title="User Ticket" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserTicket.aspx.cs" Inherits="KumariCinemas.UserTicket" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="kc-page-header">
        <h2><i class="bi bi-person-badge-fill"></i> User Ticket</h2>
        <p>Select a customer to view their ticket bookings from the last 6 months.</p>
    </div>

    <asp:Label ID="lblMsg" runat="server" CssClass="kc-msg"></asp:Label>

    <div class="kc-card">
        <h4><i class="bi bi-funnel-fill"></i> Search Filter</h4>
        <div class="row">
            <div class="col-md-4 mb-2">
                <label class="form-label fw-bold">Select Customer</label>
                <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-select"></asp:DropDownList>
            </div>
            <div class="col-md-2 mb-2 d-flex align-items-end">
                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-kc-primary w-100" OnClick="btnSearch_Click" />
            </div>
        </div>
    </div>

    <asp:Panel ID="pnlResults" runat="server" Visible="false">

        <div class="card kc-detail-card">
            <div class="card-header"><i class="bi bi-person-fill"></i> Customer Details</div>
            <div class="card-body">
                <div class="row">
                    <div class="col-md-3"><strong>Customer ID:</strong>&nbsp;<asp:Label ID="lblCustomerId" runat="server"></asp:Label></div>
                    <div class="col-md-3"><strong>Name:</strong>&nbsp;<asp:Label ID="lblCustomerName" runat="server"></asp:Label></div>
                    <div class="col-md-3"><strong>Contact:</strong>&nbsp;<asp:Label ID="lblContact" runat="server"></asp:Label></div>
                    <div class="col-md-3"><strong>Username:</strong>&nbsp;<asp:Label ID="lblUsername" runat="server"></asp:Label></div>
                </div>
                <div class="row mt-2">
                    <div class="col-md-6"><strong>Address:</strong>&nbsp;<asp:Label ID="lblAddress" runat="server"></asp:Label></div>
                </div>
            </div>
        </div>

        <div class="kc-card">
            <h4><i class="bi bi-ticket-perforated"></i> Tickets Booked (Last 6 Months)</h4>
            <div class="table-responsive">
            <asp:GridView ID="gvTickets" runat="server"
                AutoGenerateColumns="False"
                CssClass="table table-bordered table-hover"
                EmptyDataText="No tickets found for this customer in the last 6 months.">
                <Columns>
                    <asp:BoundField DataField="TICKET_ID"      HeaderText="Ticket ID" />
                    <asp:BoundField DataField="BOOKING_ID"     HeaderText="Booking ID" />
                    <asp:BoundField DataField="BOOKING_STATUS" HeaderText="Status" />
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
