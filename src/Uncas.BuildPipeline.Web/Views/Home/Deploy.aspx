<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Uncas.BuildPipeline.Web.ViewModels.DeployViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Deploy
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    Deploy this revision to one of the following environments:
    <table>
        <% foreach (var item in Model.Environments)
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
    This revision has previously been deployed as follows:
    <table>
        <tr>
            <th>
                Created
            </th>
            <th>
                Started
            </th>
            <th>
                Completed
            </th>
        </tr>
        <% foreach (var item in Model.Deployments)
           { %>
        <tr>
            <td>
                <%: item.Created %>
            </td>
            <td>
                <%: item.Started %>
            </td>
            <td>
                <%: item.Completed %>
            </td>
        </tr>
        <%} %>
    </table>
</asp:Content>
