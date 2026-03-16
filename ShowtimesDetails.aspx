<%@ Page Title="Showtimes Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShowtimesDetails.aspx.cs" Inherits="KumariCinemas.ShowtimesDetails" MaintainScrollPositionOnPostBack="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="kc-showtimes-page">

    <div class="kc-page-header">
        <h2><i class="bi bi-clock-fill"></i> Showtimes Scheduling Panel</h2>
        <p>Create and maintain cinema schedules with clearer date/time planning and quick updates.</p>
    </div>

    <div class="kc-card">
        <h4><i class="bi bi-calendar2-check"></i> Showtime Form</h4>
        <div class="row g-3 align-items-end kc-entry-row">
            <div class="col-md-6 col-xl-2">
                <label class="form-label">Show ID</label>
                <asp:TextBox ID="txtShowId" runat="server" CssClass="form-control" placeholder="e.g. S001"></asp:TextBox>
            </div>
            <div class="col-md-6 col-xl-2">
                <label class="form-label">City Hall ID</label>
                <asp:DropDownList ID="ddlCityhallId" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlCityhallId_SelectedIndexChanged"></asp:DropDownList>
            </div>
            <div class="col-md-6 col-xl-2">
                <label class="form-label">Movie ID</label>
                <asp:DropDownList ID="ddlMovieId" runat="server" CssClass="form-select"></asp:DropDownList>
            </div>
            <div class="col-md-6 col-xl-2">
                <label class="form-label">Show Date</label>
                <asp:TextBox ID="txtShowDate" runat="server" CssClass="form-control" TextMode="Date" placeholder="MM/DD/YYYY"></asp:TextBox>
            </div>
            <div class="col-md-6 col-xl-2">
                <label class="form-label">Show Time</label>
                <asp:TextBox ID="txtShowTime" runat="server" CssClass="form-control" TextMode="Time" placeholder="HH:MM"></asp:TextBox>
            </div>
            <div class="col-md-12 col-xl-2 kc-entry-actions-col">
                <div class="kc-form-actions kc-entry-actions">
                    <asp:Button ID="btnAdd" runat="server" Text="Add Showtime" CssClass="btn btn-kc-primary" OnClick="btnAdd_Click" />
                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-kc-secondary" OnClick="btnClear_Click" CausesValidation="false" />
                </div>
            </div>
        </div>
        <div class="row g-3 kc-entry-help-row">
            <div class="col-md-6 col-xl-2"><small class="text-muted d-block">Must be unique. Keep format like S001. Can be auto-generated later.</small></div>
            <div class="col-md-6 col-xl-2"><small class="text-muted d-block">Use an existing hall ID from theater records.</small></div>
            <div class="col-md-6 col-xl-2"><small class="text-muted d-block kc-showtimes-movie-help">Pick a mapped movie. Movies appear only after selecting a hall.</small></div>
            <div class="col-md-6 col-xl-2"><small class="text-muted d-block">Choose the screening date.</small></div>
            <div class="col-md-6 col-xl-2"><small class="text-muted d-block">Use local cinema time slot.</small></div>
        </div>
    </div>

    <asp:Label ID="lblMsg" runat="server" CssClass="kc-msg" />

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
                OnRowDeleting="gvShows_RowDeleting"
                OnRowDataBound="gvShows_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="SHOW_ID"     HeaderText="Show ID"      ReadOnly="True" />
                    <asp:TemplateField HeaderText="City Hall ID">
                        <ItemTemplate>
                            <%# Eval("CITYHALL_ID") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlEditCityhallId" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlEditCityhallId_SelectedIndexChanged"></asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Movie ID">
                        <ItemTemplate>
                            <%# Eval("MOVIE_ID") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlEditMovieId" runat="server" CssClass="form-select"></asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Show Date">
                        <ItemTemplate>
                            <%# Eval("SHOW_DATE", "{0:yyyy-MM-dd}") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditShowDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Show Time">
                        <ItemTemplate>
                            <%# Eval("SHOW_TIME") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditShowTime" runat="server" CssClass="form-control" TextMode="Time"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ShowEditButton="True" ShowDeleteButton="True"
                        HeaderText="Actions" ButtonType="Link" ControlStyle-CssClass="kc-grid-action" />
                </Columns>
            </asp:GridView>
        </div>
    </div>

    </div>

</asp:Content>
