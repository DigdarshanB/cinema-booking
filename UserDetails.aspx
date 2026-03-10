<%@ Page Title="User Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserDetails.aspx.cs" Inherits="KumariCinemas.UserDetails" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="kc-page-header">
        <h2><i class="bi bi-people-fill"></i> User Details</h2>
        <p>Manage registered cinema customers.</p>
    </div>

    <asp:Label ID="lblMsg" runat="server" CssClass="kc-msg"></asp:Label>

    <div class="kc-card">
        <h4><i class="bi bi-plus-circle"></i> Add New Customer</h4>
        <div class="row mb-2">
            <div class="col-md-2 mb-2">
                <asp:TextBox ID="txtCustomerId" runat="server" CssClass="form-control" placeholder="Customer ID (e.g. C001)"></asp:TextBox>
            </div>
            <div class="col-md-2 mb-2">
                <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control" placeholder="Full Name"></asp:TextBox>
            </div>
            <div class="col-md-2 mb-2">
                <asp:TextBox ID="txtContact" runat="server" CssClass="form-control" placeholder="Contact"></asp:TextBox>
            </div>
            <div class="col-md-2 mb-2">
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Username"></asp:TextBox>
            </div>
            <div class="col-md-2 mb-2">
                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="Address"></asp:TextBox>
            </div>
            <div class="col-md-2 mb-2">
                <asp:Button ID="btnAdd" runat="server" Text="Add Customer" CssClass="btn btn-kc-primary w-100" OnClick="btnAdd_Click" />
            </div>
        </div>
    </div>

    <div class="kc-card">
        <h4><i class="bi bi-table"></i> Customer List</h4>
        <div class="table-responsive">
        <asp:GridView ID="gvCustomers" runat="server"
            AutoGenerateColumns="False"
            DataKeyNames="CUSTOMER_ID"
            CssClass="table table-bordered table-hover"
            OnRowEditing="gvCustomers_RowEditing"
            OnRowCancelingEdit="gvCustomers_RowCancelingEdit"
            OnRowUpdating="gvCustomers_RowUpdating"
            OnRowDeleting="gvCustomers_RowDeleting">
            <Columns>
                <asp:BoundField DataField="CUSTOMER_ID"       HeaderText="Customer ID" ReadOnly="True" />
                <asp:BoundField DataField="CUSTOMER_NAME"     HeaderText="Name" />
                <asp:BoundField DataField="CUSTOMER_CONTACT"  HeaderText="Contact" />
                <asp:BoundField DataField="USERNAME"          HeaderText="Username" />
                <asp:BoundField DataField="ADDRESS"           HeaderText="Address" />
                <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>
        </div>
    </div>

</asp:Content>
