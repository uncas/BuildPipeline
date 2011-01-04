<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Uncas.BuildPipeline.Web.ViewModels.PipelineViewModel>" %>
<div>
    <%: Model.ProjectName %>
</div>
<div>
    <%: Model.SourceUrlRelative %>
</div>
<div class="<%: Model.CssClass %>" title="<%: Model.StatusText %>">
    revision
    <%: Model.SourceRevision %>, by
    <%: Model.SourceAuthor %>
</div>
<div>
    <%: Model.CreatedDisplay %>
</div>
<table>
    <tr>
        <td>
            <% foreach (var step in Model.Steps)
                   Html.RenderPartial("BuildStepView", step);  %>
        </td>
    </tr>
</table>
