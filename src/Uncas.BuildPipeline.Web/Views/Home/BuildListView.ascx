<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Uncas.BuildPipeline.Web.ViewModels.BuildViewModel>" %>
<tr>
    <td>
        <%: Model.ProjectName %>
    </td>
    <td>
        <%: Model.SourceRevision %>
    </td>
    <td>
        <% Html.RenderPartial("BuildStepView", Model.StepUnit); %>
    </td>
    <td>
        <% if (Model.StepIntegration != null)
               Html.RenderPartial("BuildStepView", Model.StepIntegration); %>
    </td>
</tr>
