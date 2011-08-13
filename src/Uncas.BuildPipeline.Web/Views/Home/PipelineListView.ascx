<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Uncas.BuildPipeline.Web.ViewModels.PipelineViewModel>" %>
<%@ Import Namespace="Uncas.BuildPipeline.Web.ViewModels" %>
<tr class="BuildListView">
    <td>
        <div>
            <%:Model.ProjectName%>
        </div>
        <div>
            <%:Model.SourceUrlRelative%>
        </div>
    </td>
    <td class="SourceDetails">
        <div class="<%:Model.CssClass%>" title="<%:Model.StatusText%>">
            revision
            <%:Model.SourceRevision%>, by
            <%:Model.SourceAuthor%>
        </div>
        <div>
            <%:Model.CreatedDisplay%>
        </div>
    </td>
    <td>
        <% foreach (BuildStepViewModel step in Model.Steps)
           {
               Html.RenderPartial("BuildStepView", step);
           } %>
    </td>
    <td>
        <%:Html.ActionLink("Deploy", "Deploy", new { pipelineId = Model.PipelineId })%>
    </td>
</tr>