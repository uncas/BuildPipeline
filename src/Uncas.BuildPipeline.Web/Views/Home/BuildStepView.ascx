<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Uncas.BuildPipeline.Web.ViewModels.BuildStepViewModel>" %>
<div class="BuildStep" title="<%: Model.StatusText %>">
    <div>
        <%: Model.StepName %>
    </div>
    <div class="<%=Model.CssClass %>">
    </div>
</div>
