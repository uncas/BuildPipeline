<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Uncas.BuildPipeline.Web.Models.Build>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Index
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Index</h2>
    <table>
        <tr>
            <th>
                ProjectName
            </th>
            <th>
                SourceRevision
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
