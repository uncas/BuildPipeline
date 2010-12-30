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
            <%: Model.SourceRevision %>, by
            <%: Model.SourceAuthor %>
        </div>
        <div>
            <%: Model.CreatedDisplay %>
        </div>
    </td>
    <td>
        <% foreach (var step in Model.Steps)
               Html.RenderPartial("BuildStepView", step); %>
    </td>
    <% if (Model.ShowDeployment)
       { %>
    <td>
        QA1
        <% using (Html.BeginForm("Deploy", "Home", new { pipelineId = Model.PipelineId }))
           { %>
        <input type="submit" value="Deploy" />
        <%} %>
    </td>
    <%} %>
</tr>
