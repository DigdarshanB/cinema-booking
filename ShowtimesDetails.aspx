<%@ Page Title="Showtimes Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShowtimesDetails.aspx.cs" Inherits="KumariCinemas.ShowtimesDetails" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="kc-page-header">
        <h2><i class="bi bi-clock-fill"></i> Showtimes Scheduling Panel</h2>
        <p>Create and maintain cinema schedules with clearer date/time planning and quick updates.</p>
    </div>

    <asp:Label ID="lblMsg" runat="server" CssClass="kc-msg" />

    <div class="kc-card">
        <h4><i class="bi bi-calendar2-check"></i> Showtime Form</h4>
        <div class="row g-3 align-items-end">
            <div class="col-md-6 col-xl-2">
                <label class="form-label">Show ID</label>
                <asp:TextBox ID="txtShowId" runat="server" CssClass="form-control" placeholder="e.g. S001"></asp:TextBox>
                <small class="text-muted d-block mt-1">Must be unique. Keep format like S001. Can be auto-generated later.</small>
            </div>
            <div class="col-md-6 col-xl-2">
                <label class="form-label">City Hall ID</label>
                <asp:TextBox ID="txtCityhallId" runat="server" CssClass="form-control" placeholder="e.g. CH001"></asp:TextBox>
                <small class="text-muted d-block mt-1">Use an existing hall ID from theater records.</small>
            </div>
            <div class="col-md-6 col-xl-2">
                <label class="form-label">Show Date</label>
                <asp:TextBox ID="txtShowDate" runat="server" CssClass="form-control" TextMode="Date" placeholder="MM/DD/YYYY"></asp:TextBox>
                <small class="text-muted d-block mt-1">Choose the screening date.</small>
            </div>
            <div class="col-md-6 col-xl-2">
                <label class="form-label">Show Time</label>
                <asp:TextBox ID="txtShowTime" runat="server" CssClass="form-control" TextMode="Time" placeholder="HH:MM"></asp:TextBox>
                <small class="text-muted d-block mt-1">Use local cinema time slot.</small>
            </div>
            <div class="col-md-6 col-xl-2">
                <label class="form-label">Day Type</label>
                <asp:TextBox ID="txtDayType" runat="server" CssClass="form-control" placeholder="Weekday / Weekend"></asp:TextBox>
                <small class="text-muted d-block mt-1">Example: Weekday, Weekend, Holiday.</small>
            </div>
            <div class="col-md-12 col-xl-2">
                <div class="kc-form-actions">
                    <asp:Button ID="btnAdd" runat="server" Text="Add Showtime" CssClass="btn btn-kc-primary" OnClick="btnAdd_Click" />
                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-kc-secondary" OnClick="btnClear_Click" CausesValidation="false" />
                </div>
            </div>
        </div>
    </div>

    <div class="kc-card">
        <h4><i class="bi bi-table"></i> Scheduled Showtimes</h4>
        <div class="table-responsive">
            <asp:GridView ID="gvShows" runat="server"
                AutoGenerateColumns="False"
                DataKeyNames="SHOW_ID"
                CssClass="table table-bordered table-hover"
                EmptyDataText="No showtimes found."
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
                    <asp:CommandField ShowEditButton="True" ShowDeleteButton="True"
                        HeaderText="Actions" ButtonType="Link" ControlStyle-CssClass="kc-grid-action" />
                </Columns>
            </asp:GridView>
        </div>
    </div>

</asp:Content>
