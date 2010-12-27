<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Uncas.BuildPipeline.Web.ViewModels.BuildViewModel>" %>
<tr class="BuildListView">
    <td>
        <%: Model.ProjectName %>
    </td>
    <td class="SourceDetails">
        <div>
            <%: Model.SourceUrlRelative %>
        </div>
        <div>
            revision <%: Model.SourceRevision %>
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
