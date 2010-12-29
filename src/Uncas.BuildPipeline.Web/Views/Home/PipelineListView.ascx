<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Uncas.BuildPipeline.Web.ViewModels.PipelineViewModel>" %>
<tr class="BuildListView">
    <td>
        <div>
            <%: Model.ProjectName %>
        </div>
        <div>
            <%: Model.SourceUrlRelative %>
        </div>
    </td>
    <td class="SourceDetails">
        <div class="<%: Model.CssClass %>" title="<%: Model.StatusText %>">
            revision
            <%: Model.SourceRevision %>
        </div>
        <div>
            <%: Model.CreatedDisplay %>
        </div>
    </td>
    <td>
        <% foreach (var step in Model.Steps)
               Html.RenderPartial("BuildStepView", step); %>
    </td>
</tr>
