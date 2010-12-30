<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Uncas.BuildPipeline.Web.ViewModels.EnvironmentViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Deploy
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    Deploy this revision to one of the following environments:
    <div>
        TODO: Show details about the revision...
    </div>
    <table>
        <% foreach (var item in Model)
           { %>
        <tr>
            <td>
                <%: item.EnvironmentName %>
            </td>
            <td>
                <% using (Html.BeginForm("Deploy", "Home", new { environmentId = item.EnvironmentId }))
                   { %>
                <%: Html.Hidden("PipelineId") %>
                <input type="submit" value="Deploy" />
                <%} %>
            </td>
        </tr>
        <% } %>
    </table>
</asp:Content>
