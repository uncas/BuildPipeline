<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Uncas.BuildPipeline.Web.ViewModels.BuildViewModel>" %>
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
        <div>
            revision
            <%: Model.SourceRevision %>
        </div>
        <div>
            <%: Model.CreatedDisplay %>
        </div>
    </td>
    <td>
        <% Html.RenderPartial("BuildStepView", Model.StepCommit); %>
    </td>
    <td>
        <% if (Model.StepAcceptance != null)
               Html.RenderPartial("BuildStepView", Model.StepAcceptance); %>
    </td>
</tr>
