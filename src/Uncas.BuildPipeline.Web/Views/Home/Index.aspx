<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Uncas.BuildPipeline.Web.ViewModels.BuildViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Builds
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Builds</h2>
    <table>
        <tr>
            <th>
                Project name
            </th>
            <th>
                Source revision
            </th>
        </tr>
        <% foreach (var item in Model)
           { %>
        <tr>
            <td>
                <%: item.ProjectName %>
            </td>
            <td>
                <%: item.SourceRevision %>
            </td>
        </tr>
        <% } %>
    </table>
</asp:Content>
