<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerTest.aspx.cs" Inherits="KumariCinemas.CustomerTest" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Customer CRUD Test</title>
</head>
<body>
<form id="form1" runat="server">
    <div style="max-width:900px;margin:24px auto;font-family:Segoe UI;">
        <h2>Oracle CRUD Test (TEST_CUSTOMER)</h2>

        <asp:Label ID="lblMsg" runat="server" ForeColor="DarkRed"></asp:Label>
        <br /><br />

        <h3>Add new customer</h3>
        <asp:TextBox ID="txtName" runat="server" Width="320px" placeholder="Full name"></asp:TextBox>
        &nbsp;
        <asp:TextBox ID="txtPhone" runat="server" Width="200px" placeholder="Phone"></asp:TextBox>
        &nbsp;
        <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" />
        <br /><br />

        <h3>Customers</h3>
        <asp:GridView ID="gvCustomers" runat="server"
            AutoGenerateColumns="False"
            DataKeyNames="CUSTOMER_ID"
            OnRowEditing="gvCustomers_RowEditing"
            OnRowCancelingEdit="gvCustomers_RowCancelingEdit"
            OnRowUpdating="gvCustomers_RowUpdating"
            OnRowDeleting="gvCustomers_RowDeleting">
            <Columns>
                <asp:BoundField DataField="CUSTOMER_ID" HeaderText="ID" ReadOnly="True" />
                <asp:BoundField DataField="FULL_NAME" HeaderText="Full Name" />
                <asp:BoundField DataField="PHONE" HeaderText="Phone" />
                <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>
    </div>
</form>
</body>
</html>
