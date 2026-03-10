<%@ Page Title="Ticket Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TicketDetails.aspx.cs" Inherits="KumariCinemas.TicketDetails" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="kc-page-header">
        <h2><i class="bi bi-ticket-perforated-fill"></i> Ticket Details</h2>
        <p>Manage ticket records.</p>
    </div>

    <asp:Label ID="lblMsg" runat="server" CssClass="kc-msg"></asp:Label>

    <div class="kc-card">
        <h4><i class="bi bi-plus-circle"></i> Add New Ticket</h4>
        <div class="row mb-2">
            <div class="col-md-2 mb-2">
                <asp:TextBox ID="txtTicketId" runat="server" CssClass="form-control" placeholder="Ticket ID"></asp:TextBox>
            </div>
            <div class="col-md-3 mb-2">
                <asp:DropDownList ID="ddlSeatId" runat="server" CssClass="form-select"></asp:DropDownList>
            </div>
            <div class="col-md-2 mb-2">
                <asp:TextBox ID="txtTicketPrice" runat="server" CssClass="form-control" placeholder="Ticket Price"></asp:TextBox>
            </div>
            <div class="col-md-2 mb-2">
                <asp:Button ID="btnAdd" runat="server" Text="Add Ticket" CssClass="btn btn-kc-primary w-100" OnClick="btnAdd_Click" />
            </div>
        </div>
    </div>

    <div class="kc-card">
        <h4><i class="bi bi-table"></i> Ticket List</h4>
        <div class="table-responsive">
        <asp:GridView ID="gvTickets" runat="server"
            AutoGenerateColumns="False"
            DataKeyNames="TICKET_ID"
            CssClass="table table-bordered table-hover"
            OnRowEditing="gvTickets_RowEditing"
            OnRowCancelingEdit="gvTickets_RowCancelingEdit"
            OnRowUpdating="gvTickets_RowUpdating"
            OnRowDeleting="gvTickets_RowDeleting">
            <Columns>
                <asp:BoundField DataField="TICKET_ID"    HeaderText="Ticket ID"    ReadOnly="True" />
                <asp:BoundField DataField="SEAT_ID"      HeaderText="Seat ID" />
                <asp:BoundField DataField="TICKET_PRICE" HeaderText="Price" />
                <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>
        </div>
    </div>

</asp:Content>
