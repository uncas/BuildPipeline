<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Uncas.BuildPipeline.Models.ProjectReadModel>>" %>
<%@ Import Namespace="Uncas.BuildPipeline.Models" %>
<%@ Import Namespace="Uncas.BuildPipeline.Repositories" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Projects
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <table>
        <tr>
            <th>Project name
            </th>
            <th>Git remote URL
            </th>
            <th>Github URL
            </th>
            <th>Deployment script
            </th>
            <th></th>
        </tr>

        <% foreach (ProjectReadModel item in Model)
           { %>
            <tr>
                <td>
                    <%: Html.DisplayFor(modelItem => item.ProjectName) %>
                </td>
                <td>
                    <%: Html.DisplayFor(modelItem => item.GitRemoteUrl) %>
                </td>
                <td>
                    <%: Html.DisplayFor(modelItem => item.GithubUrl) %>
                </td>
                <td>
                    <pre><%: Html.DisplayFor(modelItem => item.DeploymentScript) %></pre>
                </td>
                <td>
                    <%: Html.ActionLink("Edit", "Edit", new {id = item.ProjectId}) %> 
                </td>
            </tr>
        <% } %>
    </table>

</asp:Content>