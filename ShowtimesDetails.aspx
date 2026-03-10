<%@ Page Title="Showtimes Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShowtimesDetails.aspx.cs" Inherits="KumariCinemas.ShowtimesDetails" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="kc-page-header">
        <h2><i class="bi bi-clock-fill"></i> Showtimes Details</h2>
        <p>Manage show records.</p>
    </div>

    <asp:Label ID="lblMsg" runat="server" CssClass="kc-msg"></asp:Label>

    <div class="kc-card">
        <h4><i class="bi bi-plus-circle"></i> Add New Showtime</h4>
        <div class="row mb-2">
            <div class="col-md-2 mb-2">
                <asp:TextBox ID="txtShowId" runat="server" CssClass="form-control" placeholder="Show ID"></asp:TextBox>
            </div>
            <div class="col-md-2 mb-2">
                <asp:TextBox ID="txtCityhallId" runat="server" CssClass="form-control" placeholder="City Hall ID"></asp:TextBox>
            </div>
            <div class="col-md-2 mb-2">
                <asp:TextBox ID="txtShowDate" runat="server" CssClass="form-control" placeholder="Show Date (MM/DD/YYYY)"></asp:TextBox>
            </div>
            <div class="col-md-2 mb-2">
                <asp:TextBox ID="txtShowTime" runat="server" CssClass="form-control" placeholder="Show Time"></asp:TextBox>
            </div>
            <div class="col-md-2 mb-2">
                <asp:TextBox ID="txtDayType" runat="server" CssClass="form-control" placeholder="Day Type"></asp:TextBox>
            </div>
            <div class="col-md-2 mb-2">
                <asp:Button ID="btnAdd" runat="server" Text="Add Showtime" CssClass="btn btn-kc-primary w-100" OnClick="btnAdd_Click" />
            </div>
        </div>
    </div>

    <div class="kc-card">
        <h4><i class="bi bi-table"></i> Showtimes List</h4>
        <div class="table-responsive">
        <asp:GridView ID="gvShows" runat="server"
            AutoGenerateColumns="False"
            DataKeyNames="SHOW_ID"
            CssClass="table table-bordered table-hover"
            OnRowEditing="gvShows_RowEditing"
            OnRowCancelingEdit="gvShows_RowCancelingEdit"
            OnRowUpdating="gvShows_RowUpdating"
            OnRowDeleting="gvShows_RowDeleting">
            <Columns>
                <asp:BoundField DataField="SHOW_ID"     HeaderText="Show ID"      ReadOnly="True" />
                <asp:BoundField DataField="CITYHALL_ID" HeaderText="City Hall ID" />
                <asp:BoundField DataField="SHOW_DATE"   HeaderText="Show Date" />
                <asp:BoundField DataField="SHOW_TIME"   HeaderText="Show Time" />
                <asp:BoundField DataField="DAY_TYPE"    HeaderText="Day Type" />
                <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>
        </div>
    </div>

</asp:Content>
