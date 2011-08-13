<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Uncas.BuildPipeline.Web.ViewModels.DeployViewModel>" %>
<%@ Import Namespace="Uncas.BuildPipeline.Web.ViewModels" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Deploy
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("PipelineDetailsView", Model.Pipeline); %>
    <div>
        Deploy this revision to one of the following environments:
        <table>
            <% foreach (EnvironmentViewModel item in Model.Environments)
               { %>
            <tr>
                <td>
                    <%:item.EnvironmentName%>
                </td>
                <td>
                    <% using (Html.BeginForm("Deploy", "Home", new { environmentId = item.EnvironmentId }))
{ %>
                    <%:Html.Hidden("PipelineId")%>
                    <input type="submit" value="Deploy" />
                    <% } %>
                </td>
            </tr>
            <% } %>
        </table>
    </div>
    <div>
        This revision has previously been deployed as follows:
        <table>
            <tr>
                <th>
                    Environment
                </th>
                <th>
                    Scheduled
                </th>
                <th>
                    Started
                </th>
                <th>
                    Completed
                </th>
            </tr>
            <% foreach (DeploymentViewModel item in Model.Deployments)
               { %>
            <tr>
                <td>
                    <%:item.EnvironmentName%>
                </td>
                <td>
                    <%:item.Created%>
                </td>
                <td>
                    <%:item.Started%>
                </td>
                <td>
                    <%:item.Completed%>
                </td>
            </tr>
            <% } %>
        </table>
    </div>
</asp:Content>