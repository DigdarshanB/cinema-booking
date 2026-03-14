<%@ Page Title="User Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserDetails.aspx.cs" Inherits="KumariCinemas.UserDetails" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="kc-page-header">
        <h2><i class="bi bi-people-fill"></i> Users Management</h2>
        <p>Create, update, and maintain cinema customer profiles from a single screen.</p>
    </div>

    <asp:Label ID="lblMsg" runat="server" CssClass="kc-msg" />

    <div class="kc-card">
        <h4><i class="bi bi-person-plus-fill"></i> Customer Form</h4>
        <div class="row g-3">
            <div class="col-md-6 col-xl-2">
                <label class="form-label">Customer ID</label>
                <asp:TextBox ID="txtCustomerId" runat="server" CssClass="form-control" placeholder="e.g. C001"></asp:TextBox>
            </div>
            <div class="col-md-6 col-xl-3">
                <label class="form-label">Full Name</label>
                <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control" placeholder="Full Name"></asp:TextBox>
            </div>
            <div class="col-md-6 col-xl-2">
                <label class="form-label">Contact</label>
                <asp:TextBox ID="txtContact" runat="server" CssClass="form-control" placeholder="Contact"></asp:TextBox>
            </div>
            <div class="col-md-6 col-xl-2">
                <label class="form-label">Username</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Username"></asp:TextBox>
            </div>
            <div class="col-md-6 col-xl-3">
                <label class="form-label">Address</label>
                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="Address"></asp:TextBox>
            </div>
            <div class="col-12">
                <div class="kc-form-actions">
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-kc-primary" OnClick="btnAdd_Click" />
                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-kc-secondary" OnClick="btnClear_Click" CausesValidation="false" />
                </div>
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
                EmptyDataText="No customers found."
                OnRowEditing="gvCustomers_RowEditing"
                OnRowCancelingEdit="gvCustomers_RowCancelingEdit"
                OnRowUpdating="gvCustomers_RowUpdating"
                OnRowDeleting="gvCustomers_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="CUSTOMER_ID"       HeaderText="Customer ID"  ReadOnly="True" />
                    <asp:BoundField DataField="CUSTOMER_NAME"     HeaderText="Name" />
                    <asp:BoundField DataField="CUSTOMER_CONTACT"  HeaderText="Contact" />
                    <asp:BoundField DataField="USERNAME"          HeaderText="Username" />
                    <asp:BoundField DataField="ADDRESS"           HeaderText="Address" />
                    <asp:CommandField ShowEditButton="True" ShowDeleteButton="True"
                        HeaderText="Actions" ButtonType="Link" ControlStyle-CssClass="kc-grid-action" />
                </Columns>
            </asp:GridView>
        </div>
    </div>

</asp:Content>
