<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Uncas.BuildPipeline.Web.ViewModels.PipelineViewModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Pipelines
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table>
        <tr>
            <th>
                Project
            </th>
            <th>
                Activity
            </th>
            <th>
                Build stages
            </th>
        </tr>
        <% foreach (var pipeline in Model)
           { Html.RenderPartial("PipelineListView", pipeline); } %>
    </table>
</asp:Content>
