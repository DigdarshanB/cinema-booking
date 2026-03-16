<%@ Page Title="Ticket Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TicketDetails.aspx.cs" Inherits="KumariCinemas.TicketDetails" MaintainScrollPositionOnPostBack="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="kc-page-header">
        <h2><i class="bi bi-ticket-perforated-fill"></i> Ticket Management</h2>
        <p>Issue and maintain ticket records with clear seat mapping and pricing controls.</p>
    </div>

    <div class="kc-card">
        <h4><i class="bi bi-plus-circle"></i> Ticket Entry Form</h4>
        <div class="row g-3 align-items-end kc-entry-row">
            <div class="col-md-6 col-xl-2">
                <label class="form-label">Customer ID</label>
                <asp:DropDownList ID="ddlCustomerId" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlCustomerId_SelectedIndexChanged"></asp:DropDownList>
            </div>
            <div class="col-md-6 col-xl-2">
                <label class="form-label">Ticket ID</label>
                <asp:TextBox ID="txtTicketId" runat="server" CssClass="form-control" placeholder="e.g. T001"></asp:TextBox>
            </div>
            <div class="col-md-6 col-xl-4">
                <label class="form-label">Booking</label>
                <asp:DropDownList ID="ddlBookingId" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlBookingId_SelectedIndexChanged"></asp:DropDownList>
            </div>
            <div class="col-md-6 col-xl-4">
                <label class="form-label">Seat</label>
                <asp:DropDownList ID="ddlSeatId" runat="server" CssClass="form-select"></asp:DropDownList>
            </div>
            <div class="col-md-6 col-xl-2">
                <label class="form-label">Ticket Price</label>
                <asp:TextBox ID="txtTicketPrice" runat="server" CssClass="form-control" placeholder="e.g. 12.50"></asp:TextBox>
            </div>
            <div class="col-md-6 col-xl-4 kc-entry-actions-col">
                <div class="kc-form-actions kc-entry-actions">
                    <asp:Button ID="btnAdd" runat="server" Text="Add Ticket" CssClass="btn btn-kc-primary" OnClick="btnAdd_Click" />
                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-kc-secondary" OnClick="btnClear_Click" CausesValidation="false" />
                </div>
            </div>
        </div>
        <div class="row g-3 kc-entry-help-row">
            <div class="col-md-6 col-xl-2"><small class="text-muted d-block">Keep a unique ID for each ticket.</small></div>
            <div class="col-md-6 col-xl-4"><small class="text-muted d-block">Choose a customer first, then booking and seat options will load.</small></div>
            <div class="col-md-6 col-xl-2"><small class="text-muted d-block">Enter numeric value only.</small></div>
        </div>
    </div>

    <asp:Label ID="lblMsg" runat="server" CssClass="kc-msg" />

    <div class="kc-card">
        <h4><i class="bi bi-table"></i> Ticket Records</h4>
        <div class="table-responsive">
            <asp:GridView ID="gvTickets" runat="server"
                AutoGenerateColumns="False"
                DataKeyNames="TICKET_ID"
                CssClass="table table-bordered table-hover"
                EmptyDataText="No tickets found."
                OnRowEditing="gvTickets_RowEditing"
                OnRowCancelingEdit="gvTickets_RowCancelingEdit"
                OnRowUpdating="gvTickets_RowUpdating"
                OnRowDeleting="gvTickets_RowDeleting"
                OnRowDataBound="gvTickets_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="CUSTOMER_ID"  HeaderText="Customer ID" ReadOnly="True" />
                    <asp:BoundField DataField="TICKET_ID"    HeaderText="Ticket ID" ReadOnly="True" />
                    <asp:TemplateField HeaderText="Booking ID">
                        <ItemTemplate>
                            <%# Eval("BOOKING_ID") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlEditBookingId" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlEditBookingId_SelectedIndexChanged"></asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Seat ID">
                        <ItemTemplate>
                            <%# Eval("SEAT_ID") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlEditSeatId" runat="server" CssClass="form-select"></asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="TICKET_PRICE" HeaderText="Price" />
                    <asp:CommandField ShowEditButton="True" ShowDeleteButton="True"
                        HeaderText="Actions" ButtonType="Link" ControlStyle-CssClass="kc-grid-action" />
                </Columns>
            </asp:GridView>
        </div>
    </div>

</asp:Content>
