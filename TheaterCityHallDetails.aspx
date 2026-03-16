<%@ Page Title="Theater City Hall Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TheaterCityHallDetails.aspx.cs" Inherits="KumariCinemas.TheaterCityHallDetails" MaintainScrollPositionOnPostBack="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="kc-page-header">
        <h2><i class="bi bi-building"></i> Theater &amp; City Hall Management</h2>
        <p>Manage venue halls, theatre mapping, and locations with a clean admin workflow.</p>
    </div>

    <div class="kc-card">
        <h4><i class="bi bi-plus-circle"></i> City Hall Form</h4>
        <div class="row g-3">
            <div class="col-md-6 col-xl-2">
                <label class="form-label">City Hall ID</label>
                <asp:TextBox ID="txtCityhallId" runat="server" CssClass="form-control" placeholder="e.g. CH001"></asp:TextBox>
            </div>
            <div class="col-md-6 col-xl-3">
                <label class="form-label">Theatre</label>
                <asp:DropDownList ID="ddlTheatreId" runat="server" CssClass="form-select"></asp:DropDownList>
                <small class="text-muted d-block">Select a theatre before saving this city hall.</small>
            </div>
            <div class="col-md-6 col-xl-3">
                <label class="form-label">City Hall Name</label>
                <asp:TextBox ID="txtCityhallName" runat="server" CssClass="form-control" placeholder="Name"></asp:TextBox>
            </div>
            <div class="col-md-6 col-xl-4">
                <label class="form-label">Location</label>
                <asp:TextBox ID="txtCityhallLocation" runat="server" CssClass="form-control" placeholder="Location"></asp:TextBox>
            </div>
            <div class="col-12">
                <div class="kc-form-actions">
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-kc-primary" OnClick="btnAdd_Click" />
                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-kc-secondary" OnClick="btnClear_Click" CausesValidation="false" />
                </div>
            </div>
        </div>
    </div>

    <asp:Label ID="lblMsg" runat="server" CssClass="kc-msg" />

    <div class="kc-card">
        <h4><i class="bi bi-table"></i> Theater City Hall Records</h4>
        <div class="table-responsive">
            <asp:GridView ID="gvTheaters" runat="server"
                AutoGenerateColumns="False"
                DataKeyNames="CITYHALL_ID"
                CssClass="table table-bordered table-hover"
                EmptyDataText="No theater city halls found."
                OnRowEditing="gvTheaters_RowEditing"
                OnRowCancelingEdit="gvTheaters_RowCancelingEdit"
                OnRowUpdating="gvTheaters_RowUpdating"
                OnRowDeleting="gvTheaters_RowDeleting"
                OnRowDataBound="gvTheaters_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="CITYHALL_ID"       HeaderText="City Hall ID" ReadOnly="True" />
                    <asp:TemplateField HeaderText="Theatre ID">
                        <ItemTemplate>
                            <%# Eval("THEATRE_ID") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlEditTheatreId" runat="server" CssClass="form-select"></asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CITYHALL_NAME"     HeaderText="Name" />
                    <asp:BoundField DataField="CITYHALL_LOCATION" HeaderText="Location" />
                    <asp:CommandField ShowEditButton="True" ShowDeleteButton="True"
                        HeaderText="Actions" ButtonType="Link" ControlStyle-CssClass="kc-grid-action" />
                </Columns>
            </asp:GridView>
        </div>
    </div>

</asp:Content>
