<%@ Page Title="Theater City Hall Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TheaterCityHallDetails.aspx.cs" Inherits="KumariCinemas.TheaterCityHallDetails" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="kc-page-header">
        <h2><i class="bi bi-building"></i> Theater City Hall Details</h2>
        <p>Manage theater city hall records.</p>
    </div>

    <asp:Label ID="lblMsg" runat="server" CssClass="kc-msg"></asp:Label>

    <div class="kc-card">
        <h4><i class="bi bi-plus-circle"></i> Add New Theater City Hall</h4>
        <div class="row mb-2">
            <div class="col-md-2 mb-2">
                <asp:TextBox ID="txtCityhallId" runat="server" CssClass="form-control" placeholder="City Hall ID"></asp:TextBox>
            </div>
            <div class="col-md-3 mb-2">
                <asp:DropDownList ID="ddlTheatreId" runat="server" CssClass="form-select"></asp:DropDownList>
            </div>
            <div class="col-md-2 mb-2">
                <asp:TextBox ID="txtCityhallName" runat="server" CssClass="form-control" placeholder="City Hall Name"></asp:TextBox>
            </div>
            <div class="col-md-2 mb-2">
                <asp:TextBox ID="txtCityhallLocation" runat="server" CssClass="form-control" placeholder="Location"></asp:TextBox>
            </div>
            <div class="col-md-2 mb-2">
                <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-kc-primary w-100" OnClick="btnAdd_Click" />
            </div>
        </div>
    </div>

    <div class="kc-card">
        <h4><i class="bi bi-table"></i> Theater City Hall List</h4>
        <div class="table-responsive">
        <asp:GridView ID="gvTheaters" runat="server"
            AutoGenerateColumns="False"
            DataKeyNames="CITYHALL_ID"
            CssClass="table table-bordered table-hover"
            OnRowEditing="gvTheaters_RowEditing"
            OnRowCancelingEdit="gvTheaters_RowCancelingEdit"
            OnRowUpdating="gvTheaters_RowUpdating"
            OnRowDeleting="gvTheaters_RowDeleting">
            <Columns>
                <asp:BoundField DataField="CITYHALL_ID"       HeaderText="City Hall ID" ReadOnly="True" />
                <asp:BoundField DataField="THEATRE_ID"        HeaderText="Theatre ID" />
                <asp:BoundField DataField="CITYHALL_NAME"     HeaderText="Name" />
                <asp:BoundField DataField="CITYHALL_LOCATION" HeaderText="Location" />
                <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>
        </div>
    </div>

</asp:Content>
