<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Uncas.BuildPipeline.Web.ViewModels.PipelineViewModel>" %>
<%@ Import Namespace="Uncas.BuildPipeline.Web.ViewModels" %>
<div>
    <%:Model.ProjectName%>
</div>
<div>
    <%:Model.BranchName%>
</div>
<div class="<%:Model.CssClass%>" title="<%:Model.StatusText%>">
    revision
    <%:Model.Revision%>, by
    <%:Model.SourceAuthor%>
</div>
<div>
    <%:Model.CreatedDisplay%>
</div>
<table>
    <tr>
        <td>
            <% foreach (BuildStepViewModel step in Model.Steps)
               {
                   Html.RenderPartial("BuildStepView", step);
               } %>
        </td>
    </tr>
</table>