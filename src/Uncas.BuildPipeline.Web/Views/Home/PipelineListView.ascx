<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Uncas.BuildPipeline.Web.ViewModels.PipelineListItemViewModel>" %>
<%@ Import Namespace="Uncas.BuildPipeline.Models" %>
<%@ Import Namespace="Uncas.BuildPipeline.Web.ViewModels" %>
<tr class="BuildListView">
    <td>
        <div>
            <%: Model.ProjectName %>
        </div>
        <div>
            <%: Model.BranchName %>
        </div>
    </td>
    <td class="SourceDetails">
        <div class="<%: Model.CssClass %>" title="<%: Model.StatusText %>">
            revision
            <a href="<%: Model.CommitLink %>"><%: Revision.Short(Model.Revision) %></a>, by
            <%: Model.SourceAuthor %>
        </div>
        <div>
            <%: Model.CreatedDisplay %>
        </div>
    </td>
    <td>
        <% foreach (BuildStepViewModel step in Model.Steps)
           {
               Html.RenderPartial("BuildStepView", step);
           } %>
    </td>
    <td>
        <%: Html.ActionLink("Deploy", "Deploy", new {pipelineId = Model.PipelineId}) %>
        <%: Html.ActionLink("Download", "Download", new {id = Model.PackagePath}) %>
    </td>
</tr>